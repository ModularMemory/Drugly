using Drugly.Server.Services;
using Drugly.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace Drugly.Server;

public static class Startup
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection ConfigureServices()
        {
            serviceCollection
                .AddSingleton<IAccountDatabaseService, AccountDatabaseService>()
                .AddSingleton<IPrescriptionDatabaseService, PrescriptionDatabaseService>()
                .AddSingleton<IMedicationDatabaseService, MedicationDatabaseService>()
                .AddSingleton<IImageDatabaseService, ImageDatabaseService>()
                .AddSingleton<IAuthenticationService, AuthenticationService>()
                .AddSingleton<IStateMachineFactoryService, StateMachineFactoryService>();

            return serviceCollection;
        }
    }
}