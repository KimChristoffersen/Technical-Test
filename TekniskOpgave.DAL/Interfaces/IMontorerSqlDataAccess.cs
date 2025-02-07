using TekniskOpgave.DAL.Models;

namespace TekniskOpgave.DAL.Interfaces
{
    public interface IMontorerSqlDataAccess
    {
        IEnumerable<Montor> GetAllMontors();

        Montor GetMontorById(int montorId);

        void AddMontorToOvermontor(int montorId, int overmontorId);

        void RemoveMontorFromOvermontor(int montorId, int overmontorId);

        void CreateMontor(Montor montor, int? overmontorId);

        void UpdateMontor(Montor montor);

        void DeleteMontor(int montorId);
    }
}
