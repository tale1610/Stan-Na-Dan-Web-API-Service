namespace StanNaDanLibrary;

#region Poslovnica
public class PoslovnicaView
{
    public int ID { get; set; }
    public string? Adresa { get; set; }
    public string? RadnoVreme { get; set; }
    public SefDTO? Sef { get; set; }
    public IList<ZaposleniView>? Zaposleni { get; set; }
    public IList<KvartView>? Kvartovi { get; set; }

    public PoslovnicaView()
    {
        Zaposleni = new List<ZaposleniView>();
        Kvartovi = new List<KvartView>();
    }

    public PoslovnicaView(Poslovnica? poslovnica) : this()
    {
        if (poslovnica != null)
        {
            ID = poslovnica.ID;
            Adresa = poslovnica.Adresa;
            RadnoVreme = poslovnica.RadnoVreme;
            Sef = poslovnica.Sef != null ? new SefDTO(poslovnica.Sef) : null;
            //Zaposleni = poslovnica.Zaposleni.Select(z => new ZaposleniView(z)).ToList();
            //Kvartovi = poslovnica.Kvartovi.Select(k => new KvartView(k)).ToList();
        }
    }
}

public class PoslovnicaDTO
{
    public int ID { get; set; }
    public string? Adresa { get; set; }
    public string? RadnoVreme { get; set; }

    public PoslovnicaDTO()
    {

    }

    public PoslovnicaDTO(Poslovnica? poslovnica) : this()
    {
        if (poslovnica != null)
        {
            ID = poslovnica.ID;
            Adresa = poslovnica.Adresa;
            RadnoVreme = poslovnica.RadnoVreme;
        }
    }
}

#endregion

#region Zaposleni
public class ZaposleniView
{
    public string? MBR { get; set; }
    public string? Ime { get; set; }
    public string? Prezime { get; set; }
    public DateTime DatumZaposlenja { get; set; }
    public PoslovnicaDTO? Poslovnica { get; set; }

    public ZaposleniView()
    {
    }
    public ZaposleniView(Zaposleni? zaposleni)
    {
        if (zaposleni != null)
        {
            MBR = zaposleni.MBR;
            Ime = zaposleni.Ime;
            Prezime = zaposleni.Prezime;
            DatumZaposlenja = zaposleni.DatumZaposlenja;
            Poslovnica = new PoslovnicaDTO(zaposleni.Poslovnica);
        }
    }
}

public class SefView : ZaposleniView
{
    public DateTime? DatumPostavljanja { get; set; }
    public SefView() : base()
    {
    }
    public SefView(Sef? sef) : base(sef)
    {
        if (sef != null)
        {
            DatumPostavljanja = sef.DatumPostavljanja;
        }
    }
}

public class AgentView : ZaposleniView
{
    public string? StrucnaSprema { get; set; }
    public AgentView() : base()
    {
    }
    public AgentView(Agent? agent) : base(agent)
    {
        if (agent != null)
        {
            StrucnaSprema = agent.StrucnaSprema;
        }
    }
}
public class ZaposleniDTO
{
    public string? MBR { get; set; }
    public string? Ime { get; set; }
    public string? Prezime { get; set; }
    public DateTime DatumZaposlenja { get; set; }

    public ZaposleniDTO()
    {
    }
    public ZaposleniDTO(Zaposleni? zaposleni)
    {
        if (zaposleni != null)
        {
            MBR = zaposleni.MBR;
            Ime = zaposleni.Ime;
            Prezime = zaposleni.Prezime;
            DatumZaposlenja = zaposleni.DatumZaposlenja;
        }
    }
}

public class SefDTO : ZaposleniDTO
{
    public DateTime? DatumPostavljanja { get; set; }
    public SefDTO() : base()
    {
    }
    public SefDTO(Sef? sef) : base(sef)
    {
        if (sef != null)
        {
            DatumPostavljanja = sef.DatumPostavljanja;
        }
    }
}

public class AgentDTO : ZaposleniDTO
{
    public string? StrucnaSprema { get; set; }
    public AgentDTO() : base()
    {
    }
    public AgentDTO(Agent? agent) : base(agent)
    {
        if (agent != null)
        {
            StrucnaSprema = agent.StrucnaSprema;
        }
    }
}
//public class ZaposleniOstaloView : ZaposleniView
//{
//    public ZaposleniOstaloView()
//    {
//    }

