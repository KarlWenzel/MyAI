using CnnData.Lib.BO;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.WPF.ViewModels
{
  public class ImageFileVM : ViewModelBase
  {
    public ImageFileVM(ImageFile imageFile, bool inDatabase, bool inFileSystem, List<FeatureType> featureTypes)
    {
      this.DirectoryName = imageFile.DirectoryName;
      this.FileName = imageFile.FileName;
      this.InDatabase = inDatabase;
      this.InFileSystem = inFileSystem;

      this.Features = new Dictionary<string, string>();
      featureTypes.Where(x => x.UseWithImageFiles).ToList().ForEach(x => this.Features.Add(x.FeatureName, null));
      if (imageFile.ImageFileFeatures != null)
      {
        imageFile.ImageFileFeatures.ToList().ForEach(x => this.Features[x.FeatureName] = x.Value);
      }
      RaisePropertyChanged(() => this.Features);
    }

    public string DirectoryName { get; set; }
    public string FileName { get; set; }
    public bool InDatabase { get; set; }
    public bool InFileSystem { get; set; }
    public Dictionary<string, string> Features { get; set; }

    public Stream GetStream()
    {
      return File.OpenRead(Path.Combine(this.DirectoryName, this.FileName));
    }
  }
}
