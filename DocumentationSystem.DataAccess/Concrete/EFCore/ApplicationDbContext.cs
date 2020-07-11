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
            optionsBuilder.UseSqlServer(@"Server=tcp:srvw52.hostixo.com; Initial Catalog=gocerenc_document; User ID=goceren_document; Password=Abcd..123; MultipleActiveResultSets=true;");
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<DocSysDepartments> DocSysDepartments { get; set; }
        public DbSet<DocSysDocument> DocSysDocument { get; set; }

    }
}
