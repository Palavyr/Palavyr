using Microsoft.EntityFrameworkCore.Migrations;

namespace DashboardServer.Data.Migrations.Dash
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AreaIdentifier = table.Column<string>(nullable: true),
                    AreaName = table.Column<string>(nullable: true),
                    AreaDisplayTitle = table.Column<string>(nullable: true),
                    Prologue = table.Column<string>(nullable: true),
                    Epilogue = table.Column<string>(nullable: true),
                    EmailTemplate = table.Column<string>(nullable: true),
                    IsComplete = table.Column<bool>(nullable: false),
                    GroupId = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileNameMaps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SafeName = table.Column<string>(nullable: true),
                    RiskyName = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true),
                    AreaId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileNameMaps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GroupId = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true),
                    GroupName = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SelectOneFlats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<string>(nullable: true),
                    AreaId = table.Column<string>(nullable: true),
                    TableId = table.Column<string>(nullable: true),
                    Option = table.Column<string>(nullable: true),
                    ValueMin = table.Column<double>(nullable: false),
                    ValueMax = table.Column<double>(nullable: false),
                    Range = table.Column<bool>(nullable: false),
                    TableTag = table.Column<string>(nullable: true)
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
                        .Annotation("Sqlite:Autoincrement", true),
                    Min = table.Column<double>(nullable: false),
                    Max = table.Column<double>(nullable: false),
                    FeeId = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WidgetPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(nullable: true),
                    Subtitle = table.Column<string>(nullable: true),
                    Placeholder = table.Column<string>(nullable: true),
                    ShouldGroup = table.Column<bool>(nullable: false),
                    AccountId = table.Column<string>(nullable: true)
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
                        .Annotation("Sqlite:Autoincrement", true),
                    NodeId = table.Column<string>(nullable: true),
                    NodeType = table.Column<string>(nullable: true),
                    Fallback = table.Column<bool>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    IsRoot = table.Column<bool>(nullable: false),
                    AreaIdentifier = table.Column<string>(nullable: true),
                    OptionPath = table.Column<string>(nullable: true),
                    IsCritical = table.Column<bool>(nullable: false),
                    ValueOptions = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true),
                    NodeChildrenString = table.Column<string>(nullable: true),
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
                        .Annotation("Sqlite:Autoincrement", true),
                    TableTag = table.Column<string>(nullable: true),
                    PrettyName = table.Column<string>(nullable: true),
                    TableType = table.Column<string>(nullable: true),
                    TableId = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true),
                    AreaIdentifier = table.Column<string>(nullable: true),
                    ValuesAsPaths = table.Column<bool>(nullable: false),
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
                        .Annotation("Sqlite:Autoincrement", true),
                    TableOrder = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AreaIdentifier = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true),
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
                        .Annotation("Sqlite:Autoincrement", true),
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
                name: "ConversationNodes");

            migrationBuilder.DropTable(
                name: "DynamicTableMetas");

            migrationBuilder.DropTable(
                name: "FileNameMaps");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "SelectOneFlats");

            migrationBuilder.DropTable(
                name: "StaticTablesRows");

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
