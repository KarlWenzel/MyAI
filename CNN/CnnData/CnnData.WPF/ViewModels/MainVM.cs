using CnnData.App.Interfaces;
using CnnData.Lib.BO;
using CnnData.WPF.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CnnData.WPF.ViewModels
{
  public class MainVM : ViewModelBase
  {
    public MainVM(IWindowService windowService, DataService dataService)
    {
      this.WindowService = windowService;
      this.DataService = dataService;
      this.FeatureTypes = new ObservableCollection<FeatureType>(this.DataService.GetFeatureTypes());
      this.LabelCategories = new ObservableCollection<LabelCategory>(this.DataService.GetLabelCategories());
    }

    private const string BaseWindowTitle = "Image Tagger";

    public readonly IWindowService WindowService;
    public readonly DataService DataService;
    
    public ObservableCollection<FeatureType> FeatureTypes { get; set; }
    public ObservableCollection<LabelCategory> LabelCategories { get; set; }

    private LabelCategory _SelectedLabelCategory;
    public LabelCategory SelectedLabelCategory
    {
      get { return this._SelectedLabelCategory; }
      set { Set(() => this.SelectedLabelCategory, ref this._SelectedLabelCategory, value); }
    }

    private ImageDirectory _CurrentDirectory;
    public ImageDirectory CurrentDirectory
    {
      get { return this._CurrentDirectory; }
      set
      {
        Set(() => this.CurrentDirectory, ref this._CurrentDirectory, value);
        this.ImageFileListVM = new ImageFileListVM(this);
        RaisePropertyChanged(() => this.ImageFileListVM);
      }
    }

    public ImageFileListVM ImageFileListVM { get; set; }

    private ImageFileVM _CurrentImageFileVM;
    public ImageFileVM CurrentImageFileVM
    {
      get { return this._CurrentImageFileVM; }
      set
      {
        Set(() => this.CurrentImageFileVM, ref this._CurrentImageFileVM, value);
        if (this.CurrentImageFileVM != null && this.CurrentImageFileVM.InFileSystem)
        {
          var bitmap = new BitmapImage();
          bitmap.BeginInit();
          bitmap.StreamSource = this.CurrentImageFileVM.GetStream();
          bitmap.CacheOption = BitmapCacheOption.OnLoad;
          bitmap.EndInit();
          this.CurrentImage = bitmap;
        }
      }
    }

    private BitmapImage _CurrentImage;
    public BitmapImage CurrentImage
    {
      get { return this._CurrentImage; }
      private set { Set(() => this.CurrentImage, ref this._CurrentImage, value); }
    }

    private RelayCommand _OpenDirectoryManagerCommand;
    public ICommand OpenDirectoryManagerCommand
    {
      get { return _OpenDirectoryManagerCommand ?? (_OpenDirectoryManagerCommand = new RelayCommand(() => { OpenDirectoryManager(); })); }
    }

    private void OpenDirectoryManager()
    {
      var directoryManagerVM = new DirectoryManagerVM(this);
      this.WindowService.ShowDialog(DirectoryManagerVM.BaseWindowTitle, directoryManagerVM);
    }

    internal void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      e.Cancel = false;
    }
  }
}
