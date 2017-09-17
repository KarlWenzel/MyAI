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

    [ForeignKey("InstanceSetRole")]
    public InstanceSetRoleEnum InstanceSetRoleID { get; set; }
    public virtual InstanceSetRole InstanceSetRole { get; set; }

    public virtual ICollection<FileInstance> FileInstances { get; set; }
  }
}
