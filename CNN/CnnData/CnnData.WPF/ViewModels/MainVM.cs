using CnnData.App.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CnnData.WPF.ViewModels
{
  public class MainVM : ViewModelBase
  {
    public MainVM(IWindowService windowService)
    {
      this.WindowService = windowService;
    }

    private const string BaseWindowTitle = "Image Tagger";
    private readonly IWindowService WindowService;

    internal void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      e.Cancel = false;
    }

    private RelayCommand _TalkCommand;
    public ICommand TalkCommand
    {
      get { return _TalkCommand ?? (_TalkCommand = new RelayCommand(() => { Talk(); } )); }
    }

    public void Talk()
    {
      var reader = new SpeechSynthesizer();
      //reader.Speak("You are some hot hot shat, " + Environment.UserName);
      reader.Speak("thanks a lot bro! The easy way that I found in the web (Y)");
      reader.Speak("Thanks for sharing idea about the wpf desing.");
    }
  }
}
