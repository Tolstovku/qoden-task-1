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
                    Id = 123, NickName = "Nimatora", Password = pass1, FirstName = "Vlad", Lastname = "Nimatora",
                    Email = "tatata@ayndex.ru",  UserRoleId= -1, DepartmentId = -1
                },
                new User
                {
                    Id = 124, NickName = "Tolstovku", Password = pass2, FirstName = "Dan", Lastname = "Tolstovku",
                    Email = "shitmail@ayndex.ru", UserRoleId = -2, DepartmentId = -2
                });
            modelBuilder.Entity<Role>().HasData(
                new Role
                    {Id = 1, Name = "user"}, new Role{Id = 2, Name = "manager"}, 
                new Role{Id = 3, Name = "admin"});
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole {Id = -1, UserId = 123, RoleId = 3},
                new UserRole {Id = -2, UserId = 124, RoleId = 1}
            );


        }
    }
}