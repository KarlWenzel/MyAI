using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  [Table("ImageDirectoryFeatures")]
  public class ImageDirectoryFeature
  {
    [Key, Column(Order = 0)]
    [MaxLength(128)]
    [ForeignKey("Feature")]
    public string FeatureName { get; set; }
    public virtual FeatureType Feature { get; set; }

    [Key, Column(Order = 1)]
    [ForeignKey("ImageDirectory")]
    public string DirectoryName { get; set; }
    public virtual ImageDirectory ImageDirectory { get; set; }

    public string Value { get; set; }
  }
}