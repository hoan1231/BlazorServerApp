using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ManagementPackage.Models
{
    public partial class PackageDBContext : DbContext
    {
        public PackageDBContext()
        {
        }

        public PackageDBContext(DbContextOptions<PackageDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<CustomerPackage> CustomerPackages { get; set; } = null!;
        public virtual DbSet<LogRequest> LogRequests { get; set; } = null!;
        public virtual DbSet<MngHistoryTransaction> MngHistoryTransactions { get; set; } = null!;
        public virtual DbSet<MngPackage> MngPackages { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-4DNVAF0;Database=PackageDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id)
                    .HasMaxLength(200)
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy).HasMaxLength(200);

                entity.Property(e => e.CreatedDate)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(500);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.PhoneNumber).HasMaxLength(100);

                entity.Property(e => e.TotalMoney)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy).HasMaxLength(200);

                entity.Property(e => e.UpdatedDate)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(200);
            });

            modelBuilder.Entity<CustomerPackage>(entity =>
            {
                entity.ToTable("Customer_Package");

                entity.Property(e => e.Id)
                    .HasMaxLength(200)
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy).HasMaxLength(200);

                entity.Property(e => e.CreatedDate)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(200)
                    .HasColumnName("CustomerID");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.PackageId)
                    .HasMaxLength(200)
                    .HasColumnName("PackageID");

                entity.Property(e => e.UpdatedBy).HasMaxLength(200);

                entity.Property(e => e.UpdatedDate)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LogRequest>(entity =>
            {
                entity.ToTable("LogRequest");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.StatusCode)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MngHistoryTransaction>(entity =>
            {
                entity.ToTable("MNG_HistoryTransaction");

                entity.Property(e => e.Id)
                    .HasMaxLength(200)
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy).HasMaxLength(200);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(200)
                    .HasColumnName("CustomerID");

                entity.Property(e => e.FullName).HasMaxLength(500);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.NamePackage).HasMaxLength(200);

                entity.Property(e => e.PackageId)
                    .HasMaxLength(200)
                    .HasColumnName("PackageID");

                entity.Property(e => e.PricePackage)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TypeTransaction).HasMaxLength(200);

                entity.Property(e => e.UpdatedBy).HasMaxLength(200);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<MngPackage>(entity =>
            {
                entity.ToTable("MNG_Package");

                entity.Property(e => e.Id)
                    .HasMaxLength(200)
                    .HasColumnName("ID");

                entity.Property(e => e.CodePackage).HasMaxLength(200);

                entity.Property(e => e.CreatedBy).HasMaxLength(200);

                entity.Property(e => e.CreatedDate)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.NamePackage).HasMaxLength(500);

                entity.Property(e => e.PricePackage)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TypePackage).HasMaxLength(100);

                entity.Property(e => e.UpdatedBy).HasMaxLength(200);

                entity.Property(e => e.UpdatedDate)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
