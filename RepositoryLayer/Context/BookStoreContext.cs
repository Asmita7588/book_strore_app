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


    }
}
