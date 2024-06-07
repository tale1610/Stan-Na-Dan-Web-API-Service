namespace StanNaDanLibrary.Entiteti
{
    public class Poslovnica
    {
        public virtual int ID { get; protected set; }//protected zato sto se koristi trigger identity
        public virtual required string Adresa { get; set; }
        public virtual required string RadnoVreme { get; set; }
        public virtual Sef? Sef { get; set; }
        public virtual IList<Zaposleni>? Zaposleni { get; set; } = [];
        public virtual IList<Kvart>? Kvartovi { get; set; } = [];
    }
}
