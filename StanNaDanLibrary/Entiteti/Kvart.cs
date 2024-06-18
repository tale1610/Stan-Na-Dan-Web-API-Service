namespace StanNaDanLibrary.Entiteti
{
    internal class Kvart
    {
        internal protected virtual int ID { get; protected set; }
        internal protected required virtual string GradskaZona { get; set; }

        //veze
        internal protected virtual required Poslovnica PoslovnicaZaduzenaZaNjega { get; set; }
        internal protected virtual IList<Nekretnina>? Nekretnine { get; set; } = [];
    }
}
