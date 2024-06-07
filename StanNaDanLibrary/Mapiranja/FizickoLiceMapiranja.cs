namespace StanNaDanLibrary.Mapiranja
{
    public class FizickoLiceMapiranja : ClassMap<FizickoLice>
    {
        public FizickoLiceMapiranja()
        {
            Table("FIZICKO_LICE");

            Id(x => x.JMBG, "JMBG");

            Map(x => x.Ime, "IME");
            Map(x => x.ImeRoditelja, "IME_RODITELJA");
            Map(x => x.Prezime, "PREZIME");
            Map(x => x.Drzava, "DRZAVA");
            Map(x => x.MestoStanovanja, "MESTO_STANOVANJA");
            Map(x => x.AdresaStanovanja, "ADRESA_STANOVANJA");
            Map(x => x.DatumRodjenja, "DATUM_RODJENJA");
            Map(x => x.Email, "EMAIL");

            References(x => x.Vlasnik, "ID_VLASNIKA").Unique().Cascade.All();

            HasMany(x => x.BrojeviTelefona).KeyColumn("JMBG_LICA").LazyLoad().Cascade.All();
        }
    }
}
