using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WebApplication1.Migrations
{
    public partial class initial : Migration
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
                    password = table.Column<string>(nullable: false),
                    first_name = table.Column<string>(nullable: false),
                    lastname = table.Column<string>(nullable: false),
                    patronymic = table.Column<string>(nullable: true),
                    nick_name = table.Column<string>(nullable: false),
                    email = table.Column<string>(nullable: false),
                    phone_number = table.Column<string>(nullable: true),
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
                    salary_rate_request_status = table.Column<int>(nullable: false),
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
                columns: new[] { "id", "department_id", "description", "email", "first_name", "invited_at", "lastname", "nick_name", "password", "patronymic", "phone_number", "user_role_id" },
                values: new object[,]
                {
                    { 124, -2, null, "shitmail@ayndex.ru", "Dan", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tolstovku", "User", "AQAAAAEAACcQAAAAEEDtPOVG68PulY0K2yfG3i3ygnuNXS5pg6sO+WXLk8LbQ+xsqogz9EU/4i0c4NsE5A==", null, null, -2 },
                    { 123, -1, null, "tatata@ayndex.ru", "Vlad", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nimatora", "Admin", "AQAAAAEAACcQAAAAEM7zi4OZ7QGMrSspQwWNzdsGuHnqXGWAeX/5f+loswfRFKEbdsCMPUIAW1BZQzRLiA==", null, null, -1 },
                    { 125, -1, null, "managerEmail@ayndex.ru", "Someone", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Something", "Manager", "AQAAAAEAACcQAAAAEM7zi4OZ7QGMrSspQwWNzdsGuHnqXGWAeX/5f+loswfRFKEbdsCMPUIAW1BZQzRLiA==", null, null, -3 }
                });

            migrationBuilder.InsertData(
                table: "salary_rate_requests",
                columns: new[] { "id", "created_at", "internal_comment", "reason", "request_chain_id", "reviewer_comment", "reviewer_id", "salary_rate_request_status", "sender_id", "suggested_rate" },
                values: new object[,]
                {
                    { -1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Want money", -1, null, null, 0, 124, 1337 },
                    { -2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Want more money", -2, null, null, 0, 124, 1338 }
                });

            migrationBuilder.InsertData(
                table: "user_managers",
                columns: new[] { "user_id", "manager_id" },
                values: new object[] { 124, 125 });

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "id", "role_id", "user_id" },
                values: new object[,]
                {
                    { -2, 1, 124 },
                    { -1, 3, 123 },
                    { -3, 2, 125 }
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
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_nick_name",
                table: "users",
                column: "nick_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_phone_number",
                table: "users",
                column: "phone_number",
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
        }
    }
}
