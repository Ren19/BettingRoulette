using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Common.GenericClass
{
    public class RegisterModel
    {
        public string LogGroupName { get; set; }
        public string LogStreamName { get; set; }
        public string ErrorMessage { get; set; }
        public string Exception { get; set; }
    }
}
