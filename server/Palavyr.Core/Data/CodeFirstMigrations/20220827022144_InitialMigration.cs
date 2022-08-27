using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Palavyr.Core.Data.CodeFirstMigrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Password = table.Column<string>(type: "text", nullable: false),
                    EmailAddress = table.Column<string>(type: "text", nullable: false),
                    DefaultEmailIsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccountLogoUri = table.Column<string>(type: "text", nullable: false),
                    GeneralFallbackSubject = table.Column<string>(type: "text", nullable: false),
                    GeneralFallbackEmailTemplate = table.Column<string>(type: "text", nullable: false),
                    ApiKey = table.Column<string>(type: "text", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Locale = table.Column<string>(type: "text", nullable: false),
                    AccountType = table.Column<int>(type: "integer", nullable: false),
                    PlanType = table.Column<int>(type: "integer", nullable: false),
                    PaymentInterval = table.Column<int>(type: "integer", nullable: true),
                    HasUpgraded = table.Column<bool>(type: "boolean", nullable: false),
                    StripeCustomerId = table.Column<string>(type: "text", nullable: false),
                    CurrentPeriodEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IntroIntentId = table.Column<string>(type: "text", nullable: false),
                    ShowSeenEnquiries = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryNestedThresholdTableRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    IntentId = table.Column<string>(type: "text", nullable: false),
                    TableId = table.Column<string>(type: "text", nullable: false),
                    ValueMin = table.Column<double>(type: "double precision", nullable: false),
                    ValueMax = table.Column<double>(type: "double precision", nullable: false),
                    Range = table.Column<bool>(type: "boolean", nullable: false),
                    RowId = table.Column<string>(type: "text", nullable: false),
                    RowOrder = table.Column<int>(type: "integer", nullable: false),
                    ItemId = table.Column<string>(type: "text", nullable: false),
                    ItemOrder = table.Column<int>(type: "integer", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Threshold = table.Column<double>(type: "double precision", nullable: false),
                    TriggerFallback = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryNestedThresholdTableRows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversationHistoryRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConversationId = table.Column<string>(type: "text", nullable: false),
                    Prompt = table.Column<string>(type: "text", nullable: false),
                    UserResponse = table.Column<string>(type: "text", nullable: false),
                    NodeId = table.Column<string>(type: "text", nullable: false),
                    NodeCritical = table.Column<bool>(type: "boolean", nullable: false),
                    NodeType = table.Column<string>(type: "text", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccountId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationHistoryRows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversationRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConversationId = table.Column<string>(type: "text", nullable: false),
                    ResponsePdfId = table.Column<string>(type: "text", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    IntentName = table.Column<string>(type: "text", nullable: false),
                    EmailTemplateUsed = table.Column<string>(type: "text", nullable: false),
                    Seen = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    IntentId = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsFallback = table.Column<bool>(type: "boolean", nullable: false),
                    Locale = table.Column<string>(type: "text", nullable: false),
                    IsComplete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailVerifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthenticationToken = table.Column<string>(type: "text", nullable: false),
                    EmailAddress = table.Column<string>(type: "text", nullable: false),
                    AccountId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    FileId = table.Column<string>(type: "text", nullable: false),
                    RiskyNameStem = table.Column<string>(type: "text", nullable: false),
                    Extension = table.Column<string>(type: "text", nullable: false),
                    LocationKey = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAssets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Intents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IntentId = table.Column<string>(type: "text", nullable: false),
                    IntentName = table.Column<string>(type: "text", nullable: false),
                    Prologue = table.Column<string>(type: "text", nullable: false),
                    Epilogue = table.Column<string>(type: "text", nullable: false),
                    EmailTemplate = table.Column<string>(type: "text", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    IntentSpecificEmail = table.Column<string>(type: "text", nullable: false),
                    EmailIsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    UseIntentFallbackEmail = table.Column<bool>(type: "boolean", nullable: false),
                    FallbackSubject = table.Column<string>(type: "text", nullable: false),
                    FallbackEmailTemplate = table.Column<string>(type: "text", nullable: false),
                    SendAttachmentsOnFallback = table.Column<bool>(type: "boolean", nullable: false),
                    SendPdfResponse = table.Column<bool>(type: "boolean", nullable: false),
                    IncludePricingStrategyTableTotals = table.Column<bool>(type: "boolean", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    AccountLogoFileId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PercentOfThresholds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    IntentId = table.Column<string>(type: "text", nullable: false),
                    TableId = table.Column<string>(type: "text", nullable: false),
                    RowId = table.Column<string>(type: "text", nullable: false),
                    Threshold = table.Column<double>(type: "double precision", nullable: false),
                    ValueMin = table.Column<double>(type: "double precision", nullable: false),
                    ValueMax = table.Column<double>(type: "double precision", nullable: false),
                    Range = table.Column<bool>(type: "boolean", nullable: false),
                    Modifier = table.Column<double>(type: "double precision", nullable: false),
                    PosNeg = table.Column<bool>(type: "boolean", nullable: false),
                    RowOrder = table.Column<int>(type: "integer", nullable: false),
                    TriggerFallback = table.Column<bool>(type: "boolean", nullable: false),
                    ItemOrder = table.Column<int>(type: "integer", nullable: false),
                    ItemId = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PercentOfThresholds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SessionId = table.Column<string>(type: "text", nullable: false),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    ApiKey = table.Column<string>(type: "text", nullable: false),
                    Expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimpleSelectTableRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    IntentId = table.Column<string>(type: "text", nullable: false),
                    TableId = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    ValueMin = table.Column<double>(type: "double precision", nullable: false),
                    ValueMax = table.Column<double>(type: "double precision", nullable: false),
                    Range = table.Column<bool>(type: "boolean", nullable: false),
                    RowOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleSelectTableRows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimpleThresholdTableRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    IntentId = table.Column<string>(type: "text", nullable: false),
                    TableId = table.Column<string>(type: "text", nullable: false),
                    RowId = table.Column<string>(type: "text", nullable: false),
                    Threshold = table.Column<double>(type: "double precision", nullable: false),
                    ValueMin = table.Column<double>(type: "double precision", nullable: false),
                    ValueMax = table.Column<double>(type: "double precision", nullable: false),
                    Range = table.Column<bool>(type: "boolean", nullable: false),
                    ItemName = table.Column<string>(type: "text", nullable: false),
                    RowOrder = table.Column<int>(type: "integer", nullable: false),
                    TriggerFallback = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleThresholdTableRows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticFees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Min = table.Column<double>(type: "double precision", nullable: false),
                    Max = table.Column<double>(type: "double precision", nullable: false),
                    FeeId = table.Column<string>(type: "text", nullable: false),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    IntentId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StripeWebhookReceivedRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecordId = table.Column<string>(type: "text", nullable: false),
                    PayloadSignature = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StripeWebhookReceivedRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    ApiKey = table.Column<string>(type: "text", nullable: false),
                    NumIntents = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwoNestedSelectTableRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    IntentId = table.Column<string>(type: "text", nullable: false),
                    TableId = table.Column<string>(type: "text", nullable: false),
                    ValueMin = table.Column<double>(type: "double precision", nullable: false),
                    ValueMax = table.Column<double>(type: "double precision", nullable: false),
                    Range = table.Column<bool>(type: "boolean", nullable: false),
                    RowId = table.Column<string>(type: "text", nullable: false),
                    RowOrder = table.Column<int>(type: "integer", nullable: false),
                    ItemId = table.Column<string>(type: "text", nullable: false),
                    ItemOrder = table.Column<int>(type: "integer", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    InnerItemName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwoNestedSelectTableRows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WidgetPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Placeholder = table.Column<string>(type: "text", nullable: false),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    LandingHeader = table.Column<string>(type: "text", nullable: false),
                    ChatHeader = table.Column<string>(type: "text", nullable: false),
                    SelectListColor = table.Column<string>(type: "text", nullable: false),
                    ListFontColor = table.Column<string>(type: "text", nullable: false),
                    HeaderColor = table.Column<string>(type: "text", nullable: false),
                    HeaderFontColor = table.Column<string>(type: "text", nullable: false),
                    FontFamily = table.Column<string>(type: "text", nullable: false),
                    OptionsHeaderColor = table.Column<string>(type: "text", nullable: false),
                    OptionsHeaderFontColor = table.Column<string>(type: "text", nullable: false),
                    ChatFontColor = table.Column<string>(type: "text", nullable: false),
                    ChatBubbleColor = table.Column<string>(type: "text", nullable: false),
                    ButtonColor = table.Column<string>(type: "text", nullable: false),
                    ButtonFontColor = table.Column<string>(type: "text", nullable: false),
                    WidgetState = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WidgetPreferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    IntentId = table.Column<string>(type: "text", nullable: false),
                    FileId = table.Column<string>(type: "text", nullable: false),
                    IntentId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachmentRecords_Intents_IntentId1",
                        column: x => x.IntentId1,
                        principalTable: "Intents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ConversationNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IntentId = table.Column<string>(type: "text", nullable: false),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    NodeId = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    IsRoot = table.Column<bool>(type: "boolean", nullable: false),
                    IsCritical = table.Column<bool>(type: "boolean", nullable: false),
                    IsMultiOptionType = table.Column<bool>(type: "boolean", nullable: false),
                    IsTerminalType = table.Column<bool>(type: "boolean", nullable: false),
                    ShouldRenderChildren = table.Column<bool>(type: "boolean", nullable: false),
                    IsLoopbackAnchorType = table.Column<bool>(type: "boolean", nullable: false),
                    IsAnabranchType = table.Column<bool>(type: "boolean", nullable: false),
                    IsAnabranchMergePoint = table.Column<bool>(type: "boolean", nullable: false),
                    ShouldShowMultiOption = table.Column<bool>(type: "boolean", nullable: false),
                    IsPricingStrategyTableNode = table.Column<bool>(type: "boolean", nullable: false),
                    IsMultiOptionEditable = table.Column<bool>(type: "boolean", nullable: false),
                    IsImageNode = table.Column<bool>(type: "boolean", nullable: false),
                    FileId = table.Column<string>(type: "text", nullable: false),
                    OptionPath = table.Column<string>(type: "text", nullable: false),
                    ValueOptions = table.Column<string>(type: "text", nullable: false),
                    NodeType = table.Column<string>(type: "text", nullable: false),
                    PricingStrategyType = table.Column<string>(type: "text", nullable: false),
                    NodeComponentType = table.Column<string>(type: "text", nullable: false),
                    ResolveOrder = table.Column<int>(type: "integer", nullable: false),
                    IsCurrency = table.Column<bool>(type: "boolean", nullable: false),
                    NodeChildrenString = table.Column<string>(type: "text", nullable: false),
                    NodeTypeCodeEnum = table.Column<int>(type: "integer", nullable: false),
                    IntentId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversationNodes_Intents_IntentId1",
                        column: x => x.IntentId1,
                        principalTable: "Intents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PricingStrategyTableMetas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TableTag = table.Column<string>(type: "text", nullable: false),
                    PrettyName = table.Column<string>(type: "text", nullable: false),
                    TableType = table.Column<string>(type: "text", nullable: false),
                    TableId = table.Column<string>(type: "text", nullable: false),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    IntentId = table.Column<string>(type: "text", nullable: false),
                    ValuesAsPaths = table.Column<bool>(type: "boolean", nullable: false),
                    UseTableTagAsResponseDescription = table.Column<bool>(type: "boolean", nullable: false),
                    UnitIdEnum = table.Column<int>(type: "integer", nullable: false),
                    IntentId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingStrategyTableMetas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PricingStrategyTableMetas_Intents_IntentId1",
                        column: x => x.IntentId1,
                        principalTable: "Intents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StaticTablesMetas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TableOrder = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IntentId = table.Column<string>(type: "text", nullable: false),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    PerPersonInputRequired = table.Column<bool>(type: "boolean", nullable: false),
                    IncludeTotals = table.Column<bool>(type: "boolean", nullable: false),
                    TableId = table.Column<string>(type: "text", nullable: false),
                    IntentId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticTablesMetas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaticTablesMetas_Intents_IntentId1",
                        column: x => x.IntentId1,
                        principalTable: "Intents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StaticTablesRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RowOrder = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    FeeId = table.Column<int>(type: "integer", nullable: false),
                    Range = table.Column<bool>(type: "boolean", nullable: false),
                    PerPerson = table.Column<bool>(type: "boolean", nullable: false),
                    TableOrder = table.Column<int>(type: "integer", nullable: false),
                    IntentId = table.Column<string>(type: "text", nullable: false),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    StaticTablesMetaId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticTablesRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaticTablesRows_StaticFees_FeeId",
                        column: x => x.FeeId,
                        principalTable: "StaticFees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaticTablesRows_StaticTablesMetas_StaticTablesMetaId",
                        column: x => x.StaticTablesMetaId,
                        principalTable: "StaticTablesMetas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentRecords_IntentId1",
                table: "AttachmentRecords",
                column: "IntentId1");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationNodes_IntentId1",
                table: "ConversationNodes",
                column: "IntentId1");

            migrationBuilder.CreateIndex(
                name: "IX_PricingStrategyTableMetas_IntentId1",
                table: "PricingStrategyTableMetas",
                column: "IntentId1");

            migrationBuilder.CreateIndex(
                name: "IX_StaticTablesMetas_IntentId1",
                table: "StaticTablesMetas",
                column: "IntentId1");

            migrationBuilder.CreateIndex(
                name: "IX_StaticTablesRows_FeeId",
                table: "StaticTablesRows",
                column: "FeeId");

            migrationBuilder.CreateIndex(
                name: "IX_StaticTablesRows_StaticTablesMetaId",
                table: "StaticTablesRows",
                column: "StaticTablesMetaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AttachmentRecords");

            migrationBuilder.DropTable(
                name: "CategoryNestedThresholdTableRows");

            migrationBuilder.DropTable(
                name: "ConversationHistoryRows");

            migrationBuilder.DropTable(
                name: "ConversationNodes");

            migrationBuilder.DropTable(
                name: "ConversationRecords");

            migrationBuilder.DropTable(
                name: "EmailVerifications");

            migrationBuilder.DropTable(
                name: "FileAssets");

            migrationBuilder.DropTable(
                name: "Logos");

            migrationBuilder.DropTable(
                name: "PercentOfThresholds");

            migrationBuilder.DropTable(
                name: "PricingStrategyTableMetas");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "SimpleSelectTableRows");

            migrationBuilder.DropTable(
                name: "SimpleThresholdTableRows");

            migrationBuilder.DropTable(
                name: "StaticTablesRows");

            migrationBuilder.DropTable(
                name: "StripeWebhookReceivedRecords");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "TwoNestedSelectTableRows");

            migrationBuilder.DropTable(
                name: "WidgetPreferences");

            migrationBuilder.DropTable(
                name: "StaticFees");

            migrationBuilder.DropTable(
                name: "StaticTablesMetas");

            migrationBuilder.DropTable(
                name: "Intents");
        }
    }
}
