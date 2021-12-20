﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Palavyr.Core.Data;

namespace Palavyr.Core.Data.Migrations.ConfigurationMigrations
{
    [DbContext(typeof(DashContext))]
    [Migration("20211220085317_Optional_totals_row_in_tables")]
    partial class Optional_totals_row_in_tables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Palavyr.Core.Models.Configuration.Schemas.Area", b =>
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

                    b.Property<string>("FallbackEmailTemplate")
                        .HasColumnType("text");

                    b.Property<string>("FallbackSubject")
                        .HasColumnType("text");

                    b.Property<bool>("IncludeDynamicTableTotals")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("Prologue")
                        .HasColumnType("text");

                    b.Property<bool>("SendAttachmentsOnFallback")
                        .HasColumnType("boolean");

                    b.Property<bool>("SendPdfResponse")
                        .HasColumnType("boolean");

                    b.Property<string>("Subject")
                        .HasColumnType("text");

                    b.Property<bool>("UseAreaFallbackEmail")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("Palavyr.Core.Models.Configuration.Schemas.ConversationNode", b =>
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

                    b.Property<string>("DynamicType")
                        .HasColumnType("text");

                    b.Property<string>("ImageId")
                        .HasColumnType("text");

                    b.Property<bool>("IsAnabranchMergePoint")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsAnabranchType")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsCritical")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsCurrency")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDynamicTableNode")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsImageNode")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsLoopbackAnchorType")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsMultiOptionEditable")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsMultiOptionType")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsRoot")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsTerminalType")
                        .HasColumnType("boolean");

                    b.Property<string>("NodeChildrenString")
                        .HasColumnType("text");

                    b.Property<string>("NodeComponentType")
                        .HasColumnType("text");

                    b.Property<string>("NodeId")
                        .HasColumnType("text");

                    b.Property<string>("NodeType")
                        .HasColumnType("text");

                    b.Property<int>("NodeTypeCode")
                        .HasColumnType("integer");

                    b.Property<string>("OptionPath")
                        .HasColumnType("text");

                    b.Property<int?>("ResolveOrder")
                        .HasColumnType("integer");

                    b.Property<bool>("ShouldRenderChildren")
                        .HasColumnType("boolean");

                    b.Property<bool>("ShouldShowMultiOption")
                        .HasColumnType("boolean");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.Property<string>("ValueOptions")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AreaId");

                    b.ToTable("ConversationNodes");
                });

            modelBuilder.Entity("Palavyr.Core.Models.Configuration.Schemas.DynamicTableMeta", b =>
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

                    b.Property<bool>("UseTableTagAsResponseDescription")
                        .HasColumnType("boolean");

                    b.Property<bool>("ValuesAsPaths")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("AreaId");

                    b.ToTable("DynamicTableMetas");
                });

            modelBuilder.Entity("Palavyr.Core.Models.Configuration.Schemas.DynamicTables.BasicThreshold", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("AreaIdentifier")
                        .HasColumnType("text");

                    b.Property<string>("ItemName")
                        .HasColumnType("text");

                    b.Property<bool>("Range")
                        .HasColumnType("boolean");

                    b.Property<string>("RowId")
                        .HasColumnType("text");

                    b.Property<int>("RowOrder")
                        .HasColumnType("integer");

                    b.Property<string>("TableId")
                        .HasColumnType("text");

                    b.Property<double>("Threshold")
                        .HasColumnType("double precision");

                    b.Property<bool>("TriggerFallback")
                        .HasColumnType("boolean");

                    b.Property<double>("ValueMax")
                        .HasColumnType("double precision");

                    b.Property<double>("ValueMin")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("BasicThresholds");
                });

            modelBuilder.Entity("Palavyr.Core.Models.Configuration.Schemas.DynamicTables.CategoryNestedThreshold", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("AreaIdentifier")
                        .HasColumnType("text");

                    b.Property<string>("ItemId")
                        .HasColumnType("text");

                    b.Property<string>("ItemName")
                        .HasColumnType("text");

                    b.Property<int>("ItemOrder")
                        .HasColumnType("integer");

                    b.Property<bool>("Range")
                        .HasColumnType("boolean");

                    b.Property<string>("RowId")
                        .HasColumnType("text");

                    b.Property<int>("RowOrder")
                        .HasColumnType("integer");

                    b.Property<string>("TableId")
                        .HasColumnType("text");

                    b.Property<double>("Threshold")
                        .HasColumnType("double precision");

                    b.Property<bool>("TriggerFallback")
                        .HasColumnType("boolean");

                    b.Property<double>("ValueMax")
                        .HasColumnType("double precision");

                    b.Property<double>("ValueMin")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("CategoryNestedThresholds");
                });

            modelBuilder.Entity("Palavyr.Core.Models.Configuration.Schemas.DynamicTables.PercentOfThreshold", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("AreaIdentifier")
                        .HasColumnType("text");

                    b.Property<string>("ItemId")
                        .HasColumnType("text");

                    b.Property<string>("ItemName")
                        .HasColumnType("text");

                    b.Property<int>("ItemOrder")
                        .HasColumnType("integer");

                    b.Property<double>("Modifier")
                        .HasColumnType("double precision");

                    b.Property<bool>("PosNeg")
                        .HasColumnType("boolean");

                    b.Property<bool>("Range")
                        .HasColumnType("boolean");

                    b.Property<string>("RowId")
                        .HasColumnType("text");

                    b.Property<int>("RowOrder")
                        .HasColumnType("integer");

                    b.Property<string>("TableId")
                        .HasColumnType("text");

                    b.Property<double>("Threshold")
                        .HasColumnType("double precision");

                    b.Property<bool>("TriggerFallback")
                        .HasColumnType("boolean");

                    b.Property<double>("ValueMax")
                        .HasColumnType("double precision");

                    b.Property<double>("ValueMin")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("PercentOfThresholds");
                });

            modelBuilder.Entity("Palavyr.Core.Models.Configuration.Schemas.DynamicTables.SelectOneFlat", b =>
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

                    b.Property<int>("RowOrder")
                        .HasColumnType("integer");

                    b.Property<string>("TableId")
                        .HasColumnType("text");

                    b.Property<double>("ValueMax")
                        .HasColumnType("double precision");

                    b.Property<double>("ValueMin")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("SelectOneFlats");
                });

            modelBuilder.Entity("Palavyr.Core.Models.Configuration.Schemas.DynamicTables.TwoNestedCategory", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("AreaIdentifier")
                        .HasColumnType("text");

                    b.Property<string>("InnerItemName")
                        .HasColumnType("text");

                    b.Property<string>("ItemId")
                        .HasColumnType("text");

                    b.Property<string>("ItemName")
                        .HasColumnType("text");

                    b.Property<int>("ItemOrder")
                        .HasColumnType("integer");

                    b.Property<bool>("Range")
                        .HasColumnType("boolean");

                    b.Property<string>("RowId")
                        .HasColumnType("text");

                    b.Property<int>("RowOrder")
                        .HasColumnType("integer");

                    b.Property<string>("TableId")
                        .HasColumnType("text");

                    b.Property<double>("ValueMax")
                        .HasColumnType("double precision");

                    b.Property<double>("ValueMin")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("TwoNestedCategories");
                });

            modelBuilder.Entity("Palavyr.Core.Models.Configuration.Schemas.FileNameMap", b =>
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

                    b.Property<string>("S3Key")
                        .HasColumnType("text");

                    b.Property<string>("SafeName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("FileNameMaps");
                });

            modelBuilder.Entity("Palavyr.Core.Models.Configuration.Schemas.Image", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsUrl")
                        .HasColumnType("boolean");

                    b.Property<string>("RiskyName")
                        .HasColumnType("text");

                    b.Property<string>("S3Key")
                        .HasColumnType("text");

                    b.Property<string>("SafeName")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Palavyr.Core.Models.Configuration.Schemas.StaticFee", b =>
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

            modelBuilder.Entity("Palavyr.Core.Models.Configuration.Schemas.StaticTableRow", b =>
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

            modelBuilder.Entity("Palavyr.Core.Models.Configuration.Schemas.StaticTablesMeta", b =>
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

                    b.Property<bool>("IncludeTotals")
                        .HasColumnType("boolean");

                    b.Property<bool>("PerPersonInputRequired")
                        .HasColumnType("boolean");

                    b.Property<int>("TableOrder")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AreaId");

                    b.ToTable("StaticTablesMetas");
                });

            modelBuilder.Entity("Palavyr.Core.Models.Configuration.Schemas.WidgetPreference", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("ButtonColor")
                        .HasColumnType("text");

                    b.Property<string>("ButtonFontColor")
                        .HasColumnType("text");

                    b.Property<string>("ChatBubbleColor")
                        .HasColumnType("text");

                    b.Property<string>("ChatFontColor")
                        .HasColumnType("text");

                    b.Property<string>("ChatHeader")
                        .HasColumnType("text");

                    b.Property<string>("FontFamily")
                        .HasColumnType("text");

                    b.Property<string>("HeaderColor")
                        .HasColumnType("text");

                    b.Property<string>("HeaderFontColor")
                        .HasColumnType("text");

                    b.Property<string>("LandingHeader")
                        .HasColumnType("text");

                    b.Property<string>("ListFontColor")
                        .HasColumnType("text");

                    b.Property<string>("OptionsHeaderColor")
                        .HasColumnType("text");

                    b.Property<string>("OptionsHeaderFontColor")
                        .HasColumnType("text");

                    b.Property<string>("Placeholder")
                        .HasColumnType("text");

                    b.Property<string>("SelectListColor")
                        .HasColumnType("text");

                    b.Property<bool>("WidgetState")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("WidgetPreferences");
                });

            modelBuilder.Entity("Palavyr.Core.Models.Configuration.Schemas.ConversationNode", b =>
                {
                    b.HasOne("Palavyr.Core.Models.Configuration.Schemas.Area", null)
                        .WithMany("ConversationNodes")
                        .HasForeignKey("AreaId");
                });

            modelBuilder.Entity("Palavyr.Core.Models.Configuration.Schemas.DynamicTableMeta", b =>
                {
                    b.HasOne("Palavyr.Core.Models.Configuration.Schemas.Area", null)
                        .WithMany("DynamicTableMetas")
                        .HasForeignKey("AreaId");
                });

            modelBuilder.Entity("Palavyr.Core.Models.Configuration.Schemas.StaticTableRow", b =>
                {
                    b.HasOne("Palavyr.Core.Models.Configuration.Schemas.StaticFee", "Fee")
                        .WithMany()
                        .HasForeignKey("FeeId");

                    b.HasOne("Palavyr.Core.Models.Configuration.Schemas.StaticTablesMeta", null)
                        .WithMany("StaticTableRows")
                        .HasForeignKey("StaticTablesMetaId");
                });

            modelBuilder.Entity("Palavyr.Core.Models.Configuration.Schemas.StaticTablesMeta", b =>
                {
                    b.HasOne("Palavyr.Core.Models.Configuration.Schemas.Area", null)
                        .WithMany("StaticTablesMetas")
                        .HasForeignKey("AreaId");
                });
#pragma warning restore 612, 618
        }
    }
}
