﻿namespace StanNaDanLibrary.Entiteti;

internal class DodatnaOpremaId
{
    virtual internal protected required int IdOpreme { get; set; }
    virtual internal protected required Nekretnina Nekretnina { get; set; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj?.GetType() != typeof(DodatnaOpremaId))
        {
            return false;
        }

        DodatnaOpremaId compare = (DodatnaOpremaId)obj;

        return IdOpreme == compare.IdOpreme && Nekretnina?.IdNekretnine == compare.Nekretnina?.IdNekretnine;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IdOpreme, Nekretnina?.IdNekretnine);
    }
}
