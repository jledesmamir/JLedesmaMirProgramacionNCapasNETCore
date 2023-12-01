using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    public class Email
    {
        public string From;

        public string To;
        public string FromDisplayName { get; set; }

        public string Subject;

        public string Body { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }
    }
}