//    public ZaposleniOstaloView(ZaposleniOstalo zaposleniOstalo) : base(zaposleniOstalo)
//    {
//    }
//}

#endregion

#region SpoljniSaradnik
public class SpoljniSaradnikIdView
{
    public AgentView? AgentAngazovanja { get; set; }
    public int IdSaradnika { get; set; }

    public SpoljniSaradnikIdView()
    {
    }

    public SpoljniSaradnikIdView(SpoljniSaradnikId? s)
    {
        if (s != null)
        {
            AgentAngazovanja = new AgentView(s.AgentAngazovanja);
            IdSaradnika = s.IdSaradnika;
        }
    }
}

public class SpoljniSaradnikView
{
    public SpoljniSaradnikIdView? ID { get; set; }
    public string? Ime { get; set; }
    public string? Prezime { get; set; }
    public DateTime DatumAngazovanja { get; set; }
    public string? Telefon { get; set; }
    public double ProcenatOdNajma { get; set; }

    public SpoljniSaradnikView()
    {
    }

    public SpoljniSaradnikView(SpoljniSaradnik saradnik)
    {
        if (saradnik != null)
        {
            ID = new SpoljniSaradnikIdView(saradnik.ID);
            Ime = saradnik.Ime;
            Prezime = saradnik.Prezime;
            DatumAngazovanja = saradnik.DatumAngazovanja;
            Telefon = saradnik.Telefon;
            ProcenatOdNajma = saradnik.ProcenatOdNajma;
        }
    }
}
#endregion

#region Kvart
public class KvartView
{
    public int IdKvarta { get; set; }
    public string? GradskaZona { get; set; }
    public PoslovnicaView? PoslovnicaZaduzenaZaNjega { get; set; }

    public KvartView()
    {
    }

    public KvartView(Kvart? kvart)
    {
        if (kvart != null)
        {
            IdKvarta = kvart.ID;
            GradskaZona = kvart.GradskaZona;
            PoslovnicaZaduzenaZaNjega = new PoslovnicaView(kvart.PoslovnicaZaduzenaZaNjega);
        }
    }
}

#endregion

#region Vlasnik
public class VlasnikView
{
    public int IdVlasnika { get; set; }
    public string? Banka { get; set; }
    public string? BrojBankovnogRacuna { get; set; }

    public VlasnikView() { }

    public VlasnikView(Vlasnik? vlasnik)
    {
        if (vlasnik != null)
        {
            IdVlasnika = vlasnik.IdVlasnika;
            Banka = vlasnik.Banka;
            BrojBankovnogRacuna = vlasnik.BrojBankovnogRacuna;
        }
    }
}
#endregion

#region FizickoLice
public class FizickoLiceView
{
    public string? JMBG { get; set; }
    public string? Ime { get; set; }
    public string? ImeRoditelja { get; set; }
    public string? Prezime { get; set; }
    public string? Drzava { get; set; }
    public string? MestoStanovanja { get; set; }
    public string? AdresaStanovanja { get; set; }
    public DateTime DatumRodjenja { get; set; }
    public string? Email { get; set; }
    public VlasnikView? Vlasnik { get; set; }

    public FizickoLiceView() { }

    public FizickoLiceView(FizickoLice? fizickoLice)
    {
        if (fizickoLice != null)
        {
            JMBG = fizickoLice.JMBG;
            Ime = fizickoLice.Ime;
            ImeRoditelja = fizickoLice.ImeRoditelja;
            Prezime = fizickoLice.Prezime;
            Drzava = fizickoLice.Drzava;
            MestoStanovanja = fizickoLice.MestoStanovanja;
            AdresaStanovanja = fizickoLice.AdresaStanovanja;
            DatumRodjenja = fizickoLice.DatumRodjenja;
            Email = fizickoLice.Email;
            Vlasnik = new VlasnikView(fizickoLice.Vlasnik);
        }
    }
}
#endregion

#region PravnoLice
public class PravnoLiceView
{
    public string? PIB { get; set; }
    public string? Naziv { get; set; }
    public string? AdresaSedista { get; set; }
    public string? ImeKontaktOsobe { get; set; }
    public string? EmailKontaktOsobe { get; set; }
    public VlasnikView? Vlasnik { get; set; }

