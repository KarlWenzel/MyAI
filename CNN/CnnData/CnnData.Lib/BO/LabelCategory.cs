using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  [Table("LabelCategories")]
  public class LabelCategory
  {
    [Key]
    public string CategoryName { get; set; }

    public virtual ICollection<Label> Labels { get; set; }
  }
}
