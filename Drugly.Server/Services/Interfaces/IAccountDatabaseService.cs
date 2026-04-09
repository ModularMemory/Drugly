using Drugly.Server.Models;

namespace Drugly.Server.Services.Interfaces;

public interface IAccountDatabaseService {
    Task<AccountDetails> GetAccountById(Guid id);
    Task<bool> SetAccountById(Guid id, AccountDetails details);
}