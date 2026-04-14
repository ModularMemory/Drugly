using System.Net.Http.Headers;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
using Serilog;

namespace Drugly.AvaloniaApp.Services;

/// <inheritdoc />
public sealed class AccountSessionService : IAccountSessionService
{
    private readonly ILogger _logger;

    private AuthenticationHeaderValue? _authHeader;

    private AccountSession? Session
    {
        get;
        set
        {
            field = value;
            _authHeader = value is null
                ? null
                : new AuthenticationHeaderValue("Bearer", value.SessionToken);
        }
    }

    public AccountType AccountType => Session?.AccountType ?? AccountType.Unknown;

    public AccountSessionService(
        ILogger logger
    )
    {
        _logger = logger;
    }

    public void StoreSession(AccountSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        Session = session;
    }

    public bool TryAuthorizeClient(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        if (_authHeader is null)
        {
            _logger.Error("Failed to authorize HttpClient: {AuthHeaderName} was null", _authHeader);
            return false;
        }

        httpClient.DefaultRequestHeaders.Authorization = _authHeader;
        return true;
    }
}