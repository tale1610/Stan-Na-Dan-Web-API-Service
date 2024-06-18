namespace StanNaDanLibrary.Entiteti
{
    internal class Vlasnik
    {
        internal protected virtual int IdVlasnika { get; protected set; }
        internal protected virtual required string Banka { get; set; }
        internal protected virtual required string BrojBankovnogRacuna { get; set; }
        internal protected virtual IList<Nekretnina>? Nekretnine { get; set; } = [];
        internal protected virtual FizickoLice? FizickoLice { get; set; }
        internal protected virtual PravnoLice? PravnoLice { get; set; }


        //ODRADI POSLE FIZICKO I PRAVNO LICE NEKAKO
    }
}
