namespace Drugly.Server.Models;

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