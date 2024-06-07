namespace StanNaDanLibrary.Mapiranja
{
    class DodatnaOpremaMapiranja : ClassMap<DodatnaOprema>
    {
        public DodatnaOpremaMapiranja() {

            Table("DODATNA_OPREMA");

            CompositeId(p => p.ID)
            .KeyProperty(p => p.IdOpreme, "ID_OPREME")
            .KeyReference(p => p.Nekretnina, "ID_NEKRETNINE");
             
            Map(p => p.TipOpreme, "TIP_OPREME");
            Map(p => p.BesplatnoKoriscenje, "BESPLATNO_KORISCENJE").CustomType<BooleanToStringType>();
            Map(p => p.CenaKoriscenja, "CENA_KORISCENJA").Nullable();

        }
    }
}
