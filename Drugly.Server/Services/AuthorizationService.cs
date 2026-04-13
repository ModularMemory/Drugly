using Drugly.AvaloniaApp.Models;
using Drugly.DTO;
using Drugly.Server.Services.Interfaces;

namespace Drugly.Server.Services;

public class AuthorizationService : IAuthorizationService
{
    Dictionary<string, AccountSession> Authorizations { get; }

    public string Authorize()
    {
        throw new NotImplementedException();
    }

    public bool IsUserAuthorized(HttpRequest request, AccountType accountType)
    {
        throw new NotImplementedException();
    }
}