namespace StanNaDanLibrary.Entiteti;

internal class SpoljniSaradnikId
{
    virtual internal protected required Agent AgentAngazovanja { get; set; }
    virtual internal protected required int IdSaradnika { get; set; }


    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj?.GetType() != typeof(SpoljniSaradnikId))
        {
            return false;
        }

        SpoljniSaradnikId compare = (SpoljniSaradnikId)obj;

        return IdSaradnika == compare.IdSaradnika && AgentAngazovanja?.MBR == compare.AgentAngazovanja?.MBR;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
