namespace StanNaDanLibrary.Entiteti;

public class DodatnaOprema
{
    public virtual required DodatnaOpremaId ID { get; set; }
    //virtual public int IdOpreme { get; protected set; }
    //virtual public required Nekretnina Nekretnina { get; set; }
    virtual public required string TipOpreme { get; set; }
    virtual public required bool BesplatnoKoriscenje { get; set; }
    virtual public double? CenaKoriscenja { get; set; }
}
