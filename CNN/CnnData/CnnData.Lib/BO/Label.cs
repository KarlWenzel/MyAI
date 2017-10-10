using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  [Table("Labels")]
  public class Label
  {
    [Key, Column(Order = 0)]
    public string LabelName { get; set; }

    [Key, Column(Order = 1)]
    [ForeignKey("LabelCategory")]
    public string CategoryName { get; set; }
    public virtual LabelCategory LabelCategory { get; set; }

    public string HotKey { get; set; }

    public virtual ICollection<Instance> Instance { get; set; }
  }
}
