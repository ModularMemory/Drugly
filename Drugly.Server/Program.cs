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

        var patientId2 = Guid.NewGuid();
        var detailsPatient2 = new AccountDetails(patientId2, AccountType.Patient, "Sam", "Patient", "Sam@patient.com");
        var credentialsPatient2 = new AccountCredentials("123", detailsPatient2);
        accountDb.SetAccountById(detailsPatient2.UserId, detailsPatient2.Email, credentialsPatient2);

        var patientId3 = Guid.NewGuid();
        var detailsPatient3 = new AccountDetails(patientId3, AccountType.Patient, "Tim", "Patient", "Tim@patient.com");
        var credentialsPatient3 = new AccountCredentials("123", detailsPatient3);
        accountDb.SetAccountById(detailsPatient3.UserId, detailsPatient3.Email, credentialsPatient3);

        var detailsDoctor = new AccountDetails(Guid.NewGuid(), AccountType.Doctor, "John", "Doctor", "John@doctor.com");
        var credentialsDoctor = new AccountCredentials("123", detailsDoctor);
        accountDb.SetAccountById(detailsDoctor.UserId, detailsDoctor.Email, credentialsDoctor);

        var medicationDb = app.Services.GetRequiredService<IMedicationDatabaseService>();
        var medicationId1 = Guid.NewGuid();
        var medication1 = new Medication(medicationId1, "Estrogen", "Feminizing hormone", "https://i.redd.it/2yp7s912k6m81.jpg");
        medicationDb.SetMedicationById(medicationId1, medication1);

        var medicationId2 = Guid.NewGuid();
        var medication2 = new Medication(medicationId2, "Adderall", "Treats ADHD", "https://f4.bcbits.com/img/a4229702017_10.jpg");
        medicationDb.SetMedicationById(medicationId2, medication2);

        var medicationId3 = Guid.NewGuid();
        var medication3 = new Medication(medicationId3, "Ibuprofen", "Pain killer", "https://ih1.redbubble.net/image.6073234997.2641/raf,360x360,075,t,fafafa:ca443f4786.jpg");
        medicationDb.SetMedicationById(medicationId3, medication3);

        var medicationId4 = Guid.NewGuid();
        var medication4 = new Medication(medicationId4, "Ibuprofen", "Pain killer", "https://ih1.redbubble.net/image.6073234997.2641/raf,360x360,075,t,fafafa:ca443f4786.jpg");
        medicationDb.SetMedicationById(medicationId4, medication4);

        var prescriptionDb = app.Services.GetRequiredService<IPrescriptionDatabaseService>();
        var prescription = new Prescription(medicationId1, patientId, "1", 1, 10, "no notes","https://upload.wikimedia.org/wikipedia/commons/1/15/Cat_August_2010-4.jpg");
        prescriptionDb.SetPrescriptionById(Guid.NewGuid(), prescription);
        //
        // var prescription2 = new Prescription(medicationId2, patientId2, "1", 1, 10, "no notes","https://upload.wikimedia.org/wikipedia/commons/1/15/Cat_August_2010-4.jpg");
        // prescriptionDb.SetPrescriptionById(Guid.NewGuid(), prescription2);
        //
        // var prescription3 = new Prescription(medicationId3, patientId, "1", 1, 10, "no notes","https://upload.wikimedia.org/wikipedia/commons/1/15/Cat_August_2010-4.jpg");
        // prescriptionDb.SetPrescriptionById(Guid.NewGuid(), prescription3);

        app.Run();
    }
}