    public PravnoLiceView() { }

    public PravnoLiceView(PravnoLice? pravnoLice)
    {
        if (pravnoLice != null)
        {
            PIB = pravnoLice.PIB;
            Naziv = pravnoLice.Naziv;
            AdresaSedista = pravnoLice.AdresaSedista;
            ImeKontaktOsobe = pravnoLice.ImeKontaktOsobe;
            EmailKontaktOsobe = pravnoLice.EmailKontaktOsobe;
            Vlasnik = new VlasnikView(pravnoLice.Vlasnik);
        }
    }
}
#endregion

#region Nekretnina
public class NekretninaView
{
    public int IdNekretnine { get; set; }
    //public string Tip { get; set; }
    public string? Ulica { get; set; }
    public string? Broj { get; set; }
    public double Kvadratura { get; set; }
    public int BrojTerasa { get; set; }
    public int BrojKupatila { get; set; }
    public int BrojSpavacihSoba { get; set; }
    public bool PosedujeTV { get; set; }
    public bool PosedujeInternet { get; set; }
    public bool PosedujeKuhinju { get; set; }
    public string? GradskaZona { get; set; }
    public int IdVlasnika { get; set; }

    public NekretninaView() { }

    public NekretninaView(Nekretnina? nekretnina)
    {
        if (nekretnina != null)
        {
            IdNekretnine = nekretnina.IdNekretnine;
            //Tip = nekretnina.Tip;
            Ulica = nekretnina.Ulica;
            Broj = nekretnina.Broj;
            Kvadratura = nekretnina.Kvadratura;
            BrojTerasa = nekretnina.BrojTerasa;
            BrojKupatila = nekretnina.BrojKupatila;
            BrojSpavacihSoba = nekretnina.BrojSpavacihSoba;
            PosedujeTV = nekretnina.PosedujeTV;
            PosedujeInternet = nekretnina.PosedujeInternet;
            PosedujeKuhinju = nekretnina.PosedujeKuhinju;
            GradskaZona = nekretnina.Kvart.GradskaZona;
            IdVlasnika = nekretnina.Vlasnik.IdVlasnika;
        }
    }
}
#endregion

#region Stan
public class StanView : NekretninaView
{
    public int Sprat { get; set; }
    public bool PosedujeLift { get; set; }

    public StanView() { }

    public StanView(Stan? stan)
        : base(stan)
    {
        if (stan != null)
        {
            Sprat = stan.Sprat;
            PosedujeLift = stan.PosedujeLift;
        }
    }
}
#endregion

#region Kuca
public class KucaView : NekretninaView
{
    public int Spratnost { get; set; }
    public bool PosedujeDvoriste { get; set; }

    public KucaView() { }

    public KucaView(Kuca? kuca)
        : base(kuca)
    {
        if (kuca != null)
        {
            Spratnost = kuca.Spratnos;
            PosedujeDvoriste = kuca.PosedujeDvoriste;
        }
    }
}
#endregion

#region BrojeviTelefona
public class BrojeviTelefonaView
{
    public string? BrojTelefona { get; set; }
    public string? JMBG { get; set; }
    public string? Ime { get; set; }
    public string? Prezime { get; set; }

    public BrojeviTelefonaView() { }

    public BrojeviTelefonaView(BrojeviTelefona? brojTelefona)
    {
        if (brojTelefona != null)
        {
            BrojTelefona = brojTelefona.BrojTelefona;
            JMBG = brojTelefona.FizickoLice.JMBG;
            Ime = brojTelefona.FizickoLice.Ime;
            Prezime = brojTelefona.FizickoLice.Prezime;
        }
    }
}
#endregion

#region DodatnaOprema
public class DodatnaOpremaIdView
{
    public int IdOpreme { get; set; }
    public NekretninaView? Nekretnina { get; set; }

    public DodatnaOpremaIdView() { }

    public DodatnaOpremaIdView(DodatnaOpremaId? d)
    {
        if (d != null)
        {
            IdOpreme = d.IdOpreme;
            Nekretnina = new NekretninaView(d.Nekretnina);
        }
    }
}

