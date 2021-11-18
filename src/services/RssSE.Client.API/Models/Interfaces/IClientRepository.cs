using RssSE.Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssSE.Client.API.Models.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        void Add(Client client);
        Task<IEnumerable<Client>> GetClients();
        Task<Client> GetByCpf(string cpf);
        Task<bool> ClientExists(string cpf);
    }
}
