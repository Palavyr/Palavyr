﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Palavyr.Data.Migrations.AccountsMigrations
{
    [DbContext(typeof(AccountsContext))]
    [Migration("20210122123625_AddbackupTableToRecordLatestBackupS3Keys")]
    partial class AddbackupTableToRecordLatestBackupS3Keys
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Server.Domain.Accounts.Backup", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("LatestFullDbBackup")
                        .HasColumnType("text");

                    b.Property<string>("LatestUserDataBackup")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Backups");
                });

            modelBuilder.Entity("Server.Domain.Accounts.EmailVerification", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("AuthenticationToken")
                        .HasColumnType("text");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("EmailVerifications");
                });

            modelBuilder.Entity("Server.Domain.Accounts.Session", b =>
                {
                    b.Property<string>("SessionId")
                        .HasColumnType("text");

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("ApiKey")
                        .HasColumnType("text");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("SessionId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Server.Domain.Accounts.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("ApiKey")
                        .HasColumnType("text");

                    b.Property<int>("NumAreas")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("Server.Domain.Accounts.UserAccount", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("AccountLogoUri")
                        .HasColumnType("text");

                    b.Property<int>("AccountType")
                        .HasColumnType("integer");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("ApiKey")
                        .HasColumnType("text");

                    b.Property<string>("CompanyName")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("CurrentPeriodEnd")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("DefaultEmailIsVerified")
                        .HasColumnType("boolean");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("text");

                    b.Property<bool>("HasUpgraded")
                        .HasColumnType("boolean");

                    b.Property<string>("Locale")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<int>("PaymentInterval")
                        .HasColumnType("integer");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<int>("PlanType")
                        .HasColumnType("integer");

                    b.Property<string>("StripeCustomerId")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
