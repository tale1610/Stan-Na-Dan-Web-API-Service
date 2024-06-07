namespace StanNaDanLibrary.Entiteti
{
    public class Kvart
    {
        public virtual int ID { get; protected set; }
        public required virtual string GradskaZona { get; set; }

        //veze
        public virtual required Poslovnica PoslovnicaZaduzenaZaNjega { get; set; }
        public virtual IList<Nekretnina>? Nekretnine { get; set; } = [];
    }
}
