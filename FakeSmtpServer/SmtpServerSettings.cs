using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeSmtpServer
{
    public class SmtpServerSettings
    {
        public SmtpServerSettings()
        {
            Port = 25;
            IsEnabled = true;
        }

        public bool IsEnabled { get; set; }
        public int Port { get; set; }
    }
}
