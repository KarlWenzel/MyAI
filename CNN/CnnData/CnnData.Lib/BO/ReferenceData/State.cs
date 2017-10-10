using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO.ReferenceData
{
  [Table("States", Schema="Ref")]
  public class State
  {
    [Key]
    public string StateText { get; set; }
    public string Abbrev { get; set; }
    public string ANSI { get; set; }
    public virtual ICollection<County> Counties { get; set; }
  }
}
