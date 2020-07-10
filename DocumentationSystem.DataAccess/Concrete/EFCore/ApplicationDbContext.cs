using DocumentationSystem.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentationSystem.DataAccess.Concrete.EFCore
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=Goceren;Initial Catalog=DocumentationSystem;Integrated Security=True; Trusted_Connection=true; MultipleActiveResultSets=true;");
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<DocSysDepartments> DocSysDepartments { get; set; }
        public DbSet<DocSysDocument> DocSysDocument { get; set; }

    }
}
