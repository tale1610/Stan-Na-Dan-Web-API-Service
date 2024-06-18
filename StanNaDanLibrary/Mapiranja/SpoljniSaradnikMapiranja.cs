namespace StanNaDanLibrary.Mapiranja
{
    internal class SpoljniSaradnikMapiranja : ClassMap<SpoljniSaradnik>
    {
        public SpoljniSaradnikMapiranja()
        {
            Table("SPOLJNI_SARADNIK");

            CompositeId(p => p.ID)
            .KeyReference(p => p.AgentAngazovanja, "MBR_AGENTA")
            .KeyProperty(p => p.IdSaradnika, "ID_SARADNIKA");

            Map(p => p.Ime, "IME");
            Map(p => p.Prezime, "PREZIME");
            Map(p => p.DatumAngazovanja, "DATUM_ANGAZOVANJA");
            Map(p => p.Telefon, "TELEFON");
            Map(p => p.ProcenatOdNajma, "PROCENAT_OD_NAJMA");


            HasMany(p => p.RealizovaniNajmovi)
            .Table("NAJAM")
            .KeyColumns.Add("MBR_AGENTA_ZA_SPOLJNOG", "ID_Spoljnjeg_Radnika")
            .LazyLoad()
            .Cascade.All();
        }
    }
}
