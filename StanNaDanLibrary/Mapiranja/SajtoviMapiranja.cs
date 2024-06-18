namespace StanNaDanLibrary.Mapiranja
{
    internal class SajtoviMapiranja : ClassMap<SajtoviNekretnine>
    {
        public SajtoviMapiranja()
        {
            Table("SAJTOVI");

            CompositeId(p => p.ID)
            .KeyProperty(p => p.Sajt, "SAJT")
            .KeyReference(p => p.Nekretnina, "ID_NEKRETNINE");


        }
    }
}
