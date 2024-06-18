namespace StanNaDanLibrary.Entiteti;

internal class Soba
{
    //kompozitni kljuc
    virtual internal protected required SobaId ID { get; set; }
    //virtual public required int IdSobe { get; set; }
    //veza
    //virtual public required Nekretnina Nekretnina { get; set; } // ova veza na klasu je prebacena u klasu Id
    virtual internal protected IList<Najam> Najmovi { get; set; } = [];
    virtual internal protected IList<IznajmljenaSoba> IznajmljivanjaSobe { get; set; } = [];
    virtual internal protected IList<ZajednickeProstorije> ZajednickeProstorije { get; set; } = [];
}
