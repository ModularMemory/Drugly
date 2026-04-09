using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;

namespace Drugly.Server.Services;
//TODO: make this a hosted service
public class AccountDatabaseService : IHostedService, IAccountDatabaseService
{
    public Task<AccountDetails> GetAccountById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetAccountById(Guid id, AccountDetails details)
    {
        throw new NotImplementedException();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}