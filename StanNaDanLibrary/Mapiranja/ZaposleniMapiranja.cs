namespace StanNaDanLibrary.Mapiranja
{
    internal class ZaposleniMapiranja : ClassMap<Zaposleni>
    {
        public ZaposleniMapiranja() {
            Table("ZAPOSLENI");

            DiscriminateSubClassesOnColumn("TIP_POSLA");

            Id(p => p.MBR, "MBR").GeneratedBy.Assigned();

            Map(p => p.Ime, "IME");
            Map(p => p.Prezime, "PREZIME");
            Map(p => p.DatumZaposlenja, "DATUM_ZAPOSLENJA");

            //1:N veza ka prodanici, ako je sef to mu je prodavnica sefovanja ako je agent ili ostalo tu radi
            References(p => p.Poslovnica).Column("ID_POSLOVNICE").LazyLoad();
        }
    }
    internal class SefMapiranja : SubclassMap<Sef>
    {
        public SefMapiranja()
        {
            DiscriminatorValue("sef");

            Map(p => p.DatumPostavljanja, "DATUM_POSTAVLJANJA");
        }
    }
    internal class AgentMapiranja : SubclassMap<Agent>
    {
        public AgentMapiranja()
        {
            DiscriminatorValue("agent");

            Map(p => p.StrucnaSprema, "STRUCNA_SPREMA");

            HasMany(p => p.AngazovaniSaradnici).KeyColumn("MBR_AGENTA").Cascade.All().Inverse();
            HasMany(p => p.RealizovaniNajmovi).KeyColumn("MBR_AGENTA").Cascade.All().Inverse();
        }
    }
    internal class RadnikMapiranja : SubclassMap<Radnik>
    {
        public RadnikMapiranja()
        {
            DiscriminatorValue("ostalo");
        }
    }
}


