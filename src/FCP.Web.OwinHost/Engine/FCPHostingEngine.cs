using Microsoft.Owin.Hosting.Builder;
using Microsoft.Owin.Hosting.Engine;
using Microsoft.Owin.Hosting.Loader;
using Microsoft.Owin.Hosting.ServerFactory;
using Microsoft.Owin.Hosting.Tracing;
using Microsoft.Owin.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace FCP.Web.OwinHost
{
    internal class FCPHostingEngine : HostingEngine, IHostingEngine
    {
        private readonly IAppBuilderFactory _appBuilderFactory;

        public FCPHostingEngine(IAppBuilderFactory appBuilderFactory, ITraceOutputFactory traceOutputFactory,
            IAppLoader appLoader, IServerFactoryLoader serverFactoryLoader, ILoggerFactory loggerFactory)
            : base(appBuilderFactory, traceOutputFactory, appLoader, serverFactoryLoader, loggerFactory)
        {
            _appBuilderFactory = appBuilderFactory;
        }

        IDisposable IHostingEngine.Start(StartContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.Builder = context.Builder ?? _appBuilderFactory.Create();

            using (EnableStarting(context))
            {
                return Start(context);
            }
        }

        private static IDisposable EnableStarting(StartContext context)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            context.Builder.Properties[FCPOwinHostConstants.AppProperty_OnStarting] = cts.Token;
            context.EnvironmentData.Add(new KeyValuePair<string, object>(FCPOwinHostConstants.AppProperty_OnStarting, cts.Token));

            return new Disposable(() => cts.Cancel(false));
        }
    }
}
