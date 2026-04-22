namespace Drugly.Server.Extensions;

/// <summary>Provides extensions for <see cref="IServiceCollection"/>s.</summary>
public static class ServiceCollectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection AddHostedService<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService, IHostedService
        {
            return serviceCollection
                .AddSingleton<TService, TImplementation>()
                .AddHostedService<TImplementation>(provider => (TImplementation)provider.GetRequiredService<TService>());
        }
    }
}