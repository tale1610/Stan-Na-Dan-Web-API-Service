namespace StanNaDanLibrary.Entiteti;

internal class DodatnaOprema
{
    internal protected virtual required DodatnaOpremaId ID { get; set; }
    //virtual public int IdOpreme { get; protected set; }
    //virtual public required Nekretnina Nekretnina { get; set; }
    virtual internal protected required string TipOpreme { get; set; }
    virtual internal protected required bool BesplatnoKoriscenje { get; set; }
    virtual internal protected double? CenaKoriscenja { get; set; }
}
