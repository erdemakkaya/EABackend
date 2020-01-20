using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EAContext : DbContext
    {
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server =.; Database=Northwind;Trusted_Connection=True;");
        }
        public DbSet<Product> Products { get; set; }
        //public DbSet<Category> Categories { get; set; }
        //public DbSet<User> Users { get; set; }
        //public DbSet<Entities.Concrete.UserRole> UserRoles { get; set; }
        //public DbSet<Role> Roles { get; set; }

    }
}