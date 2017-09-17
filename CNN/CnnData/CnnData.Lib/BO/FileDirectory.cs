using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  [Table("FileDirectories")]
  public class FileDirectory
  {
    [Key]
    [MaxLength(255)]
    public string DirectoryName { get; set; }
    public virtual ICollection<Feature> Features { get; set; }
  }
}
