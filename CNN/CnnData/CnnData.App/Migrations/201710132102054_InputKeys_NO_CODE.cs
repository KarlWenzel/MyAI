namespace CnnData.App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InputKeys_NO_CODE : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.LabelInstances", newName: "InstanceLabels");
            DropPrimaryKey("dbo.InstanceLabels");
            CreateTable(
                "dbo.ImageFileLabels",
                c => new
                    {
                        LabelName = c.String(nullable: false, maxLength: 128),
                        CategoryName = c.String(nullable: false, maxLength: 128),
                        ImageFileID = c.Int(nullable: false),
                        SetByHuman = c.Boolean(nullable: false),
                        SetByMachine = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.LabelName, t.CategoryName, t.ImageFileID })
                .ForeignKey("dbo.ImageFiles", t => t.ImageFileID, cascadeDelete: true)
                .ForeignKey("dbo.Labels", t => new { t.LabelName, t.CategoryName }, cascadeDelete: true)
                .Index(t => new { t.LabelName, t.CategoryName })
                .Index(t => t.ImageFileID);
            
            CreateTable(
                "Ref.InputKeys",
                c => new
                    {
                        KeyText = c.String(nullable: false, maxLength: 32),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.KeyText)
                .Index(t => t.Order, unique: true, name: "IX_InputKeyOrder");
            
            CreateTable(
                "Ref.WinMediaColors",
                c => new
                    {
                        ColorText = c.String(nullable: false, maxLength: 32),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ColorText)
                .Index(t => t.Order, unique: true, name: "IX_WinMediaColorOrder");
            
            AddColumn("dbo.Labels", "InputKeyText", c => c.String(maxLength: 32));
            AddColumn("dbo.Labels", "ColorText", c => c.String(maxLength: 32));
            AlterColumn("dbo.ImageFiles", "WidthPixels", c => c.Int());
            AlterColumn("dbo.ImageFiles", "HeightPixels", c => c.Int());
            AddPrimaryKey("dbo.InstanceLabels", new[] { "Instance_ID", "Label_LabelName", "Label_CategoryName" });
            CreateIndex("dbo.Labels", "InputKeyText");
            CreateIndex("dbo.Labels", "ColorText");
            AddForeignKey("dbo.Labels", "InputKeyText", "Ref.InputKeys", "KeyText");
            AddForeignKey("dbo.Labels", "ColorText", "Ref.WinMediaColors", "ColorText");
            DropColumn("dbo.Labels", "HotKey");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Labels", "HotKey", c => c.String());
            DropForeignKey("dbo.ImageFileLabels", new[] { "LabelName", "CategoryName" }, "dbo.Labels");
            DropForeignKey("dbo.Labels", "ColorText", "Ref.WinMediaColors");
            DropForeignKey("dbo.Labels", "InputKeyText", "Ref.InputKeys");
            DropForeignKey("dbo.ImageFileLabels", "ImageFileID", "dbo.ImageFiles");
            DropIndex("Ref.WinMediaColors", "IX_WinMediaColorOrder");
            DropIndex("Ref.InputKeys", "IX_InputKeyOrder");
            DropIndex("dbo.Labels", new[] { "ColorText" });
            DropIndex("dbo.Labels", new[] { "InputKeyText" });
            DropIndex("dbo.ImageFileLabels", new[] { "ImageFileID" });
            DropIndex("dbo.ImageFileLabels", new[] { "LabelName", "CategoryName" });
            DropPrimaryKey("dbo.InstanceLabels");
            AlterColumn("dbo.ImageFiles", "HeightPixels", c => c.Int(nullable: false));
            AlterColumn("dbo.ImageFiles", "WidthPixels", c => c.Int(nullable: false));
            DropColumn("dbo.Labels", "ColorText");
            DropColumn("dbo.Labels", "InputKeyText");
            DropTable("Ref.WinMediaColors");
            DropTable("Ref.InputKeys");
            DropTable("dbo.ImageFileLabels");
            AddPrimaryKey("dbo.InstanceLabels", new[] { "Label_LabelName", "Label_CategoryName", "Instance_ID" });
            RenameTable(name: "dbo.InstanceLabels", newName: "LabelInstances");
        }
    }
}
