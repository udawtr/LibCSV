using System;

namespace Youworks.Text
{
    public interface ITypeResolver
    {
        string ResolveString(object value);
        double ResolveDouble(object value);
        int ResolveInt(object value);
        DateTime ResolveDateTime(object value);
        object ResolveEnum(Type type, object value);
    }
}