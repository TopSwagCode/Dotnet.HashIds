using System.ComponentModel;
using System.Globalization;
using HashidsNet;

namespace Dotnet.HashIds.API;

class ProductIdConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        if (sourceType == typeof(string))
        {
            return true;
        }
        return base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, 
        CultureInfo culture, object value)
    {
        if (value is string s && (new Hashids("Our super secret salt!", 5)).TryDecodeSingle(s, out int productId))
        {
            return new ProductId(productId);
        }
        
        return base.ConvertFrom(context, culture, value);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is ProductId productId)
        {
            return (new Hashids("Our super secret salt!", 5)).Encode(productId.Value);
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }
}