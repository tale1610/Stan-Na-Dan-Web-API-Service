namespace StanNaDanLibrary.Entiteti;

public class KrevetId
{
    virtual public required int IdKreveta { get; set; }
    virtual public required Nekretnina Nekretnina { get; set; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj?.GetType() != typeof(KrevetId))
        {
            return false;
        }

        KrevetId compare = (KrevetId)obj;

        return IdKreveta == compare.IdKreveta && Nekretnina?.IdNekretnine == compare.Nekretnina?.IdNekretnine;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IdKreveta, Nekretnina?.IdNekretnine);
    }
}
