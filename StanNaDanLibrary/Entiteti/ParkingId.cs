namespace StanNaDanLibrary.Entiteti;

internal class ParkingId
{
    virtual internal protected required Nekretnina Nekretnina { get; set; }
    virtual internal protected required int IdParkinga { get; set; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj?.GetType() != typeof(ParkingId))
        {
            return false;
        }

        ParkingId compare = (ParkingId)obj;

        return IdParkinga == compare.IdParkinga && Nekretnina?.IdNekretnine == compare.Nekretnina?.IdNekretnine;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IdParkinga, Nekretnina?.IdNekretnine);
    }
}
