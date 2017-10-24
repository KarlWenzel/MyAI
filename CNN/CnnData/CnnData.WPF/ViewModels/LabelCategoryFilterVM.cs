using CnnData.Lib.BO;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.WPF.ViewModels
{
  public class LabelCategoryFilterVM : ViewModelBase
  {
    public const string ALL_LABELS = "All";

    public LabelCategoryFilterVM()
    {
      this.LabelFilterVMs = new List<LabelFilterVM>();
    }

    public string CategoryName { get; set; }
    public List<LabelFilterVM> LabelFilterVMs { get; set; }

    public static List<LabelCategoryFilterVM> FromLabelCategories(IEnumerable<LabelCategory> labelCategories)
    {
      var filters = new List<LabelCategoryFilterVM>();

      foreach (var labelCategory in labelCategories)
      {
        var filter = new LabelCategoryFilterVM();
        filter.CategoryName = labelCategory.CategoryName;
        filter.LabelFilterVMs.Add(new LabelFilterVM(filter, ALL_LABELS, true));
        foreach (var label in labelCategory.Labels)
        {
          var newFilter = new LabelFilterVM(filter, label.LabelName);
          filter.LabelFilterVMs.Add(newFilter);
        }
        filters.Add(filter);
      }

      return filters;
    }


  }
}
