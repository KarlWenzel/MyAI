namespace CnnData.App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_CODE : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Ref.Counties",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CountyFIPS = c.Int(nullable: false),
                        CountyText = c.String(),
                        ANSI = c.String(),
                        StateText = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("Ref.States", t => t.StateText)
                .Index(t => t.StateText);
            
            CreateTable(
                "Ref.States",
                c => new
                    {
                        StateText = c.String(nullable: false, maxLength: 128),
                        Abbrev = c.String(),
                        ANSI = c.String(),
                    })
                .PrimaryKey(t => t.StateText);
            
            CreateTable(
                "dbo.FeatureTypes",
                c => new
                    {
                        FeatureName = c.String(nullable: false, maxLength: 128),
                        UseWithImageDirectories = c.Boolean(nullable: false),
                        UseWithImageFiles = c.Boolean(nullable: false),
                        UseWithInstances = c.Boolean(nullable: false),
                        UseWithMultiPageImageFiles = c.Boolean(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.FeatureName);
            
            CreateTable(
                "dbo.ImageDirectories",
                c => new
                    {
                        DirectoryName = c.String(nullable: false, maxLength: 255),
                        CreatedOn = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        UpdatedOn = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    })
                .PrimaryKey(t => t.DirectoryName);
            
            CreateTable(
                "dbo.ImageDirectoryFeatures",
                c => new
                    {
                        FeatureName = c.String(nullable: false, maxLength: 128),
                        DirectoryName = c.String(nullable: false, maxLength: 255),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.FeatureName, t.DirectoryName })
                .ForeignKey("dbo.FeatureTypes", t => t.FeatureName, cascadeDelete: true)
                .ForeignKey("dbo.ImageDirectories", t => t.DirectoryName, cascadeDelete: true)
                .Index(t => t.FeatureName)
                .Index(t => t.DirectoryName);
            
            CreateTable(
                "dbo.ImageFiles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 128),
                        DirectoryName = c.String(maxLength: 255),
                        MultiPageImageFileID = c.Int(),
                        PageSequence = c.Int(),
                        ImageExtension = c.String(),
                        Checksum = c.String(),
                        WidthPixels = c.Int(nullable: false),
                        HeightPixels = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        UpdatedOn = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ImageDirectories", t => t.DirectoryName)
                .ForeignKey("dbo.MultiPageImageFiles", t => t.MultiPageImageFileID)
                .Index(t => new { t.FileName, t.DirectoryName }, unique: true, name: "IX_FileNameDirectoryName")
                .Index(t => t.MultiPageImageFileID);
            
            CreateTable(
                "dbo.ImageFileFeatures",
                c => new
                    {
                        FeatureName = c.String(nullable: false, maxLength: 128),
                        ImageFileID = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.FeatureName, t.ImageFileID })
                .ForeignKey("dbo.FeatureTypes", t => t.FeatureName, cascadeDelete: true)
                .ForeignKey("dbo.ImageFiles", t => t.ImageFileID, cascadeDelete: true)
                .Index(t => t.FeatureName)
                .Index(t => t.ImageFileID);
            
            CreateTable(
                "dbo.Instances",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ImageFileID = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        UpdatedOn = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ImageFiles", t => t.ImageFileID, cascadeDelete: true)
                .Index(t => t.ImageFileID);
            
            CreateTable(
                "dbo.InstanceFeatures",
                c => new
                    {
                        FeatureName = c.String(nullable: false, maxLength: 128),
                        InstanceID = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.FeatureName, t.InstanceID })
                .ForeignKey("dbo.FeatureTypes", t => t.FeatureName, cascadeDelete: true)
                .ForeignKey("dbo.Instances", t => t.InstanceID, cascadeDelete: true)
                .Index(t => t.FeatureName)
                .Index(t => t.InstanceID);
            
            CreateTable(
                "dbo.InstanceSetInstances",
                c => new
                    {
                        InstanceSetID = c.Int(nullable: false),
                        InstanceID = c.Int(nullable: false),
                        InstanceSetRoleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.InstanceSetID, t.InstanceID })
                .ForeignKey("dbo.Instances", t => t.InstanceID, cascadeDelete: true)
                .ForeignKey("dbo.InstanceSets", t => t.InstanceSetID, cascadeDelete: true)
                .ForeignKey("dbo.InstanceSetRoles", t => t.InstanceSetRoleID, cascadeDelete: true)
                .Index(t => t.InstanceSetID)
                .Index(t => t.InstanceID)
                .Index(t => t.InstanceSetRoleID);
            
            CreateTable(
                "dbo.InstanceSets",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Seed = c.Int(),
                        MinFileInstanceID = c.Int(),
                        MaxFileInstanceID = c.Int(),
                        UsesCrossValidation = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        UpdatedOn = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        DateCreated = c.DateTime(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.InstanceSetRoles",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        RoleText = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Labels",
                c => new
                    {
                        LabelName = c.String(nullable: false, maxLength: 128),
                        CategoryName = c.String(nullable: false, maxLength: 128),
                        HotKey = c.String(),
                    })
                .PrimaryKey(t => new { t.LabelName, t.CategoryName })
                .ForeignKey("dbo.LabelCategories", t => t.CategoryName, cascadeDelete: true)
                .Index(t => t.CategoryName);
            
            CreateTable(
                "dbo.LabelCategories",
                c => new
                    {
                        CategoryName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.CategoryName);
            
            CreateTable(
                "dbo.MultiPageImageFiles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 128),
                        DirectoryName = c.String(maxLength: 255),
                        CreatedOn = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        UpdatedOn = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ImageDirectories", t => t.DirectoryName)
                .Index(t => new { t.FileName, t.DirectoryName }, unique: true, name: "IX_FileNameDirectoryName");
            
            CreateTable(
                "dbo.MultiPageImageFileFeatures",
                c => new
                    {
                        FeatureName = c.String(nullable: false, maxLength: 128),
                        MultiPageImageFileID = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.FeatureName, t.MultiPageImageFileID })
                .ForeignKey("dbo.FeatureTypes", t => t.FeatureName, cascadeDelete: true)
                .ForeignKey("dbo.MultiPageImageFiles", t => t.MultiPageImageFileID, cascadeDelete: true)
                .Index(t => t.FeatureName)
                .Index(t => t.MultiPageImageFileID);
            
            CreateTable(
                "dbo.LabelInstances",
                c => new
                    {
                        Label_LabelName = c.String(nullable: false, maxLength: 128),
                        Label_CategoryName = c.String(nullable: false, maxLength: 128),
                        Instance_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Label_LabelName, t.Label_CategoryName, t.Instance_ID })
                .ForeignKey("dbo.Labels", t => new { t.Label_LabelName, t.Label_CategoryName }, cascadeDelete: true)
                .ForeignKey("dbo.Instances", t => t.Instance_ID, cascadeDelete: true)
                .Index(t => new { t.Label_LabelName, t.Label_CategoryName })
                .Index(t => t.Instance_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MultiPageImageFileFeatures", "MultiPageImageFileID", "dbo.MultiPageImageFiles");
            DropForeignKey("dbo.MultiPageImageFileFeatures", "FeatureName", "dbo.FeatureTypes");
            DropForeignKey("dbo.ImageFiles", "MultiPageImageFileID", "dbo.MultiPageImageFiles");
            DropForeignKey("dbo.MultiPageImageFiles", "DirectoryName", "dbo.ImageDirectories");
            DropForeignKey("dbo.Labels", "CategoryName", "dbo.LabelCategories");
            DropForeignKey("dbo.LabelInstances", "Instance_ID", "dbo.Instances");
            DropForeignKey("dbo.LabelInstances", new[] { "Label_LabelName", "Label_CategoryName" }, "dbo.Labels");
            DropForeignKey("dbo.InstanceSetInstances", "InstanceSetRoleID", "dbo.InstanceSetRoles");
            DropForeignKey("dbo.InstanceSetInstances", "InstanceSetID", "dbo.InstanceSets");
            DropForeignKey("dbo.InstanceSetInstances", "InstanceID", "dbo.Instances");
            DropForeignKey("dbo.InstanceFeatures", "InstanceID", "dbo.Instances");
            DropForeignKey("dbo.InstanceFeatures", "FeatureName", "dbo.FeatureTypes");
            DropForeignKey("dbo.Instances", "ImageFileID", "dbo.ImageFiles");
            DropForeignKey("dbo.ImageFileFeatures", "ImageFileID", "dbo.ImageFiles");
            DropForeignKey("dbo.ImageFileFeatures", "FeatureName", "dbo.FeatureTypes");
            DropForeignKey("dbo.ImageFiles", "DirectoryName", "dbo.ImageDirectories");
            DropForeignKey("dbo.ImageDirectoryFeatures", "DirectoryName", "dbo.ImageDirectories");
            DropForeignKey("dbo.ImageDirectoryFeatures", "FeatureName", "dbo.FeatureTypes");
            DropForeignKey("Ref.Counties", "StateText", "Ref.States");
            DropIndex("dbo.LabelInstances", new[] { "Instance_ID" });
            DropIndex("dbo.LabelInstances", new[] { "Label_LabelName", "Label_CategoryName" });
            DropIndex("dbo.MultiPageImageFileFeatures", new[] { "MultiPageImageFileID" });
            DropIndex("dbo.MultiPageImageFileFeatures", new[] { "FeatureName" });
            DropIndex("dbo.MultiPageImageFiles", "IX_FileNameDirectoryName");
            DropIndex("dbo.Labels", new[] { "CategoryName" });
            DropIndex("dbo.InstanceSetInstances", new[] { "InstanceSetRoleID" });
            DropIndex("dbo.InstanceSetInstances", new[] { "InstanceID" });
            DropIndex("dbo.InstanceSetInstances", new[] { "InstanceSetID" });
            DropIndex("dbo.InstanceFeatures", new[] { "InstanceID" });
            DropIndex("dbo.InstanceFeatures", new[] { "FeatureName" });
            DropIndex("dbo.Instances", new[] { "ImageFileID" });
            DropIndex("dbo.ImageFileFeatures", new[] { "ImageFileID" });
            DropIndex("dbo.ImageFileFeatures", new[] { "FeatureName" });
            DropIndex("dbo.ImageFiles", new[] { "MultiPageImageFileID" });
            DropIndex("dbo.ImageFiles", "IX_FileNameDirectoryName");
            DropIndex("dbo.ImageDirectoryFeatures", new[] { "DirectoryName" });
            DropIndex("dbo.ImageDirectoryFeatures", new[] { "FeatureName" });
            DropIndex("Ref.Counties", new[] { "StateText" });
            DropTable("dbo.LabelInstances");
            DropTable("dbo.MultiPageImageFileFeatures");
            DropTable("dbo.MultiPageImageFiles");
            DropTable("dbo.LabelCategories");
            DropTable("dbo.Labels");
            DropTable("dbo.InstanceSetRoles");
            DropTable("dbo.InstanceSets");
            DropTable("dbo.InstanceSetInstances");
            DropTable("dbo.InstanceFeatures");
            DropTable("dbo.Instances");
            DropTable("dbo.ImageFileFeatures");
            DropTable("dbo.ImageFiles");
            DropTable("dbo.ImageDirectoryFeatures");
            DropTable("dbo.ImageDirectories");
            DropTable("dbo.FeatureTypes");
            DropTable("Ref.States");
            DropTable("Ref.Counties");
        }
    }
}
