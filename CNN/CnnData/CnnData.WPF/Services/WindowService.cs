using CnnData.App.Interfaces;
using CnnData.WPF.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace CnnData.WPF.Services
{
  public class WindowService : IWindowService
  {
    /*
		 * Simple service that expects a ViewModel-to-View implicit template to be set up
		 * http://stackoverflow.com/a/25846192
		 */

    public void ShowWindow(object viewModel)
    {
      var win = new Window();
      win.Content = viewModel;
      if (viewModel is IWindowable)
      {
        (viewModel as IWindowable).Window = win;
      }
      win.Show();
    }

    public bool? ShowDialog(string title, object viewModel)
    {
      var win = new Window();
      win.Title = title;
      win.Content = viewModel;
      if (viewModel is IWindowable)
      {
        (viewModel as IWindowable).Window = win;
      }
      return win.ShowDialog();
    }

    public bool? ShowOpenSingleFileDialog(out string fileName, string filter, string initialDirectory)
    {
      fileName = null;
      var openFileDialog = new Microsoft.Win32.OpenFileDialog();

      openFileDialog.Multiselect = false;
      openFileDialog.Filter = filter;
      //openFileDialog.InitialDirectory = initialDirectory;

      var result = openFileDialog.ShowDialog();

      if (result ?? false)
      {
        fileName = openFileDialog.FileName;
      }

      return result;
    }

    public bool ShowOpenSingleFolderDialog(out string fileName, string filter, string initialDirectory)
    {
      fileName = null;
      using (var folderBrowserDialog = new FolderBrowserDialog())
      {
        var result = folderBrowserDialog.ShowDialog();
        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
        {
          fileName = folderBrowserDialog.SelectedPath;
          return true;
        }
      }

      return false;
    }

    public bool? ShowSaveFileDialog(out string fileName, string initialDirectory)
    {
      fileName = null;
      var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
      //saveFileDialog.InitialDirectory = initialDirectory;

      var result = saveFileDialog.ShowDialog();

      if (result ?? false)
      {
        fileName = saveFileDialog.FileName;
      }

      return result;
    }

    public YesNoCancel ShowYesNoCancelMessageBox(string message, string caption)
    {
      switch (System.Windows.MessageBox.Show(message, caption, MessageBoxButton.YesNoCancel))
      {
        case MessageBoxResult.Yes:
          return YesNoCancel.Yes;
        case MessageBoxResult.No:
          return YesNoCancel.No;
        default:
          return YesNoCancel.Cancel;
      }
    }
  }
}
