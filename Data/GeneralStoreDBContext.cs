using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using GeneralStoreAPI.Models;

namespace GeneralStoreAPI.Data
{
    public partial class GeneralStoreDBContext : DbContext
    {
        public GeneralStoreDBContext()
        {
        }

        public GeneralStoreDBContext(DbContextOptions<GeneralStoreDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:GeneralStoreDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers", "dev");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products", "dev");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transactions", "dev");

                entity.Property(e => e.DateOfTransaction).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Transacti__Custo__3E52440B");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Transacti__Produ__3D5E1FD2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
