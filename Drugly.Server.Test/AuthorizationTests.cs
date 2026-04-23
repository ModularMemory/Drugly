using Drugly.DTO;
using Drugly.Server.Services;
using Drugly.Server.Services.Interfaces;
using Drugly.Server.Test.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NSubstitute;

namespace Drugly.Server.Test;


public class AuthorizationTests
{
    private IAuthorizationService CreateAuthService(TimeProvider tp) => new AuthorizationService(tp);
    private IAuthorizationService AuthorizationServiceFactory => CreateAuthService(TimeProvider.System);

    [Fact]
    public void TestCreateSession()
    {
        //Arrange
        IAuthorizationService authorizationService = AuthorizationServiceFactory;
        var patientId = Guid.NewGuid();
        AccountDetails details = new AccountDetails(patientId, AccountType.Patient, "John", "Patient", "John@patient.com");

        //Act
        var session = authorizationService.CreateSession(details);

        //Assert
        Assert.Equal(patientId, session.AccountId);
        Assert.Equal(AccountType.Patient, session.AccountType);
        Assert.NotNull(session.SessionToken);
    }


    [Fact]
    public void TestSuccessfulDeleteSession()
    {
        //arrange
        IAuthorizationService authorizationService = AuthorizationServiceFactory;
        AccountDetails details = new AccountDetails(Guid.NewGuid(), AccountType.Patient, "John", "Patient", "John@patient.com");
        AccountSession session = authorizationService.CreateSession(details);
        var mockHeader = Substitute.For<IHeaderDictionary>();
        var arg = new StringValues("Bearer " + session.SessionToken);
        mockHeader.TryGetValue("Authorization", out Arg.Any<StringValues>())
            .Returns(x=>
            {
                x[1] = arg;
                return true;
            });

        //act
        var result = authorizationService.DeleteSession(mockHeader);

        //assert
        Assert.True(result);
    }

    [Fact]
    public void TestSessionNotFoundDeleteSession()
    {
        //arrange
        IAuthorizationService authorizationService = AuthorizationServiceFactory;
        AccountDetails details = new AccountDetails(Guid.NewGuid(), AccountType.Patient, "John", "Patient", "John@patient.com");
        AccountSession session = authorizationService.CreateSession(details);
        var mockHeader = Substitute.For<IHeaderDictionary>();
        var arg = new StringValues("Bearer " + session.SessionToken);
        mockHeader.TryGetValue("Authorization", out Arg.Any<StringValues>())
            .Returns(x=>
            {
                x[1] = arg;
                return true;
            });

        //act
        authorizationService.DeleteSession(mockHeader);
        var result = authorizationService.DeleteSession(mockHeader);

        //assert
        Assert.False(result);
    }

    [Fact]
    public void TestBadHeaderDeleteSession()
    {
        //Arrange
        IAuthorizationService authorizationService = AuthorizationServiceFactory;
        AccountDetails details = new AccountDetails(Guid.NewGuid(), AccountType.Patient, "John", "Patient", "John@patient.com");
        AccountSession session = authorizationService.CreateSession(details);
        var mockHeader = Substitute.For<IHeaderDictionary>();
        var arg = new StringValues("Bearer " + session.SessionToken);
        mockHeader.TryGetValue("BadHeader", out Arg.Any<StringValues>())
            .Returns(x=>
            {
                x[1] = arg;
                return true;
            });

        //Act
        var result = authorizationService.DeleteSession(mockHeader);

        //assert
        Assert.False(result);

    }

    [Fact]
    public void TestNullTokenDeleteSession()
    {
        //arrange
        IAuthorizationService authorizationService = AuthorizationServiceFactory;
        var mockHeader = Substitute.For<IHeaderDictionary>();
        var arg = new StringValues("Bearer ");
        mockHeader.TryGetValue("Authorization", out Arg.Any<StringValues>())
            .Returns(x=>
            {
                x[1] = arg;
                return true;
            });

        //act
        var result = authorizationService.DeleteSession(mockHeader);

        //assert
        Assert.False(result);
    }

    [Fact]
    public void TestTokenNotFoundDeleteSession()
    {
        //arrange
        IAuthorizationService authorizationService = AuthorizationServiceFactory;
        AccountDetails details = new AccountDetails(Guid.NewGuid(), AccountType.Patient, "John", "Patient", "John@patient.com");
        AccountSession session = authorizationService.CreateSession(details);
        var mockHeader = Substitute.For<IHeaderDictionary>();
        var arg = new StringValues("Bearer " + session.SessionToken);
        mockHeader.TryGetValue("Authorization", out Arg.Any<StringValues>())
            .Returns(x=>
            {
                x[1] = arg;
                return true;
            });

        //act
        var deleted = authorizationService.DeleteSession(mockHeader);
        var result = authorizationService.DeleteSession(mockHeader);

        //assert
        Assert.True(deleted);
        Assert.False(result);
    }

