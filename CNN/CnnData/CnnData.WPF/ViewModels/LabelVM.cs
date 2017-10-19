using CnnData.Lib.BO;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace CnnData.WPF.ViewModels
{
  public enum HotKeyParamTypes
  {
    NextPage,
    PrevPage,
    ApplyLabel
  }

  public class LabelVM : ViewModelBase
  {
    public LabelVM() { }

    public LabelVM(Label label)
    {
      this.HotKeyParamType = HotKeyParamTypes.ApplyLabel;
      this.LabelObject = label;
      this.LabelName = label.LabelName;
      this.Order = label.InputKey.Order;
      this.Key = InputKeyFromText(label.InputKey.KeyText);
      this.HotKeyText = label.InputKeyText;
      this.ModifierKey = ModifierKeyFromText(label.ModifierKeyText);
      this.ModifierKeyText = label.ModifierKeyText;
      this.BackgroundBrush = BrushFromText(label.BackgroundColorText);
      this.BackgroundColorText = label.BackgroundColorText;
      this.ForegroundBrush = BrushFromText(label.ForegroundColorText);
      this.ForegroundColorText = label.ForegroundColorText;
    }

    public LabelVM(HotKeyParamTypes hotKeyParamType, string labelName, System.Windows.Input.Key key, ModifierKeys modifierKey, string hotKeyText)
    {
      this.HotKeyParamType = hotKeyParamType;
      this.LabelName = labelName;
      this.Key = key;
      this.ModifierKey = modifierKey;
      this.HotKeyText = hotKeyText;
    }

    public HotKeyParamTypes HotKeyParamType { get; set; }
    public Label LabelObject { get; set; }
    public string LabelName { get; set; }
    public int Order { get; set; }
    public System.Windows.Input.Key Key { get; set; }
    public string HotKeyText { get; set; }
    public ModifierKeys ModifierKey { get; set; }
    public string ModifierKeyText { get; set; }
    public System.Windows.Media.Brush BackgroundBrush { get; set; }
    public string BackgroundColorText { get; set; }
    public System.Windows.Media.Brush ForegroundBrush { get; set; }
    public string ForegroundColorText { get; set; }

    public static System.Windows.Input.Key InputKeyFromText(string keyText)
    {
      System.Windows.Input.Key key;
      if (!Enum.TryParse(keyText, out key))
      {
        key = System.Windows.Input.Key.None;
      }
      return key;
    }

    public static ModifierKeys ModifierKeyFromText(string modifierKeyText)
    {
      ModifierKeys modifierKey;
      if (!Enum.TryParse(modifierKeyText, out modifierKey))
      {
        modifierKey = System.Windows.Input.ModifierKeys.None;
      }
      return modifierKey;
    }

    public static Brush BrushFromText(string colorText)
    {      
      var colorPropertyInfo = typeof(System.Windows.Media.Colors).GetProperty(colorText);
      System.Windows.Media.Brush brush;
      try
      {
        var color = (System.Windows.Media.Color)colorPropertyInfo.GetValue(null);
        brush = new SolidColorBrush(color);
      }
      catch (Exception e)
      {
        brush = new SolidColorBrush(Colors.White);
      }
      return brush;
    }
  }
}
