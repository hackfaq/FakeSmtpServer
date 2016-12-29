using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Rnwood.SmtpServer;
using Rnwood.SmtpServer.Extensions;
using Rnwood.SmtpServer.Extensions.Auth;

namespace FakeSmtpServer
{
    internal class SmtpServerBehaviour : IServerBehaviour
    {
        internal SmtpServerBehaviour(SmtpServerSettings settings, Action<SmtpServerMessage> messageRecievedHandler)
        {
            _settings = settings;
            _messageReceivedHandler = messageRecievedHandler;
        }

        private Action<SmtpServerMessage> _messageReceivedHandler;
        private SmtpServerSettings _settings;

        public string DomainName
        {
            get
            {
                return "fakesmtp";
            }
        }

        public IPAddress IpAddress
        {
            get
            {
                return IPAddress.Any;
            }
        }

        public int MaximumNumberOfSequentialBadCommands
        {
            get
            {
                return 10;
            }
        }

        public int PortNumber
        {
            get
            {
                return _settings.Port;
            }
        }

        public Encoding GetDefaultEncoding(IConnection connection)
        {
            return new ASCIISevenBitTruncatingEncoding();
        }

        public IEnumerable<IExtension> GetExtensions(IConnection connection)
        {
            return Enumerable.Empty<IExtension>();
        }

        public long? GetMaximumMessageSize(IConnection connection)
        {
            return null;
        }

        public TimeSpan GetReceiveTimeout(IConnection connection)
        {
            return TimeSpan.FromMinutes(5);
        }

        public TimeSpan GetSendTimeout(IConnection connection)
        {
            return TimeSpan.FromMinutes(5);
        }

        public X509Certificate GetSSLCertificate(IConnection connection)
        {
            return null;
        }

        public bool IsAuthMechanismEnabled(IConnection connection, IAuthMechanism authMechanism)
        {
            return true;
        }

        public bool IsSessionLoggingEnabled(IConnection connection)
        {
            return true;
        }

        public bool IsSSLEnabled(IConnection connection)
        {
            return false;
        }

        public void OnCommandReceived(IConnection connection, SmtpCommand command)
        {
        }

        public IMessageBuilder OnCreateNewMessage(IConnection connection)
        {
            return new SmtpServerMessage.Builder();
        }

        public IEditableSession OnCreateNewSession(IConnection connection, IPAddress clientAddress, DateTime startDate)
        {
            return new MemorySession(clientAddress, startDate);
        }

        public void OnMessageCompleted(IConnection connection)
        {
        }

        public void OnMessageReceived(IConnection connection, IMessage message)
        {
            _messageReceivedHandler((SmtpServerMessage)message);
        }

        public void OnMessageRecipientAdding(IConnection connection, IMessageBuilder message, string recipient)
        {
        }

        public void OnMessageStart(IConnection connection, string from)
        {
        }

        public void OnSessionCompleted(IConnection connection, ISession Session)
        {
        }

        public void OnSessionStarted(IConnection connection, ISession session)
        {
        }

        public Task<AuthenticationResult> ValidateAuthenticationCredentialsAsync(IConnection connection, IAuthenticationCredentials authenticationRequest)
        {
            return Task.FromResult(AuthenticationResult.Success);
        }
    }
}
