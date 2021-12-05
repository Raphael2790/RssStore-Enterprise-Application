using RssSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssSE.Client.API.Models.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        void Add(Customer client);
        void AddAddress(Address address);
        Task<IEnumerable<Customer>> GetClients();
        Task<Customer> GetByCpf(string cpf);
        Task<bool> ClientExists(string cpf);
        Task<Address> GetAddresByCustomerId(Guid guid);
    }
}
