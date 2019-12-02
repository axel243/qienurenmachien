using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using QienUrenMachien.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace QienUrenMachien.Data
{
    public class RepositoryContext : IdentityDbContext<ApplicationUser>
    {

        public RepositoryContext(DbContextOptions<RepositoryContext> options)
            : base(options) 
        {

        }


        public DbSet<TimeSheet> TimeSheets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder){

            builder.Entity<ApplicationUser>()
                .HasOne(au => au.Werkgever)
                .WithMany(au => au.Werknemers)
                .HasForeignKey(au => au.WerkgeverID);
            base.OnModelCreating(builder);
        }
    }
}
