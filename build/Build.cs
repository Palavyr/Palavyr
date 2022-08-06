using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
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
    AbsolutePath UISourceDirectory => RootDirectory / "ui";

    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath TempOutputServer => ArtifactsDirectory / "tempserver";

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
                DotNetRestore(s => s
                    .SetProjectFile(Solution));
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
                DotNetPublish(_ => _
                    .SetProject(ServerSourceDirectory / "Palavyr.API")
                    .SetConfiguration(Configuration)
                    .SetOutput(TempOutputServer)
                    .EnableNoBuild()
                    .AddProperty("Version", Version)
                );
            });

    Target Zip =>
        _ => _
            .DependsOn(PublishArtifacts)
            .Executes(() =>
            {
                var pipelinesPackageOutput = ArtifactsDirectory / $"Palavyr.API.{Version}.zip";
                Compress(ArtifactsDirectory, pipelinesPackageOutput);

                var terraformPackageOutput = ArtifactsDirectory / $"Octopus.CrowsNestPipelines.Terraform.{Version}.zip";
                Compress(TerraformSourceDirectory, terraformPackageOutput);

                Console.WriteLine($"::set-output name=packages_to_push::{pipelinesPackageOutput},{terraformPackageOutput}");
            });

    Target CleanUp =>
        _ => _
            .DependsOn(Zip)
            .Executes(() =>
            {
                DeleteDirectory(TempOutputServer);
            });

    Target EntryPoint => _ => _.DependsOn(CleanUp);

    public static int Main() => Execute<Build>(x => x.EntryPoint);
}