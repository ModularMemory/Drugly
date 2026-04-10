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
                .AddSingleton<IAccountDatabaseService, AccountDatabaseService>()
                .AddSingleton<IPrescriptionDatabaseService, PrescriptionDatabaseService>()
                .AddSingleton<IMedicationDatabaseService, MedicationDatabaseService>()
                .AddSingleton<IImageDatabaseService, ImageDatabaseService>();
            return serviceCollection;
        }
    }
}