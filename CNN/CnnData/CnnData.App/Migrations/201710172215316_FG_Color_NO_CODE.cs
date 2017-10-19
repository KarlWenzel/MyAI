namespace CnnData.App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FG_Color_NO_CODE : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Labels", name: "ColorText", newName: "BackgroundColorText");
            RenameIndex(table: "dbo.Labels", name: "IX_ColorText", newName: "IX_BackgroundColorText");
            CreateTable(
                "Ref.ModifierKeys",
                c => new
                    {
                        KeyText = c.String(nullable: false, maxLength: 8),
                    })
                .PrimaryKey(t => t.KeyText);
            
            AddColumn("dbo.Labels", "ModifierKeyText", c => c.String(maxLength: 8));
            AddColumn("dbo.Labels", "ForegroundColorText", c => c.String(maxLength: 32));
            CreateIndex("dbo.Labels", "ModifierKeyText");
            CreateIndex("dbo.Labels", "ForegroundColorText");
            AddForeignKey("dbo.Labels", "ForegroundColorText", "Ref.WinMediaColors", "ColorText");
            AddForeignKey("dbo.Labels", "ModifierKeyText", "Ref.ModifierKeys", "KeyText");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Labels", "ModifierKeyText", "Ref.ModifierKeys");
            DropForeignKey("dbo.Labels", "ForegroundColorText", "Ref.WinMediaColors");
            DropIndex("dbo.Labels", new[] { "ForegroundColorText" });
            DropIndex("dbo.Labels", new[] { "ModifierKeyText" });
            DropColumn("dbo.Labels", "ForegroundColorText");
            DropColumn("dbo.Labels", "ModifierKeyText");
            DropTable("Ref.ModifierKeys");
            RenameIndex(table: "dbo.Labels", name: "IX_BackgroundColorText", newName: "IX_ColorText");
            RenameColumn(table: "dbo.Labels", name: "BackgroundColorText", newName: "ColorText");
        }
    }
}
