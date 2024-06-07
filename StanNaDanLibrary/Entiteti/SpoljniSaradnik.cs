namespace StanNaDanLibrary.Entiteti
{
    public class SpoljniSaradnik
    {
        //virtual public int ID { get; protected set; }

        virtual public required SpoljniSaradnikId ID { get; set; }
        virtual public required string Ime { get; set; }
        virtual public required string Prezime { get; set; }
        virtual public required DateTime DatumAngazovanja { get; set; }
        virtual public required string Telefon { get; set; }
        virtual public required double ProcenatOdNajma { get; set; }

        //veza
        //virtual public required Agent AgentAngazovanja { get; set; } // ova veza na klasu je prebacena u klasu Id
        virtual public IList<Najam> RealizovaniNajmovi { get; set; } = [];
    }
}
