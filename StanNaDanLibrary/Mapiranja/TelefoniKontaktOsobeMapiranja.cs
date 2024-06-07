namespace StanNaDanLibrary.Mapiranja
{
    class TelefoniKontaktOsobeMapiranja : ClassMap<TelefoniKontaktOsobe>
    {
        public TelefoniKontaktOsobeMapiranja()
        {
            Table("TELEFONI_KONTAKT_OSOBE");
            Id(p => p.BrojTelefona, "BROJ_TELEFONA").GeneratedBy.Assigned();
            References(p => p.PravnoLice).Column("PIB_FIRME").LazyLoad();
        }
    }
}
