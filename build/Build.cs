using System;
using System.Collections.Generic;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.IO.CompressionTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    string Version => IsLocalBuild ? "0.0.0" : Environment.GetEnvironmentVariable("Version");

    [Solution] readonly Solution Solution;

    AbsolutePath TerraformSourceDirectory => RootDirectory / "terraform";
    AbsolutePath ServerSourceDirectory => RootDirectory / "server";

    // Server Deployment Files
    AbsolutePath ServerEnvFile => RootDirectory / "production.env";
    AbsolutePath ServerPdfServerDockerFile => RootDirectory / "production.pdf.env";

    // AbsolutePath UISourceDirectory => RootDirectory / "ui";

    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath ServerDeploymentFilesZipDir => ArtifactsDirectory / "server-deployment-files";
    AbsolutePath ServerBuildForZipping => ArtifactsDirectory / "serverbuild";
    AbsolutePath MigratorBuildForZipping => ArtifactsDirectory / "migratorbuild";

    Target Clean =>
        _ => _
            .Before(Restore)
            .Executes(() =>
            {
                ServerSourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
                EnsureCleanDirectory(ArtifactsDirectory);
            });

    Target Restore =>
        _ => _
            .DependsOn(Clean)
            .Executes(() =>
            {
                DotNetRestore(s => s.SetProjectFile(Solution));
            });

    Target Compile =>
        _ => _
            .DependsOn(Restore)
            .Executes(() =>
            {
                DotNetBuild(s => s
                    .SetProjectFile(Solution)
                    .SetConfiguration(Configuration)
                    .EnableNoRestore());
            });

    Target Test =>
        _ => _
            .DependsOn(Compile)
            .Executes(() =>
            {
                DotNetTest(_ => _
                    .SetProjectFile(Solution)
                    .SetConfiguration(Configuration)
                    .SetNoBuild(true)
                    .EnableNoRestore());
            });

    Target PublishArtifacts =>
        _ => _
            .DependsOn(Compile)
            .Executes(() =>
            {
                CopyFileToDirectory(ServerEnvFile, ServerDeploymentFilesZipDir);
                CopyFileToDirectory(ServerPdfServerDockerFile, ServerDeploymentFilesZipDir);

                DotNetPublish(_ => _
                    .SetProject(ServerSourceDirectory / "Palavyr.API")
                    .SetConfiguration(Configuration)
                    .SetOutput(ServerBuildForZipping)
                    .EnableNoBuild()
                    .AddProperty("Version", Version)
                );

                DotNetPublish(_ => _
                    .SetProject(ServerSourceDirectory / "Palavyr.Data.Migrator")
                    .SetConfiguration(Configuration)
                    .SetOutput(MigratorBuildForZipping)
                    .EnableNoBuild()
                    .AddProperty("Version", Version)
                );
                // TODO: Add publishing the npm packages here
            });

    Target Zip =>
        _ => _
            .DependsOn(PublishArtifacts)
            .Executes(() =>
            {
                // var serverPackageOutput = ArtifactsDirectory / $"palavyr-server.{Version}.zip";
                // Compress(MigratorBuildForZipping, serverPackageOutput);

                // var migratorPackageOutput = ArtifactsDirectory / $"palavyr-migrator.{Version}.zip";
                // Compress(MigratorBuildForZipping, migratorPackageOutput);

                var serverEnvFilePackageOutput = ArtifactsDirectory / $"palavyr-server-deployment-files.{Version}.zip";
                Compress(ServerDeploymentFilesZipDir, serverEnvFilePackageOutput);

                var terraformPackageOutput = ArtifactsDirectory / $"palavyr-terraform.{Version}.zip";
                Compress(TerraformSourceDirectory, terraformPackageOutput);

                var packages = string.Join(",", new List<string>()
                {
                    serverEnvFilePackageOutput,
                    terraformPackageOutput,
                    // serverPackageOutput,
                    // migratorPackageOutput
                });

                Console.WriteLine($"::set-output name=packages_to_push::{packages}");
            });

    Target EntryPoint => _ => _.DependsOn(Zip);

    public static int Main() => Execute<Build>(x => x.EntryPoint);
}