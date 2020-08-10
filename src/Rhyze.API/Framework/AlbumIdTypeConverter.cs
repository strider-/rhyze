using Rhyze.Core.Models;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Rhyze.API.Framework
{
    public class AlbumIdTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(AlbumId))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string str &&
                !string.IsNullOrEmpty(str))
            {
                return new AlbumId(str);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(AlbumId) && value is AlbumId id)
            {
                return id.Value;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
