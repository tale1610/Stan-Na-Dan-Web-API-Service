namespace StanNaDanLibrary.Entiteti;

public class Soba
{
    //kompozitni kljuc
    virtual public required SobaId ID { get; set; }
    //virtual public required int IdSobe { get; set; }
    //veza
    //virtual public required Nekretnina Nekretnina { get; set; } // ova veza na klasu je prebacena u klasu Id
    virtual public IList<Najam> Najmovi { get; set; } = [];
    virtual public IList<IznajmljenaSoba> IznajmljivanjaSobe { get; set; } = [];
    virtual public IList<ZajednickeProstorije> ZajednickeProstorije { get; set; } = [];
}
