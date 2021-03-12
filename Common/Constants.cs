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
        public const int CODE_ERROR_001 = 1;
        public const int CODE_ERROR_002 = 2;
        public const int CODE_ERROR_003 = 3;
        public const int CODE_ERROR_004 = 4;
        public const int CODE_ERROR_005 = 5;
        public const int CODE_ERROR_006 = 6;
        public const int CODE_ERROR_007 = 7;
        public const string MESSAGE_ERROR_001 = "La ruleta ingresada no existe";
        public const string MESSAGE_ERROR_002 = "La ruleta se encuentra aperturada";
        public const string MESSAGE_ERROR_003 = "La ruleta se encuentra cerrada";
        public const string MESSAGE_ERROR_004 = "El monto ingresado no esta permitido en la apuesta";
        public const string MESSAGE_ERROR_005 = "Error en el modelo enviado";
        public const string MESSAGE_ERROR_006 = "Debe ingresar una ruleta";
        public const string MESSAGE_ERROR_007 = "El servicio ejecutado presento problemas, volver a intentar";
        public const int MIN_AMOUNT_BET = 0;
        public const int MAX_AMOUNT_BET = 10000;
        public const int MIN_NUMBER_BET = 0;
        public const int MAX_NUMBER_BET = 38;
    }
}
