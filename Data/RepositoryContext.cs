using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using QienUrenMachien.Models;

namespace QienUrenMachien.Data
{
    public class RepositoryContext : IdentityDbContext
    {

        public RepositoryContext(DbContextOptions<RepositoryContext> options)
    :       base(options)
        {
        }
    }
}