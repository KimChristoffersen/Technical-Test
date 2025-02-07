namespace TekniskOpgave.DAL.Models
{
    public class Montor
    {
        public required int MontorId { get; set; }

        public required string MontorNavn { get; set; }

        public required string MontorTelefonnummer { get; set; }

        public List<Overmontor> Overmontorer { get; set; } = new List<Overmontor>();
    }
}
