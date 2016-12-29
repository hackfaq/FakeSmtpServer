using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;
using Common.Logging;

using Rnwood.SmtpServer;

using MimeKit;

using StringTokenFormatter;

namespace FakeSmtpServer
{
    public class FakeSmtpService
    {
        private readonly ILog _log;
        private IContainer _container;

        private Rnwood.SmtpServer.Server _server;
        private Exception _serverError;

        public event EventHandler StateChanged;
        private SmtpServerSettings _settings;
        public FakeSmtpService(ILog log, IContainer container, SmtpServerSettings settings)
        {
            _log = log;
            _container = container;
            _settings = settings;
        }

        public void Start()
        {
            _log.Trace($"FakeSmtpService starting on port:{_settings.Port}");

            TryStart();

            _log.Trace("FakeSmtpService started");
        }
        public void TryStart()
        {
            try
            {
                _server = new Server(new SmtpServerBehaviour(_settings, OnMessageReceived));
                _server.IsRunningChanged += OnServerStateChanged;
                _server.Start();
            }
            catch (Exception e)
            {
                _log.Fatal(e);
            }

        }
        public void Stop()
        {
            if (_server != null)
            {
                try
                {
                    _log.Trace("FakeSmtpService stopping");

                    _server.Stop(true);

                    _log.Trace("FakeSmtpService stopped");
                }
                catch (Exception ex)
                {
                    _log.FatalFormat("FakeSmtpService error - {0}", ex);
                }
            }
        }

        private void OnServerStateChanged(object sender, EventArgs eventArgs)
        {
            _log.Trace($@"Server state changed");
        }
        private void OnMessageReceived(SmtpServerMessage message)
        {
            _log.Trace($@"Message received from:{message.From} to:{string.Join(",", message.To)}, subject:{message.Subject}");

            MimeMessage mimeMessage = MimeMessage.Load(message.GetData());

            var str = System.Configuration.ConfigurationManager.AppSettings["PathTemplate"];

            var tokenValues = new Dictionary<string, object>();

            tokenValues.Add("From", message.From);
            tokenValues.Add("TOs", string.Join(",", message.To));
            tokenValues.Add("ReceivedDate", message.ReceivedDate);
            tokenValues.Add("Subject", message.Subject);

            var path = str.FormatToken(tokenValues);

            var fileInfo = new System.IO.FileInfo(path);

            if (!fileInfo.Directory.Exists)
            {
                System.IO.Directory.CreateDirectory(fileInfo.Directory.ToString());
            }

            mimeMessage.WriteTo(path);
        }
    }
}
