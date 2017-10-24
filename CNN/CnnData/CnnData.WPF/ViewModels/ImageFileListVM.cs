using CnnData.Lib.BO;
using CnnData.WPF.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace CnnData.WPF.ViewModels
{
  public enum TagStatuses
  {
    All,
    Tagged,
    Untagged
  }

  public class ImageFileListVM : ViewModelBase
  {
    public ImageFileListVM(MainVM mainVM, Window mainWindow)
    {
      this.MainVM = mainVM;
      this.MainWindow = mainWindow;

      if (this.MainVM.CurrentDirectory == null)
      {
        return;
      }

      var fileList = new List<ImageFileVM>();
      var dbImageFiles = this.MainVM.DataService.GetImageFiles(this.MainVM.CurrentDirectory.DirectoryName);
      var fsFileInfos = this.MainVM.DataService.GetFileInfos(this.MainVM.CurrentDirectory.DirectoryName);

      foreach (var imageFile in dbImageFiles)
      {
        var inFileSystem = fsFileInfos.Any(x => x.Name == imageFile.FileName);
        fileList.Add(ImageFileVM.FromDB(imageFile, inFileSystem, this.MainVM));
      }

      foreach (var fileInfo in fsFileInfos.Where(x => !dbImageFiles.Any(y => y.FileName == x.Name)))
      {
        fileList.Add(ImageFileVM.FromFileInfo(fileInfo, this.MainVM));
      }

      this.ImageFileVMs = new ObservableCollection<ImageFileVM>(fileList.OrderBy(x => x.ImageFile.FileName));
      this.FilteredImageFileVMs = CollectionViewSource.GetDefaultView(this.ImageFileVMs);

      this.TagStatusFilter = TagStatuses.All;
      this.LabelCategoryFilterVMs = new ObservableCollection<LabelCategoryFilterVM>(
        LabelCategoryFilterVM.FromLabelCategories(this.MainVM.DataService.GetLabelCategories())
      );
      
      this.FilteredImageFileVMs.MoveCurrentToFirst();
      this.SelectedImageFileVM = this.FilteredImageFileVMs.CurrentItem as ImageFileVM;
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
        this.FilteredImageFileVMs.MoveCurrentTo(this.SelectedImageFileVM);
      }
    }
    
    public ObservableCollection<LabelCategoryFilterVM> LabelCategoryFilterVMs { get; set; }

    public ObservableCollection<ImageFileVM> ImageFileVMs { get; set; }
    public ICollectionView FilteredImageFileVMs { get; set; }

    private string _FileNameFilter;
    public string FileNameFilter
    {
      get { return this._FileNameFilter; }
      set
      {
        Set(() => this.FileNameFilter, ref this._FileNameFilter, value);
        SetFilter();
      }
    }
    
    private TagStatuses _TagStatusFilter;
    public TagStatuses TagStatusFilter
    {
      get { return this._TagStatusFilter; }
      set
      {
        Set(() => this.TagStatusFilter, ref this._TagStatusFilter, value);
        SetFilter();
      }
    }

    private RelayCommand _ClearFileNameFilterCommand;
    public ICommand ClearFileNameFilterCommand
    {
      get { return _ClearFileNameFilterCommand ?? (_ClearFileNameFilterCommand = new RelayCommand(() => { OnClearFileNameFilter(); })); }
    }

    private void OnClearFileNameFilter()
    {
      this.FileNameFilter = string.Empty;
    }

    private RelayCommand _ApplyLabelFilterCommand;
    public ICommand ApplyLabelFilterCommand
    {
      get { return _ApplyLabelFilterCommand ?? (_ApplyLabelFilterCommand = new RelayCommand(() => { SetFilter(); })); }
    }

    private void SetFilter()
    {
      this.FilteredImageFileVMs.Filter = (item) => 
      {
        var vm = item as ImageFileVM;
        if (vm == null || vm.ImageFile == null)
        {
          return false;
        }

        var tagStatusMatch = ImageFileHasTagStatus(vm.ImageFile, this.TagStatusFilter, this.MainVM.CurrentLabelCategory);
        var labelsTextMatch = ImageFileHasLabels(vm.ImageFile, this.LabelCategoryFilterVMs);
        var filePatternMatch = string.IsNullOrWhiteSpace(this.FileNameFilter) || vm.ImageFile.FileName.Contains(this.FileNameFilter);

        return tagStatusMatch && labelsTextMatch && filePatternMatch;
      };

      if (!this.FilteredImageFileVMs.Contains(this.SelectedImageFileVM))
      {
        this.FilteredImageFileVMs.MoveCurrentToFirst();
        this.SelectedImageFileVM = this.FilteredImageFileVMs.CurrentItem as ImageFileVM;
      }
    }

    private static bool ImageFileHasTagStatus(ImageFile imageFile, TagStatuses tagStatus, LabelCategory labelCategory)
    {
      switch (tagStatus)
      {
        case TagStatuses.All:
          return true;
        case TagStatuses.Tagged:
          return imageFile.ImageFileLabels.Any(x => x.Label.CategoryName == labelCategory.CategoryName);
        case TagStatuses.Untagged:
          return !imageFile.ImageFileLabels.Any(x => x.Label.CategoryName == labelCategory.CategoryName);
        default:
          return false;
      }
    }

    private static bool ImageFileHasLabels(ImageFile imageFile, IEnumerable<LabelCategoryFilterVM> labelCategoryFilterVMs)
    {
      if (labelCategoryFilterVMs == null || !labelCategoryFilterVMs.Any())
      {
        return true;
      }

      foreach (var category in labelCategoryFilterVMs)
      {
        var label = category.LabelFilterVMs.FirstOrDefault(x => x.IsSelected && x.LabelName != LabelCategoryFilterVM.ALL_LABELS);
        
        if (label == null)
        {
          continue;
        }

        if (!imageFile.ImageFileLabels.Any(x => x.LabelName == label.LabelName && x.CategoryName == category.CategoryName))
        {
          return false;
        }
      }

      return true;
    }

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
      this.FilteredImageFileVMs.MoveCurrentToNext();

      if (this.FilteredImageFileVMs.IsCurrentAfterLast)
      {
        this.FilteredImageFileVMs.MoveCurrentToLast();
      }

      this.SelectedImageFileVM = this.FilteredImageFileVMs.CurrentItem as ImageFileVM;
    }

    public void SelectPrevImage()
    {
      this.FilteredImageFileVMs.MoveCurrentToPrevious();

      if (this.FilteredImageFileVMs.IsCurrentBeforeFirst)
      {
        this.FilteredImageFileVMs.MoveCurrentToFirst();
      }

      this.SelectedImageFileVM = this.FilteredImageFileVMs.CurrentItem as ImageFileVM;
    }


  }
}
