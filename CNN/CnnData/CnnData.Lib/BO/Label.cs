using CnnData.Lib.BO.ReferenceData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  [Table("Labels")]
  public class Label
  {
    public Label() {}

    public Label(string labelName, string categoryName, string inputKeyText, string modifierKeyText, string bgColorText, string fgColorText)
    {
      this.LabelName = labelName;
      this.CategoryName = categoryName;
      this.InputKeyText = inputKeyText;
      this.ModifierKeyText = modifierKeyText;
      this.BackgroundColorText = bgColorText;
      this.ForegroundColorText = fgColorText;
    }

    [Key, Column(Order = 0)]
    [MaxLength(128)]
    public string LabelName { get; set; }

    [Key, Column(Order = 1)]
    [ForeignKey("LabelCategory")]
    [MaxLength(128)]
    public string CategoryName { get; set; }
    public virtual LabelCategory LabelCategory { get; set; }

    [ForeignKey("InputKey")]
    [MaxLength(32)]
    public string InputKeyText { get; set; }
    public virtual InputKey InputKey { get; set; }

    [ForeignKey("ModifierKey")]
    [MaxLength(8)]
    public string ModifierKeyText { get; set; }
    public virtual ModifierKey ModifierKey { get; set; }

    [ForeignKey("BackgroundColor")]
    [MaxLength(32)]
    public string BackgroundColorText { get; set; }
    public virtual WinMediaColor BackgroundColor { get; set; }

    [ForeignKey("ForegroundColor")]
    [MaxLength(32)]
    public string ForegroundColorText { get; set; }
    public virtual WinMediaColor ForegroundColor { get; set; }

    public virtual ICollection<Instance> Instance { get; set; }
    public virtual ICollection<ImageFileLabel> ImageFileLabels { get; set; }
  }
}
