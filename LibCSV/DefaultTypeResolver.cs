using System;

namespace Youworks.Text
{
    public class DefaultTypeResolver : ITypeResolver
    {
        public virtual string ResolveString(object value)
        {
            return value.ToString();
        }

        public virtual double ResolveDouble(object value)
        {
            return Convert.ToDouble(value);
        }

        public virtual int ResolveInt(object value)
        {
            return Convert.ToInt32(value);
        }

        public virtual DateTime ResolveDateTime(object value)
        {
            return Convert.ToDateTime(value);
        }

        public virtual object ResolveEnum(Type type, object value)
        {
            return Enum.Parse(type, (string) value);
        }
    }
}
