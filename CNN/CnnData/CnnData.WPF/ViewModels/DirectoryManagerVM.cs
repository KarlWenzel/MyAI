using CnnData.App.Interfaces;
using CnnData.Lib.BO;
using CnnData.WPF.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CnnData.WPF.ViewModels
{
  public class DirectoryManagerVM: ViewModelBase, IWindowable
  {
    public const string BaseWindowTitle = "Folder Manager";

    public DirectoryManagerVM(MainVM mainVM)
    {
      this.MainVM = mainVM;
      this.ImageDirectories = new ObservableCollection<ImageDirectory>();

      var selectedDirectoryName = (mainVM.CurrentDirectory == null) ? null : mainVM.CurrentDirectory.DirectoryName;
      this.LoadImageDirectories(selectedDirectoryName);
    }
        
    private readonly MainVM MainVM;
    public Window Window { get; set; }

    private ImageDirectory _SelectedImageDirectory;
    public ImageDirectory SelectedImageDirectory
    {
      get { return this._SelectedImageDirectory; }
      set
      {
        Set(() => this.SelectedImageDirectory, ref this._SelectedImageDirectory, value);

        if (this._SelectCommand != null)
        {
          this._SelectCommand.RaiseCanExecuteChanged();
        }

        if (this.SelectedImageDirectory == null)
        {
          this.SelectedImageDirectoryVM = null;
        }
        else
        {
          var imageDirectory = this.MainVM.DataService.GetImageDirectory(this.SelectedImageDirectory.DirectoryName);
          this.SelectedImageDirectoryVM = new ImageDirectoryVM(imageDirectory, this.MainVM.FeatureTypes.ToList());
        }
      }
    }

    public ImageDirectoryVM _SelectedImageDirectoryVM;
    public ImageDirectoryVM SelectedImageDirectoryVM
    {
      get { return this._SelectedImageDirectoryVM; }
      set { Set(() => this.SelectedImageDirectoryVM, ref this._SelectedImageDirectoryVM, value); }
    }

    public ObservableCollection<ImageDirectory> ImageDirectories { get; set; }
    
    private void LoadImageDirectories(string selectedDirectoryName = null)
    {
      if (selectedDirectoryName == null)
      {
        selectedDirectoryName = (this.SelectedImageDirectoryVM == null) ? null : this.SelectedImageDirectoryVM.DirectoryName;
      }

      var list = new List<ImageDirectory>();
      foreach (var imageDirectory in this.MainVM.DataService.GetImageDirectories())
      {
        list.Add(imageDirectory);
      }
      this.ImageDirectories = new ObservableCollection<ImageDirectory>(list);
      RaisePropertyChanged(() => this.ImageDirectories);

      var found = (selectedDirectoryName == null) ? null : list.FirstOrDefault(x => x.DirectoryName == selectedDirectoryName);
      if (found != null)
      {
        this.SelectedImageDirectory = found;
      }
      else if (list.Any())
      {
        this.SelectedImageDirectory = list[0];
      }
      else
      {
        this.SelectedImageDirectoryVM = null;
      }
    }

    private void CloseWindow()
    {
      if (this.Window != null)
      {
        this.Window.Close();
      }
    }

    private RelayCommand _AddNewDirectoryCommand;
    public ICommand AddNewDirectoryCommand
    {
      get { return _AddNewDirectoryCommand ?? (_AddNewDirectoryCommand = new RelayCommand(() => { OnAddNewDirectoryCommand(); })); }
    }

    private void OnAddNewDirectoryCommand()
    {
      string directoryName;

      if (!this.MainVM.WindowService.ShowOpenSingleFolderDialog(out directoryName, "", ""))
      {
        return;
      }

      ImageDirectory newImageDirectory;
      var success = this.MainVM.DataService.CreateImageDirectory(directoryName, out newImageDirectory);

      if (success)
      {
        LoadImageDirectories(newImageDirectory.DirectoryName);
      }

    }

    private RelayCommand _SelectCommand;
    public ICommand SelectCommand
    {
      get { return _SelectCommand ?? (_SelectCommand = new RelayCommand(() => { OnSelect(); }, CanSelect)); }
    }

    public void OnSelect()
    {
      this.MainVM.CurrentDirectory = this.SelectedImageDirectory;
      this.CloseWindow();
    }

    private bool CanSelect()
    {
      return this.SelectedImageDirectory != null;
    }

    private RelayCommand _CancelCommand;
    public ICommand CancelCommand
    {
      get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(() => { OnCancel(); })); }
    }

    private void OnCancel()
    {
      this.CloseWindow();
    }
  }
}
