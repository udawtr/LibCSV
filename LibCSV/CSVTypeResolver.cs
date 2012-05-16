using System;

namespace Youworks.Text
{
    public interface ICSVTypeResolver
    {
        string ResolveString(CSVTypeResolverArgs args);
        double ResolveDouble(CSVTypeResolverArgs args);
        int ResolveInt(CSVTypeResolverArgs args);
        DateTime ResolveDateTime(CSVTypeResolverArgs args);
        object ResolveEnum(CSVTypeResolverArgs args);
    }

    public struct CSVTypeResolverArgs
    {
        public Type Type;
        public object Value;
        public int LineNo;
        public string ColumnName;
    }

    public class CSVGeneralTypeResolver : ICSVTypeResolver
    {
        public virtual string ResolveString(CSVTypeResolverArgs args)
        {
            return args.Value.ToString();
        }

        public virtual double ResolveDouble(CSVTypeResolverArgs args)
        {
            return Convert.ToDouble(args.Value);
        }

        public virtual int ResolveInt(CSVTypeResolverArgs args)
        {
            return Convert.ToInt32(args.Value);
        }

        public virtual DateTime ResolveDateTime(CSVTypeResolverArgs args)
        {
            return Convert.ToDateTime(args.Value);
        }

        public virtual object ResolveEnum(CSVTypeResolverArgs args)
        {
            return Enum.Parse(args.Type, (string)args.Value);
        }
    }
}
