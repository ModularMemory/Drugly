// ReSharper disable RedundantTypeArgumentsOfMethod

using Drugly.Server.Extensions;
using Drugly.Server.Services;
using Drugly.Server.Services.Interfaces;

namespace Drugly.Server;

public static class Startup
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection ConfigureServices()
        {
            serviceCollection
                .AddHostedService<IAccountDatabaseService, AccountDatabaseService>()
                .AddHostedService<IPrescriptionDatabaseService, PrescriptionDatabaseService>()
                .AddHostedService<IMedicationDatabaseService, MedicationDatabaseService>()
                .AddSingleton<IImageDatabaseService, ImageDatabaseService>()
                .AddSingleton<IAuthorizationService, AuthorizationService>()
                .AddSingleton<IStateMachineFactoryService, StateMachineFactoryService>()
                .AddSingleton<TimeProvider>(TimeProvider.System);

            return serviceCollection;
        }
    }
}