namespace StanNaDanLibrary.Mapiranja
{
    internal class NajamMapiranja : ClassMap<Najam>
    {
        public NajamMapiranja() {

            Table("NAJAM");

            Id(p => p.IdNajma, "ID_NAJMA").GeneratedBy.TriggerIdentity();

            Map(p => p.DatumPocetka,"DATUM_POCETKA");
            Map(p => p.DatumZavrsetka, "DATUM_ZAVRSETKA");
            Map(p => p.BrojDana, "BROJ_DANA");
            Map(p => p.CenaPoDanu, "CENA_PO_DANU");
            Map(p => p.Popust, "POPUST").Nullable();
            Map(p => p.CenaSaPopustom, "CENA_SA_POPUSTOM");
            Map(p => p.ZaradaOdDodatnihUsluga, "ZARADA_OD_DODATNIH_USLUGA");
            Map(p => p.UkupnaCena, "UKUPNA_CENA");
            Map(p => p.ProvizijaAgencije, "PROVIZIJA_AGENCIJE");

            References(p => p.Nekretnina).Column("ID_NEKRETNINE").LazyLoad();

            
            References(p => p.Agent).Column("MBR_AGENTA").LazyLoad();

            References(p => p.SpoljniSaradnik)
            .Columns("MBR_AGENTA_ZA_SPOLJNOG", "ID_SPOLJNJEG_RADNIKA");

            HasManyToMany(p => p.Sobe)
                .Table("IZNAJMLJENA_SOBA")
                .ParentKeyColumn("ID_NAJMA")
                .ChildKeyColumns.Add("ID_NEKRETNINE", "ID_SOBE");
                //.Cascade.All();

            HasMany(p => p.IznajmljivanjaSoba).KeyColumn("ID_NAJMA").LazyLoad().Cascade.All().Inverse();
        }
    }
}
