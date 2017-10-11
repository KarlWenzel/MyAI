using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  [Table("Instances")]
  public class Instance : IDatedEntity
  {
    [Key]
    public int ID { get; set; }
    
    [ForeignKey("ImageFile")]
    public int ImageFileID { get; set; }
    public virtual ImageFile ImageFile { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<InstanceFeature> InstanceFeatures { get; set; }
    public virtual ICollection<Label> Labels { get; set; }
    public virtual ICollection<InstanceSetInstance> InstanceSetInstances { get; set; }
  }
}
