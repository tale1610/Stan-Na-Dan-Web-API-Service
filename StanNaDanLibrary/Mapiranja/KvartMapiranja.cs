namespace StanNaDanLibrary.Mapiranja
{
    class KvartMapiranja : ClassMap<Kvart>
    {
        public KvartMapiranja() {
            Table("KVART");

            Id(p => p.ID, "ID_KVARTA").GeneratedBy.TriggerIdentity();

            Map(p => p.GradskaZona, "GRADSKA_ZONA");

            HasMany(p => p.Nekretnine).KeyColumn("ID_KVARTA").LazyLoad().Cascade.All().Inverse();

            References(p => p.PoslovnicaZaduzenaZaNjega).Column("ID_POSLOVNICE").LazyLoad();

        }
    }
}
