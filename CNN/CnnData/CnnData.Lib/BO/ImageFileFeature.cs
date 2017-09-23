using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  [Table("FileRecordFeatures")]
  public class ImageFileFeature
  {
    [Key, Column(Order = 0)]
    [MaxLength(128)]
    [ForeignKey("Feature")]
    public string FeatureName { get; set; }
    public virtual FeatureType Feature { get; set; }

    [Key, Column(Order = 1)]
    [ForeignKey("ImageFile")]
    public int ImageFileID { get; set; }
    public virtual ImageFile ImageFile { get; set; }

    public string Value { get; set; }
  }
}