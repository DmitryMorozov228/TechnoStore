namespace ProductStore.Domein.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DimasikaMigration2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "CategoryName", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categories", "CategoryName", c => c.String());
        }
    }
}
