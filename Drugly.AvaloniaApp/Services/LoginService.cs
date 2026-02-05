using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;

namespace Drugly.AvaloniaApp.Services;

public sealed class LoginService : ILoginService
{
    public event EventHandler<AccountType>? SuccessfulLogin;

    private void OnSuccessfulLogin(AccountType e)
    {
        SuccessfulLogin?.Invoke(this, e);
    }
}