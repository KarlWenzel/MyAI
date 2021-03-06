﻿using CnnData.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CnnData.WPF.Controls
{
  /// <summary>
  /// Interaction logic for DirectoryManager.xaml
  /// </summary>
  public partial class DirectoryManager : UserControl
  {
    public DirectoryManager()
    {
      InitializeComponent();
    }

    private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      (this.DataContext as DirectoryManagerVM).OnSelect();
    }
  }
}
