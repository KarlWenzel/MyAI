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

    public string Notes { get; set; }
  }
}
