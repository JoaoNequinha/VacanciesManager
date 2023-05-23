using Dashboard.Domain.Models;
using Dashboard.Domain.Seedwork;

namespace Dashboard.Infrastructure.Repositories
{
    public interface IClientRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task<Client> GetAsync(int id, CancellationToken cancellationToken);
        Task<List<Client>> GetAllClients();

        Task<Client> Add(Client client);
        void Modify(Client clientRecord, Client modifiedClient);

        Task<Client> GetMismatchIdClientByName(int client_id, string name, CancellationToken cancellationToken);
        Task<Client> GetClientByName(string name, CancellationToken cancellationToken);
    }
}