using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  [Table("InstanceSets")]
  public class InstanceSet
  {
    [Key]
    public int ID { get; set; }

    public int? Seed { get; set; }
    public int? MinFileInstanceID { get; set; }
    public int? MaxFileInstanceID { get; set; }
    public bool UsesCrossValidation { get; set; }

    public DateTime DateCreated { get; set; }
    public string Notes { get; set; }
    
    public virtual ICollection<InstanceSetInstance> InstanceSetInstances { get; set; }
  }
}
