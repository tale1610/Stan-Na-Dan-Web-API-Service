namespace StanNaDanLibrary.Entiteti
{
    internal class FizickoLice
    {
        internal protected virtual required string JMBG { get; set; }
        internal protected virtual required string Ime { get; set; }
        internal protected virtual required string ImeRoditelja { get; set; }
        internal protected virtual required string Prezime { get; set; }
        internal protected virtual required string Drzava { get; set; }
        internal protected virtual required string MestoStanovanja { get; set; }
        internal protected virtual required string AdresaStanovanja { get; set; }
        internal protected virtual required DateTime DatumRodjenja { get; set; }
        internal protected virtual required string Email { get; set; }

        //veza za vlasnik klasu obrati paznju da l ce tako da ostane
        internal protected virtual required Vlasnik Vlasnik { get; set; }
        //treba da bude vezana i za telefone fizickog lica i te klase bi trebale da imaju svoje surogat kljuceve za lakse prevodjenje
        internal protected virtual IList<BrojeviTelefona> BrojeviTelefona { get; set; } = [];
    }
}
