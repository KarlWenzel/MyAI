using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  [Table("Features")]
  public class Feature
  {
    [Key]
    public int ID { get; set; }
    public string FeatureName { get; set; }
    public string FeatureValue { get; set; }
  }
}
