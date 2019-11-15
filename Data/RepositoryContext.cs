using Microsoft.EntityFrameworkCore;
using QienUrenMachien.Models;


namespace QienUrenMachien.Data
{
    public class RepositoryContext : DbContext
    {

        public RepositoryContext(DbContextOptions<RepositoryContext> options)
    :       base(options)
        {
        }
    }
}