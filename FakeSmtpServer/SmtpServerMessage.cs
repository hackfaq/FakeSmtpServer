using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rnwood.SmtpServer;
using MimeKit;

namespace FakeSmtpServer
{
    public class SmtpServerMessage : MemoryMessage
    {
        private SmtpServerMessage(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }

        public string Subject { get; private set; }

        public new class Builder : MemoryMessage.Builder
        {
            public Builder() : base(new SmtpServerMessage(Guid.NewGuid()))
            {
            }

            public override IMessage ToMessage()
            {
                SmtpServerMessage message = (SmtpServerMessage)base.ToMessage();

                try
                {
                    MimeMessage mimeMessage = MimeMessage.Load(message.GetData());
                    message.Subject = mimeMessage.Subject;
                }
                catch (FormatException)
                {
                }

                return message;
            }
        }
    }
}
