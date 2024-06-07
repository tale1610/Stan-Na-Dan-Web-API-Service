namespace StanNaDanLibrary.Entiteti;

public class Parking
{
    virtual public required ParkingId ID { get; set; }
    //virtual public required Nekretnina Nekretnina { get; set; }
    //virtual public required int IdParkinga { get; set; }
    virtual public required bool Besplatan { get; set; }
    virtual public double? Cena { get; set; }
    virtual public required bool USastavuNekretnine { get; set; }
    virtual public required bool USastavuJavnogParkinga { get; set; }

}