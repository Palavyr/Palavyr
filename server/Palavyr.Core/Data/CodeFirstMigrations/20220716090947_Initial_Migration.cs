using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Palavyr.Core.Data.CodeFirstMigrations
{
    public partial class Initial_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Password = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    DefaultEmailIsVerified = table.Column<bool>(nullable: false),
                    AccountId = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    AccountLogoUri = table.Column<string>(nullable: true),
                    GeneralFallbackSubject = table.Column<string>(nullable: true),
                    GeneralFallbackEmailTemplate = table.Column<string>(nullable: true),
                    ApiKey = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Locale = table.Column<string>(nullable: true),
                    AccountType = table.Column<int>(nullable: false),
                    PlanType = table.Column<int>(nullable: false),
                    PaymentInterval = table.Column<int>(nullable: true),
                    HasUpgraded = table.Column<bool>(nullable: false),
                    StripeCustomerId = table.Column<string>(nullable: true),
                    CurrentPeriodEnd = table.Column<DateTime>(nullable: false),
                    IntroductionId = table.Column<string>(nullable: true),
                    ShowSeenEnquiries = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryNestedThresholdTableRows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(nullable: true),
                    IntentId = table.Column<string>(nullable: true),
                    TableId = table.Column<string>(nullable: true),
                    ValueMin = table.Column<double>(nullable: false),
                    ValueMax = table.Column<double>(nullable: false),
                    Range = table.Column<bool>(nullable: false),
                    RowId = table.Column<string>(nullable: true),
                    RowOrder = table.Column<int>(nullable: false),
                    ItemId = table.Column<string>(nullable: true),
                    ItemOrder = table.Column<int>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    Threshold = table.Column<double>(nullable: false),
                    TriggerFallback = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryNestedThresholdTableRows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversationHistoryRows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConversationId = table.Column<string>(nullable: true),
                    Prompt = table.Column<string>(nullable: true),
                    UserResponse = table.Column<string>(nullable: true),
                    NodeId = table.Column<string>(nullable: true),
                    NodeCritical = table.Column<bool>(nullable: false),
                    NodeType = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    AccountId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationHistoryRows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversationRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConversationId = table.Column<string>(nullable: true),
                    ResponsePdfId = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    AccountId = table.Column<string>(nullable: true),
                    IntentName = table.Column<string>(nullable: true),
                    EmailTemplateUsed = table.Column<string>(nullable: true),
                    Seen = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    IntentId = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsFallback = table.Column<bool>(nullable: false),
                    Locale = table.Column<string>(nullable: true),
                    IsComplete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailVerifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthenticationToken = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileAssets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(nullable: true),
                    FileId = table.Column<string>(nullable: true),
                    RiskyNameStem = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    LocationKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAssets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Intents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AreaIdentifier = table.Column<string>(nullable: true),
                    AreaName = table.Column<string>(nullable: true),
                    AreaDisplayTitle = table.Column<string>(nullable: true),
                    Prologue = table.Column<string>(nullable: true),
                    Epilogue = table.Column<string>(nullable: true),
                    EmailTemplate = table.Column<string>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false),
                    AccountId = table.Column<string>(nullable: true),
                    AreaSpecificEmail = table.Column<string>(nullable: true),
                    EmailIsVerified = table.Column<bool>(nullable: false),
                    UseAreaFallbackEmail = table.Column<bool>(nullable: false),
                    FallbackSubject = table.Column<string>(nullable: true),
                    FallbackEmailTemplate = table.Column<string>(nullable: true),
                    SendAttachmentsOnFallback = table.Column<bool>(nullable: false),
                    SendPdfResponse = table.Column<bool>(nullable: false),
                    IncludeDynamicTableTotals = table.Column<bool>(nullable: false),
                    Subject = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(nullable: true),
                    AccountLogoFileId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PercentOfThresholds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(nullable: true),
                    IntentId = table.Column<string>(nullable: true),
                    TableId = table.Column<string>(nullable: true),
                    RowId = table.Column<string>(nullable: true),
                    Threshold = table.Column<double>(nullable: false),
                    ValueMin = table.Column<double>(nullable: false),
                    ValueMax = table.Column<double>(nullable: false),
                    Range = table.Column<bool>(nullable: false),
                    Modifier = table.Column<double>(nullable: false),
                    PosNeg = table.Column<bool>(nullable: false),
                    RowOrder = table.Column<int>(nullable: false),
                    TriggerFallback = table.Column<bool>(nullable: false),
                    ItemOrder = table.Column<int>(nullable: false),
                    ItemId = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PercentOfThresholds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SessionId = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true),
                    ApiKey = table.Column<string>(nullable: true),
                    Expiration = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimpleSelectTableRows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(nullable: true),
                    IntentId = table.Column<string>(nullable: true),
                    TableId = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    ValueMin = table.Column<double>(nullable: false),
                    ValueMax = table.Column<double>(nullable: false),
                    Range = table.Column<bool>(nullable: false),
                    RowOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleSelectTableRows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimpleThresholdTableRows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(nullable: true),
                    IntentId = table.Column<string>(nullable: true),
                    TableId = table.Column<string>(nullable: true),
                    RowId = table.Column<string>(nullable: true),
                    Threshold = table.Column<double>(nullable: false),
                    ValueMin = table.Column<double>(nullable: false),
                    ValueMax = table.Column<double>(nullable: false),
                    Range = table.Column<bool>(nullable: false),
                    ItemName = table.Column<string>(nullable: true),
                    RowOrder = table.Column<int>(nullable: false),
                    TriggerFallback = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleThresholdTableRows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticFees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Min = table.Column<double>(nullable: false),
                    Max = table.Column<double>(nullable: false),
                    FeeId = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true),
                    IntentId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StripeWebhookReceivedRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecordId = table.Column<string>(nullable: true),
                    PayloadSignature = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StripeWebhookReceivedRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(nullable: true),
                    ApiKey = table.Column<string>(nullable: true),
                    NumAreas = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwoNestedSelectTableRows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(nullable: true),
                    IntentId = table.Column<string>(nullable: true),
                    TableId = table.Column<string>(nullable: true),
                    ValueMin = table.Column<double>(nullable: false),
                    ValueMax = table.Column<double>(nullable: false),
                    Range = table.Column<bool>(nullable: false),
                    RowId = table.Column<string>(nullable: true),
                    RowOrder = table.Column<int>(nullable: false),
                    ItemId = table.Column<string>(nullable: true),
                    ItemOrder = table.Column<int>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    InnerItemName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwoNestedSelectTableRows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WidgetPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Placeholder = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true),
                    LandingHeader = table.Column<string>(nullable: true),
                    ChatHeader = table.Column<string>(nullable: true),
                    SelectListColor = table.Column<string>(nullable: true),
                    ListFontColor = table.Column<string>(nullable: true),
                    HeaderColor = table.Column<string>(nullable: true),
                    HeaderFontColor = table.Column<string>(nullable: true),
                    FontFamily = table.Column<string>(nullable: true),
                    OptionsHeaderColor = table.Column<string>(nullable: true),
                    OptionsHeaderFontColor = table.Column<string>(nullable: true),
                    ChatFontColor = table.Column<string>(nullable: true),
                    ChatBubbleColor = table.Column<string>(nullable: true),
                    ButtonColor = table.Column<string>(nullable: true),
                    ButtonFontColor = table.Column<string>(nullable: true),
                    WidgetState = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WidgetPreferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(nullable: true),
                    IntentId = table.Column<string>(nullable: true),
                    FileId = table.Column<string>(nullable: true),
                    IntentId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachmentRecords_Intents_IntentId1",
                        column: x => x.IntentId1,
                        principalTable: "Intents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConversationNodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IntentId = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true),
                    NodeId = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    IsRoot = table.Column<bool>(nullable: false),
                    IsCritical = table.Column<bool>(nullable: false),
                    IsMultiOptionType = table.Column<bool>(nullable: false),
                    IsTerminalType = table.Column<bool>(nullable: false),
                    ShouldRenderChildren = table.Column<bool>(nullable: false),
                    IsLoopbackAnchorType = table.Column<bool>(nullable: false),
                    IsAnabranchType = table.Column<bool>(nullable: false),
                    IsAnabranchMergePoint = table.Column<bool>(nullable: false),
                    ShouldShowMultiOption = table.Column<bool>(nullable: false),
                    IsDynamicTableNode = table.Column<bool>(nullable: false),
                    IsMultiOptionEditable = table.Column<bool>(nullable: false),
                    IsImageNode = table.Column<bool>(nullable: false),
                    ImageId = table.Column<string>(nullable: true),
                    OptionPath = table.Column<string>(nullable: true),
                    ValueOptions = table.Column<string>(nullable: true),
                    NodeType = table.Column<string>(nullable: true),
                    DynamicType = table.Column<string>(nullable: true),
                    NodeComponentType = table.Column<string>(nullable: true),
                    ResolveOrder = table.Column<int>(nullable: true),
                    IsCurrency = table.Column<bool>(nullable: false),
                    NodeChildrenString = table.Column<string>(nullable: true),
                    NodeTypeCode = table.Column<int>(nullable: false),
                    IntentId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversationNodes_Intents_IntentId1",
                        column: x => x.IntentId1,
                        principalTable: "Intents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PricingStrategyTableMetas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TableTag = table.Column<string>(nullable: true),
                    PrettyName = table.Column<string>(nullable: true),
                    TableType = table.Column<string>(nullable: true),
                    TableId = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true),
                    IntentId = table.Column<string>(nullable: true),
                    ValuesAsPaths = table.Column<bool>(nullable: false),
                    UseTableTagAsResponseDescription = table.Column<bool>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    IntentId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingStrategyTableMetas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PricingStrategyTableMetas_Intents_IntentId1",
                        column: x => x.IntentId1,
                        principalTable: "Intents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaticTablesMetas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TableOrder = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IntentId = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true),
                    PerPersonInputRequired = table.Column<bool>(nullable: false),
                    IncludeTotals = table.Column<bool>(nullable: false),
                    IntentId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticTablesMetas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaticTablesMetas_Intents_IntentId1",
                        column: x => x.IntentId1,
                        principalTable: "Intents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaticTablesRows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RowOrder = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FeeId = table.Column<int>(nullable: true),
                    Range = table.Column<bool>(nullable: false),
                    PerPerson = table.Column<bool>(nullable: false),
                    TableOrder = table.Column<int>(nullable: false),
                    IntentId = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true),
                    StaticTablesMetaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticTablesRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaticTablesRows_StaticFees_FeeId",
                        column: x => x.FeeId,
                        principalTable: "StaticFees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaticTablesRows_StaticTablesMetas_StaticTablesMetaId",
                        column: x => x.StaticTablesMetaId,
                        principalTable: "StaticTablesMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
