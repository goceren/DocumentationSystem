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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DocSysDocumentUser>()
                .HasKey(x => new { x.UserId, x.DocumentId });

            builder.Entity<DocSysDocumentUser>()               
                .HasOne<DocSysUser>(e => e.User)           
                .WithMany(e => e.DocumentUsers)         
                .HasForeignKey(e => e.UserId)         
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DocSysDocumentUser>()
               .HasOne<DocSysDocument>(e => e.Document)
               .WithMany(e => e.DocumentUsers)
               .HasForeignKey(e => e.DocumentId)
               .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }

        public DbSet<DocSysDepartments> DocSysDepartments { get; set; }
        public DbSet<DocSysDocument> DocSysDocument { get; set; }

    }
}
