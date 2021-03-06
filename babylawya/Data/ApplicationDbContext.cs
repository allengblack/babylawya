﻿using babylawya.Models;
using babylawya.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace babylawya.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Document> Documents { get; set; }
        public DbSet<Keyword> Keywords { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Document>()
                .HasMany(c => c.Keywords)
                .WithOne(x => x.Document)
                .HasForeignKey(x => x.DocumentId)
                .IsRequired();
        }
    }
}
