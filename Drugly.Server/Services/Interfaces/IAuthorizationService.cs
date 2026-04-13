using Drugly.DTO;

namespace Drugly.Server.Services.Interfaces;

public interface IAuthorizationService
{
    string Authorize();
    bool IsUserAuthorized(HttpRequest request, AccountType accountType);
}