using CnnData.App.Interfaces;
using CnnData.WPF.Services;
using CnnData.WPF.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.WPF
{
  public class MyLocator
  {
    public MyLocator()
    {
      ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
      SimpleIoc.Default.Register<IWindowService, WindowService>();
      SimpleIoc.Default.Register<ImageFileService>();
      SimpleIoc.Default.Register<MainVM>();
    }

    public MainVM Main
    {
      get
      {
        return ServiceLocator.Current.GetInstance<MainVM>();
      }
    }

  }
}
