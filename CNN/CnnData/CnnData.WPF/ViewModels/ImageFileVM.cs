using CnnData.Lib.BO;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.WPF.ViewModels
{
  public class ImageFileVM : ViewModelBase
  {
    // contructor is private: use the public static generator methods FromDB() and FromFileInfo()
    private ImageFileVM(ImageFile imageFile, bool inDatabase, bool inFileSystem, MainVM mainVM)
    {
      this.MainVM = mainVM;

      // Features and LabelVMs are from function call in ImageFile property setter
      this.Features = new Dictionary<string, string>();
      this.LabelVMs = new ObservableCollection<LabelVM>();

      this.ImageFile = imageFile;
      this.InDatabase = inDatabase;
      this.InFileSystem = inFileSystem;
      
      mainVM.PropertyChanged += MainVM_PropertyChanged;
    }

    private readonly MainVM MainVM;

    private ImageFile _ImageFile;
    public ImageFile ImageFile
    {
      get { return this._ImageFile; }
      set
      {
        Set(() => this.ImageFile, ref this._ImageFile, value);
        SetLabelsAndFeatures();
      }
    }
    
    private bool _InDatabase;
    public bool InDatabase
    {
      get { return this._InDatabase; }
      set { Set(() => this.InDatabase, ref this._InDatabase, value); }
    }

    public bool InFileSystem { get; set; }
    public Dictionary<string, string> Features { get; set; }
    public ObservableCollection<LabelVM> LabelVMs { get; set; }

    private LabelVM _CurrentLabelVM;
    public LabelVM CurrentLabelVM
    {
      get { return this._CurrentLabelVM; }
      set {
        Set(() => this.CurrentLabelVM, ref this._CurrentLabelVM, value);
      }
    }
    
    private void MainVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "CurrentLabelCategory")
      {
        SetCurrentLabelVM();
      }
    }

    public Stream GetStream()
    {
      return File.OpenRead(Path.Combine(this.ImageFile.DirectoryName, this.ImageFile.FileName));
    }

    public void SetCurrentLabelVM()
    {
      this.CurrentLabelVM = this.LabelVMs.FirstOrDefault(x => x.LabelObject.CategoryName == this.MainVM.CurrentLabelCategory.CategoryName);
    }

    public void SetLabelsAndFeatures()
    {
      this.LabelVMs.Clear();
      foreach (var imageFileLabel in this.ImageFile.ImageFileLabels)
      {
        this.LabelVMs.Add(new LabelVM(imageFileLabel.Label));
      }

      SetCurrentLabelVM();

      this.Features.Clear();
      foreach (var ft in this.MainVM.FeatureTypes.Where(x => x.UseWithImageFiles).ToList())
      {
        // initialize Features Dict with all possible feature keys
        this.Features.Add(ft.FeatureName, null);
      }

      if (this.ImageFile.ImageFileFeatures != null)
      {
        // now refer to the data stored with the ImageFile and set the features key values when appropriate
        this.ImageFile.ImageFileFeatures.ToList().ForEach(x => this.Features[x.FeatureName] = x.Value);
        this.RaisePropertyChanged(() => this.Features);
      }
    }

    public static ImageFileVM FromDB(ImageFile imageFile, bool inFileSystem, MainVM mainVM)
    {
      return new ImageFileVM(imageFile, true, inFileSystem, mainVM);
    }

    public static ImageFileVM FromFileInfo(FileInfo fileInfo, MainVM mainVM)
    {
      var imageFile = new ImageFile()
      {
        FileName = fileInfo.Name,
        DirectoryName = fileInfo.DirectoryName,
        ImageExtension = fileInfo.Extension,
        HeightPixels = 0,
        WidthPixels = 0,
        ImageFileFeatures = new List<ImageFileFeature>(),
        ImageFileLabels = new List<ImageFileLabel>()
      };

      return new ImageFileVM(imageFile, false, true, mainVM);
    }
  }
}
