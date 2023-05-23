using Dashboard.Domain.Models;
using Dashboard.Domain.Seedwork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DashboardContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public ClientRepository(DashboardContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Client> Add(Client client) => _context.Clients.Add(client).Entity;

        public void Modify(Client clientRecord, Client modifiedClient)
        {
            clientRecord.Name = modifiedClient.Name;
            clientRecord.Description = modifiedClient.Description;
            clientRecord.AccountManager = modifiedClient.AccountManager;
            clientRecord.ClientLogo = modifiedClient.ClientLogo;

            _context.Entry(clientRecord).State = EntityState.Modified;
        }

        public async Task<Client> GetMismatchIdClientByName(int client_id, string name, CancellationToken cancellationToken)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.Id != client_id && c.Name.Equals(name));
        }

        public async Task<Client> GetClientByName(string name, CancellationToken cancellationToken)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<Client> GetAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Clients.Include("Projects.Vacancies").Where(c => c.Id == id).FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException();
        }

        public async Task<List<Client>> GetAllClients()
        {
            return await _context.Clients.Include("Projects.Vacancies").ToListAsync();
        }
    }
}
