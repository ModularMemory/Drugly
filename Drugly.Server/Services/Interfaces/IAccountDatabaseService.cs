using Drugly.Server.Models;

namespace Drugly.Server.Services.Interfaces;

public interface IAccountDatabaseService {
    /// <summary>Gets account details from an ID</summary>
    /// <param name="id">The id you want to find</param>
    /// <returns>details of the account associated with the id</returns>
    /// <exception cref="AccountNotFoundException">Thrown when an account is not found</exception>
    /// <exception cref="IOException">Thrown when there's an error</exception>
    Task<AccountDetails> GetAccountById(Guid id);

    /// <summary>Sets the account details for a given ID</summary>
    /// <param name="id">The ID you want to save the details to</param>
    /// <param name="detailsDto">The AccountDetails object being saved to the ID</param>
    /// <returns>returns A task that can be awaited</returns>
    /// <exception cref="IOException">Thrown on error</exception>
    Task SetAccountById(Guid id, AccountDetails detailsDto);

    /// <summary>Gets the ID of an account by searching for the associated email</summary>
    /// <param name="email">the email being searched for</param>
    /// <returns>The account ID</returns>
    /// <exception cref="AccountNotFoundException">Thrown when the email cannot be found</exception>
    /// <exception cref="IOException">Thrown when there's an error</exception>
    Task<Guid> GetIdByEmail(string email);

    /// <summary>Gets a list of all the patient accounts</summary>
    /// <returns>the list of patient accounts</returns>
    /// <exception cref="AccountNotFoundException">Thrown when no accounts are found</exception>
    /// <exception cref="IOException">Thrown when there's an error</exception>
    Task<List<AccountDetails>> GetAllPatientAccounts();
}