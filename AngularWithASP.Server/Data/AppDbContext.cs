using Microsoft.EntityFrameworkCore;
using AngularWithASP.Server.Models;
using System.Collections.Generic;

namespace AngularWithASP.Server.Data
{
    /// <summary>
    /// Represents the database context of the application.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options"></param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// All garages
        /// </summary>
        public DbSet<Garage> Garages { get; set; }
        /// <summary>
        /// All cars
        /// </summary>
        public DbSet<Car> Cars { get; set; }

        /// <summary>
        /// Configures the database context.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Garage>().HasKey(g => g.Id);
            modelBuilder.Entity<Car>().HasKey(c => c.Id);

            modelBuilder.Entity<Garage>()
                .HasMany(g => g.Cars)
                .WithOne(c => c.Garage)
                .HasForeignKey(c => c.GarageId)
                .OnDelete(DeleteBehavior.NoAction);

//#if DEBUG
//            //seeding datas
//            modelBuilder.Entity<Garage>().HasData(
//                new Garage { Id = 1, Name = "Garage 1", Address = "Address 1", City = "City 1" },
//                new Garage { Id = 2, Name = "Garage 2", Address = "Address 2", City = "City 2" },
//                new Garage { Id = 3, Name = "Garage 3", Address = "Address 3", City = "City 3" }
//            );

//            modelBuilder.Entity<Car>()
//                .HasData(
//                    new Car { Id = 1, Brand = "Brand 1", Model = "Model 1", Color = "Color 1", GarageId = 1 },
//                    new Car { Id = 2, Brand = "Brand 2", Model = "Model 2", Color = "Color 2", GarageId = 1 },
//                    new Car { Id = 3, Brand = "Brand 3", Model = "Model 3", Color = "Color 3", GarageId = 2 },
//                    new Car { Id = 4, Brand = "Brand 4", Model = "Model 4", Color = "Color 4", GarageId = 2 },
//                    new Car { Id = 5, Brand = "Brand 5", Model = "Model 5", Color = "Color 5", GarageId = 3 },
//                    new Car { Id = 6, Brand = "Brand 6", Model = "Model 6", Color = "Color 6", GarageId = 3 }
//                );
//#endif

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Configures the database context.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);
        }

    }
}
