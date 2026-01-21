using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using SimplecMethod.SimplexEntities;

namespace SimplecMethod.DbContext;

public partial class SimplexContext : Microsoft.EntityFrameworkCore.DbContext
{
    public SimplexContext()
    {
    }

    public SimplexContext(DbContextOptions<SimplexContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ProductRequirment> ProductRequirments { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<ResourceStock> ResourceStocks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=Bier_Mixture;user id=root;password=123mysql", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.1.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<ProductRequirment>(entity =>
        {
            entity.HasKey(e => new { e.ResourceId, e.ProductId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("product_requirments");

            entity.HasIndex(e => e.ProductId, "product_fk");

            entity.Property(e => e.ResourceId).HasColumnName("resource_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.UnitsRequired)
                .HasPrecision(10, 2)
                .HasColumnName("units_required");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductRequirments)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("product_fk");

            entity.HasOne(d => d.Resource).WithMany(p => p.ProductRequirments)
                .HasForeignKey(d => d.ResourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_requirments_resources_id_fk");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("product_types");

            entity.HasIndex(e => e.Name, "name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.ProfitPreHl)
                .HasPrecision(10, 2)
                .HasColumnName("profit_pre_hl");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("resources");

            entity.HasIndex(e => e.Name, "name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ResourceStock>(entity =>
        {
            entity.HasKey(e => e.ResourceId).HasName("PRIMARY");

            entity.ToTable("resource_stock");

            entity.Property(e => e.ResourceId)
                .ValueGeneratedNever()
                .HasColumnName("resource_id");
            entity.Property(e => e.AvailibleUnits)
                .HasPrecision(10, 2)
                .HasColumnName("availible_units");

            entity.HasOne(d => d.Resource).WithOne(p => p.ResourceStock)
                .HasForeignKey<ResourceStock>(d => d.ResourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("resource_stock_resources_id_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
