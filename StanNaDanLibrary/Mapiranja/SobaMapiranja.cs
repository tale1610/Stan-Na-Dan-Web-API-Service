namespace StanNaDanLibrary.Mapiranja;

internal class SobaMapiranja : ClassMap<Soba>
{
    public SobaMapiranja() 
    {
        Table("SOBA");

        CompositeId(p => p.ID)
            .KeyReference(p => p.Nekretnina, "ID_NEKRETNINE")
            .KeyProperty(p => p.IdSobe, "ID_SOBE");

        HasMany(x => x.ZajednickeProstorije)
                .KeyColumns.Add("ID_NEKRETNINE", "ID_SOBE")
                .Cascade.All()
                .Inverse();

        HasManyToMany(p => p.Najmovi)
                .Table("IZNAJMLJENA_SOBA")
                .ParentKeyColumns.Add("ID_NEKRETNINE", "ID_SOBE")
                .ChildKeyColumn("ID_NAJMA").Inverse();
                //.Cascade.All();

        HasMany(p => p.IznajmljivanjaSobe).KeyColumns.Add("ID_NEKRETNINE", "ID_SOBE").LazyLoad().Cascade.All().Inverse();
    }
}