public class DodatnaOpremaView
{
    public DodatnaOpremaIdView? ID { get; set; }
    public string? TipOpreme { get; set; }
    public bool BesplatnoKoriscenje { get; set; }
    public double? CenaKoriscenja { get; set; }

    public DodatnaOpremaView() { }

    public DodatnaOpremaView(DodatnaOprema? dodatnaOprema)
    {
        if (dodatnaOprema != null)
        {
            ID = new DodatnaOpremaIdView(dodatnaOprema.ID);
            TipOpreme = dodatnaOprema.TipOpreme;
            BesplatnoKoriscenje = dodatnaOprema.BesplatnoKoriscenje;
            CenaKoriscenja = dodatnaOprema.CenaKoriscenja;
        }
    }
}
#endregion

#region IznajmljenaSoba
public class IznajmljenaSobaIdView
{
    public SobaView? Soba { get; set; }
    public NajamView? Najam { get; set; }

    public IznajmljenaSobaIdView() { }

    public IznajmljenaSobaIdView(IznajmljenaSobaId? i)
    {
        if (i != null)
        {
            Soba = new SobaView(i.Soba);
            Najam = new NajamView(i.Najam);
        }
    }
}
public class IznajmljenaSobaView
{
    public IznajmljenaSobaIdView? ID { get; set; }

    public IznajmljenaSobaView() { }

    public IznajmljenaSobaView(IznajmljenaSoba? iznajmljenaSoba)
    {
        if (iznajmljenaSoba != null)
        {
            ID = new IznajmljenaSobaIdView(iznajmljenaSoba.ID);
        }
    }
}
#endregion

#region Krevet
public class KrevetIdView
{
    public int IdKreveta { get; set; }
    public NekretninaView? Nekretnina { get; set; }

    public KrevetIdView() { }

    public KrevetIdView(KrevetId? k)
    {
        if (k != null)
        {
            IdKreveta = k.IdKreveta;
            Nekretnina = new NekretninaView(k.Nekretnina);
        }
    }
}
public class KrevetView
{
    public KrevetIdView? ID { get; set; }
    public string? Tip { get; set; }
    public string? Dimenzije { get; set; }

    public KrevetView() { }

    public KrevetView(Krevet? krevet)
    {
        if (krevet != null)
        {
            ID = new KrevetIdView(krevet.ID);
            Tip = krevet.Tip;
            Dimenzije = krevet.Dimenzije;
        }
    }
}
#endregion

#region Najam
public class NajamView
{
    public int IdNajma { get; set; }
    public DateTime DatumPocetka { get; set; }
    public DateTime DatumZavrsetka { get; set; }
    public int BrojDana { get; set; }
    public double CenaPoDanu { get; set; }
    public int? Popust { get; set; }
    public double? CenaSaPopustom { get; set; }
    public double ZaradaOdDodatnihUsluga { get; set; }
    public double UkupnaCena { get; set; }
    public double ProvizijaAgencije { get; set; }
    public string? AdresaNekretnine { get; set; }
    public string? ImeAgenta { get; set; }
    public string? ImeSpoljnogSaradnika { get; set; }

    public NajamView() { }

    public NajamView(Najam? najam)
    {
        if (najam != null)
        {
            IdNajma = najam.IdNajma;
            DatumPocetka = najam.DatumPocetka;
            DatumZavrsetka = najam.DatumZavrsetka;
            BrojDana = najam.BrojDana;
            CenaPoDanu = najam.CenaPoDanu;
            Popust = najam.Popust;
            CenaSaPopustom = najam.CenaSaPopustom;
            ZaradaOdDodatnihUsluga = najam.ZaradaOdDodatnihUsluga;
            UkupnaCena = najam.UkupnaCena;
            ProvizijaAgencije = najam.ProvizijaAgencije;
            AdresaNekretnine = najam.Nekretnina.Ulica + " " + najam.Nekretnina.Broj;
            ImeAgenta = najam.Agent.Ime;
            ImeSpoljnogSaradnika = najam.SpoljniSaradnik?.Ime ?? "";
        }
    }
}
#endregion

#region Parking
public class ParkingIdView
{
    public int IdParkinga { get; set; }
    public NekretninaView? Nekretnina { get; set; }

    public ParkingIdView() { }

