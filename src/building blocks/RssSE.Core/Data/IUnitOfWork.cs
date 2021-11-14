using System.Threading.Tasks;

namespace RssSE.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
