namespace StanNaDanLibrary.Entiteti
{
    internal class Nekretnina
    {
        virtual internal protected int IdNekretnine { get; protected set; }
        virtual internal protected required string Ulica { get; set; }
        virtual internal protected required string Broj { get; set; }
        virtual internal protected required double Kvadratura { get; set; }
        virtual internal protected required int BrojTerasa { get; set; }
        virtual internal protected required int BrojKupatila { get; set; }
        virtual internal protected required int BrojSpavacihSoba { get; set; }
        virtual internal protected required bool PosedujeTV { get; set; }
        virtual internal protected required bool PosedujeInternet { get; set; }
        virtual internal protected required bool PosedujeKuhinju { get; set; }

        //veze
        virtual internal protected required Kvart Kvart { get; set; }
        virtual internal protected required Vlasnik Vlasnik { get; set; }
        virtual internal protected IList<DodatnaOprema> DodatnaOprema { get; set; } = [];
        virtual internal protected IList<Parking> Parking { get; set; } = [];
        virtual internal protected IList<Krevet> Kreveti { get; set; } = [];
        virtual internal protected IList<Soba> Sobe {  get; set; } = [];
        virtual internal protected IList<SajtoviNekretnine> SajtoviNekretnine { get; set; } = [];
        virtual internal protected IList<Najam> Najmovi { get; set; } = [];
    }

    internal class Stan : Nekretnina
    {
        virtual internal protected int Sprat { get; set; }
        virtual internal protected bool PosedujeLift { get; set; }
    }

    internal class Kuca : Nekretnina
    {
        virtual internal protected int Spratnos { get; set; }
        virtual internal protected bool PosedujeDvoriste { get; set; }
    }
}

