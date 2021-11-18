using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Palavyr.Core.Data.Migrations.ConfigurationMigrations
{
    public partial class initial_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
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
                    Subject = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BasicThresholds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(nullable: true),
                    AreaIdentifier = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_BasicThresholds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryNestedThresholds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(nullable: true),
                    AreaIdentifier = table.Column<string>(nullable: true),
                    TableId = table.Column<string>(nullable: true),
                    ValueMin = table.Column<double>(nullable: false),
                    ValueMax = table.Column<double>(nullable: false),
                    Range = table.Column<bool>(nullable: false),
                    RowId = table.Column<string>(nullable: true),
                    RowOrder = table.Column<int>(nullable: false),
                    ItemId = table.Column<string>(nullable: true),
                    ItemOrder = table.Column<int>(nullable: false),
                    ItemName = table.Column<string>(nullable: true),
                    Threshold = table.Column<double>(nullable: false),
                    TriggerFallback = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryNestedThresholds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileNameMaps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SafeName = table.Column<string>(nullable: true),
                    S3Key = table.Column<string>(nullable: true),
                    RiskyName = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true),
                    AreaIdentifier = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileNameMaps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImageId = table.Column<string>(nullable: false),
                    SafeName = table.Column<string>(nullable: true),
                    RiskyName = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: false),
                    S3Key = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    IsUrl = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PercentOfThresholds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(nullable: true),
                    AreaIdentifier = table.Column<string>(nullable: true),
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
                    ItemName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PercentOfThresholds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SelectOneFlats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(nullable: true),
                    AreaIdentifier = table.Column<string>(nullable: true),
                    TableId = table.Column<string>(nullable: true),
                    Option = table.Column<string>(nullable: true),
                    ValueMin = table.Column<double>(nullable: false),
                    ValueMax = table.Column<double>(nullable: false),
                    Range = table.Column<bool>(nullable: false),
                    RowOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectOneFlats", x => x.Id);
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
                    AreaIdentifier = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwoNestedCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(nullable: true),
                    AreaIdentifier = table.Column<string>(nullable: true),
                    TableId = table.Column<string>(nullable: true),
                    ValueMin = table.Column<double>(nullable: false),
                    ValueMax = table.Column<double>(nullable: false),
                    Range = table.Column<bool>(nullable: false),
                    RowId = table.Column<string>(nullable: true),
                    RowOrder = table.Column<int>(nullable: false),
                    ItemId = table.Column<string>(nullable: true),
                    ItemOrder = table.Column<int>(nullable: false),
                    ItemName = table.Column<string>(nullable: true),
                    InnerItemName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwoNestedCategories", x => x.Id);
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
                name: "ConversationNodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AreaIdentifier = table.Column<string>(nullable: true),
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
                    AreaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversationNodes_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DynamicTableMetas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TableTag = table.Column<string>(nullable: true),
                    PrettyName = table.Column<string>(nullable: true),
                    TableType = table.Column<string>(nullable: true),
                    TableId = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true),
                    AreaIdentifier = table.Column<string>(nullable: true),
                    ValuesAsPaths = table.Column<bool>(nullable: false),
                    UseTableTagAsResponseDescription = table.Column<bool>(nullable: false),
                    AreaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicTableMetas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DynamicTableMetas_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
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
                    AreaIdentifier = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true),
                    PerPersonInputRequired = table.Column<bool>(nullable: false),
                    AreaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticTablesMetas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaticTablesMetas_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
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
                    AreaIdentifier = table.Column<string>(nullable: true),
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
                name: "IX_ConversationNodes_AreaId",
                table: "ConversationNodes",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicTableMetas_AreaId",
                table: "DynamicTableMetas",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_StaticTablesMetas_AreaId",
                table: "StaticTablesMetas",
                column: "AreaId");

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
                name: "BasicThresholds");

            migrationBuilder.DropTable(
                name: "CategoryNestedThresholds");

            migrationBuilder.DropTable(
                name: "ConversationNodes");

            migrationBuilder.DropTable(
                name: "DynamicTableMetas");

            migrationBuilder.DropTable(
                name: "FileNameMaps");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "PercentOfThresholds");

            migrationBuilder.DropTable(
                name: "SelectOneFlats");

            migrationBuilder.DropTable(
                name: "StaticTablesRows");

            migrationBuilder.DropTable(
                name: "TwoNestedCategories");

            migrationBuilder.DropTable(
                name: "WidgetPreferences");

            migrationBuilder.DropTable(
                name: "StaticFees");

            migrationBuilder.DropTable(
                name: "StaticTablesMetas");

            migrationBuilder.DropTable(
                name: "Areas");
        }
    }
}
