using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace import_manager.Model
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Document> Documents { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=dev-ws-v-07;Port=5432;Database=manager-dev-db;Username=mobnius;Password=mobnius-0");
        }
    }
}
