using Drugly.DTO;
using Drugly.Server.Data;
using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;

namespace Drugly.Server.Services;

public class AccountDatabaseService : IHostedService, IAccountDatabaseService
{
    private readonly ILogger<AccountDatabaseService> _logger;
    private readonly Dictionary<Guid, AccountCredentials> _accounts = new();
    private readonly Dictionary<string, Guid> _emailToId = new(StringComparer.OrdinalIgnoreCase);
    private const string FOLDER_PATH = "Accounts";

    public AccountDatabaseService(
        ILogger<AccountDatabaseService> logger
    )
    {
        _logger = logger;
    }

    public Task<AccountCredentials> GetAccountById(Guid id)
    {
        if (!_accounts.TryGetValue(id, out var account))
        {
            throw new AccountNotFoundException();
        }

        return Task.FromResult(account);
    }

    public async Task SetAccountById(Guid id, string email, AccountCredentials entry)
    {
        _accounts[id] = entry;
        _emailToId[email] = id;

        var filePath = Path.Combine(FOLDER_PATH, $"{id}.json");

        await JsonWriteAccountDatabaseEntry.SaveAccount(entry, filePath);
    }

    public Task<Guid> GetIdByEmail(string email)
    {
        if (!_emailToId.TryGetValue(email, out var id))
        {
            throw new AccountNotFoundException("Email not found");
        }

        return Task.FromResult(id);
    }

    public Task<AccountDetails[]> GetAllPatientAccounts()
    {
        var patients = _accounts.Values
            .Where(a => a.AccountDetails.AccountType == AccountType.Patient)
            .Select(a => a.AccountDetails)
            .ToArray();

        if (patients.Length == 0)
        {
            throw new AccountNotFoundException("Patient accounts not found");
        }

        return Task.FromResult(patients);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!Directory.Exists(FOLDER_PATH))
            Directory.CreateDirectory(FOLDER_PATH);

        var files = Directory.GetFiles(FOLDER_PATH, "*.json");

        foreach (var file in files)
        {
            var entry = await JsonReadAccountDatabaseEntry.LoadAccount(file);

            if (entry != null)
            {
                var id = entry.AccountDetails.UserId;
                var email = entry.AccountDetails.Email;

                _accounts[id] = entry;
                _emailToId[email] = id;
            }
        }

        _logger.LogInformation("Loaded {AccountsCount} accounts", _accounts.Count);
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}