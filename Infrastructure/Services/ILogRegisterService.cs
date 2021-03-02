using BettingRoulette.Common.GenericClass;
using System.Threading.Tasks;

namespace BettingRoulette.Infrastructure.Services
{
    public interface ILogRegisterService
    {
        Task<bool> RegisterLog(RegisterModel model);
    }
}
