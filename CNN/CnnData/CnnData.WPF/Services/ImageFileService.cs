using CnnData.App;
using CnnData.Lib.BO;
using CnnData.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.WPF.Services
{
  public class ImageFileService
  {
    public ImageFileService()
    {
    }

    public List<ImageDirectory> GetImageDirectories()
    {
      using (var db = new ApplicationDbContext())
      {
        return db.ImageDirectories
          .OrderBy(x => x.DirectoryName)
          .ToList();
      }
    }

    public List<ImageFile> GetImageFiles(string imageDirectoryName)
    {
      using (var db = new ApplicationDbContext())
      {
        return db.ImageFiles
          .Where(x => x.DirectoryName == imageDirectoryName)
          .OrderBy(x => x.FileName)
          .ToList();
      }
    }

    public List<FileInfo> GetFileInfos(string directoryName)
    {
      var dir = new DirectoryInfo(directoryName);
      return dir.GetFiles().ToList();
    }

    public List<ImageFileVM> GetImageFileVMs(string directoryName)
    {
      return this.GetFileInfos(directoryName).Select(x => new ImageFileVM() { FileName = x.Name }).ToList();
    }
  }
}
