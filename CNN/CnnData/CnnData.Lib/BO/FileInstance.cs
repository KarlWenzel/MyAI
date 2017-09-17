using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  [Table("FileInstances")]
  public class FileInstance
  {
    [Key]
    public int ID { get; set; }

    [Index("IX_FileNameDirectoryName", 0, IsUnique = true)]
    [MaxLength(128)]
    public string FileName { get; set; }
    
    [Index("IX_FileNameDirectoryName", 1, IsUnique = true)]
    [ForeignKey("FileDirectory")]
    [MaxLength(255)]
    public string DirectoryName { get; set; }
    public virtual FileDirectory FileDirectory { get; set; }

    public virtual ICollection<Feature> Features { get; set; }
    public virtual ICollection<Label> Labels { get; set; }
    public virtual ICollection<InstanceSet> FileSets { get; set; }
  }
}
