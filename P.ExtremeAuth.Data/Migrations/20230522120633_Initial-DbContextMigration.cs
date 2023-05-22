using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P.ExtremeAuth.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDbContextMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authorization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeCode = table.Column<int>(type: "int", nullable: false),
                    ConditionOperator = table.Column<int>(type: "int", nullable: false),
                    ConditionValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mock = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureDefinition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureDefinition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizationOf",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationOf", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorizationOf_Authorization_AuthorizationId",
                        column: x => x.AuthorizationId,
                        principalTable: "Authorization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Procedure",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Runtime = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    ProcedureDefinitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedure", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Procedure_Authorization_AuthorizationId",
                        column: x => x.AuthorizationId,
                        principalTable: "Authorization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Procedure_ProcedureDefinition_ProcedureDefinitionId",
                        column: x => x.ProcedureDefinitionId,
                        principalTable: "ProcedureDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizationTo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorizationOfId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StateValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationTo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorizationTo_AuthorizationOf_AuthorizationOfId",
                        column: x => x.AuthorizationOfId,
                        principalTable: "AuthorizationOf",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationOf_AuthorizationId",
                table: "AuthorizationOf",
                column: "AuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationTo_AuthorizationOfId",
                table: "AuthorizationTo",
                column: "AuthorizationOfId");

            migrationBuilder.CreateIndex(
                name: "IX_Procedure_AuthorizationId",
                table: "Procedure",
                column: "AuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Procedure_ProcedureDefinitionId",
                table: "Procedure",
                column: "ProcedureDefinitionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorizationTo");

            migrationBuilder.DropTable(
                name: "Procedure");

            migrationBuilder.DropTable(
                name: "AuthorizationOf");

            migrationBuilder.DropTable(
                name: "ProcedureDefinition");

            migrationBuilder.DropTable(
                name: "Authorization");
        }
    }
}
