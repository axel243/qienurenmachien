using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using QienUrenMachien.Models;
using Microsoft.AspNetCore.Identity;

namespace QienUrenMachien.Data
{
    public class RepositoryContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<TimeSheet> TimeSheets { get; set; }
        public RepositoryContext(DbContextOptions<RepositoryContext> options)
    :       base(options)
        {

        
        }
    }
}