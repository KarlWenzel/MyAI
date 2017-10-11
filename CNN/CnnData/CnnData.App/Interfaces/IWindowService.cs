using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.App.Interfaces
{
  public enum YesNoCancel
  {
    Yes,
    No,
    Cancel
  }

  public interface IWindowService
  {
    void ShowWindow(object viewModel);
    bool? ShowDialog(string title, object viewModel);
    bool? ShowOpenSingleFileDialog(out string fileName, string filter, string initialDirectory);
    bool ShowOpenSingleFolderDialog(out string fileName, string filter, string initialDirectory);
    bool? ShowSaveFileDialog(out string fileName, string initialDirectory);
    YesNoCancel ShowYesNoCancelMessageBox(string message, string caption);
  }
}
