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

        var patientId = Guid.NewGuid();
        var accountDb = app.Services.GetRequiredService<IAccountDatabaseService>();
        var detailsPatient = new AccountDetails(patientId, AccountType.Patient, "John", "Patient", "John@patient.com");
        var credentialsPatient = new AccountCredentials("123", detailsPatient);
        accountDb.SetAccountById(detailsPatient.UserId, detailsPatient.Email, credentialsPatient);

        var detailsDoctor = new AccountDetails(Guid.NewGuid(), AccountType.Doctor, "John", "Doctor", "John@doctor.com");
        var credentialsDoctor = new AccountCredentials("123", detailsDoctor);
        accountDb.SetAccountById(detailsDoctor.UserId, detailsDoctor.Email, credentialsDoctor);

        var medicationDb = app.Services.GetRequiredService<IMedicationDatabaseService>();
        var medicationId = Guid.NewGuid();
        var medication = new Medication(medicationId, "Estrogen", "Feminizing hormone", "https://i.redd.it/2yp7s912k6m81.jpg");
        medicationDb.SetMedicationById(medicationId, medication);

        var prescriptionDb = app.Services.GetRequiredService<IPrescriptionDatabaseService>();
        var prescription = new Prescription(medicationId, patientId, "1", 1, 10, "no notes","https://upload.wikimedia.org/wikipedia/commons/1/15/Cat_August_2010-4.jpg");
        prescriptionDb.SetPrescriptionById(Guid.NewGuid(), prescription);

        app.Run();
    }
}