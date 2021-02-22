﻿// <auto-generated />

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Palavyr.Data.Migrations.ConfigurationMigrations
{
    [DbContext(typeof(DashContext))]
    [Migration("20201129110218_AddIsTerminalType")]
    partial class AddIsTerminalType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Server.Domain.Configuration.Schemas.Area", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("AreaDisplayTitle")
                        .HasColumnType("text");

                    b.Property<string>("AreaIdentifier")
                        .HasColumnType("text");

                    b.Property<string>("AreaName")
                        .HasColumnType("text");

                    b.Property<string>("AreaSpecificEmail")
                        .HasColumnType("text");

                    b.Property<bool>("EmailIsVerified")
                        .HasColumnType("boolean");

                    b.Property<string>("EmailTemplate")
                        .HasColumnType("text");

                    b.Property<string>("Epilogue")
                        .HasColumnType("text");

                    b.Property<string>("GroupId")
                        .HasColumnType("text");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("boolean");

                    b.Property<string>("Prologue")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("Server.Domain.Configuration.Schemas.ConversationNode", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<int?>("AreaId")
                        .HasColumnType("integer");

                    b.Property<string>("AreaIdentifier")
                        .HasColumnType("text");

                    b.Property<bool>("Fallback")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsCritical")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsMultiOptionType")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsRoot")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsTerminalType")
                        .HasColumnType("boolean");

                    b.Property<string>("NodeChildrenString")
                        .HasColumnType("text");

                    b.Property<string>("NodeId")
                        .HasColumnType("text");

                    b.Property<string>("NodeType")
                        .HasColumnType("text");

                    b.Property<string>("OptionPath")
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.Property<string>("ValueOptions")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AreaId");

                    b.ToTable("ConversationNodes");
                });

            modelBuilder.Entity("Server.Domain.Configuration.Schemas.DynamicTableMeta", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<int?>("AreaId")
                        .HasColumnType("integer");

                    b.Property<string>("AreaIdentifier")
                        .HasColumnType("text");

                    b.Property<string>("PrettyName")
                        .HasColumnType("text");

                    b.Property<string>("TableId")
                        .HasColumnType("text");

                    b.Property<string>("TableTag")
                        .HasColumnType("text");

                    b.Property<string>("TableType")
                        .HasColumnType("text");

                    b.Property<bool>("ValuesAsPaths")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("AreaId");

                    b.ToTable("DynamicTableMetas");
                });

            modelBuilder.Entity("Server.Domain.Configuration.Schemas.FileNameMap", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("AreaIdentifier")
                        .HasColumnType("text");

                    b.Property<string>("RiskyName")
                        .HasColumnType("text");

                    b.Property<string>("SafeName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("FileNameMaps");
                });

            modelBuilder.Entity("Server.Domain.Configuration.Schemas.GroupMap", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("GroupId")
                        .HasColumnType("text");

                    b.Property<string>("GroupName")
                        .HasColumnType("text");

                    b.Property<string>("ParentId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Server.Domain.Configuration.Schemas.SelectOneFlat", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("AreaIdentifier")
                        .HasColumnType("text");

                    b.Property<string>("Option")
                        .HasColumnType("text");

                    b.Property<bool>("Range")
                        .HasColumnType("boolean");

                    b.Property<string>("TableId")
                        .HasColumnType("text");

                    b.Property<string>("TableTag")
                        .HasColumnType("text");

                    b.Property<double>("ValueMax")
                        .HasColumnType("double precision");

                    b.Property<double>("ValueMin")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("SelectOneFlats");
                });

            modelBuilder.Entity("Server.Domain.Configuration.Schemas.StaticFee", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("AreaIdentifier")
                        .HasColumnType("text");

                    b.Property<string>("FeeId")
                        .HasColumnType("text");

                    b.Property<double>("Max")
                        .HasColumnType("double precision");

                    b.Property<double>("Min")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("StaticFees");
                });

            modelBuilder.Entity("Server.Domain.Configuration.Schemas.StaticTableRow", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("AreaIdentifier")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int?>("FeeId")
                        .HasColumnType("integer");

                    b.Property<bool>("PerPerson")
                        .HasColumnType("boolean");

                    b.Property<bool>("Range")
                        .HasColumnType("boolean");

                    b.Property<int>("RowOrder")
                        .HasColumnType("integer");

                    b.Property<int?>("StaticTablesMetaId")
                        .HasColumnType("integer");

                    b.Property<int>("TableOrder")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FeeId");

                    b.HasIndex("StaticTablesMetaId");

                    b.ToTable("StaticTablesRows");
                });

            modelBuilder.Entity("Server.Domain.Configuration.Schemas.StaticTablesMeta", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<int?>("AreaId")
                        .HasColumnType("integer");

                    b.Property<string>("AreaIdentifier")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("TableOrder")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AreaId");

                    b.ToTable("StaticTablesMetas");
                });

            modelBuilder.Entity("Server.Domain.Configuration.Schemas.WidgetPreference", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("FontFamily")
                        .HasColumnType("text");

                    b.Property<string>("Header")
                        .HasColumnType("text");

                    b.Property<string>("HeaderColor")
                        .HasColumnType("text");

                    b.Property<string>("HeaderFontColor")
                        .HasColumnType("text");

                    b.Property<string>("ListFontColor")
                        .HasColumnType("text");

                    b.Property<string>("Placeholder")
                        .HasColumnType("text");

                    b.Property<string>("SelectListColor")
                        .HasColumnType("text");

                    b.Property<bool>("ShouldGroup")
                        .HasColumnType("boolean");

                    b.Property<string>("Subtitle")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("WidgetPreferences");
                });

            modelBuilder.Entity("Server.Domain.Configuration.Schemas.ConversationNode", b =>
                {
                    b.HasOne("Server.Domain.Configuration.Schemas.Area", null)
                        .WithMany("ConversationNodes")
                        .HasForeignKey("AreaId");
                });

            modelBuilder.Entity("Server.Domain.Configuration.Schemas.DynamicTableMeta", b =>
                {
                    b.HasOne("Server.Domain.Configuration.Schemas.Area", null)
                        .WithMany("DynamicTableMetas")
                        .HasForeignKey("AreaId");
                });

            modelBuilder.Entity("Server.Domain.Configuration.Schemas.StaticTableRow", b =>
                {
                    b.HasOne("Server.Domain.Configuration.Schemas.StaticFee", "Fee")
                        .WithMany()
                        .HasForeignKey("FeeId");

                    b.HasOne("Server.Domain.Configuration.Schemas.StaticTablesMeta", null)
                        .WithMany("StaticTableRows")
                        .HasForeignKey("StaticTablesMetaId");
                });

            modelBuilder.Entity("Server.Domain.Configuration.Schemas.StaticTablesMeta", b =>
                {
                    b.HasOne("Server.Domain.Configuration.Schemas.Area", null)
                        .WithMany("StaticTablesMetas")
                        .HasForeignKey("AreaId");
                });
#pragma warning restore 612, 618
        }
    }
}
