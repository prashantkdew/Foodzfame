using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Foodzfame.Data.Entities;

#nullable disable

namespace Foodzfame.Data.FoodzfameContext
{
    public partial class FoodzfameContext : DbContext
    {
        public FoodzfameContext()
        {
        }

        public FoodzfameContext(DbContextOptions<FoodzfameContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Gallery> Galleries { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<BlogPost> BlogPosts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<DishIngMap> DishIngMaps { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<SubScription> SubScriptions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            modelBuilder.Entity<Gallery>(entity =>
            {
                entity.ToTable("Gallery");

                entity.Property(e => e.Title).HasMaxLength(50);
            });
            modelBuilder.Entity<Blog>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(256);
            });

            modelBuilder.Entity<BlogPost>(entity =>
            {
                entity.ToTable("BlogPost");

                entity.Property(e => e.Title).HasMaxLength(256);

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.BlogPosts)
                    .HasForeignKey(d => d.BlogId)
                    .HasConstraintName("FK__BlogPost__BlogId__4BAC3F29");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Category1)
                    .HasMaxLength(100)
                    .HasColumnName("Category");
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.Property(e => e.AddedBy).HasMaxLength(100);

                entity.Property(e => e.AddedDate).HasColumnType("datetime");

                entity.Property(e => e.CookingTime).HasMaxLength(50);

                entity.Property(e => e.DishName).HasMaxLength(256);

                entity.Property(e => e.Likes).HasColumnName("likes");

                entity.Property(e => e.Yields).HasMaxLength(50);

                entity.HasOne(d => d.DishCategory)
                    .WithMany(p => p.Dishes)
                    .HasForeignKey(d => d.DishCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Dishes__DishCate__3E52440B");
            });

            modelBuilder.Entity<DishIngMap>(entity =>
            {
                entity.ToTable("DishIngMap");

                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.DishIngMaps)
                    .HasForeignKey(d => d.DishId)
                    .HasConstraintName("FK__DishIngMa__DishI__440B1D61");

                entity.HasOne(d => d.Ing)
                    .WithMany(p => p.DishIngMaps)
                    .HasForeignKey(d => d.IngId)
                    .HasConstraintName("FK__DishIngMa__IngId__4316F928");
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.Property(e => e.IngName).HasMaxLength(100);

                entity.Property(e => e.Precessed).HasMaxLength(100);

                entity.Property(e => e.Qty).HasMaxLength(100);
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Review");

                entity.Property(e => e.Review1)
                    .HasMaxLength(500)
                    .HasColumnName("Review");

                entity.Property(e => e.ReviewDate).HasColumnType("datetime");

                entity.Property(e => e.ReviewTitle).HasMaxLength(100);

                entity.Property(e => e.ReviewerName).HasMaxLength(56);

                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.DishId)
                    .HasConstraintName("FK__Review__DishId__48CFD27E");
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.ToTable("SubCategory");

                entity.Property(e => e.Category).HasMaxLength(100);

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.SubCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubCatego__Categ__3B75D760");
            });

            modelBuilder.Entity<SubScription>(entity =>
            {
                entity.ToTable("SubScription");

                entity.Property(e => e.AddedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(256);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
