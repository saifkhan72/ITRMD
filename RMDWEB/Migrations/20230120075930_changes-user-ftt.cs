using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMDWEB.Migrations
{
    /// <inheritdoc />
    public partial class changesuserftt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_DepartmentTbl_DepartmentId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_StatusTbl_StatusId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropTable(
                name: "FTTComment",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FTTDocumentfile",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FTTTransaction",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CurrencyTbl",
                schema: "dbo");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                schema: "dbo",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                schema: "dbo",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankId",
                schema: "dbo",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_BankId",
                schema: "dbo",
                table: "User",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_BankTbl_BankId",
                schema: "dbo",
                table: "User",
                column: "BankId",
                principalSchema: "dbo",
                principalTable: "BankTbl",
                principalColumn: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_DepartmentTbl_DepartmentId",
                schema: "dbo",
                table: "User",
                column: "DepartmentId",
                principalSchema: "dbo",
                principalTable: "DepartmentTbl",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_StatusTbl_StatusId",
                schema: "dbo",
                table: "User",
                column: "StatusId",
                principalSchema: "dbo",
                principalTable: "StatusTbl",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_BankTbl_BankId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_DepartmentTbl_DepartmentId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_StatusTbl_StatusId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_BankId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "BankId",
                schema: "dbo",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                schema: "dbo",
                table: "User",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                schema: "dbo",
                table: "User",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "CurrencyTbl",
                schema: "dbo",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CurCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurNameDa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurSign = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyTbl", x => x.CurrencyId);
                    table.ForeignKey(
                        name: "FK_CurrencyTbl_StatusTbl_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "dbo",
                        principalTable: "StatusTbl",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FTTComment",
                schema: "dbo",
                columns: table => new
                {
                    fttcommentid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TID = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FTTComment", x => x.fttcommentid);
                    table.ForeignKey(
                        name: "FK_FTTComment_StatusTbl_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "dbo",
                        principalTable: "StatusTbl",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FTTDocumentfile",
                schema: "dbo",
                columns: table => new
                {
                    DocId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    DocImage = table.Column<byte[]>(type: "image", nullable: true),
                    DocImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FTTDocumentfile", x => x.DocId);
                    table.ForeignKey(
                        name: "FK_FTTDocumentfile_StatusTbl_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "dbo",
                        principalTable: "StatusTbl",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FTTTransaction",
                schema: "dbo",
                columns: table => new
                {
                    TID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    BankLetterId = table.Column<int>(type: "int", nullable: false),
                    BenBank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BenCompany = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BenCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    InvoiceContractNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurposeTransaction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TTAmount = table.Column<float>(type: "real", nullable: false),
                    TTDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TTNumber = table.Column<int>(type: "int", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FTTTransaction", x => x.TID);
                    table.ForeignKey(
                        name: "FK_FTTTransaction_CurrencyTbl_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "dbo",
                        principalTable: "CurrencyTbl",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FTTTransaction_DepartmentTbl_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "dbo",
                        principalTable: "DepartmentTbl",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FTTTransaction_StatusTbl_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "dbo",
                        principalTable: "StatusTbl",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTbl_StatusId",
                schema: "dbo",
                table: "CurrencyTbl",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FTTComment_StatusId",
                schema: "dbo",
                table: "FTTComment",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FTTDocumentfile_StatusId",
                schema: "dbo",
                table: "FTTDocumentfile",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FTTTransaction_CurrencyId",
                schema: "dbo",
                table: "FTTTransaction",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_FTTTransaction_DepartmentId",
                schema: "dbo",
                table: "FTTTransaction",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FTTTransaction_StatusId",
                schema: "dbo",
                table: "FTTTransaction",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_DepartmentTbl_DepartmentId",
                schema: "dbo",
                table: "User",
                column: "DepartmentId",
                principalSchema: "dbo",
                principalTable: "DepartmentTbl",
                principalColumn: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_StatusTbl_StatusId",
                schema: "dbo",
                table: "User",
                column: "StatusId",
                principalSchema: "dbo",
                principalTable: "StatusTbl",
                principalColumn: "StatusId");
        }
    }
}
