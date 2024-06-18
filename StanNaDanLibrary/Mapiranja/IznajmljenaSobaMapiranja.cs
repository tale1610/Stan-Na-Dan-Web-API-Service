namespace StanNaDanLibrary.Mapiranja
{
    internal class IznajmljenaSobaMapiranja : ClassMap<IznajmljenaSoba>
    {
        public IznajmljenaSobaMapiranja()
        {
            Table("IZNAJMLJENA_SOBA");
            CompositeId(p => p.ID)
                .KeyReference(p => p.Najam, "ID_NAJMA")
                .KeyReference(p => p.Soba, "ID_NEKRETNINE", "ID_SOBE");
        }
    }
}
