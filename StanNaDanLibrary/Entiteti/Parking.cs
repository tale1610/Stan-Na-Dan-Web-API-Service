namespace StanNaDanLibrary.Entiteti;

internal class Parking
{
    virtual internal protected required ParkingId ID { get; set; }
    //virtual public required Nekretnina Nekretnina { get; set; }
    //virtual public required int IdParkinga { get; set; }
    virtual internal protected required bool Besplatan { get; set; }
    virtual internal protected double? Cena { get; set; }
    virtual internal protected required bool USastavuNekretnine { get; set; }
    virtual internal protected required bool USastavuJavnogParkinga { get; set; }

}