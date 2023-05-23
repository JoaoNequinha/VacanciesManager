using Dashboard.Domain.Models;
using Dashboard.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace Dashboard.Domain.Logic
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Client> GetClientAsync(int id, CancellationToken cancellationToken)
        {
            Client? client = await _clientRepository.GetAsync(id, cancellationToken);

            int roleSum = 0;

            foreach (Project proj in client.Projects)
            {
                roleSum += proj.Vacancies.Sum(v => v.Vacancy_count);
            }
            client.ProjectCount = client.Projects.Count();
            client.VacancyCount = roleSum;

            return client;
        }

        public async Task<List<Client>> GetClients()
        {
            List<Client> allClients = await _clientRepository.GetAllClients();

            foreach (Client client in allClients)
            {
                int roleSum = 0;
                foreach (Project proj in client.Projects)
                {
                    foreach (Vacancy v in proj.Vacancies)
                    {
                        roleSum += v.Vacancy_count;
                    }
                }

                client.ProjectCount = client.Projects.Count();
                client.VacancyCount = roleSum;
            }

            return allClients;
        }

        public async Task<Client> AddClient(Client client)
        {
            Client? existingClient = await _clientRepository.GetClientByName(client.Name, CancellationToken.None);
            
            if(existingClient != null)
                throw new ArgumentException(); //TODO change exception type customized

            await _clientRepository.Add(client);
            await _clientRepository.UnitOfWork.SaveChangesAsync();
            return client;
        }

        public async Task Modifyclient(int client_id, Client modifiedClient)
        {
            Client? existingClient = await _clientRepository.GetMismatchIdClientByName(client_id, modifiedClient.Name, CancellationToken.None);
           
            if (existingClient != null)
                throw new ArgumentException(); //TODO change exception type customized

            Client? clientRecord = await _clientRepository.GetAsync(client_id, CancellationToken.None);
            _clientRepository.Modify(clientRecord, modifiedClient);
            await _clientRepository.UnitOfWork.SaveChangesAsync();
        }
    }
}
