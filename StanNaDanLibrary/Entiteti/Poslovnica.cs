namespace StanNaDanLibrary.Entiteti
{
    internal class Poslovnica
    {
        internal protected virtual int ID { get; protected set; }//protected zato sto se koristi trigger identity
        internal protected virtual required string Adresa { get; set; }
        internal protected virtual required string RadnoVreme { get; set; }
        internal protected virtual Sef? Sef { get; set; }
        internal protected virtual IList<Zaposleni>? Zaposleni { get; set; } = [];
        internal protected virtual IList<Kvart>? Kvartovi { get; set; } = [];
    }
}
