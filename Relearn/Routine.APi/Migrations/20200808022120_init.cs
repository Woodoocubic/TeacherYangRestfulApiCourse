using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Routine.APi.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Introduction = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    EmployeeNo = table.Column<string>(maxLength: 10, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("c0e917f9-1640-4ffd-8dfe-d3946913a180"), "Great Company", "Microsoft" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("543d0978-084e-43f5-82f8-6417ffce0c0c"), "Don't be evil", "Google" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("50817877-f6f8-4161-9926-20817ac3bd08"), "fubao Company", "Alipapa" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[,]
                {
                    { new Guid("ca268a19-0f39-4d8b-b8d6-5bace54f8027"), new Guid("c0e917f9-1640-4ffd-8dfe-d3946913a180"), new DateTime(1955, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "M001", "William", 1, "Gates" },
                    { new Guid("265348d2-1276-4ada-ae33-4c1b8348edce"), new Guid("c0e917f9-1640-4ffd-8dfe-d3946913a180"), new DateTime(1998, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "M024", "Kent", 1, "Back" },
                    { new Guid("47b70abc-98b8-4fdc-b9fa-5dd6716f6e6b"), new Guid("543d0978-084e-43f5-82f8-6417ffce0c0c"), new DateTime(1986, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "G003", "Mary", 0, "King" },
                    { new Guid("059e2fcb-e5a4-4188-9b46-06184bcb111b"), new Guid("543d0978-084e-43f5-82f8-6417ffce0c0c"), new DateTime(1977, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "G007", "Kevin", 1, "Richardson" },
                    { new Guid("a868ff18-3398-4598-b420-4878974a517a"), new Guid("50817877-f6f8-4161-9926-20817ac3bd08"), new DateTime(1964, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "A001", "Jack", 1, "Ma" },
                    { new Guid("2c3bb40c-5907-4eb7-bb2c-7d62edb430c9"), new Guid("50817877-f6f8-4161-9926-20817ac3bd08"), new DateTime(1997, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "A201", "Lorraine", 0, "Shaw" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
