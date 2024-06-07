namespace StanNaDanLibrary.Entiteti
{
    public class PravnoLice
    {
        public virtual required string PIB { get; set; }
        public virtual required string Naziv { get; set; }
        public virtual required string AdresaSedista { get; set; }
        public virtual required string ImeKontaktOsobe { get; set; }
        public virtual required string EmailKontaktOsobe { get; set; }
        //veze
        public virtual required Vlasnik Vlasnik { get; set; }
        public virtual IList<TelefoniKontaktOsobe> TelefoniKontaktOsobe { get; set; } = [];
    }
}
