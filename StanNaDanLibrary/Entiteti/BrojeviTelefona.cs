namespace StanNaDanLibrary.Entiteti
{
    internal class BrojeviTelefona
    {
        virtual internal protected required string BrojTelefona { get; set; }//ovo je PK nema potrebe za surogatom jer je broj jedinstven
        //veza
        virtual internal protected required FizickoLice FizickoLice { get; set; }
    }
}
