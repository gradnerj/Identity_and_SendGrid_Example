using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendGridExample.Services.Models {
   public class AuthSenderOptions {
        private string user = "<YourUser>";
        private string key = "<YourKey>";
        //BitsRUsAPIKEY
        public string SendGridUser { get { return user; } }

        //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
        public string SendGridKey { get { return key; } }
    }
}
