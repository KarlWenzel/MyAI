using CnnData.Lib.BO;
using CnnData.WPF.Interfaces;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CnnData.WPF.ViewModels
{
  public class ImageFileListVM : ViewModelBase
  {
    public ImageFileListVM(MainVM mainVM, Window mainWindow)
    {
      this.MainVM = mainVM;
      this.MainWindow = mainWindow;
      this.ImageFileVMs = new ObservableCollection<ImageFileVM>();

      if (this.MainVM.CurrentDirectory != null)
      {
        var list = new List<ImageFileVM>();

        var dbImageFiles = this.MainVM.DataService.GetImageFiles(this.MainVM.CurrentDirectory.DirectoryName);
        var fsFileInfos = this.MainVM.DataService.GetFileInfos(this.MainVM.CurrentDirectory.DirectoryName);

        foreach (var imageFile in dbImageFiles)
        {
          var inFileSystem = fsFileInfos.Any(x => x.Name == imageFile.FileName);
          list.Add(ImageFileVM.FromDB(imageFile, inFileSystem, this.MainVM));
        }

        foreach (var fileInfo in fsFileInfos.Where(x => !dbImageFiles.Any(y => y.FileName == x.Name)))
        {
          list.Add(ImageFileVM.FromFileInfo(fileInfo, this.MainVM));
        }
        
        this.ImageFileVMs = new ObservableCollection<ImageFileVM>(list.OrderBy(x => x.ImageFile.FileName));
      }

      RaisePropertyChanged(() => this.ImageFileVMs);
    }

    public readonly MainVM MainVM;
    public readonly Window MainWindow;

    private ImageFileVM _SelectedImageFileVM;
    public ImageFileVM SelectedImageFileVM
    {
      get { return this._SelectedImageFileVM; }
      set
      {
        Set(() => this.SelectedImageFileVM, ref this._SelectedImageFileVM, value);
        this.MainVM.CurrentImageFileVM = this.SelectedImageFileVM;
      }
    }
    
    public ObservableCollection<ImageFileVM> ImageFileVMs { get; set; }

    public void SetLabel(Label label)
    {
      if (this.SelectedImageFileVM == null)
      {
        return;
      }

      this.MainVM.DataService.AddLabelToImageFile(this.SelectedImageFileVM, label);
    }

    public void SelectNextImage()
    {
      if (!this.ImageFileVMs.Any())
      {
        this.SelectedImageFileVM = null;
        return;
      }

      if (this.SelectedImageFileVM == null)
      {
        this.SelectedImageFileVM = this.ImageFileVMs[0];
        return;
      }

      int nextIndex = (this.ImageFileVMs.IndexOf(this.SelectedImageFileVM) + 1) % this.ImageFileVMs.Count;
      this.SelectedImageFileVM = this.ImageFileVMs[nextIndex];
    }

    public void SelectPrevImage()
    {
      if (!this.ImageFileVMs.Any())
      {
        this.SelectedImageFileVM = null;
        return;
      }

      if (this.SelectedImageFileVM == null)
      {
        this.SelectedImageFileVM = this.ImageFileVMs[this.ImageFileVMs.Count - 1];
        return;
      }

      int prevIndex = this.ImageFileVMs.IndexOf(this.SelectedImageFileVM) - 1;
      if (prevIndex < 0)
      {
        prevIndex = this.ImageFileVMs.Count - 1;
      }

      this.SelectedImageFileVM = this.ImageFileVMs[prevIndex];
    }


  }
}
