namespace DapperDesigners.Data.EF6.Migrations
{
  using System.Data.Entity.Migrations;

  public partial class init : DbMigration
  {
    public override void Up() {
      CreateTable(
          "dbo.Clients",
          c => new
          {
            Id = c.Int(nullable: false, identity: true),
            Name = c.String(),
            DefaultDappernessLevel = c.Int(nullable: false),
          })
          .PrimaryKey(t => t.Id);

      CreateTable(
          "dbo.DapperDesigners",
          c => new
          {
            Id = c.Int(nullable: false, identity: true),
            LabelName = c.String(),
            Founder = c.String(),
            Dapperness = c.Int(nullable: false),
          })
          .PrimaryKey(t => t.Id);

      CreateTable(
          "dbo.ContactInfoes",
          c => new
          {
            Id = c.Int(nullable: false),
            Twitter = c.String(),
          })
          .PrimaryKey(t => t.Id)
          .ForeignKey("dbo.DapperDesigners", t => t.Id)
          .Index(t => t.Id);

      CreateTable(
          "dbo.Products",
          c => new
          {
            Id = c.Int(nullable: false, identity: true),
            Description = c.String(),
            DapperDesignerId = c.Int(nullable: false),
          })
          .PrimaryKey(t => t.Id)
          .ForeignKey("dbo.DapperDesigners", t => t.DapperDesignerId, cascadeDelete: true)
          .Index(t => t.DapperDesignerId);

      CreateTable(
          "dbo.DapperDesignerClients",
          c => new
          {
            DapperDesigner_Id = c.Int(nullable: false),
            Client_Id = c.Int(nullable: false),
          })
          .PrimaryKey(t => new { t.DapperDesigner_Id, t.Client_Id })
          .ForeignKey("dbo.DapperDesigners", t => t.DapperDesigner_Id, cascadeDelete: true)
          .ForeignKey("dbo.Clients", t => t.Client_Id, cascadeDelete: true)
          .Index(t => t.DapperDesigner_Id)
          .Index(t => t.Client_Id);
    }

    public override void Down() {
      DropForeignKey("dbo.Products", "DapperDesignerId", "dbo.DapperDesigners");
      DropForeignKey("dbo.ContactInfoes", "Id", "dbo.DapperDesigners");
      DropForeignKey("dbo.DapperDesignerClients", "Client_Id", "dbo.Clients");
      DropForeignKey("dbo.DapperDesignerClients", "DapperDesigner_Id", "dbo.DapperDesigners");
      DropIndex("dbo.DapperDesignerClients", new[] { "Client_Id" });
      DropIndex("dbo.DapperDesignerClients", new[] { "DapperDesigner_Id" });
      DropIndex("dbo.Products", new[] { "DapperDesignerId" });
      DropIndex("dbo.ContactInfoes", new[] { "Id" });
      DropTable("dbo.DapperDesignerClients");
      DropTable("dbo.Products");
      DropTable("dbo.ContactInfoes");
      DropTable("dbo.DapperDesigners");
      DropTable("dbo.Clients");
    }
  }
}