using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  [Table("FeatureTypes")]
  public class FeatureType
  {
    [Key]
    [MaxLength(128)]
    public string FeatureName { get; set; }

    public bool UseWithImageDirectories { get; set; }
    public bool UseWithImageFiles { get; set; }
    public bool UseWithInstances { get; set; }
    public bool UseWithMultiPageImageFiles { get; set; }

    public string Notes { get; set; }
  }
}
