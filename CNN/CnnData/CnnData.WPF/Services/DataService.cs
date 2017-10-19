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
        return db.LabelCategories
          .Include(x => x.Labels)
          .Include(x => x.Labels.Select(y => y.InputKey))
          .ToList();
      }
    }

    public ImageDirectory GetImageDirectory(string directoryName)
    {
      using (var db = new ApplicationDbContext())
      {
        return db.ImageDirectories
          .Include(x => x.ImageDirectoryFeatures)
          .FirstOrDefault(x => x.DirectoryName == directoryName);
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
          .Include(x => x.ImageFileLabels)
          .Include(x => x.ImageFileLabels.Select(y => y.Label))
          .Include(x => x.ImageFileLabels.Select(y => y.Label.InputKey))
          .Include(x => x.ImageFileLabels.Select(y => y.Label.BackgroundColor))
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
    
    public void AddLabel(ImageFileVM imageFileVM, Label label)
    {
      using (var db = new ApplicationDbContext())
      {
        label = db.Labels.FirstOrDefault(x => x.CategoryName == label.CategoryName && x.LabelName == label.LabelName);
        if (label == null)
        {
          return;
        }

        //var imageFile = db.ImageFiles.FirstOrDefault()
        db.ImageFiles.Attach(imageFileVM.ImageFile);

        var obsoleteLabels = imageFileVM.ImageFile.ImageFileLabels
          .Where(x => x.CategoryName == label.CategoryName && x.LabelName != label.LabelName)
          .ToList();

        foreach (var obsoleteLabel in obsoleteLabels)
        {
          imageFileVM.ImageFile.ImageFileLabels.Remove(obsoleteLabel);
        }

        var existingGoodLabel = imageFileVM.ImageFile.ImageFileLabels
          .FirstOrDefault(x => x.CategoryName == label.CategoryName && x.LabelName == label.LabelName);

        if (existingGoodLabel != null)
        {
          existingGoodLabel.SetByHuman = true;
        }
        else
        {
          imageFileVM.ImageFile.ImageFileLabels.Add(new ImageFileLabel()
          {
            Label = label,
            SetByHuman = true
          });
        }

        db.SaveChanges();
        imageFileVM.InDatabase = true;
      }
    }

    public void AddLabelToImageFile(ImageFileVM imageFileVM, Label label)
    {
      ImageFile imageFile;

      using (var db = new ApplicationDbContext())
      {
        label = db.Labels
          .Include(x => x.InputKey)
          .Include(x => x.BackgroundColor)
          .FirstOrDefault(x => x.CategoryName == label.CategoryName && x.LabelName == label.LabelName);

        if (label == null)
        {
          return;
        }

        imageFile = db.ImageFiles
          .Include(x => x.ImageFileFeatures)
          .Include(x => x.ImageFileLabels)
          .Include(x => x.ImageFileLabels.Select(y => y.Label))
          .Include(x => x.ImageFileLabels.Select(y => y.Label.InputKey))
          .Include(x => x.ImageFileLabels.Select(y => y.Label.BackgroundColor))
          .FirstOrDefault(x => x.DirectoryName == imageFileVM.ImageFile.DirectoryName && x.FileName == imageFileVM.ImageFile.FileName);

        if (imageFile == null)
        {
          imageFile = db.ImageFiles.Add(imageFileVM.ImageFile);
          //imageFile = db.ImageFiles.Add(new ImageFile() {
          //  FileName = imageFileVM.ImageFile.FileName,
          //  DirectoryName = imageFileVM.ImageFile.DirectoryName,
          //  ImageExtension = Path.GetExtension(imageFileVM.ImageFile.FileName),
          //  ImageFileLabels = new List<ImageFileLabel>()
          //});
        }

        var obsoleteLabels = imageFile.ImageFileLabels.Where(x => x.CategoryName == label.CategoryName && x.LabelName != label.LabelName).ToList();
        foreach (var obsoleteLabel in obsoleteLabels)
        {
          imageFile.ImageFileLabels.Remove(obsoleteLabel);
        }

        var existingGoodLabel = imageFile.ImageFileLabels.FirstOrDefault(x => x.CategoryName == label.CategoryName && x.LabelName == label.LabelName);
        if (existingGoodLabel != null)
        {
          existingGoodLabel.SetByHuman = true;
        }
        else
        {
          imageFile.ImageFileLabels.Add(new ImageFileLabel() {
            Label = label,
            SetByHuman = true
          });
        }

        db.SaveChanges();
      }

      imageFileVM.ImageFile = imageFile;
      imageFileVM.InDatabase = true;
    }
  }
}
