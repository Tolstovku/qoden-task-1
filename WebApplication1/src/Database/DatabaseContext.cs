using Lesson1.Helpers;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Database.Entities;

namespace WebApplication1.src.Database
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.ApplySnakeCase(modelBuilder);
            this.ApplyOnModelCreatingFromAllEntities(modelBuilder);
            modelBuilder.Entity<Department>()
                .HasData(new Department {Id = -2, Name = "Frontend"},
                    new Department {Id = -1, Name = "Backend"});
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 123, NickName = "Nimatora", Password = "123", FirstName = "Vlad", Lastname = "Nimatora",
                    Email = "tatata@ayndex.ru", Role = Role.Admin, DepartmentId = -1
                },
                new User
                {
                    Id = 124, NickName = "Tolstovku", Password = "124", FirstName = "Dan", Lastname = "Tolstovku",
                    Email = "shitmail@ayndex.ru", Role = Role.User, DepartmentId = -2
                });
//            modelBuilder.Entity<User>()
//                .HasOne(u => u.Department)
//                .WithMany(d => d.Users)
//                .IsRequired()
//                .HasForeignKey(u => u.DepartmentId);
//
//
//            modelBuilder.Entity<SalaryRate>()
//                .HasOne(s => s.User)
//                .WithOne(u => u.SalaryRate)
//                .IsRequired()
//                .HasForeignKey<SalaryRate>(s => s.UserId);
        }
    }
}