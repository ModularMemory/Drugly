using Drugly.DTO;
using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;

namespace Drugly.Server.Services;

public class AccountDatabaseService : IHostedService, IAccountDatabaseService
{
    private readonly Dictionary<Guid, AccountDatabaseEntry> _accounts = new();
    private readonly Dictionary<string, Guid> _emailToId = new();

    private readonly string _folderPath = "Accounts";
    public Task<AccountDatabaseEntry> GetAccountById(Guid id)
    {
        _accounts.TryGetValue(id, out var account);
        return Task.FromResult(account);
    }

    public Task SetAccountById(Guid id, string email, AccountDatabaseEntry entry)
    {
        _accounts[id] = entry;
        _emailToId[email] = id;

        var filePath = Path.Combine(_folderPath, $"{id}.json");
        JsonWriteAccountDatabaseEntry.SaveAccount(entry, filePath);

        return Task.CompletedTask;
    }

    public Task<Guid> GetIdByEmail(string email)
    {
        if (_emailToId.TryGetValue(email, out var id))
            return Task.FromResult(id);

        throw new KeyNotFoundException("Email not found");
    }

    public Task<List<AccountDetails>> GetAllPatientAccounts()
    {

        var patients = _accounts.Values
            .Where(a => a.AccountDetails.AccountType == AccountType.Patient)
            .Select(a => a.AccountDetails)
            .ToList();

        return Task.FromResult(patients);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (!Directory.Exists(_folderPath))
            Directory.CreateDirectory(_folderPath);

        var files = Directory.GetFiles(_folderPath, "*.json");

        foreach (var file in files)
        {
            var entry = JsonReadAccountDatabaseEntry.LoadAccount(file);

            if (entry != null)
            {
                var id = entry.AccountDetails.UserId;
                var email = entry.AccountDetails.Email;

                _accounts[id] = entry;
                _emailToId[email] = id;
            }
        }

        Console.WriteLine($"Loaded {_accounts.Count} accounts.");
        return Task.CompletedTask;
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Account service stopping.");
        return Task.CompletedTask;
    }
}