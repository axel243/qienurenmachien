using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using QienUrenMachien.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace QienUrenMachien.Data
{
    public class RepositoryContext : IdentityDbContext
    {
        public RepositoryContext()
        {

        }

        public RepositoryContext(DbContextOptions<RepositoryContext> options)
    :       base(options)
        {
        }
    }
}