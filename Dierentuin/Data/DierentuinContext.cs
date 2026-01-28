using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dierentuin.Models;

namespace Dierentuin.Data
{
    // Database context for Entity Framework
    public class DierentuinContext : DbContext
    {
        public DierentuinContext(DbContextOptions<DierentuinContext> options)
            : base(options)
        {
        }

        // Database tables
        public DbSet<Animal> Animal { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Enclosure> Enclosure { get; set; }
        public DbSet<Zoo> Zoo { get; set; }

        // Configure relationships and constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships with cascading deletes
            ConfigureRelationships(modelBuilder);

            // Add unique constraints on Name properties
            AddUniqueConstraints(modelBuilder);
        }

        // Set up relationships between entities
        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enclosure>()
                .HasMany(e => e.Animals)
                .WithOne(a => a.Enclosure)
                .HasForeignKey(a => a.EnclosureId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Zoo>()
                .HasMany(z => z.Enclosures)
                .WithOne(e => e.Zoo)
                .HasForeignKey(e => e.ZooId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Animals)
                .WithOne(a => a.Category)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        // Add unique constraints to prevent duplicate names
        private void AddUniqueConstraints(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Enclosure>()
                .HasIndex(e => e.Name)
                .IsUnique();

            modelBuilder.Entity<Zoo>()
                .HasIndex(z => z.Name)
                .IsUnique();
        }
    }
}
