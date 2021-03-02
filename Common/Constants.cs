using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Common
{
    public class Constants
    {
        public const string REDIS_KEY_ROULETTE = "REDIS_KEY_ROULETTE";

        public const double NUMBER_BET_PAYAOUT = 5;
        public const double COLOR_BET_PAYAOUT = 1.8;

        public const string CODE_ERROR_001 = "001";
        public const string MESSAGE_ERROR_001 = "La ruleta ingresada no existe";
    }
}
