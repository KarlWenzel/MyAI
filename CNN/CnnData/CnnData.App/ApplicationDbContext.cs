using CnnData.Lib.BO;
using CnnData.Lib.BO.ReferenceData;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.App
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext() : base("name=ApplicationDbContext")
    {
      var objectContext = ((IObjectContextAdapter)this).ObjectContext;
      objectContext.SavingChanges += OnSavingChanged;
    }

    private void OnSavingChanged(object sender, EventArgs e)
    {
      var now = DateTime.Now;
      foreach (var entry in this.ChangeTracker.Entries<IDatedEntity>())
      {
        var entity = entry.Entity;
        switch (entry.State)
        {
          case EntityState.Added:
            entity.CreatedOn = now;
            entity.UpdatedOn = now;
            break;
          case EntityState.Modified:
            entity.UpdatedOn = now;
            break;
        }
      }
      this.ChangeTracker.DetectChanges();
    }

    #region dbo Schema
    public virtual DbSet<FeatureType> FeatureTypes { get; set; }
    public virtual DbSet<ImageDirectory> ImageDirectories { get; set; }
    public virtual DbSet<ImageDirectoryFeature> ImageDirectoryFeatures { get; set; }
    public virtual DbSet<ImageFile> ImageFiles { get; set; }
    public virtual DbSet<ImageFileFeature> ImageFileFeatures { get; set; }
    public virtual DbSet<Instance> Instances { get; set; }
    public virtual DbSet<InstanceFeature> InstanceFeatures { get; set; }
    public virtual DbSet<InstanceSetInstance> InstanceSetInstances { get; set; }
    public virtual DbSet<InstanceSetRole> InstanceSetRoles { get; set; }
    public virtual DbSet<InstanceSet> InstanceSets { get; set; }
    public virtual DbSet<LabelCategory> LabelCategories { get; set; }
    public virtual DbSet<Label> Labels { get; set; }
    public virtual DbSet<MultiPageImageFile> MultiPageImageFiles { get; set; }
    public virtual DbSet<MultiPageImageFileFeature> MultiPageImageFileFeatures { get; set; }
    #endregion dbo Schema

    #region Ref Schema
    public virtual DbSet<State> States { get; set; }
    public virtual DbSet<County> Counties { get; set; }
    #endregion Ref Schema
  }
}
