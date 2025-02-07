namespace TekniskOpgave.DAL.Models
{
    public class Overmontor
    {
        public int OvermontorId { get; set; }

        public string OvermontorNavn { get; set; }

        public string OvermontorTelefonnummer { get; set; }

        public List<Montor> Montorer { get; set; } = new List<Montor>();
    }
}
