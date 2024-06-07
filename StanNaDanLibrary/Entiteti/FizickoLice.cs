namespace StanNaDanLibrary.Entiteti
{
    public class FizickoLice
    {
        public virtual required string JMBG { get; set; }
        public virtual required string Ime { get; set; }
        public virtual required string ImeRoditelja { get; set; }
        public virtual required string Prezime { get; set; }
        public virtual required string Drzava { get; set; }
        public virtual required string MestoStanovanja { get; set; }
        public virtual required string AdresaStanovanja { get; set; }
        public virtual required DateTime DatumRodjenja { get; set; }
        public virtual required string Email { get; set; }

        //veza za vlasnik klasu obrati paznju da l ce tako da ostane
        public virtual required Vlasnik Vlasnik { get; set; }
        //treba da bude vezana i za telefone fizickog lica i te klase bi trebale da imaju svoje surogat kljuceve za lakse prevodjenje
        public virtual IList<BrojeviTelefona> BrojeviTelefona { get; set; } = [];
    }
}
