using System;
using Timothy.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public DbSet<CallRegister> CallRegister {get; set;}

        public DbSet<Classification> Classification {get; set;}

        public DbSet<ContactMethod> ContactMethod {get; set;}

        public DbSet<GuestType> GuestType {get; set;}

        public DbSet<Inquiry> Inquiry {get; set;}

        public DbSet<Timothy.Models.Entities.System> System {get; set;}

        public DbSet<User> User {get; set;}
    }
}