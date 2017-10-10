using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO.ReferenceData
{
  [Table("Counties", Schema="Ref")]
  public class County
  {
    [Key]
    public int ID { get; set; }
    public int CountyFIPS { get; set; }
    public string CountyText { get; set; }
    public string ANSI { get; set; }

    [ForeignKey("State")]
    public string StateText { get; set; }
    public virtual State State { get; set; }
  }
}