    public ParkingIdView(ParkingId? id)
    {
        if (id != null)
        {
            IdParkinga = id.IdParkinga;
            Nekretnina = new NekretninaView(id.Nekretnina);
        }
    }
}
public class ParkingView
{
    public ParkingIdView? ID { get; set; }
    public bool Besplatan { get; set; }
    public double? Cena { get; set; }
    public bool USastavuNekretnine { get; set; }
    public bool USastavuJavnogParkinga { get; set; }

    public ParkingView() { }

    public ParkingView(Parking parking)
    {
        if (parking != null)
        {
            ID = new ParkingIdView(parking.ID);
            Besplatan = parking.Besplatan;
            Cena = parking.Cena;
            USastavuNekretnine = parking.USastavuNekretnine;
            USastavuJavnogParkinga = parking.USastavuJavnogParkinga;
        }
    }
}
#endregion

#region SajtoviNekretnine
public class SajtoviNekretnineIdView
{
    public string? Sajt { get; set; }
    public NekretninaView? Nekretnina { get; set; }

    public SajtoviNekretnineIdView() { }

    public SajtoviNekretnineIdView(SajtoviNekretnineId? id)
    {
        if (id != null)
        {
            Sajt = id.Sajt;
            Nekretnina = new NekretninaView(id.Nekretnina);
        }

    }
}
public class SajtoviNekretnineView
{
    public SajtoviNekretnineIdView? ID { get; set; }
    public string? AdresaNekretnine { get; set; }

    public SajtoviNekretnineView() { }

    public SajtoviNekretnineView(SajtoviNekretnine? sajtoviNekretnine)
    {
        if (sajtoviNekretnine != null)
        {
            ID = new SajtoviNekretnineIdView(sajtoviNekretnine.ID);
            AdresaNekretnine = sajtoviNekretnine.ID.Nekretnina.Ulica + " " + sajtoviNekretnine.ID.Nekretnina.Broj;
        }

    }
}
#endregion

#region Soba
public class SobaIdView
{
    public int IdSobe { get; set; }
    public NekretninaView? Nekretnina { get; set; }

    public SobaIdView() { }

    public SobaIdView(SobaId? id)
    {
        if (id != null)
        {
            IdSobe = id.IdSobe;
            Nekretnina = new NekretninaView(id.Nekretnina);
        }
    }
}
public class SobaView
{
    public SobaIdView? ID { get; set; }

    public SobaView() { }

    public SobaView(Soba? soba)
    {
        if (soba != null)
        {
            ID = new SobaIdView(soba.ID);
        }
    }
}
#endregion

#region TelefoniKontaktOsobe
public class TelefoniKontaktOsobeView
{
    public string? BrojTelefona { get; set; }
    public string? PIB { get; set; }
    public string? Naziv { get; set; }
    public string? ImeKontaktOsobe { get; set; }

    public TelefoniKontaktOsobeView() { }

    public TelefoniKontaktOsobeView(TelefoniKontaktOsobe? telefoniKontaktOsobe)
    {
        if (telefoniKontaktOsobe != null)
        {
            BrojTelefona = telefoniKontaktOsobe.BrojTelefona;
            PIB = telefoniKontaktOsobe.PravnoLice.PIB;
            Naziv = telefoniKontaktOsobe.PravnoLice.Naziv;
            ImeKontaktOsobe = telefoniKontaktOsobe.PravnoLice.ImeKontaktOsobe;
        }
    }
}
#endregion

#region ZajednickeProstorije
public class ZajednickeProstorijeIdView
{
    public SobaView? Soba { get; set; }
    public string? ZajednickaProstorija { get; set; }

    public ZajednickeProstorijeIdView() { }

    public ZajednickeProstorijeIdView(ZajednickeProstorijeId? id)
    {
        if (id != null)
        {
            Soba = new SobaView(id.Soba);
            ZajednickaProstorija = id.ZajednickaProstorija;
        }
    }
}
public class ZajednickeProstorijeView
{
    public ZajednickeProstorijeIdView? ID { get; set; }

    public ZajednickeProstorijeView() { }

    public ZajednickeProstorijeView(ZajednickeProstorije? zajednickeProstorije)
    {
        if (zajednickeProstorije != null)
        {
            ID = new ZajednickeProstorijeIdView(zajednickeProstorije.ID);
        }
    }
}
#endregion