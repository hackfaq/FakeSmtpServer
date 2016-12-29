using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Common.Logging;
namespace FakeSmtpServer
{
    public class LoggingConfigModule : Module
    {
        protected override void AttachToComponentRegistration(IComponentRegistry registry,
            IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
        }

        private static void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            var type = e.Component.Activator.LimitType;

            e.Parameters = e.Parameters.Union(new[]
            {
                new ResolvedParameter((p, i) => p.ParameterType == typeof(ILog),
                    (p, i) =>  LogManager.GetLogger(type))
            });
        }
    }
}
