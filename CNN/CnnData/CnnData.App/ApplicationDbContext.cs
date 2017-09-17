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

    public virtual DbSet<Feature> Features { get; set; }
    public virtual DbSet<FileDirectory> FileDirectories { get; set; }
    public virtual DbSet<FileInstance> FileInstances { get; set; }
    public virtual DbSet<InstanceSetRole> InstanceSetRoles { get; set; }
    public virtual DbSet<InstanceSet> InstanceSets { get; set; }
    public virtual DbSet<LabelCategory> LabelCategories { get; set; }
    public virtual DbSet<Label> Labels { get; set; }
  }
}
