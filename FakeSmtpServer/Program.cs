using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using Autofac.Extras.NLog;
using Common.Logging;
using NLog.Internal;
using Topshelf;
using Topshelf.Autofac;
using NLog.Extensions.Logging;
namespace FakeSmtpServer
{
    class Program
    {
        private static ILog _log;//= LogManager.GetLogger<Program>();
        private static IContainer _container;
        static Microsoft.Extensions.Logging.ILoggerFactory _factory = new Microsoft.Extensions.Logging.LoggerFactory();
        static void Main(string[] args)
        {
            _factory.AddNLog();

            _container = ConfigureContainer(new ContainerBuilder())
                .Build();

            var builder = new ContainerBuilder();
            builder.RegisterInstance(_container);
            builder.Update(_container);

            _log = LogManager.GetLogger<Program>();

            try
            {
                HostFactory.Run(x =>
                {
                    x.SetDisplayName("FakeSmtpServer");
                    x.SetInstanceName("FakeSmtpServer");
                    x.SetServiceName("FakeSmtpServer");

                    x.RunAsNetworkService();

                    x.UseAutofacContainer(_container);
                    x.Service<FakeSmtpService>(s =>
                    {
                        s.ConstructUsingAutofacContainer();
                        s.WhenStarted(tc => tc.Start());
                        s.WhenStopped(tc => tc.Stop());
                    });
                    x.RunAsNetworkService();
                    x.EnableShutdown();
                });

                #region Log
                _log.Info("Normal shutdown");
                #endregion
            }
            catch (Exception ex)
            {
                _log.Fatal(ex);
            }
        }

        internal static ContainerBuilder ConfigureContainer(ContainerBuilder cb)
        {
            RegisterComponents(cb);
            return cb;
        }

        internal static void RegisterComponents(ContainerBuilder builder)
        {
            builder.RegisterModule<LoggingConfigModule>();
            builder.RegisterModule<NLogModule>();

            builder.RegisterInstance(new SmtpServerSettings()
            {
                IsEnabled = true,
                Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SmtpPort"])
            });

            builder.RegisterType<FakeSmtpService>()
                .AsSelf();
        }
    }
}
