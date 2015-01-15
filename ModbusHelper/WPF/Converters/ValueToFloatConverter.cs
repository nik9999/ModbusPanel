using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ModbusHelper.WPF.Converters
{
    public class ValueToFloatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double result = 0;
            if (value != null)
            {
                result = System.Convert.ToDouble(value) / 10;
                
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int result = 0;
            if (value != null)
            {
                result = (int)Math.Round(decimal.Parse(value.ToString(), culture) * 10);
            }

            return result;
        }
    }
}
