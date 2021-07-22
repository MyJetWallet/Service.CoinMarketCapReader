using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service;
using MyNoSqlServer.DataReader;
using Service.CoinMarketCapReader.Jobs;

namespace Service.CoinMarketCapReader
{
    public class ApplicationLifetimeManager : ApplicationLifetimeManagerBase
    {
        private readonly ILogger<ApplicationLifetimeManager> _logger;
        private readonly CMCUpdateJob _cmcUpdateJob;
        private readonly MyNoSqlTcpClient[] _myNoSqlTcpClientManagers;

        public ApplicationLifetimeManager(IHostApplicationLifetime appLifetime, ILogger<ApplicationLifetimeManager> logger, CMCUpdateJob cmcUpdateJob, MyNoSqlTcpClient[] myNoSqlTcpClientManagers)
            : base(appLifetime)
        {
            _logger = logger;
            _cmcUpdateJob = cmcUpdateJob;
            _myNoSqlTcpClientManagers = myNoSqlTcpClientManagers;
        }

        protected override void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");
            foreach(var client in _myNoSqlTcpClientManagers)
            {
                client.Start();
            }
            _cmcUpdateJob.Start();

        }

        protected override void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");
            _cmcUpdateJob.Dispose();
            foreach(var client in _myNoSqlTcpClientManagers)
            {
                try
                {
                    client.Stop();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        protected override void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");
        }
    }
}
