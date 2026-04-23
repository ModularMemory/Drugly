using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using Drugly.DTO;
using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;
using AccountType = Drugly.DTO.AccountType;

namespace Drugly.Server.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly RandomNumberGenerator _randomNumberGenerator = RandomNumberGenerator.Create();
    private ConcurrentDictionary<string, AccountSession> Authorizations { get; } = [];
    private readonly TimeProvider _timeProvider;

    public AuthorizationService(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public AccountSession CreateSession(AccountDetails accountDetails)
    {
        Span<byte> tokenBytes = stackalloc byte[16];
        _randomNumberGenerator.GetBytes(tokenBytes);
        string token = Convert.ToBase64String(tokenBytes);
        var expiration = _timeProvider.GetUtcNow().AddHours(1);

        AccountSession session = new AccountSession(token, accountDetails.AccountType, expiration, accountDetails.UserId);
        Authorizations.AddOrUpdate(token, _ => session, (_, __) => session);

        return session;
    }

    public bool DeleteSession(IHeaderDictionary headers)
    {
        if (!headers.TryGetValue("Authorization", out var values))
        {
            return false;
        }
        var token = values.FirstOrDefault();

        if (token is null)
        {
            return false;
        }

        const string BEARER = "Bearer ";
        if (token.StartsWith(BEARER))
        {
            token = token[BEARER.Length..];
        }

        return Authorizations.TryRemove(token, out _);
    }

    public bool IsUserAuthorized(IHeaderDictionary headers, AccountType allowedType)
    {
        return IsUserAuthorized(headers, [allowedType]);
    }

    public bool IsUserAuthorized(IHeaderDictionary headers, ReadOnlySpan<AccountType> allowedTypes)
    {
        if (!headers.TryGetValue("Authorization", out var values))
        {
            return false;
        }
        var token = values.FirstOrDefault();

        if (token is null)
        {
            return false;
        }

        const string BEARER = "Bearer ";
        if (token.StartsWith(BEARER))
        {
            token = token[BEARER.Length..];
        }
        
        if (!Authorizations.TryGetValue(token, out var accountSession))
        {
            return false;
        }

        if (accountSession.Expiration < _timeProvider.GetUtcNow())
        {
            Authorizations.TryRemove(token, out _);
            return false;
        }

        return allowedTypes.Contains(accountSession.AccountType);
    }
}