using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace OnTopReplica.StartupOptions {
    /// <summary>
    /// Type converter for ScreenPosition.
    /// </summary>
    class ScreenPositionConverter : TypeConverter {

        /// <summary>
        /// Overridden. Returns true if the source type is a string.
        /// </summary>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Overridden. Returns true if the destination type is ScreenPosition.
        /// </summary>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            if (destinationType == typeof(ScreenPosition))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Overridden. Converts from a string.
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
            var sValue = value.ToString();

            switch (sValue) {
                case "TL":
                    return ScreenPosition.TopLeft;
                case "TR":
                    return ScreenPosition.TopRight;
                case "BL":
                    return ScreenPosition.BottomLeft;
                case "BR":
                    return ScreenPosition.BottomRight;
                case "C":
                    return ScreenPosition.Center;
                default:
                    throw new ArgumentException("Invalid screen position value '" + sValue + "'.");
            }
        }

        /// <summary>
        /// Overridden. Converts to a string.
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
            if (destinationType == typeof(ScreenPosition))
                return ConvertFrom(context, culture, value);

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }
}
