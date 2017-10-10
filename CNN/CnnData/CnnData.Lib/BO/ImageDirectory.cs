using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  [Table("ImageDirectories")]
  public class ImageDirectory
  {
    [Key]
    [MaxLength(255)]
    public string DirectoryName { get; set; }
    
    public virtual ICollection<ImageDirectoryFeature> ImageDirectoryFeatures { get; set; }
    public virtual ICollection<ImageFile> ImageFiles { get; set; }
    public virtual ICollection<MultiPageImageFile> MultiPageImageFiles { get; set; }
  }
}
