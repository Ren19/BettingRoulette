using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Common.GenericClass
{
    public class SecretSettings
    {
        public string AccessKey { get; set; }
        public string SecretAccessKey { get; set; }
        public string RegionEndpoint { get; set; }
        public string LogGroupName { get; set; }
    }
}
