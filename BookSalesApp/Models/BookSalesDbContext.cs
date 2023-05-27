using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookSalesApp.Models;

public partial class BookSalesDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public BookSalesDbContext(DbContextOptions<BookSalesDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Default"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Author)
                .HasMaxLength(50)
                .HasColumnName("author");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Fileurl)
                .HasMaxLength(100)
                .HasColumnName("fileurl");
            entity.Property(e => e.Genreid).HasColumnName("genreid");
            entity.Property(e => e.Imageurl)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("imageurl");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.Genreid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Books_Genres");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bookid).HasColumnName("bookid");
            entity.Property(e => e.Issold).HasColumnName("issold");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Book).WithMany(p => p.Carts)
                .HasForeignKey(d => d.Bookid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Books");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Users");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Paymentdate)
                .HasColumnType("date")
                .HasColumnName("paymentdate");
            entity.Property(e => e.Paymentid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("paymentid");
            entity.Property(e => e.Totalamount).HasColumnName("totalamount");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Isadmin).HasColumnName("isadmin");
            entity.Property(e => e.Password)
                .HasMaxLength(65)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}