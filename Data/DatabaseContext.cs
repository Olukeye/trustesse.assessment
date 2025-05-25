using System.Diagnostics.Metrics;
using Trustesse_Assessment.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Trustesse_Assessment.Data
{
    public class DatabaseContext : IdentityDbContext<AppUser>
    {

        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<AppUser> AppUser { get; set; }

    }
}
