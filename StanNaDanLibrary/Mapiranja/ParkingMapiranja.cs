namespace StanNaDanLibrary.Mapiranja
{
    class ParkingMapiranja : ClassMap<Parking>
    {
        public ParkingMapiranja() {
            Table("PARKING");

            CompositeId(p => p.ID)
            .KeyProperty(p => p.IdParkinga, "ID_PARKINGA")
            .KeyReference(p => p.Nekretnina, "ID_NEKRETNINE");

            Map(p => p.Besplatan, "BESPLATAN").CustomType<BooleanToStringType>();
            Map(p => p.Cena, "CENA").Nullable();
            Map(p => p.USastavuNekretnine, "U_SASTAVU_NEKRETNINE").CustomType<BooleanToStringType>();
            Map(p => p.USastavuJavnogParkinga, "U_SASTAVU_JAVNOG_PARKINGA").CustomType<BooleanToStringType>();
            
        }

    }
}
