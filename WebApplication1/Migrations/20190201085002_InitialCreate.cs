using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WebApplication1.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("pk_departments", x => x.id); });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    first_name = table.Column<string>(nullable: false),
                    lastname = table.Column<string>(nullable: false),
                    patronymic = table.Column<string>(nullable: true),
                    nick_name = table.Column<string>(nullable: false),
                    password = table.Column<string>(nullable: false),
                    email = table.Column<string>(nullable: false),
                    phone_number = table.Column<int>(nullable: false),
                    invited_at = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    department_id = table.Column<int>(nullable: false),
                    role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_departments_department_id",
                        column: x => x.department_id,
                        principalTable: "departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "salary_rate_requests",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    request_chain_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    suggested_rate = table.Column<int>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    reviewer_id = table.Column<int>(nullable: true),
                    sender_id = table.Column<int>(nullable: false),
                    reviewer_comment = table.Column<string>(nullable: true),
                    internal_comment = table.Column<string>(nullable: true),
                    reason = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_salary_rate_requests", x => x.id);
                    table.ForeignKey(
                        name: "fk_salary_rate_requests_users_reviewer_id",
                        column: x => x.reviewer_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_salary_rate_requests_users_sender_id",
                        column: x => x.sender_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "salary_rates",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    rate = table.Column<int>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_salary_rates", x => x.id);
                    table.ForeignKey(
                        name: "fk_salary_rates_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "departments",
                columns: new[] {"id", "name"},
                values: new object[,]
                {
                    {-2, "Frontend"},
                    {-1, "Backend"}
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[]
                {
                    "id", "department_id", "description", "email", "first_name", "invited_at", "lastname", "nick_name",
                    "password", "patronymic", "phone_number", "role"
                },
                values: new object[,]
                {
                    {
                        124, -2, null, "shitmail@ayndex.ru", "Dan",
                        new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tolstovku", "Tolstovku", "124",
                        null, 0, 0
                    },
                    {
                        123, -1, null, "tatata@ayndex.ru", "Vlad",
                        new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nimatora", "Nimatora", "123",
                        null, 0, 2
                    }
                });

            migrationBuilder.CreateIndex(
                name: "ix_salary_rate_requests_reviewer_id",
                table: "salary_rate_requests",
                column: "reviewer_id");

            migrationBuilder.CreateIndex(
                name: "ix_salary_rate_requests_sender_id",
                table: "salary_rate_requests",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "ix_salary_rates_user_id",
                table: "salary_rates",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_department_id",
                table: "users",
                column: "department_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "salary_rate_requests");

            migrationBuilder.DropTable(
                name: "salary_rates");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "departments");
        }
    }
}