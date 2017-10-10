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
    public DirectoryManagerVM(MainVM mainVM)
    {
      this.MainVM = mainVM;
      this.ImageDirectories = new ObservableCollection<ImageDirectory>();
      this.LoadImageDirectories();
    }
    
    public const string BaseWindowTitle = "Folder Manager";
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
      }
    }

    public ObservableCollection<ImageDirectory> ImageDirectories { get; set; }


    private void LoadImageDirectories()
    {
      ImageDirectories.Clear();
      foreach (var imageDirectory in this.MainVM.ImageFileService.GetImageDirectories())
      {
        ImageDirectories.Add(imageDirectory);
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
      MessageBox.Show("Implement ME");
    }

    private RelayCommand _SelectCommand;
    public ICommand SelectCommand
    {
      get { return _SelectCommand ?? (_SelectCommand = new RelayCommand(() => { OnSelect(); }, CanSelect)); }
    }

    private void OnSelect()
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
