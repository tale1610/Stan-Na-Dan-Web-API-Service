namespace StanNaDanLibrary.Entiteti
{
    //razmisli kako da resis problem sefa, da li da omogucis null u poslovnici i da prebacis id_sefa u poslovnicu
    public class Zaposleni
    {
        virtual public required string MBR { get; set; }
        virtual public required string  Ime { get; set; }
        virtual public required string Prezime { get; set; }
        virtual public required DateTime DatumZaposlenja { get; set; }
        virtual public Poslovnica? Poslovnica { get; set; }


        //[DozvoljeniTipoviPosla(new string[] {"sef", "agent", "ostalo"}, ErrorMessage ="Tip posla moze biti samo 'agent', 'sef' ili 'ostalo'")]
        //public required string TipPosla { get; set; }        
    }

    public class Sef : Zaposleni
    {
        virtual public required DateTime? DatumPostavljanja { get; set; }
    }

    public class Agent : Zaposleni
    {
        virtual public string? StrucnaSprema { get; set; }
        //veze
        virtual public IList<SpoljniSaradnik> AngazovaniSaradnici { get; set; } = [];
        virtual public IList<Najam> RealizovaniNajmovi { get; set; } = [];
    }

    public class Radnik : Zaposleni
    {

    }

    //public class DozvoljeniTipoviPosla : ValidationAttribute
    //{
    //    private readonly string[] _dozvoljeniTipovi;

    //    public DozvoljeniTipoviPosla(string[] dozvoljeniTipovi)
    //    {
    //        _dozvoljeniTipovi = dozvoljeniTipovi;
    //    }

    //    public override bool IsValid(object value)
    //    {
    //        string valueAsString = value as string;
    //        if (valueAsString == null)
    //        {
    //            return true;
    //        }
    //        return _dozvoljeniTipovi.Contains(valueAsString.ToLower());
    //    }
    //}
}

