namespace DapperDesigners.Data.EF6.Migrations
{
  using Dapper.Domain;
  using System.Collections.Generic;
  using System.Data.Entity.Migrations;
  using System.Linq;

  internal sealed class Configuration : DbMigrationsConfiguration<DapperDesigners.Data.EF6.DapperDesignerContext>
  {
    public Configuration() {
      AutomaticMigrationsEnabled = false;
    }

    protected override void Seed(DapperDesigners.Data.EF6.DapperDesignerContext context) {
      //  This method will be called after migrating to the latest version.

      //  You can use the DbSet<T>.AddOrUpdate() helper extension method
      //  to avoid creating duplicate seed data. E.g.
      //
      //    context.People.AddOrUpdate(
      //      p => p.FullName,
      //      new Person { FullName = "Andrew Peters" },
      //      new Person { FullName = "Brice Lambson" },
      //      new Person { FullName = "Rowan Miller" }
      //    );
      //
      var julie = new Client { Name = "Julie" };
      var topfunky = new Client { Name = "Geoffrey" };
      var magnus = new Client { Name = "Magnus" };
      context.Designers.AddOrUpdate(
        d => new { d.LabelName, d.Founder, d.Dapperness },
        new DapperDesigner
        {
          LabelName = "Top Funky Creations",
          Founder = "Guy Vanchey",
          Dapperness = Dapperness.PrettyDapper,
          ContactInfo = new ContactInfo { Twitter = "@threadbare" },
          Clients = new List<Client> { topfunky, magnus },
          Products = new List<Product> { new Product {  Description="Super Skinny Jeans"} ,
          new Product {Description="Shiny Shirt" }}
        },
         new DapperDesigner
         {
           LabelName = "Emperor's Clothes",
           Founder = "Hans C. Anderson",
           Dapperness = Dapperness.SuperDapper,
           ContactInfo = new ContactInfo { Twitter = "@theemperor" },
           Clients = new List<Client> { magnus },
           Products = new List<Product> { new Product {  Description="Really Really Sheer Shirt"} ,
          new Product {Description="Barely There Pants" } }
         }, new DapperDesigner
         {
           LabelName = "Casa Casual",
           Founder = "Julie Lerman",
           Dapperness = Dapperness.Dapperless,
           ContactInfo = new ContactInfo { Twitter = "@casacasual" },
           Clients = new List<Client> { julie },
           Products = new List<Product> { new Product {  Description="Yoga Pants"} ,
          new Product {Description="More Yoga Pants" } }
         }
        );
      if (context.Designers.SingleOrDefault(d => d.LabelName == "Label0") == null){
        context.Configuration.AutoDetectChangesEnabled = false;
        for (int i = 0; i < 30000; i++) {
          context.Designers.Add(
         new DapperDesigner
         {
           LabelName = $"Label{i}",
           Founder = $"Founder{i}",
           Dapperness = Dapperness.KindaDapper,
           ContactInfo = new ContactInfo { Twitter = "@Label{i}" }
         });
        }
      }
    }
  }
}