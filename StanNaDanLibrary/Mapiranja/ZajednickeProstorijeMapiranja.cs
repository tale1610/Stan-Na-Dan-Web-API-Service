namespace StanNaDanLibrary.Mapiranja
{
    internal class ZajednickeProstorijeMapiranja : ClassMap<ZajednickeProstorije>
    {
        public ZajednickeProstorijeMapiranja()
        {
            Table("ZAJEDNICKE_PROSTORIJE");

            CompositeId(p => p.ID)
                .KeyProperty(p => p.ZajednickaProstorija, "ZAJEDNICKA_PROSTORIJA")
                .KeyReference(p => p.Soba, "ID_NEKRETNINE", "ID_SOBE");
        }
    }
}
