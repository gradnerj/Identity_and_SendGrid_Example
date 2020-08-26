using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendGridExample.Services.Models {
   public class AuthSenderOptions {
        private string user = "JaneDoe"; // The name you want to show up on your email
        private string key = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");

        public string SendGridUser { get { return user; } }

        
        public string SendGridKey { get { return key; } }
    }
}
