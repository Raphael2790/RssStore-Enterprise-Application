using RssSE.Core.Communication;
using RssSE.WebApp.MVC.Models;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<AddressViewModel> GetAddress();
        Task<ResponseResult> AddAddress(AddressViewModel address);
    }
}
