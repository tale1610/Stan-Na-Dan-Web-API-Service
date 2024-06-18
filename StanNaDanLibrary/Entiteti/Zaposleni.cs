namespace StanNaDanLibrary.Entiteti
{
    //razmisli kako da resis problem sefa, da li da omogucis null u poslovnici i da prebacis id_sefa u poslovnicu
    internal class Zaposleni
    {
        virtual internal protected required string MBR { get; set; }
        virtual internal protected required string  Ime { get; set; }
        virtual internal protected required string Prezime { get; set; }
        virtual internal protected required DateTime DatumZaposlenja { get; set; }
        virtual internal protected Poslovnica? Poslovnica { get; set; }


        //[DozvoljeniTipoviPosla(new string[] {"sef", "agent", "ostalo"}, ErrorMessage ="Tip posla moze biti samo 'agent', 'sef' ili 'ostalo'")]
        //public required string TipPosla { get; set; }        
    }

    internal class Sef : Zaposleni
    {
        virtual internal protected required DateTime? DatumPostavljanja { get; set; }
    }

    internal class Agent : Zaposleni
    {
        virtual internal protected string? StrucnaSprema { get; set; }
        //veze
        virtual internal protected IList<SpoljniSaradnik> AngazovaniSaradnici { get; set; } = [];
        virtual internal protected IList<Najam> RealizovaniNajmovi { get; set; } = [];
    }

    internal class Radnik : Zaposleni
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

