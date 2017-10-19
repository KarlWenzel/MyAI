using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  [Table("ImageFileLabels")]
  public class ImageFileLabel
  {
    [Key, Column(Order = 0)]
    [ForeignKey("Label")]
    [MaxLength(128)]
    public string LabelName { get; set; }

    [Key, Column(Order = 1)]
    [ForeignKey("Label")]
    [MaxLength(128)]
    public string CategoryName { get; set; }

    public virtual Label Label { get; set; }
    
    [Key, Column(Order = 2)]
    [ForeignKey("ImageFile")]
    public int ImageFileID { get; set; }
    public virtual ImageFile ImageFile { get; set; }
    
    public bool SetByHuman { get; set; }
    public bool SetByMachine { get; set; }
  }
}
