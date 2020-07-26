using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Routine.APi.Entities;

namespace Routine.APi.Data
{
    public class RoutineDbContext : DbContext
    {
        //use parent class options
        public RoutineDbContext(DbContextOptions<RoutineDbContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .Property(x => x.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Company>()
                .Property(x => x.Introduction).HasMaxLength(500);
            modelBuilder.Entity<Employee>()
                .Property(x => x.EmployeeNo).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Employee>()
                .Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Employee>()
                .Property(x => x.LastName).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Employee>()
                .HasOne(x => x.Company)
                .WithMany(x => x.Employees)
                //fk
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
            //seeding data
            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = Guid.Parse("C0E917F9-1640-4FFD-8DFE-D3946913A180"),
                    Name = "Microsoft",
                    Introduction = "Great Company"
                },
                new Company
                {
                    Id = Guid.Parse("543D0978-084E-43F5-82F8-6417FFCE0C0C"),
                    Name = "Google",
                    Introduction = "Don't be evil",
                },
                new Company
                {
                    Id = Guid.Parse("50817877-F6F8-4161-9926-20817AC3BD08"),
                    Name = "Alipapa",
                    Introduction = "fubao Company",
                }
            );
        }
    }
}
