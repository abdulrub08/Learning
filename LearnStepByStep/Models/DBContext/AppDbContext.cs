using LearnStepByStep.Models.DOM;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LearnStepByStep.Models.DBContext
{
    #region EntityFrameWork Command For Migration
    //Add-Migration AddPhotoPathToEmployees
    //Remove-Migration
    //Update-Database
    //Update-Database AddPhotoPathToEmployees
    #endregion
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region 1st Way to Add Data While Migration
            //modelBuilder.Entity<Employee>().HasData(
            //    new Employee
            //    {
            //        Id = 1,
            //        Name = "Mark",
            //        Department = Dept.IT,
            //        Email = "mark@pragimtech.com"
            //    }
            //);
            #endregion
            #region 2nd Way to Add Data While Migration
            //modelBuilder.Entity<Employee>().HasData(
            //    new Employee
            //    {
            //        Id = 1,
            //        Name = "Mary",
            //        Department = Dept.IT,
            //        Email = "mary@pragimtech.com"
            //    },
            //    new Employee
            //    {
            //        Id = 2,
            //        Name = "John",
            //        Department = Dept.HR,
            //        Email = "john@pragimtech.com"
            //    },
            //    new Employee
            //    {
            //        Id = 3,
            //        Name = "Mark",
            //        Department = Dept.IT,
            //        Email = "mark@pragimtech.com"
            //    }
            // );
            #endregion
            #region 3rd Way to Add Data While Migration
            modelBuilder.Seed();
            #endregion
        }
    }
}
