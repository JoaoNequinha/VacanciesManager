using Dashboard.Domain.Models;

namespace Dashboard.Domain.Logic
{
    public interface IClientService
    {
        Task<Client> GetClientAsync(int id, CancellationToken cancellationToken);
        Task<List<Client>> GetClients();
        Task<Client> AddClient(Client dto);
        Task Modifyclient(int client_id, Client dto);
    }
}