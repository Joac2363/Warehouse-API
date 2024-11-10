using Microsoft.EntityFrameworkCore;
using Warehouse_API.Models;

namespace Warehouse_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Order> Orders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) // Place for customization of tables
        {
            // Sets up the M -> M relaitionship
            modelBuilder.Entity<Stock>()
                   .HasKey(s => new { s.ProductId, s.WarehouseId });
            modelBuilder.Entity<Stock>()
                    .HasOne(p => p.Product)
                    .WithMany(s => s.Stock)
                    .HasForeignKey(p => p.ProductId);
            modelBuilder.Entity<Stock>()
                    .HasOne(w => w.Warehouse)
                    .WithMany(s => s.Stock)
                    .HasForeignKey(w => w.WarehouseId);

            //// Sets up the M -> M relaitionship
            //modelBuilder.Entity<PokemonOwner>()
            //       .HasKey(po => new { po.PokemonId, po.OwnerId });
            //modelBuilder.Entity<PokemonOwner>()
            //        .HasOne(p => p.Pokemon)
            //        .WithMany(pc => pc.PokemonOwners)
            //        .HasForeignKey(c => c.PokemonId);
            //modelBuilder.Entity<PokemonOwner>()
            //        .HasOne(p => p.Owner)
            //        .WithMany(pc => pc.PokemonOwners)
            //        .HasForeignKey(c => c.OwnerId);
        }
    }
}
