using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.WPF.ViewModels
{
  public class LabelFilterVM : ViewModelBase
  {
    public LabelFilterVM(LabelCategoryFilterVM labelCategoryFilterVM, string labelName, bool isSelected = false)
    {
      this.LabelName = labelName;
      this.IsSelected = isSelected;
      this.LabelCategoryFilterVM = labelCategoryFilterVM;
    }

    public LabelCategoryFilterVM LabelCategoryFilterVM { get; private set; }

    public string LabelName { get; set; }

    private bool _IsSelected;
    public bool IsSelected
    {
      get { return this._IsSelected; }
      set
      {
        if (Set(() => this.IsSelected, ref this._IsSelected, value) && this.LabelCategoryFilterVM != null)
        {
          this.LabelCategoryFilterVM.RaisePropertyChanged();
        }
      }
    }


  }
}
