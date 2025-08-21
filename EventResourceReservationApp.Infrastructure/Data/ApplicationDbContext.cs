using EventResourceReservationApp.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Infrastructure.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });
            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Locations");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
                entity.Property(e => e.City).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ZipCode).IsRequired();
                entity.Property(e => e.Street).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Neighborhood).HasMaxLength(100);
                entity.Property(e => e.ExteriorNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.InteriorNumber).HasMaxLength(50);
                entity.Property(e => e.CreatedByUserId).IsRequired();
                entity.Property(e => e.CreateAt).IsRequired();
            });
        }
    }
}
