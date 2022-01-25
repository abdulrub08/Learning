using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kudvenkat.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelBuilderExtensions.Seed(modelBuilder);
            //modelBuilder.Entity<Employee>().HasData(
            //    new Employee
            //    {
            //        ID = 1,
            //        Name = "Mark",
            //        Department = Dept.IT,
            //        Email = "mark@pragimtech.com"
            //    },
            //    new Employee
            //    {
            //        ID = 2,
            //        Name = "John",
            //        Department = Dept.HR,
            //        Email = "john@pragimtech.com"
            //    }
            //);

        }
    }
}
