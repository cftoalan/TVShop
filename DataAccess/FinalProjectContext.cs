using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TVShop.DataAccess
{
    public partial class FinalProjectContext : DbContext
    {
        public FinalProjectContext()
        {
        }

        public FinalProjectContext(DbContextOptions<FinalProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Invoice> Invoices { get; set; } = null!;
        public virtual DbSet<Manufacturer> Manufacturers { get; set; } = null!;
        public virtual DbSet<Television> Televisions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Invoice");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Customer)
                    .WithMany()
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Invoice__Custome__4AB81AF0");

                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__Invoice__Product__4BAC3F29");
            });

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.ToTable("Manufacturer");

                entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");

                entity.Property(e => e.Country)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Television>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PK__Televisi__B40CC6EDB1F48070");

                entity.ToTable("Television");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.HdrSupport)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Model)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Resolution)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ScreenSize)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.Televisions)
                    .HasForeignKey(d => d.ManufacturerId)
                    .HasConstraintName("FK__Televisio__Manuf__32E0915F");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
