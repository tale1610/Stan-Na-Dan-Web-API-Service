namespace StanNaDanLibrary.Mapiranja
{
    internal class NekretninaMapiranja : ClassMap<Nekretnina>
    {
        public NekretninaMapiranja() {
            Table("NEKRETNINA");

            DiscriminateSubClassesOnColumn("TIP_NEKRETNINE");


            Id(p => p.IdNekretnine, "ID_NEKRETNINE").GeneratedBy.TriggerIdentity();

            Map(p => p.Kvadratura, "KVADRATURA");
            Map(p => p.BrojTerasa, "BROJ_TERASA");
            Map(p => p.BrojKupatila, "BROJ_KUPATILA");
            Map(p => p.BrojSpavacihSoba, "BROJ_SPAVACIH_SOBA");
            Map(p => p.PosedujeTV, "POSEDUJE_TV_PRIKLJUCAK").CustomType<BooleanToStringType>();
            Map(p => p.PosedujeKuhinju, "POSEDUJE_KUHINJU").CustomType<BooleanToStringType>();
            Map(p => p.PosedujeInternet, "POSEDUJE_INTERNET").CustomType<BooleanToStringType>();
            Map(p => p.Ulica, "ULICA");
            Map(p => p.Broj, "BROJ");

            References(p => p.Kvart).Column("ID_KVARTA").LazyLoad();

            References(p => p.Vlasnik).Column("ID_VLASNIKA").LazyLoad();

            HasMany(p => p.DodatnaOprema).KeyColumn("ID_NEKRETNINE").LazyLoad().Cascade.All().Inverse();

            HasMany(p => p.Parking).KeyColumn("ID_NEKRETNINE").LazyLoad().Cascade.All().Inverse();

            HasMany(p => p.Kreveti).KeyColumn("ID_NEKRETNINE").LazyLoad().Cascade.All().Inverse();

            HasMany(p => p.Sobe).KeyColumn("ID_NEKRETNINE").LazyLoad().Cascade.All().Inverse();

            HasMany(p => p.SajtoviNekretnine).KeyColumn("ID_NEKRETNINE").LazyLoad().Cascade.All().Inverse();

            HasMany(p => p.Najmovi).KeyColumn("ID_NEKRETNINE").LazyLoad().Cascade.All().Inverse();
        }
    }

    internal class StanMapiranja : SubclassMap<Stan>
    {
        public StanMapiranja()
        {
            DiscriminatorValue("stan");

            Map(p => p.Sprat, "SPRAT");
            Map(p => p.PosedujeLift, "POSEDUJE_LIFT").CustomType<BooleanToStringType>();

        }
    }

    internal class KucaMapiranja : SubclassMap<Kuca>
    {
        public KucaMapiranja()
        {
            DiscriminatorValue("kuca");

            Map(p => p.Spratnos, "SPRATNOST_KUCE");
            Map(p => p.PosedujeDvoriste, "POSEDUJE_DVORISTE").CustomType<BooleanToStringType>();
        }

    }
}

