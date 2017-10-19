namespace CnnData.App.Migrations
{
  using CnnData.Lib.BO;
  using CnnData.Lib.BO.ReferenceData;
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
      SeedInputKeys(context);
      SeedModifierKeys(context);
      SeedWinMediaColors(context);
      SeedLabels(context);
    }

    private void SeedRoles(CnnData.App.ApplicationDbContext context)
    {
      InstanceSetRole.GetRoles().ForEach(x => context.InstanceSetRoles.AddOrUpdate(x));
    }

    private void SeedInputKeys(CnnData.App.ApplicationDbContext context)
    {
      InputKey.GetInputKeys().ForEach(x => context.InputKeys.AddOrUpdate(x));
    }

    private void SeedModifierKeys(CnnData.App.ApplicationDbContext context)
    {
      ModifierKey.GetModifierKeys().ForEach(x => context.ModifierKeys.AddOrUpdate(x));
    }

    private void SeedWinMediaColors(CnnData.App.ApplicationDbContext context)
    {
      WinMediaColor.GetWinMediaColors().ForEach(x => context.WinMediaColors.AddOrUpdate(x));
    }

    private void SeedLabels(CnnData.App.ApplicationDbContext context)
    {
      var firstPageCategory = context.LabelCategories.FirstOrDefault(x => x.CategoryName == "PageSequence");
      if (firstPageCategory == null)
      {
        firstPageCategory = new LabelCategory() { CategoryName = "PageSequence" };
        context.LabelCategories.Add(firstPageCategory);
      }

      foreach (var labelData in new List<Tuple<string, string, string, string, string>>() {
        Tuple.Create("FirstPage", "F", "None", "LightGreen", "Black"),
        Tuple.Create("NotFirstPage", "D", "None", "LightSteelBlue", "Black"),
        Tuple.Create("TooHard", "Space", "None", "Black", "White")})
      {
        var label = context.Labels.FirstOrDefault(x => x.LabelName == labelData.Item1 && x.CategoryName == firstPageCategory.CategoryName);
        if (label == null)
        {
          label = new Label(labelData.Item1, firstPageCategory.CategoryName, labelData.Item2, labelData.Item3, labelData.Item4, labelData.Item5);
          context.Labels.Add(label);
        }
      }

      var instrumentTypeCategory = context.LabelCategories.FirstOrDefault(x => x.CategoryName == "InstrumentType");
      if (instrumentTypeCategory == null)
      {
        instrumentTypeCategory = new LabelCategory() { CategoryName = "InstrumentType" };
        context.LabelCategories.Add(instrumentTypeCategory);
      }

      foreach (var labelData in new List<Tuple<string, string, string, string, string>>() {
        Tuple.Create("OilGasLease", "D1", "None", "Yellow", "Black"),
        Tuple.Create("MineralDeed", "D2", "None", "Red", "White"),
        Tuple.Create("WarrantyDeed", "D3", "None", "Purple", "White"),
        Tuple.Create("TooHard", "Space", "None", "Black", "White") })
      {
        var label = context.Labels.FirstOrDefault(x => x.LabelName == labelData.Item1 && x.CategoryName == instrumentTypeCategory.CategoryName);
        if (label == null)
        {
          label = new Label(labelData.Item1, instrumentTypeCategory.CategoryName, labelData.Item2, labelData.Item3, labelData.Item4, labelData.Item5);
          context.Labels.Add(label);
        }
      }

    }


  }
}
