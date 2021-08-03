using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogAPI.Data
{
    public class CatalogContext :DbContext

    {
        public CatalogContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<CatalogType> CatalogTypes { get; set; }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public DbSet<CatalogItem> Catalog { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CatalogType>(e =>
            {
                e.Property(t => t.Id)
                 .IsRequired()
                 .ValueGeneratedOnAdd();
                e.Property(t => t.Type)
                 .IsRequired()
                 .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<CatalogBrand>(e =>
            {
                e.Property(b => b.Id)
                 .IsRequired()
                 .ValueGeneratedOnAdd();
                e.Property(b => b.Brand)
                 .IsRequired()
                 .HasMaxLength(100);
            });

            modelBuilder.Entity<CatalogItem>(e =>
            {
                e.Property(i => i.Id)
                 .IsRequired()
                 .ValueGeneratedOnAdd();

                e.Property(i => i.Name)
                 .IsRequired()
                 .HasMaxLength(100);

                e.Property(i => i.Price)
                 .IsRequired();

                e.HasOne(i => i.CatalogType)
                 .WithMany()
                 .HasForeignKey(i => i.CatalogTypeId);

                e.HasOne(i => i.CatalogBrand)
                 .WithMany()
                 .HasForeignKey(i => i.CatalogBrandId);

            });
        }
    }
}
