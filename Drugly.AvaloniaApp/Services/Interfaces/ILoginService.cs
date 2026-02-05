using Drugly.DTO;

namespace Drugly.AvaloniaApp.Services.Interfaces;

public interface ILoginService
{
    event EventHandler<AccountType>? SuccessfulLogin;
}