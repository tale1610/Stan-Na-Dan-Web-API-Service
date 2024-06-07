namespace StanNaDanLibrary.Entiteti;

public class Najam
{
    virtual public int IdNajma { get; protected set; }
    virtual public required DateTime DatumPocetka { get; set; }
    virtual public required DateTime DatumZavrsetka { get; set; }
    virtual public required int BrojDana { get; set; }
    virtual public required double CenaPoDanu { get; set; }
    virtual public int? Popust { get; set; }
    virtual public double? CenaSaPopustom { get; set; }
    virtual public double ZaradaOdDodatnihUsluga { get; set; }
    virtual public double UkupnaCena { get; set; }
    virtual public double ProvizijaAgencije { get; set; }

    //veze:
    virtual public required Nekretnina Nekretnina { get; set; }//trenutno smo odradili da i kad se iznajmi samo soba pamti se u najmu nekretnina iz koje je soba izdata,
                                                                //a upisuju se podaci i u tabelu izdata soba
    virtual public required Agent Agent { get; set; }
    virtual public SpoljniSaradnik? SpoljniSaradnik { get; set; }
    virtual public IList<IznajmljenaSoba>? IznajmljivanjaSoba { get; set; } = [];
    virtual public IList<Soba>? Sobe { get; set; } = [];
        
}
