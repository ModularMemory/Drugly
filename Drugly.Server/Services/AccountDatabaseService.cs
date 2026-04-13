using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;

namespace Drugly.Server.Services;

public class AccountDatabaseService : IHostedService, IAccountDatabaseService
{
    public Task<AccountDetails> GetAccountById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task SetAccountById(Guid id, AccountDetails detailsDto)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> GetIdByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<List<AccountDetails>> GetAllPatientAccounts()
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