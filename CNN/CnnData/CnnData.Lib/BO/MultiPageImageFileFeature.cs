using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  [Table("MultiPageImageFileFeatures")]
  public class MultiPageImageFileFeature
  {
    [Key, Column(Order = 0)]
    [MaxLength(128)]
    [ForeignKey("FeatureType")]
    public string FeatureName { get; set; }
    public virtual FeatureType FeatureType { get; set; }

    [Key, Column(Order = 1)]
    [ForeignKey("MultiPageImageFile")]
    public int MultiPageImageFileID { get; set; }
    public virtual MultiPageImageFile MultiPageImageFile { get; set; }

    public string Value { get; set; }
  }
}
