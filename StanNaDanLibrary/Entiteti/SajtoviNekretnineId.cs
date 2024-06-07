namespace StanNaDanLibrary.Entiteti;

public class SajtoviNekretnineId
{
    virtual public required string Sajt { get; set; }
    virtual public required Nekretnina Nekretnina { get; set; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj?.GetType() != typeof(SajtoviNekretnineId))
        {
            return false;
        }

        SajtoviNekretnineId compare = (SajtoviNekretnineId)obj;

        return Sajt == compare.Sajt && Nekretnina?.IdNekretnine == compare.Nekretnina?.IdNekretnine;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Sajt, Nekretnina?.IdNekretnine);
    }
}
