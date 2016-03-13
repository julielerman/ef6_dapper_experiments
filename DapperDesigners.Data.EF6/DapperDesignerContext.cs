using Dapper.Domain;
using System.Data.Common;
using System.Data.Entity;

namespace DapperDesigners.Data.EF6
{
  public class DapperDesignerContext : DbContext
  {
    public DapperDesignerContext() : base("DapperDesigns") {
    }
    public DapperDesignerContext(DbConnection existingConnection,
      bool contextOwnsConnection) {
          
    }


    public DbSet<DapperDesigner> Designers { get; set; }
    public DbSet<Client> Clients { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
      modelBuilder.Entity<DapperDesigner>()
        .HasOptional(d => d.ContactInfo)
        .WithRequired(c => c.Designer);
      base.OnModelCreating(modelBuilder);
    }
  }
}