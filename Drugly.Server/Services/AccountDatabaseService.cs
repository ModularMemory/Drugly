using Drugly.DTO;
using Drugly.Server.Data;
using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;

namespace Drugly.Server.Services;

public class AccountDatabaseService : IHostedService, IAccountDatabaseService
{
    private readonly Dictionary<Guid, AccountCredentials> _accounts = new();
    private readonly Dictionary<string, Guid> _emailToId = new(StringComparer.OrdinalIgnoreCase);

    private readonly string _folderPath = "Accounts";

    /// <summary>
    /// grabs account from file by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="AccountNotFoundException"></exception>
    public  Task<AccountCredentials> GetAccountById(Guid id)
    {
        if (!_accounts.TryGetValue(id, out var account))
        {
            throw new AccountNotFoundException();
        }

        return Task.FromResult(account);
    }

    /// <summary>
    /// sets account by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="email"></param>
    /// <param name="entry"></param>
    /// <returns></returns>
    public async Task SetAccountById(Guid id, string email, AccountCredentials entry)
    {
        _accounts[id] = entry;
        _emailToId[email] = id;

        var filePath = Path.Combine(_folderPath, $"{id}.json");
        JsonWriteAccountDatabaseEntry.SaveAccount(entry, filePath);

        await JsonWriteAccountDatabaseEntry.SaveAccount(entry, filePath);
    }

    /// <summary>
    /// get email id by searching email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    /// <exception cref="AccountNotFoundException"></exception>
    public Task<Guid> GetIdByEmail(string email)
    {
        if (!_emailToId.TryGetValue(email, out var id))
        {
            throw new AccountNotFoundException("Email not found");
        }

        return Task.FromResult(id);

    }

    /// <summary>
    /// gets all patient accounts
    /// </summary>
    /// <returns></returns>
    /// <exception cref="AccountNotFoundException"></exception>
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

    /// <summary>
    /// begins async
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task StartAsync(CancellationToken cancellationToken)
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
    }

    /// <summary>
    /// stops async
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Account service stopping.");
        return Task.CompletedTask;
    }
}