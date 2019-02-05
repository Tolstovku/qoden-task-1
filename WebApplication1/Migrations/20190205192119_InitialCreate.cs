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
                constraints: table =>
                {
                    table.PrimaryKey("pk_departments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "passwords",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    hashed_password = table.Column<string>(nullable: true),
                    salt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_passwords", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

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
                    password_id = table.Column<int>(nullable: false),
                    email = table.Column<string>(nullable: false),
                    phone_number = table.Column<int>(nullable: false),
                    invited_at = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    department_id = table.Column<int>(nullable: false),
                    user_role_id = table.Column<int>(nullable: false)
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
                    table.ForeignKey(
                        name: "fk_users_passwords_password_id",
                        column: x => x.password_id,
                        principalTable: "passwords",
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

            migrationBuilder.CreateTable(
                name: "user_managers",
                columns: table => new
                {
                    user_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    manager_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_managers", x => new { x.user_id, x.manager_id });
                    table.ForeignKey(
                        name: "fk_user_managers_users_manager_id",
                        column: x => x.manager_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_managers_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    user_id = table.Column<int>(nullable: false),
                    role_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "departments",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { -2, "Frontend" },
                    { -1, "Backend" }
                });

            migrationBuilder.InsertData(
                table: "passwords",
                columns: new[] { "id", "hashed_password", "salt" },
                values: new object[,]
                {
                    { -1, "AQIDBAUGBwgJCgsMDQ4PEJIP0BNHVvT04i5Memxx0UF6TfpM", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 } },
                    { -2, "AQIDBAUGBwgJCgsMDQ4PEDimuL8ciFQ17DFGA0Km78YzrJbD", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 } }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "user" },
                    { 2, "manager" },
                    { 3, "admin" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "department_id", "description", "email", "first_name", "invited_at", "lastname", "nick_name", "password_id", "patronymic", "phone_number", "user_role_id" },
                values: new object[,]
                {
                    { 123, -1, null, "tatata@ayndex.ru", "Vlad", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nimatora", "Nimatora", -1, null, 0, -1 },
                    { 124, -2, null, "shitmail@ayndex.ru", "Dan", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tolstovku", "Tolstovku", -2, null, 0, -2 }
                });

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "id", "role_id", "user_id" },
                values: new object[,]
                {
                    { -1, 3, 123 },
                    { -2, 1, 124 }
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
                name: "ix_user_managers_manager_id",
                table: "user_managers",
                column: "manager_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_role_id",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_user_id",
                table: "user_roles",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_department_id",
                table: "users",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_password_id",
                table: "users",
                column: "password_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "salary_rate_requests");

            migrationBuilder.DropTable(
                name: "salary_rates");

            migrationBuilder.DropTable(
                name: "user_managers");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "passwords");
        }
    }
}
