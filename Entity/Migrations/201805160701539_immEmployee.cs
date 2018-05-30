namespace Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class immEmployee : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Password = c.String(),
                        Address = c.String(),
                        Address2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Employees");
        }
    }
}
