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
  public class ImageFile : IDatedEntity
  {
    [Key]
    public int ID { get; set; }
    
    [Index("IX_FileNameDirectoryName", 0, IsUnique = true)]
    [MaxLength(128)]
    public string FileName { get; set; }

    [Index("IX_FileNameDirectoryName", 1, IsUnique = true)]
    [ForeignKey("ImageDirectory")]
    [MaxLength(255)]
    public string DirectoryName { get; set; }
    public virtual ImageDirectory ImageDirectory { get; set; }
    
    [ForeignKey("MultiPageImageFile")]
    public int? MultiPageImageFileID { get; set; }
    public virtual MultiPageImageFile MultiPageImageFile { get; set; }

    public int? PageSequence { get; set; }
    public string ImageExtension { get; set; }
    public string Checksum { get; set; }
    public int? WidthPixels { get; set; }
    public int? HeightPixels { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<Instance> Instances { get; set; }
    public virtual ICollection<ImageFileFeature> ImageFileFeatures { get; set; }
    public virtual ICollection<ImageFileLabel> ImageFileLabels { get; set; }
  }
}
