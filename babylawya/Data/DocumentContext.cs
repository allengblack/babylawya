using babylawya.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace babylawya.Data
{
    public class DocumentContext : DbContext
    {
        public DocumentContext(DbContextOptions<DocumentContext> options) : base(options)
        { }

        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>().ToTable("DocumentsDB");
        }
    }
}
