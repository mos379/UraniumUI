using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UraniumUI.Converters
{
    public class ItemSpacingToItemsLayoutConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double spacing = 0;

            if (value is double s)
                spacing = s;

            return new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
            {
                ItemSpacing = spacing
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is LinearItemsLayout layout)
                return layout.ItemSpacing;

            return 0;
        }
    }
}
