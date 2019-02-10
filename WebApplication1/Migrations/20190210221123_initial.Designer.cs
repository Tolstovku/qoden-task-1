﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApplication1.src.Database;

namespace WebApplication1.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20190210221123_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("WebApplication1.Database.Entities.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_departments");

                    b.ToTable("departments");

                    b.HasData(
                        new
                        {
                            Id = -2,
                            Name = "Frontend"
                        },
                        new
                        {
                            Id = -1,
                            Name = "Backend"
                        });
                });

            modelBuilder.Entity("WebApplication1.Database.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.ToTable("roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "user"
                        },
                        new
                        {
                            Id = 2,
                            Name = "manager"
                        },
                        new
                        {
                            Id = 3,
                            Name = "admin"
                        });
                });

            modelBuilder.Entity("WebApplication1.Database.Entities.SalaryRate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasColumnName("description");

                    b.Property<int>("Rate")
                        .HasColumnName("rate");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnName("updated_at");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_salary_rates");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasName("ix_salary_rates_user_id");

                    b.ToTable("salary_rates");
                });

            modelBuilder.Entity("WebApplication1.Database.Entities.SalaryRateRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at");

                    b.Property<string>("InternalComment")
                        .HasColumnName("internal_comment");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnName("reason");

                    b.Property<int>("RequestChainId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("request_chain_id");

                    b.Property<string>("ReviewerComment")
                        .HasColumnName("reviewer_comment");

                    b.Property<int?>("ReviewerId")
                        .HasColumnName("reviewer_id");

                    b.Property<int>("SalaryRateRequestStatus")
                        .HasColumnName("salary_rate_request_status");

                    b.Property<int>("SenderId")
                        .HasColumnName("sender_id");

                    b.Property<int>("SuggestedRate")
                        .HasColumnName("suggested_rate");

                    b.HasKey("Id")
                        .HasName("pk_salary_rate_requests");

                    b.HasIndex("ReviewerId")
                        .HasName("ix_salary_rate_requests_reviewer_id");

                    b.HasIndex("SenderId")
                        .HasName("ix_salary_rate_requests_sender_id");

                    b.ToTable("salary_rate_requests");
                });

            modelBuilder.Entity("WebApplication1.Database.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("DepartmentId")
                        .HasColumnName("department_id");

                    b.Property<string>("Description")
                        .HasColumnName("description");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnName("first_name");

                    b.Property<DateTime>("InvitedAt")
                        .HasColumnName("invited_at");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnName("lastname");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasColumnName("nick_name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("password");

                    b.Property<string>("Patronymic")
                        .HasColumnName("patronymic");

                    b.Property<int?>("PhoneNumber")
                        .HasColumnName("phone_number");

                    b.Property<int>("UserRoleId")
                        .HasColumnName("user_role_id");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("DepartmentId")
                        .HasName("ix_users_department_id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("NickName")
                        .IsUnique();

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.ToTable("users");

                    b.HasData(
                        new
                        {
                            Id = 123,
                            DepartmentId = -1,
                            Email = "tatata@ayndex.ru",
                            FirstName = "Vlad",
                            InvitedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Lastname = "Nimatora",
                            NickName = "Nimatora",
                            Password = "AQAAAAEAACcQAAAAEMCWzGnu/R+ceDGa7++YxBJshq6KVSa1jDm1s81nSK6+5d/TA/841dK2j23oRcpD5Q==",
                            UserRoleId = -1
                        },
                        new
                        {
                            Id = 124,
                            DepartmentId = -2,
                            Email = "shitmail@ayndex.ru",
                            FirstName = "Dan",
                            InvitedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Lastname = "Tolstovku",
                            NickName = "Tolstovku",
                            Password = "AQAAAAEAACcQAAAAEFb9rOpFtTtnrNorZSV6Z5QaMreCrED7M3veh4mlv3aH/sFqtpE3/TSz38Md5POPBA==",
                            UserRoleId = -2
                        });
                });

            modelBuilder.Entity("WebApplication1.Database.Entities.UserManager", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("user_id");

                    b.Property<int>("ManagerId")
                        .HasColumnName("manager_id");

                    b.HasKey("UserId", "ManagerId");

                    b.HasIndex("ManagerId")
                        .HasName("ix_user_managers_manager_id");

                    b.ToTable("user_managers");
                });

            modelBuilder.Entity("WebApplication1.Database.Entities.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("RoleId")
                        .HasColumnName("role_id");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_roles");

                    b.HasIndex("RoleId")
                        .HasName("ix_user_roles_role_id");

                    b.HasIndex("UserId")
                        .HasName("ix_user_roles_user_id");

                    b.ToTable("user_roles");

                    b.HasData(
                        new
                        {
                            Id = -1,
                            RoleId = 3,
                            UserId = 123
                        },
                        new
                        {
                            Id = -2,
                            RoleId = 1,
                            UserId = 124
                        });
                });

            modelBuilder.Entity("WebApplication1.Database.Entities.SalaryRate", b =>
                {
                    b.HasOne("WebApplication1.Database.Entities.User", "User")
                        .WithOne("SalaryRate")
                        .HasForeignKey("WebApplication1.Database.Entities.SalaryRate", "UserId")
                        .HasConstraintName("fk_salary_rates_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebApplication1.Database.Entities.SalaryRateRequest", b =>
                {
                    b.HasOne("WebApplication1.Database.Entities.User", "Reviewer")
                        .WithMany()
                        .HasForeignKey("ReviewerId")
                        .HasConstraintName("fk_salary_rate_requests_users_reviewer_id");

                    b.HasOne("WebApplication1.Database.Entities.User", "Sender")
                        .WithMany("SalaryRateRequests")
                        .HasForeignKey("SenderId")
                        .HasConstraintName("fk_salary_rate_requests_users_sender_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebApplication1.Database.Entities.User", b =>
                {
                    b.HasOne("WebApplication1.Database.Entities.Department", "Department")
                        .WithMany("Users")
                        .HasForeignKey("DepartmentId")
                        .HasConstraintName("fk_users_departments_department_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebApplication1.Database.Entities.UserManager", b =>
                {
                    b.HasOne("WebApplication1.Database.Entities.User", "Manager")
                        .WithMany("ManagersUsers")
                        .HasForeignKey("ManagerId")
                        .HasConstraintName("fk_user_managers_users_manager_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApplication1.Database.Entities.User", "User")
                        .WithMany("UsersManagers")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_managers_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebApplication1.Database.Entities.UserRole", b =>
                {
                    b.HasOne("WebApplication1.Database.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("fk_user_roles_roles_role_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApplication1.Database.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_roles_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
