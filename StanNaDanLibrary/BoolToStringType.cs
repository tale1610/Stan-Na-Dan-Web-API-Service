using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System;
using System.Data;
using System.Data.Common;

namespace StanNaDanLibrary;
public class BooleanToStringType : IUserType
{
    public bool Equals(object x, object y)
    {
        return x == y || (x != null && x.Equals(y));
    }

    public int GetHashCode(object x)
    {
        return x == null ? 0 : x.GetHashCode();
    }

    public object DeepCopy(object value)
    {
        return value;
    }

    public object Replace(object original, object target, object owner)
    {
        return original;
    }

    public object Assemble(object cached, object owner)
    {
        return cached;
    }

    public object Disassemble(object value)
    {
        return value;
    }

    public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
    {
        var value = NHibernateUtil.String.NullSafeGet(rs, names[0], session) as string;
        if (string.IsNullOrEmpty(value))
            return false;  // vraća false umesto null za boolean tip
        return value.ToLower() == "true";
    }

    public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
    {
        if (value == null)
        {
            NHibernateUtil.String.NullSafeSet(cmd, null, index, session);
        }
        else
        {
            var boolValue = (bool)value;
            NHibernateUtil.String.NullSafeSet(cmd, boolValue ? "true" : "false", index, session);
        }
    }

    public SqlType[] SqlTypes
    {
        get { return new SqlType[] { new SqlType(DbType.String, 5) }; }
    }

    public Type ReturnedType
    {
        get { return typeof(bool); }
    }

    public bool IsMutable
    {
        get { return false; }
    }
}
