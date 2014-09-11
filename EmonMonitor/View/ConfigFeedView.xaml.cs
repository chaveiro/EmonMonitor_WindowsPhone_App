using System;
using System.Windows.Controls;
using System.Globalization;

namespace EmonMonitor.View
{
    public partial class ConfigFeedView : UserControl
    {
        public ConfigFeedView()
        {
            InitializeComponent();
        }
    }

    public class BoolOpposite : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = (bool)value;
            return !b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;
            bool b;

            if (bool.TryParse(s, out b))
            {
                return !b;
            }
            return false;
        }
    }
}
