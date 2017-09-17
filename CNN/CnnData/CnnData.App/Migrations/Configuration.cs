namespace CnnData.App.Migrations
{
  using CnnData.Lib.BO;
  using System;
  using System.Collections.Generic;
  using System.Data.Entity;
  using System.Data.Entity.Migrations;
  using System.Linq;

  internal sealed class Configuration : DbMigrationsConfiguration<CnnData.App.ApplicationDbContext>
  {
    public Configuration()
    {
      AutomaticMigrationsEnabled = true;
      AutomaticMigrationDataLossAllowed = true;
    }

    protected override void Seed(CnnData.App.ApplicationDbContext context)
    {
      SeedRoles(context);
      SeedLabels(context);
    }

    private void SeedRoles(CnnData.App.ApplicationDbContext context)
    {
      InstanceSetRole.GetRoles().ForEach(x => context.InstanceSetRoles.AddOrUpdate(x));
    }
    
    private void SeedLabels(CnnData.App.ApplicationDbContext context)
    {
      var firstPageCategory = context.LabelCategories.FirstOrDefault(x => x.CategoryName == "FirstNotFirstPage");
      if (firstPageCategory == null)
      {
        firstPageCategory = new LabelCategory() { CategoryName = "FirstNotFirstPage" };
        context.LabelCategories.Add(firstPageCategory);
      }

      foreach (var labelName in new List<string>() { "FirstPage", "NotFirstPage" })
      {
        var label = context.Labels.FirstOrDefault(x => x.LabelName == labelName && x.CategoryName == firstPageCategory.CategoryName);
        if (label == null)
        {
          label = new Label() { LabelName = labelName, LabelCategory = firstPageCategory };
          context.Labels.Add(label);
        }
      }
    }


  }
}
