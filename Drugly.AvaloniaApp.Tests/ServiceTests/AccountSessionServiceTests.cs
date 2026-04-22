using Drugly.AvaloniaApp.Services;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
using Serilog.Core;

namespace Drugly.AvaloniaApp.Tests.ServiceTests;

/// <summary>Tests for <see cref="AccountSessionService"/>.</summary>
public class AccountSessionServiceTests
{
    private static IAccountSessionService AccountSessionServiceFactory => new AccountSessionService(Logger.None);

    [Fact]
    public void Getters_ReturnEmpty_WhenSessionUnset()
    {
        // Arrange
        var service = AccountSessionServiceFactory;
        service.ClearSession();

        // Act
        var type = service.AccountType;
        var id = service.AccountId;

        // Assert
        Assert.Equal(AccountType.Unknown, type);
        Assert.Equal(Guid.Empty, id);
    }

    [Fact]
    public void TryAuthorizeClient_ReturnsFalse_AndDoesNotSetHeader_WhenSessionUnset()
    {
        // Arrange
        var service = AccountSessionServiceFactory;
        service.ClearSession();

        // Act
        var client = new HttpClient();
        var actual = service.TryAuthorizeClient(client);

        // Assert
        Assert.False(actual);
        Assert.Null(client.DefaultRequestHeaders.Authorization);
    }

    [Fact]
    public void TryAuthorizeClient_ReturnsTrue_AndSetsHeader_WhenSessionSet()
    {
        // Arrange
        var service = AccountSessionServiceFactory;
        var sessionToken = Path.GetRandomFileName();
        service.StoreSession(new AccountSession(sessionToken, AccountType.Doctor, default, Guid.Empty));

        // Act
        var client = new HttpClient();
        var actual = service.TryAuthorizeClient(client);

        // Assert
        Assert.True(actual);
        Assert.NotNull(client.DefaultRequestHeaders.Authorization);
        Assert.Equal(sessionToken, client.DefaultRequestHeaders.Authorization.Parameter);
    }

    [Fact]
    public void ClearSession_ClearsSession()
    {
        // Arrange
        var service = AccountSessionServiceFactory;
        var typeExpected = AccountType.Doctor;
        var idExpected = Guid.NewGuid();
        service.StoreSession(new AccountSession("", typeExpected, default, idExpected));

        // Act
        var typeBefore = service.AccountType;
        var idBefore = service.AccountId;
        service.ClearSession();
        var typeAfter = service.AccountType;
        var idAfter = service.AccountId;

        // Assert
        Assert.Equal(typeExpected, typeBefore);
        Assert.Equal(idExpected, idBefore);
        Assert.Equal(AccountType.Unknown, typeAfter);
        Assert.Equal(Guid.Empty, idAfter);
    }
}