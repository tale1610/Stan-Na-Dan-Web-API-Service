namespace StanNaDanLibrary.Entiteti
{
    public class TelefoniKontaktOsobe
    {
        public virtual required string BrojTelefona { get; set; }
        //veza
        public virtual required PravnoLice PravnoLice { get; set; }
    }
}
