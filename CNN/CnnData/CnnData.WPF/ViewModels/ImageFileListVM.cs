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
  public class ImageFileListVM : ViewModelBase
  {
    public ImageFileListVM(MainVM mainVM)
    {
      this.MainVM = mainVM;
      this.ImageFileVMs = new ObservableCollection<ImageFileVM>();

      if (this.MainVM.CurrentDirectory != null)
      {
        var list = new List<ImageFileVM>();

        var dbImageFiles = this.MainVM.DataService.GetImageFiles(this.MainVM.CurrentDirectory.DirectoryName);
        var fsFileInfos = this.MainVM.DataService.GetFileInfos(this.MainVM.CurrentDirectory.DirectoryName);

        foreach (var imageFile in dbImageFiles)
        {
          var inFileSystem = fsFileInfos.Any(x => x.Name == imageFile.FileName);
          list.Add(new ImageFileVM(imageFile, true, inFileSystem, this.MainVM.FeatureTypes.ToList()));
        }

        foreach (var fileInfo in fsFileInfos.Where(x => !dbImageFiles.Any(y => y.FileName == x.Name)))
        {
          var imageFile = new ImageFile() { FileName = fileInfo.Name, DirectoryName = fileInfo.DirectoryName, HeightPixels = 0, WidthPixels = 0 };
          list.Add(new ImageFileVM(imageFile, false, true, this.MainVM.FeatureTypes.ToList()));
        }
        
        this.ImageFileVMs = new ObservableCollection<ImageFileVM>(list.OrderBy(x => x.FileName));
      }

      RaisePropertyChanged(() => this.ImageFileVMs);
    }

    public readonly MainVM MainVM;

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
  }
}
