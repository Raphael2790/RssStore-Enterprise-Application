using Microsoft.EntityFrameworkCore;
using RssSE.Client.API.Data.Context;
using RssSE.Client.API.Models.Interfaces;
using RssSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.Client.API.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ClientDbContext _context;

        public ClientRepository(ClientDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Models.Client client) => _context.Clients.Add(client);

        public async Task<Models.Client> GetByCpf(string cpf) =>
            await _context.Clients.FirstOrDefaultAsync(x => x.Cpf.Number == cpf);

        public async Task<IEnumerable<Models.Client>> GetClients() => 
            await _context.Clients.AsNoTracking().ToListAsync();

        public async Task<bool> ClientExists(string cpf) => await _context.Clients.AnyAsync(x => x.Cpf.Number == cpf);

        public void Dispose() => _context?.Dispose();

    }
}
