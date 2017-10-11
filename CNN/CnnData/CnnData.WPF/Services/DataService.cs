using CnnData.App;
using CnnData.Lib.BO;
using CnnData.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CnnData.WPF.Services
{
  public class DataService
  {
    public DataService()
    {
    }

    public bool CreateImageDirectory(string directoryName, out ImageDirectory imageDirectory)
    {
      try
      {
        using (var db = new ApplicationDbContext())
        {
          imageDirectory = db.ImageDirectories.FirstOrDefault(x => x.DirectoryName == directoryName);
          if (imageDirectory != null)
          {
            return false;
          }

          imageDirectory = new ImageDirectory() { DirectoryName = directoryName };
          db.ImageDirectories.Add(imageDirectory);
          db.SaveChanges();
        }
      }
      catch (Exception err)
      {
        imageDirectory = null;
        return false;
      }

      return true;
    }

    public List<FeatureType> GetFeatureTypes()
    {
      using (var db = new ApplicationDbContext())
      {
        return db.FeatureTypes.ToList();
      }
    }

    public List<LabelCategory> GetLabelCategories()
    {
      using (var db = new ApplicationDbContext())
      {
        return db.LabelCategories.Include(x => x.Labels).ToList();
      }
    }

    public ImageDirectory GetImageDirectory(string directoryName)
    {
      using (var db = new ApplicationDbContext())
      {
        return db.ImageDirectories.Include(x => x.ImageDirectoryFeatures).FirstOrDefault(x => x.DirectoryName == directoryName);
      }
    }

    public List<ImageDirectory> GetImageDirectories()
    {
      using (var db = new ApplicationDbContext())
      {
        return db.ImageDirectories
          .Include(x => x.ImageDirectoryFeatures)
          .OrderBy(x => x.DirectoryName)
          .ToList();
      }
    }

    public List<ImageFile> GetImageFiles(string imageDirectoryName)
    {
      using (var db = new ApplicationDbContext())
      {
        return db.ImageFiles
          .Include(x => x.ImageFileFeatures)
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

    //public List<ImageFileVM> GetImageFileVMs(string directoryName)
    //{
    //  return this.GetFileInfos(directoryName).Select(x => new ImageFileVM() { FileName = x.Name }).ToList();
    //}
  }
}
