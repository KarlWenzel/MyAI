using CnnData.Lib.BO;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.WPF.ViewModels
{
  public class ImageDirectoryVM : ViewModelBase
  {
    public ImageDirectoryVM(ImageDirectory imageDirectory, List<FeatureType> featureTypes)
    {
      this.DirectoryName = imageDirectory.DirectoryName;

      this.Features = new Dictionary<string, string>();
      featureTypes.Where(x => x.UseWithImageDirectories).ToList().ForEach(x => this.Features.Add(x.FeatureName, null));
      imageDirectory.ImageDirectoryFeatures.ToList().ForEach(x => this.Features[x.FeatureName] = x.Value);
      RaisePropertyChanged(() => this.Features);
    }

    private string _DirectoryName;
    public string DirectoryName
    {
      get { return this._DirectoryName; }
      set { Set(() => this.DirectoryName, ref this._DirectoryName, value); }
    }
    
    public Dictionary<string, string> Features { get; set; }

  }
}
