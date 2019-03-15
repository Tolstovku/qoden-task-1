using Microsoft.EntityFrameworkCore;
using WebApplication1.Database.Entities;
using WebApplication1.Helpers;

namespace WebApplication1.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SalaryRate> SalaryRates { get; set; }
        public DbSet<SalaryRateRequest> SalaryRateRequests { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserManager> UserManagers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.ApplySnakeCase(modelBuilder);
            this.ApplyOnModelCreatingFromAllEntities(modelBuilder);

            modelBuilder.Entity<UserManager>().HasKey(key => new {key.UserId, key.ManagerId});
            modelBuilder.Entity<UserRole>().HasKey(key => new {key.UserId, key.RoleId});
            modelBuilder.Entity<User>().HasIndex(u => u.PhoneNumber).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.NickName).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();


            var pass1 = PasswordGenerator.HashPassword("123");
            var pass2 = PasswordGenerator.HashPassword("123");

            modelBuilder.Entity<Department>()
                .HasData(new Department {Id = -2, Name = "Frontend"},
                    new Department {Id = -1, Name = "Backend"});
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 123, NickName = "Admin", Password = pass1, FirstName = "Vlad", Lastname = "Nimatora",
                    Email = "tatata@ayndex.ru",  DepartmentId = -1
                },
                new User
                {
                    Id = 124, NickName = "User", Password = pass2, FirstName = "Dan", Lastname = "Tolstovku",
                    Email = "shitmail@ayndex.ru", DepartmentId = -2
                },
                new User
                {
                    Id = 125, NickName = "Manager", Password = pass1, FirstName = "Someone", Lastname = "Something",
                    Email = "managerEmail@ayndex.ru", DepartmentId = -1
                });
            modelBuilder.Entity<Role>().HasData(
                new Role
                    {Id = 1, Name = "user"}, new Role{Id = 2, Name = "manager"},
                new Role{Id = 3, Name = "admin"});
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole {UserId = 123, RoleId = 3},
                new UserRole {UserId = 124, RoleId = 1},
                new UserRole {UserId = 125, RoleId = 2}
            );
            modelBuilder.Entity<SalaryRateRequest>().HasData(
                new SalaryRateRequest {Id = -1, RequestChainId = -1, SuggestedRate = 1337, Reason = "Want money", SenderId = 124},
                new SalaryRateRequest {Id = -2, RequestChainId = -2, SuggestedRate = 1338, Reason = "Want more money", SenderId = 124}
            );
            modelBuilder.Entity<UserManager>().HasData(new UserManager(124, 125));


        }
    }
}