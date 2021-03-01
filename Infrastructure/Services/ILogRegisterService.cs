using BettingRoulette.Model.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Infrastructure.Services
{
    public interface ILogRegisterService
    {
        Task<bool> RegisterLog(RegisterModel ex);
    }
}
