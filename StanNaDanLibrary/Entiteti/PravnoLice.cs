namespace StanNaDanLibrary.Entiteti
{
    internal class PravnoLice
    {
        internal protected virtual required string PIB { get; set; }
        internal protected virtual required string Naziv { get; set; }
        internal protected virtual required string AdresaSedista { get; set; }
        internal protected virtual required string ImeKontaktOsobe { get; set; }
        internal protected virtual required string EmailKontaktOsobe { get; set; }
        //veze
        internal protected virtual required Vlasnik Vlasnik { get; set; }
        internal protected virtual IList<TelefoniKontaktOsobe> TelefoniKontaktOsobe { get; set; } = [];
    }
}
