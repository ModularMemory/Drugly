using System.Net.Http.Headers;
using Drugly.DTO;
using Drugly.Server.Models;
using AccountType = Drugly.DTO.AccountType;

namespace Drugly.Server.Services.Interfaces;

public interface IAuthorizationService
{
    AccountSession CreateSession(AccountDetails accountDetails);
    bool IsUserAuthorized(IHeaderDictionary headers, AccountType allowedType);
    bool IsUserAuthorized(IHeaderDictionary headers, ReadOnlySpan<AccountType> allowedTypes);
}