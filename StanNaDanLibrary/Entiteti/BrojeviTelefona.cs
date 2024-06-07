namespace StanNaDanLibrary.Entiteti
{
    public class BrojeviTelefona
    {
        virtual public required string BrojTelefona { get; set; }//ovo je PK nema potrebe za surogatom jer je broj jedinstven
        //veza
        virtual public required FizickoLice FizickoLice { get; set; }
    }
}
