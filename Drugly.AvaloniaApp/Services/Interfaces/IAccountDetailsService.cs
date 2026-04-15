using System.ComponentModel.DataAnnotations;
using Drugly.DTO;

namespace Drugly.AvaloniaApp.Services.Interfaces;

/// <summary>Provides functionality related to <see cref="AccountDetails"/>.</summary>
public interface IAccountDetailsService
{
    /// <summary>Gets an account by a given id.</summary>
    /// <param name="id">The id of the account.</param>
    /// <returns>The found account.</returns>
    /// <exception cref="IOException">The server could not be reached.</exception>
    /// <exception cref="HttpRequestException">The request was denied.</exception>
    AccountDetails GetAccountById(Guid id);

    /// <summary>Gets an account by a given email.</summary>
    /// <param name="email">The email of the account.</param>
    /// <returns>The found account.</returns>
    /// <exception cref="IOException">The server could not be reached.</exception>
    /// <exception cref="HttpRequestException">The request was denied.</exception>
    AccountDetails GetAccountByEmail([EmailAddress] string email);
}