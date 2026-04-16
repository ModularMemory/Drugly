namespace Drugly.DTO;

/// <summary>A container class that contains the details of an account as well as the account's credentials to be stored in the database</summary>
public class AccountCredentials
{
    public AccountCredentials(string password, AccountDetails accountDetails)
    {
        Password = password;
        AccountDetails = accountDetails;
    }

    /// <summary>
    /// The user's credentials
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// The container class for the account's details
    /// </summary>
    public AccountDetails AccountDetails { get; set; }
}