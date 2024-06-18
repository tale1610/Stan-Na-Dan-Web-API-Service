namespace StanNaDanLibrary.Entiteti;

internal class Najam
{
    virtual internal protected int IdNajma { get; protected set; }
    virtual internal protected required DateTime DatumPocetka { get; set; }
    virtual internal protected required DateTime DatumZavrsetka { get; set; }
    virtual internal protected required int BrojDana { get; set; }
    virtual internal protected required double CenaPoDanu { get; set; }
    virtual internal protected int? Popust { get; set; }
    virtual internal protected double? CenaSaPopustom { get; set; }
    virtual internal protected double ZaradaOdDodatnihUsluga { get; set; }
    virtual internal protected double UkupnaCena { get; set; }
    virtual internal protected double ProvizijaAgencije { get; set; }

    //veze:
    virtual internal protected required Nekretnina Nekretnina { get; set; }//trenutno smo odradili da i kad se iznajmi samo soba pamti se u najmu nekretnina iz koje je soba izdata,
                                                                //a upisuju se podaci i u tabelu izdata soba
    virtual internal protected required Agent Agent { get; set; }
    virtual internal protected SpoljniSaradnik? SpoljniSaradnik { get; set; }
    virtual internal protected IList<IznajmljenaSoba>? IznajmljivanjaSoba { get; set; } = [];
    virtual internal protected IList<Soba>? Sobe { get; set; } = [];
        
}
