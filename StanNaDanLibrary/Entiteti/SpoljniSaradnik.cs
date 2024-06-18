namespace StanNaDanLibrary.Entiteti
{
    internal class SpoljniSaradnik
    {
        //virtual public int ID { get; protected set; }

        virtual internal protected required SpoljniSaradnikId ID { get; set; }
        virtual internal protected required string Ime { get; set; }
        virtual internal protected required string Prezime { get; set; }
        virtual internal protected required DateTime DatumAngazovanja { get; set; }
        virtual internal protected required string Telefon { get; set; }
        virtual internal protected required double ProcenatOdNajma { get; set; }

        //veza
        //virtual public required Agent AgentAngazovanja { get; set; } // ova veza na klasu je prebacena u klasu Id
        virtual internal protected IList<Najam> RealizovaniNajmovi { get; set; } = [];
    }
}
