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

        public DbSet<FileUpload> FileUploads { get; set; }
        public DbSet<TimeSheet> TimeSheets { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder){

        
            base.OnModelCreating(builder);
        }
    }
}
