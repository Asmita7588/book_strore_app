using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext(DbContextOptions option) : base(option) { }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<AdminEntity> Admins { get; set; }

        public DbSet<TokenEntity> RefreshTokens { get; set; }

        public DbSet<BookEntity> Books { get; set; }

        public DbSet<CartEntity> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CartEntity>()
                .Property(c => c.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderEntity>()
               .Property(o => o.Price)
               .HasColumnType("decimal(18,2)");
        }
        public DbSet<WishListEntity> WishList { get; set; }
        public DbSet<OrderEntity> OrderSummary { get; set; }

        public DbSet<CustomerDetails> Customers { get; set; }

      






    }
}
