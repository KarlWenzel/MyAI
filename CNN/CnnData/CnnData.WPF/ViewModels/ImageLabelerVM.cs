using CnnData.Lib.BO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;

namespace CnnData.WPF.ViewModels
{
  public class ImageLabelerVM : ViewModelBase
  {
    public const string NONE_SELECTED_CATEGORY = "None";

    public ImageLabelerVM(MainVM mainVM, Window mainWindow)
    {
      this.MainVM = mainVM;
      this.MainWindow = mainWindow;
      this.LabelerKeyBindings = new List<KeyBinding>();
      this.RefreshLabelCategories();

      this.MainVM.PropertyChanged += MainVM_PropertyChanged;
    }

    private void MainVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "IsImageFileListLoaded")
      {
        this.SetHotKeys();
      }
    }

    public readonly MainVM MainVM;
    private readonly Window MainWindow;
    
    private List<LabelCategory> _LabelCategories;
    public List<LabelCategory> LabelCategories
    {
      get { return this._LabelCategories; }
      set { Set(() => this.LabelCategories, ref this._LabelCategories, value); }
    }

    private LabelCategory _SelectedLabelCategory;
    public LabelCategory SelectedLabelCategory
    {
      get { return this._SelectedLabelCategory; }
      set
      {
        Set(() => this.SelectedLabelCategory, ref this._SelectedLabelCategory, value);
        SetLabelVMs();
        SetHotKeys();
        this.MainVM.CurrentLabelCategory = this.SelectedLabelCategory;
      }
    }

    public List<KeyBinding> LabelerKeyBindings { get; set; }
    
    public HotKeyParam NextHotKeyParam
    {
      get
      {
        return new HotKeyParam()
        {
          HotKeyParamType = HotKeyParamTypes.NextPage,
          Label = null
        };
      }
    }

    public HotKeyParam PrevHotKeyParam
    {
      get
      {
        return new HotKeyParam()
        {
          HotKeyParamType = HotKeyParamTypes.PrevPage,
          Label = null
        };
      }
    }

    private List<LabelVM> _LabelVMs;
    public List<LabelVM> LabelVMs
    {
      get { return this._LabelVMs; }
      set { Set(() => this.LabelVMs, ref this._LabelVMs, value); }
    }

    public void RefreshLabelCategories()
    {
      this.LabelCategories = this.MainVM.DataService.GetLabelCategories();
      this.LabelCategories.Insert(0, new LabelCategory() {
        CategoryName = NONE_SELECTED_CATEGORY,
        Labels = new List<Label>()
      });

      if (this.SelectedLabelCategory == null)
      {
        this.SelectedLabelCategory = this.LabelCategories[0];
        return;
      }

      var selectedLabelCategory = this.LabelCategories.FirstOrDefault(x => x.CategoryName == this.SelectedLabelCategory.CategoryName);
      this.SelectedLabelCategory = selectedLabelCategory ?? this.LabelCategories[0];
    }

    private void SetLabelVMs()
    {
      this.LabelVMs = null;

      var labelVMs = new List<LabelVM>();
      labelVMs.AddRange(new LabelVM[] {
        new LabelVM(HotKeyParamTypes.NextPage, "Next Image", Key.Right, ModifierKeys.None, "Right Arrow"),
        new LabelVM(HotKeyParamTypes.PrevPage, "Prev Image", Key.Left, ModifierKeys.None, "Left Arrow")
      });

      if (this.SelectedLabelCategory == null)
      {
        return;
      }


      foreach (var label in this.SelectedLabelCategory.Labels.OrderBy(x => x.InputKey.Order))
      {
        labelVMs.Add(new LabelVM(label));
      }

      this.LabelVMs = labelVMs;
    }

    private void SetHotKeys()
    {
      foreach (var previousBinding in this.LabelerKeyBindings)
      {
        this.MainWindow.InputBindings.Remove(previousBinding);
      }

      this.LabelerKeyBindings.Clear();

      if (this.SelectedLabelCategory == null)
      {
        return;
      }

      foreach (var labelVM in this.LabelVMs)
      {
        AddHotKey(labelVM);
      }      
    }

    private void AddHotKey(LabelVM labelVM)
    {
      KeyBinding keyBinding;
      
      if (labelVM.ModifierKey == ModifierKeys.None)
      {
        // MS makes as add a non-None ModifierKey and then ClearValue()
        keyBinding = new KeyBinding(this.HotKeyCommand, new KeyGesture(labelVM.Key, ModifierKeys.Control));
        keyBinding.ClearValue(KeyBinding.ModifiersProperty);
      }
      else
      {
        keyBinding = new KeyBinding(this.HotKeyCommand, new KeyGesture(labelVM.Key, labelVM.ModifierKey));
      }

      keyBinding.CommandParameter = new HotKeyParam() { HotKeyParamType = labelVM.HotKeyParamType, Label = labelVM.LabelObject };
      this.MainWindow.InputBindings.Add(keyBinding);

      // Note that we keep a list of keyBindings added here so we can remove this later if necessary
      this.LabelerKeyBindings.Add(keyBinding);
    }

    public class HotKeyParam
    {
      public HotKeyParamTypes HotKeyParamType { get; set; }
      public Label Label { get; set; }
    }

    private RelayCommand<HotKeyParam> _HotKeyCommand;
    public ICommand HotKeyCommand
    {
      get { return this._HotKeyCommand ?? (this._HotKeyCommand = new RelayCommand<HotKeyParam>(param => { OnHotKey(param); })); }
    }

    private void OnHotKey(HotKeyParam hotKeyParam)
    {
      if (this.MainVM.ImageFileListVM == null)
      {
        return;
      }

      switch (hotKeyParam.HotKeyParamType)
      {
        case HotKeyParamTypes.ApplyLabel:
          this.MainVM.ImageFileListVM.SetLabel(hotKeyParam.Label);
          break;
        case HotKeyParamTypes.NextPage:
          this.MainVM.NextImage();
          break;
        case HotKeyParamTypes.PrevPage:
          this.MainVM.PrevImage();
          break;
      }
    }

  }
}
