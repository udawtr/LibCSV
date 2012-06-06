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
            try
            {
                return args.Value.ToString();
            }
            catch (FormatException e)
            {
                throw new InvalidValueException(e.Message, e);
            }
        }

        public virtual double ResolveDouble(CSVTypeResolverArgs args)
        {
            try
            {
                return Convert.ToDouble(args.Value);
            }
            catch (FormatException e)
            {
                throw new InvalidValueException(e.Message, e);
            }
        }

        public virtual int ResolveInt(CSVTypeResolverArgs args)
        {
            try
            {
                return Convert.ToInt32(args.Value);
            }
            catch (FormatException e)
            {
                throw new InvalidValueException(e.Message, e);
            }
        }

        public virtual DateTime ResolveDateTime(CSVTypeResolverArgs args)
        {
            try
            {
                return Convert.ToDateTime(args.Value);
            }
            catch (FormatException e)
            {
                throw new InvalidValueException(e.Message, e);
            }
        }

        public virtual object ResolveEnum(CSVTypeResolverArgs args)
        {
            return Enum.Parse(args.Type, (string)args.Value);
        }
    }
}
