namespace Drugly.Server.Models;

/// <summary>An exception class for when an account is not hound in the database</summary>
public class AccountNotFoundException : Exception
{
    public string AccountMessage;

    public AccountNotFoundException()
    {
        AccountMessage = "Account not found";
    }

    public AccountNotFoundException(string message)
    {
        AccountMessage = message;
    }
}