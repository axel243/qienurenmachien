using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using QienUrenMachien.Models;

namespace QienUrenMachien.Data
{
    public class RepositoryContext : IdentityDbContext
    {
        public DbSet<TimeSheet> TimeSheets { get; set; }
        public RepositoryContext(DbContextOptions<RepositoryContext> options)
    :       base(options)
        {

        
        }
    }
}