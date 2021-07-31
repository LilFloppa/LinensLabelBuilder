using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Windows.Data;

namespace LabelBuilder.Converters
{
	public class IntToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string stringValue = (string)value;
			if (int.TryParse(stringValue, out int result))
				return result;
			else
				return new ValidationResult("Строка не является числом");			
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((int)value).ToString();
		}
	}
}
