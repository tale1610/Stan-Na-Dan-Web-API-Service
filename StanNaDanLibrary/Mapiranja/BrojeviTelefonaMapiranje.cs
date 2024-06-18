namespace StanNaDanLibrary.Mapiranja
{
    internal class BrojeviTelefonaMapiranja : ClassMap<BrojeviTelefona>
    {
        public BrojeviTelefonaMapiranja()
        {
            Table("BROJEVI_TELEFONA");
            Id(p => p.BrojTelefona, "BROJ_TELEFONA").GeneratedBy.Assigned();
            References(p => p.FizickoLice).Column("JMBG_LICA").LazyLoad();
        }
    }
}
