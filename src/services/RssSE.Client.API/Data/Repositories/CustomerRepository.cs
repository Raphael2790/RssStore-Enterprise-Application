using Microsoft.EntityFrameworkCore;
using RssSE.Client.API.Data.Context;
using RssSE.Client.API.Models;
using RssSE.Client.API.Models.Interfaces;
using RssSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssSE.Client.API.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;

        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Customer customer) => _context.Customers.Add(customer);

        public void AddAddress(Address address) => _context.Addresses.Add(address);

        public async Task<Models.Customer> GetByCpf(string cpf) =>
            await _context.Customers.FirstOrDefaultAsync(x => x.Cpf.Number == cpf);

        public async Task<IEnumerable<Models.Customer>> GetClients() => 
            await _context.Customers.AsNoTracking().ToListAsync();

        public async Task<bool> ClientExists(string cpf) => await _context.Customers.AnyAsync(x => x.Cpf.Number == cpf);

        public void Dispose() => _context?.Dispose();

        public async Task<Address> GetAddresByCustomerId(Guid customerId) => 
            await _context.Addresses.FirstOrDefaultAsync(x => x.CustomerId == customerId);
    }
}
