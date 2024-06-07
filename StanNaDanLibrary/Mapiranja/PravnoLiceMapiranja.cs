namespace StanNaDanLibrary.Mapiranja
{
    public class PravnoLiceMapiranja : ClassMap<PravnoLice>
    {
        public PravnoLiceMapiranja()
        {
            Table("PRAVNO_LICE");

            Id(x => x.PIB, "PIB");

            Map(x => x.Naziv, "NAZIV");
            Map(x => x.AdresaSedista, "ADRESA_SEDISTA");
            Map(x => x.ImeKontaktOsobe, "IME_KONTAKT_OSOBE");
            Map(x => x.EmailKontaktOsobe, "EMAIL_KONTAKT_OSOBE");

            References(x => x.Vlasnik, "ID_VLASNIKA").Unique().Cascade.All();
            HasMany(x => x.TelefoniKontaktOsobe).KeyColumn("PIB_FIRME").LazyLoad().Cascade.All();
        }
    }
}
