using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CnnData.Lib.BO.ReferenceData
{
  [Table("ModifierKeys", Schema = "Ref")]
  public class ModifierKey
  {
    [Key]
    [MaxLength(8)]
    public string KeyText { get; set; }

    public static List<ModifierKey> GetModifierKeys()
    {
      return new List<ModifierKey>()
      {
        new ModifierKey() { KeyText = "None" },
        new ModifierKey() { KeyText = "Alt" },
        new ModifierKey() { KeyText = "Control" },
        new ModifierKey() { KeyText = "Shift" },
        new ModifierKey() { KeyText = "Windows" }
      };
    }
  }
}
