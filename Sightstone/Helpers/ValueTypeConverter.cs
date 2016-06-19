using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Caliburn.Micro;

namespace Sightstone.Helpers
{
    public class ValueTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var result = TypeDescriptor.GetConverter(targetType).ConvertFrom(value);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Binding Image.Source to an Uri typically fails.
        /// Calling the following during application bootstrap will set this up properly.
        ///    ConventionManager.ApplyValueConverter = ValueTypeConverter.ApplyValueConverter;
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="bindableProperty"></param>
        /// <param name="info"></param>
        public static void ApplyValueConverter(Binding binding, DependencyProperty bindableProperty, PropertyInfo info)
        {
            if (bindableProperty == UIElement.VisibilityProperty && typeof(bool).IsAssignableFrom(info.PropertyType))
                binding.Converter = ConventionManager.BooleanToVisibilityConverter;

            else if (bindableProperty == Image.SourceProperty && typeof(Uri).IsAssignableFrom(info.PropertyType))
                binding.Converter = new ValueTypeConverter();
            else
            {
                foreach (var item in Conventions)
                {
                    if (bindableProperty == item.Item1 && item.Item2.IsAssignableFrom(info.PropertyType))
                        binding.Converter = new ValueTypeConverter();
                }
            }
        }

        /// <summary>
        /// If there is a TypeConverter that can convert a <paramref name="TSourceType"/>
        /// to the type on <paramref name="bindableProperty"/>, then this has to
        /// be manually registered with Caliburn.Micro as Silverlight is unable to 
        /// extract sufficient TypeConverter information from a dependency property
        /// on its own.
        /// </summary>
        /// <example>
        /// ValueTypeConverter.AddTypeConverter&lt;ImageSource&gt;(Image.SourceProperty);
        /// </example>
        /// <typeparam name="TSourceType"></typeparam>
        /// <param name="bindableProperty"></param>
        public static void AddTypeConverter<TSourceType>(DependencyProperty bindableProperty)
        {
            Conventions.Add(Tuple.Create(bindableProperty, typeof(TSourceType)));
        }

        private static readonly IList<Tuple<DependencyProperty, Type>> Conventions = new List<Tuple<DependencyProperty, Type>>();
    }
}