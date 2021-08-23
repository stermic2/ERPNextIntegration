using ERPNextIntegration.Dtos.QBO;
using ERPNextIntegration.IntegrationRelationships;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ERPNextIntegration
{
    public class IntegrationDbContext: DbContext
    {
        public static string ConnectionString = string.Empty;
        
        public DbSet<entity> FailedQboWebhooks { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<ItemRelationship> ItemRelationships { get; set; }
        public DbSet<CustomerRelationship> CustomerRelationships { get; set; }
        public DbSet<CustomerAddressRelationship> CustomerAddressRelationships { get; set; }

        public IntegrationDbContext(DbContextOptions<IntegrationDbContext> options, IHttpContextAccessor accessor) :
            base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(ConnectionString);
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");
        }
    }
}