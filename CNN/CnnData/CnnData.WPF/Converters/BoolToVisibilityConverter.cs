﻿using System;
using System.Windows.Data;

namespace CnnData.WPF.Converters
{
  class BoolToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      bool val = (bool)value;

      if (val)
        return System.Windows.Visibility.Visible;
      else
        return System.Windows.Visibility.Collapsed;

    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
