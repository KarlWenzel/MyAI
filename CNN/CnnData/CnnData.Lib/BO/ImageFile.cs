using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  [Table("ImageFiles")]
  public class ImageFile
  {
    [Key]
    public int ID { get; set; }
    
    [Index("IX_FileNameDirectoryName", 0, IsUnique = true)]
    [MaxLength(128)]
    public string FileName { get; set; }

    [Index("IX_FileNameDirectoryName", 1, IsUnique = true)]
    [ForeignKey("DirectoryData")]
    [MaxLength(255)]
    public string DirectoryName { get; set; }
    public virtual ImageDirectory DirectoryData { get; set; }

    public virtual ICollection<Instance> Instances { get; set; }
    public virtual ICollection<ImageFileFeature> ImageFileFeatures { get; set; }
  }
}
