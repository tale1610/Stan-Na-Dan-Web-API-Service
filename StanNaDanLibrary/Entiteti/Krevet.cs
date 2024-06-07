namespace StanNaDanLibrary.Entiteti;

public class Krevet
{
    virtual public required KrevetId ID { get; set; }

    //virtual public required Nekretnina Nekretnina { get; set;}
    //virtual public required int IdKreveta { get; set;}

    virtual public required string Tip { get; set;}
    virtual public required string Dimenzije { get; set;}
}