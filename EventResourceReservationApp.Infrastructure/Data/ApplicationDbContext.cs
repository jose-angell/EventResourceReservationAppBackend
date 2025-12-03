using EventResourceReservationApp.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Infrastructure.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<ReservationCarItem> ReservationCarItems { get; set; }
        public DbSet<Review> Reviews { get; set; }  
        public DbSet<Resource> Resources { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                // 1. Define la relación (Uno a Muchos: HasOne y WithMany)
                entity.HasOne<ApplicationUser>()
                      // 2. Indica que no hay propiedad de navegación inversa en ApplicationUser
                      .WithMany()
                      // 3. Define la clave foránea (CORRECCIÓN DEL TYPO)
                      .HasForeignKey(e => e.CreatedByUserId);
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
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.HasOne<ApplicationUser>()
                      .WithMany()
                      .HasForeignKey(e => e.CreatedByUserId);
            });
            modelBuilder.Entity<ReservationCarItem>(entity =>
            {
                entity.ToTable("ReservationCarItems");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ClientId).IsRequired();
                entity.Property(e => e.ResourceId).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.AddedAt).IsRequired();
            });
            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Reviews");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ResourceId).IsRequired();
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Rating).IsRequired();
                entity.Property(e => e.Comment).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).IsRequired();
            });
            modelBuilder.Entity<Resource>(entity =>
            {
                entity.ToTable("Resources");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CategoryId).IsRequired();
                entity.Property(e => e.StatusId).IsRequired();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.AuthorizationType).IsRequired();
                entity.Property(e => e.LocationId).IsRequired();
                entity.Property(e => e.CreatedByUserId).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.HasOne<ApplicationUser>()
                      .WithMany()
                      .HasForeignKey(e => e.CreatedByUserId);
                entity.HasOne<Category>()
                      .WithMany()
                      .HasForeignKey(e => e.CategoryId);
                entity.HasOne<Location>()
                        .WithMany()
                        .HasForeignKey(e => e.LocationId);
            });
        }
    }
}
