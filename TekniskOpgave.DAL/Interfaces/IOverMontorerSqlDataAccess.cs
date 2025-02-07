using TekniskOpgave.DAL.Models;

namespace TekniskOpgave.DAL.Interfaces
{
    public interface IOvermontorerSqlDataAccess
    {
        IEnumerable<Overmontor> GetAllOvermontors();

        Overmontor GetOvermontorById(int overmontorId);

        void AddMontorToOvermontor(int montorId, int overmontorId);

        void RemoveMontorFromOvermontor(int montorId, int overmontorId);

        void CreateOvermontor(Overmontor overmontor);

        void DeleteOvermontor(int overmontorId);

        void UpdateOvermontor(Overmontor overmontor);
    }
}
