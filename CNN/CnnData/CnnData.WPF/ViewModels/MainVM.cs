using CnnData.App.Interfaces;
using CnnData.Lib.BO;
using CnnData.WPF.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CnnData.WPF.ViewModels
{
  public class MainVM : ViewModelBase
  {
    public MainVM(IWindowService windowService, ImageFileService imageFileService)
    {
      this.WindowService = windowService;
      this.ImageFileService = imageFileService;
    }

    private const string BaseWindowTitle = "Image Tagger";

    public readonly IWindowService WindowService;
    public readonly ImageFileService ImageFileService;

    private ImageDirectory _CurrentDirectory;
    public ImageDirectory CurrentDirectory
    {
      get { return this._CurrentDirectory; }
      set { Set(() => this.CurrentDirectory, ref this._CurrentDirectory, value); }
    }

    internal void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      e.Cancel = false;
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
  }
}
