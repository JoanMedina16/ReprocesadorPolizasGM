using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365API.INFOD
{
    public class LOGI_Session_D365
    {
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public int ext_expires_in { get; set; }
        public double expires_on { get; set; }
        public double not_before { get; set; }
        public string access_token { get; set; }
    }
}
