namespace Drugly.Server.Models;

public class AccountDatabaseEntry
{
    public AccountDatabaseEntry(string password, AccountDetails accountDetails)
    {
        Password = password;
        AccountDetails = accountDetails;
    }

    public string Password { get; set; }
    public AccountDetails AccountDetails { get; set; }
}