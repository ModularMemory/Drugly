using Drugly.DTO;
using Drugly.Server.Services.Interfaces;

namespace Drugly.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.ConfigureServices();
        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.MapControllers();

        var accountDb = app.Services.GetRequiredService<IAccountDatabaseService>();
        var detailsPatient = new AccountDetails(Guid.NewGuid(), AccountType.Patient, "John", "Patient", "John@patient.com");
        var credentialsPatient = new AccountCredentials("123", detailsPatient);
        accountDb.SetAccountById(detailsPatient.UserId, detailsPatient.Email, credentialsPatient);

        var detailsDoctor = new AccountDetails(Guid.NewGuid(), AccountType.Doctor, "John", "Doctor", "John@doctor.com");
        var credentialsDoctor = new AccountCredentials("123", detailsDoctor);
        accountDb.SetAccountById(detailsDoctor.UserId, detailsDoctor.Email, credentialsDoctor);
        app.Run();
    }
}