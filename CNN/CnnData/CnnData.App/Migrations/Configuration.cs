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
      var firstPageCategory = context.LabelCategories.FirstOrDefault(x => x.CategoryName == "PageSequence");
      if (firstPageCategory == null)
      {
        firstPageCategory = new LabelCategory() { CategoryName = "PageSequence" };
        context.LabelCategories.Add(firstPageCategory);
      }

      foreach (var labelData in new List<Tuple<string, string>>() {
        Tuple.Create("FirstPage", "f"),
        Tuple.Create("NotFirstPage", "d")})
      {
        var label = context.Labels.FirstOrDefault(x => x.LabelName == labelData.Item1 && x.CategoryName == firstPageCategory.CategoryName);
        if (label == null)
        {
          label = new Label() { LabelName = labelData.Item1, LabelCategory = firstPageCategory, HotKey = labelData.Item2 };
          context.Labels.Add(label);
        }
      }


      var instrumentTypeCategory = context.LabelCategories.FirstOrDefault(x => x.CategoryName == "InstrumentType");
      if (instrumentTypeCategory == null)
      {
        instrumentTypeCategory = new LabelCategory() { CategoryName = "InstrumentType" };
        context.LabelCategories.Add(instrumentTypeCategory);
      }

      foreach (var labelData in new List<Tuple<string, string>>() {
        Tuple.Create("OilGasLease", "1"),
        Tuple.Create("MineralDeed", "2"),
        Tuple.Create("WarrantyDeed", "3") })
      {
        var label = context.Labels.FirstOrDefault(x => x.LabelName == labelData.Item1 && x.CategoryName == instrumentTypeCategory.CategoryName);
        if (label == null)
        {
          label = new Label() { LabelName = labelData.Item1, LabelCategory = instrumentTypeCategory, HotKey = labelData.Item2 };
          context.Labels.Add(label);
        }
      }

    }


  }
}
