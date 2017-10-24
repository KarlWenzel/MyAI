using CnnData.App.Interfaces;
using CnnData.Lib.BO;
using CnnData.WPF.Interfaces;
using CnnData.WPF.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
      // note some setup work is also done in the MainWindow property set which is fired from the MainWindow.xaml code behind
    }

    private const string BaseWindowTitle = "Image Tagger";

    private Window _MainWindow;
    public Window MainWindow
    {
      /*
       *  NOTE that MainWindow's get is private.  This is so programmers avoid the mistake of trying to use it before it is 
       *  ready, as the Window is not set in the constructor by the Locator.  Instead, the programmer should register any 
       *  dependencies to the MainWindow in the setter method.
       */
      private get { return this._MainWindow; }
      set
      {
        Set(() => this.MainWindow, ref this._MainWindow, value);
        this.MainWindow.Closing += OnWindowClosing;
        this.ImageLabelerVM = new ImageLabelerVM(this, this.MainWindow);
      }
    }

    public readonly IWindowService WindowService;
    public readonly DataService DataService;

    private ImageLabelerVM _ImageLabelerVM;
    public ImageLabelerVM ImageLabelerVM
    {
      get { return this._ImageLabelerVM; }
      set { Set(() => this.ImageLabelerVM, ref this._ImageLabelerVM, value); }
    }

    public ObservableCollection<FeatureType> _FeatureTypes;
    public ObservableCollection<FeatureType> FeatureTypes
    {
      get { return this._FeatureTypes; }
      set { Set(() => this.FeatureTypes, ref this._FeatureTypes, value); }
    }

    private LabelCategory _CurrentLabelCategory;
    public LabelCategory CurrentLabelCategory
    {
      get { return this._CurrentLabelCategory; }
      set { Set(() => this.CurrentLabelCategory, ref this._CurrentLabelCategory, value); }
    }

    private ImageDirectory _CurrentDirectory;
    public ImageDirectory CurrentDirectory
    {
      get { return this._CurrentDirectory; }
      set
      {
        Set(() => this.CurrentDirectory, ref this._CurrentDirectory, value);
        this.ImageFileListVM = new ImageFileListVM(this, this.MainWindow);
      }
    }

    private ImageFileListVM _ImageFileListVM;
    public ImageFileListVM ImageFileListVM
    {
      get { return this._ImageFileListVM; }
      set
      {
        if (Set(() => this.ImageFileListVM, ref this._ImageFileListVM, value))
        {
          this.CurrentImageFileVM = this.ImageFileListVM.SelectedImageFileVM;
          this.ImageFileListVM.PropertyChanged += ImageFileListVM_PropertyChanged;
        }
      }
    }

    private bool _IsImageFileListLoaded;
    public bool IsImageFileListLoaded
    {
      get { return this._IsImageFileListLoaded; }
      set { Set(() => this.IsImageFileListLoaded, ref this._IsImageFileListLoaded, value); }
    }

    private ImageFileVM _CurrentImageFileVM;
    public ImageFileVM CurrentImageFileVM
    {
      get { return this._CurrentImageFileVM; }
      set
      {
        Set(() => this.CurrentImageFileVM, ref this._CurrentImageFileVM, value);
        if (this.CurrentImageFileVM != null && this.CurrentImageFileVM.InFileSystem)
        {
          try
          {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = this.CurrentImageFileVM.GetStream();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            this.CurrentImage = bitmap;
          }
          catch (Exception err)
          {
            this.CurrentImage = null;
          }
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
    
    private void ImageFileListVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "ImageFileVMs")
      {
        this.IsImageFileListLoaded = (this.ImageFileListVM != null && this.ImageFileListVM.ImageFileVMs.Any());
      }

      if (e.PropertyName == "SelectedImageFileVM")
      {
        this.CurrentImageFileVM = this.ImageFileListVM.SelectedImageFileVM;
      }
    }

    public void NextImage()
    {
      this.ImageFileListVM.SelectNextImage();
    }

    public void PrevImage()
    {
      this.ImageFileListVM.SelectPrevImage();
    }


  }
}