    [Fact]
    public void TestSuccessfulUserAuthorizedPatient()
    {
        //Arrange
        IAuthorizationService authorizationService = AuthorizationServiceFactory;
        AccountDetails details = new AccountDetails(Guid.NewGuid(), AccountType.Patient, "John", "Patient", "John@patient.com");
        AccountSession session = authorizationService.CreateSession(details);
        var mockHeader = Substitute.For<IHeaderDictionary>();
        var arg = new StringValues("Bearer " + session.SessionToken);
        mockHeader.TryGetValue("Authorization", out Arg.Any<StringValues>())
            .Returns(x=>
            {
                x[1] = arg;
                return true;
            });

        //Act
        bool result = authorizationService.IsUserAuthorized(mockHeader, AccountType.Patient);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void TestSuccessfulUserAuthorizedDoctor()
    {
        //Arrange
        IAuthorizationService authorizationService = AuthorizationServiceFactory;
        AccountDetails details = new AccountDetails(Guid.NewGuid(), AccountType.Doctor, "John", "Patient", "John@patient.com");
        AccountSession session = authorizationService.CreateSession(details);
        var mockHeader = Substitute.For<IHeaderDictionary>();
        var arg = new StringValues("Bearer " + session.SessionToken);
        mockHeader.TryGetValue("Authorization", out Arg.Any<StringValues>())
            .Returns(x=>
            {
                x[1] = arg;
                return true;
            });

        //Act
        bool result = authorizationService.IsUserAuthorized(mockHeader, AccountType.Doctor);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void TestSuccessfulUserAuthorizedBoth()
    {
        //Arrange
        IAuthorizationService authorizationService = AuthorizationServiceFactory;
        AccountDetails details = new AccountDetails(Guid.NewGuid(), AccountType.Patient, "John", "Patient", "John@patient.com");
        AccountSession session = authorizationService.CreateSession(details);
        AccountDetails detailsDoctor = new AccountDetails(Guid.NewGuid(), AccountType.Doctor, "John", "Doctor", "John@doctor.com");
        AccountSession sessionDoctor = authorizationService.CreateSession(detailsDoctor);
        var mockHeader = Substitute.For<IHeaderDictionary>();
        var arg = new StringValues("Bearer " + session.SessionToken);
        mockHeader.TryGetValue("Authorization", out Arg.Any<StringValues>())
            .Returns(x=>
            {
                x[1] = arg;
                return true;
            });
        var mockHeaderDoctor = Substitute.For<IHeaderDictionary>();
        var argDoctor = new StringValues("Bearer " + sessionDoctor.SessionToken);
        mockHeaderDoctor.TryGetValue("Authorization", out Arg.Any<StringValues>())
            .Returns(x=>
            {
                x[1] = argDoctor;
                return true;
            });

        //Act
        bool result = authorizationService.IsUserAuthorized(mockHeader, [AccountType.Patient,  AccountType.Doctor]);
        bool result2 = authorizationService.IsUserAuthorized(mockHeaderDoctor, [AccountType.Patient,  AccountType.Doctor]);

        //Assert
        Assert.True(result);
        Assert.True(result2);
    }

    [Fact]
    public void TestBadHeaderUserAuthorized()
    {
        //Arrange
        IAuthorizationService authorizationService = AuthorizationServiceFactory;
        AccountDetails details = new AccountDetails(Guid.NewGuid(), AccountType.Doctor, "John", "Patient", "John@patient.com");
        AccountSession session = authorizationService.CreateSession(details);
        var mockHeader = Substitute.For<IHeaderDictionary>();
        var arg = new StringValues("Bearer " + session.SessionToken);
        mockHeader.TryGetValue("bad header", out Arg.Any<StringValues>())
            .Returns(x=>
            {
                x[1] = arg;
                return true;
            });

        //Act
        bool result = authorizationService.IsUserAuthorized(mockHeader, AccountType.Doctor);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void TestNullTokenUserAuthorized()
    {
        //Arrange
        IAuthorizationService authorizationService = AuthorizationServiceFactory;
        AccountDetails details = new AccountDetails(Guid.NewGuid(), AccountType.Doctor, "John", "Patient", "John@patient.com");
        var mockHeader = Substitute.For<IHeaderDictionary>();
        var arg = new StringValues("Bearer ");
        mockHeader.TryGetValue("Authorization", out Arg.Any<StringValues>())
            .Returns(x=>
            {
                x[1] = arg;
                return true;
            });

        //Act
        bool result = authorizationService.IsUserAuthorized(mockHeader, AccountType.Doctor);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void TestExpiredTokenUserAuthorized()
    {
        //Arrange
        TestTimeProvider tp = new TestTimeProvider();
        IAuthorizationService authorizationService = CreateAuthService(tp);
        AccountDetails details = new AccountDetails(Guid.NewGuid(), AccountType.Doctor, "John", "Patient", "John@patient.com");
        AccountSession session = authorizationService.CreateSession(details);
        var mockHeader = Substitute.For<IHeaderDictionary>();
        var arg = new StringValues("Bearer " + session.SessionToken);
        mockHeader.TryGetValue("Authorization", out Arg.Any<StringValues>())
            .Returns(x=>
            {
                x[1] = arg;
                return true;
            });

        //Act
        tp.AddHours(5);
        bool result = authorizationService.IsUserAuthorized(mockHeader, AccountType.Doctor);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void TestTokenNotFoundUserAuthorized()
    {
        //Arrange
        IAuthorizationService authorizationService = AuthorizationServiceFactory;
        AccountDetails details = new AccountDetails(Guid.NewGuid(), AccountType.Patient, "John", "Patient", "John@patient.com");
        AccountSession session = authorizationService.CreateSession(details);
        var mockHeader = Substitute.For<IHeaderDictionary>();
        var arg = new StringValues("Bearer " + session.SessionToken);
        mockHeader.TryGetValue("Authorization", out Arg.Any<StringValues>())
            .Returns(x=>
            {
                x[1] = arg;
                return true;
            });

        //Act
        authorizationService.DeleteSession(mockHeader);
        bool result = authorizationService.IsUserAuthorized(mockHeader, AccountType.Patient);

        //Assert
        Assert.False(result);
    }
}