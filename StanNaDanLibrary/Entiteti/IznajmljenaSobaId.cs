namespace StanNaDanLibrary.Entiteti
{
    internal class IznajmljenaSobaId
    {
        virtual internal protected required Soba Soba { get; set; }
        virtual internal protected required Najam Najam { get; set; }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj?.GetType() != typeof(IznajmljenaSobaId))
            {
                return false;
            }

            IznajmljenaSobaId compare = (IznajmljenaSobaId)obj;

            return Soba.ID.IdSobe.Equals(compare.Soba.ID.IdSobe) && Soba.ID.Nekretnina.IdNekretnine.Equals(compare.Soba.ID.Nekretnina.IdNekretnine) && Najam?.IdNajma == compare.Najam?.IdNajma;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Soba.ID.IdSobe, Soba.ID.Nekretnina.IdNekretnine, Najam?.IdNajma);
        }
    }
}
