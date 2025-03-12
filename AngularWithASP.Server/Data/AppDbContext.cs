using Microsoft.EntityFrameworkCore;
using AngularWithASP.Server.Models;
using System.Collections.Generic;

namespace AngularWithASP.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Garage> Garages { get; set; }
        public DbSet<Car> Cars { get; set; }
    }
}
