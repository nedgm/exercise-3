using System.Threading;
using System.Threading.Tasks;
using FixMessagesApi.DataLayer.Managers;
using FixMessagesApi.Helpers;
using Microsoft.Extensions.Hosting;

namespace FixMessagesApi
{
    public class StartupService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            DbInitializer.Initialize();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}