using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  [Table("InstanceSetInstances")]
  public class InstanceSetInstance
  {
    [Key, Column(Order = 0)]
    [ForeignKey("InstanceSet")]
    public int InstanceSetID { get; set; }
    public virtual InstanceSet InstanceSet { get; set; }

    [Key, Column(Order = 1)]
    [ForeignKey("Instance")]
    public int InstanceID { get; set; }
    public virtual Instance Instance { get; set; }

    [ForeignKey("InstanceSetRole")]
    public InstanceSetRoleEnum InstanceSetRoleID { get; set; }
    public virtual InstanceSetRole InstanceSetRole { get; set; }
  }
}
