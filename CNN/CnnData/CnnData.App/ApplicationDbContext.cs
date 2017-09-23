using CnnData.Lib.BO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.App
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext() : base("name=ApplicationDbContext")
    {
    }

    public virtual DbSet<FeatureType> FeatureTypes { get; set; }
    public virtual DbSet<ImageDirectory> ImageDirectories { get; set; }
    public virtual DbSet<ImageDirectoryFeature> ImageDirectoryFeatures { get; set; }
    public virtual DbSet<ImageFile> ImageFiles { get; set; }
    public virtual DbSet<ImageFileFeature> ImageFileFeatures { get; set; }
    public virtual DbSet<Instance> Instances { get; set; }
    public virtual DbSet<InstanceFeature> InstanceFeatures { get; set; }
    public virtual DbSet<InstanceSetGroup> InstanceSetGroups { get; set; }
    public virtual DbSet<InstanceSetRole> InstanceSetRoles { get; set; }
    public virtual DbSet<InstanceSet> InstanceSets { get; set; }
    public virtual DbSet<LabelCategory> LabelCategories { get; set; }
    public virtual DbSet<Label> Labels { get; set; }
  }
}
