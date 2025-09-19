using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;

namespace OnTopReplica.StartupOptions {

    /// <summary>
    /// Type converter for Size.
    /// </summary>
    class SizeConverter : TypeConverter {

        /// <summary>
        /// Overridden. Converts from a string.
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
            if(value != null) {
                var sVal = value.ToString();
                return StringToSize(sVal);
            }
            else
                return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Overridden. Returns true if the source type is a string or a Size.
        /// </summary>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            return (sourceType == typeof(string) || sourceType == typeof(Size));
        }

        /// <summary>
        /// Overridden. Returns true if the destination type is a Size or a string.
        /// </summary>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            return (destinationType == typeof(Size) || destinationType == typeof(string));
        }

        /// <summary>
        /// Overridden. Converts to a string or a Size.
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
            if(value == null)
                return base.ConvertTo(context, culture, value, destinationType);

            if(destinationType == typeof(Size)) {
                var sVal = value.ToString();
                return StringToSize(sVal);
            }
            else if(destinationType == typeof(string)) {
                if(value is Size) {
                    Size sValue = (Size)value;
                    return string.Format("{0}, {1}", sValue.Width, sValue.Height);
                }

                return value.ToString();
            }
            else
                return base.ConvertTo(context, culture, value, destinationType);
        }

        static Regex _sizeRegex = new Regex("^[^0-9-]*(?<x>-?\\d*)\\s*,\\s*(?<y>-?\\d*)\\D*$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private Size StringToSize(string s) {
            var match = _sizeRegex.Match(s);

            var x = match.Groups["x"];
            var y = match.Groups["y"];

            if(!match.Success || !x.Success || !y.Success)
                throw new ArgumentException("Cannot convert '" + s + "' to coordinates pair.");

            var xVal = Int32.Parse(x.Value);
            var yVal = Int32.Parse(y.Value);

            return new Size(xVal, yVal);
        }

    }

}
