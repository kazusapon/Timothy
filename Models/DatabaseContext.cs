using System;
using Microsoft.EntityFrameworkCore;

namespace Database.Models
{
    class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }

        public DbSet<EntityModels.CallRegister> CallRegister {get; set;}

        public DbSet<EntityModels.Classification> Classification {get; set;}

        public DbSet<EntityModels.ContactMethod> ContactMethod {get; set;}

        public DbSet<EntityModels.GuestType> GuestType {get; set;}

        public DbSet<EntityModels.Inquiry> Inquiry {get; set;}

        public DbSet<EntityModels.System> System {get; set;}

        public DbSet<EntityModels.User> User {get; set;}
    }
}