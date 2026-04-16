using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.ViewModels;
using Drugly.AvaloniaApp.ViewModels.Pages;
using Drugly.AvaloniaApp.ViewModels.Pages.Doctor;
using Drugly.AvaloniaApp.ViewModels.Pages.Patient;
using Drugly.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.Design;

public static partial class DesignData
{
    public static MainViewModel MainViewModel
    {
        get
        {
            if (field == null)
            {
                field = ServiceProvider.GetRequiredService<MainViewModel>();
            }

            return field;
        }
    }

    public static SettingsViewModel SettingsViewModel
    {
        get
        {
            if (field == null)
            {
                field = ServiceProvider.GetRequiredService<SettingsViewModel>();
            }

            return field;
        }
    }

    public static PatientMainViewModel PatientMainViewModel
    {
        get
        {
            if (field == null)
            {
                field = ServiceProvider.GetRequiredService<PatientMainViewModel>();
                field.Prescriptions.AddRange(ExamplePrescriptions.Select(x => new PatientPrescription(x, ExampleMedication)));
            }

            return field;
        }
    }

    public static PatientPrescriptionDetailsViewModel PatientPrescriptionDetailsViewModel
    {
        get
        {
            if (field == null)
            {
                field = ServiceProvider.GetRequiredService<PatientPrescriptionDetailsViewModel>();
                field.Patient = ExamplePatient with { AccountType = AccountType.Doctor };
                field.Prescription = new PatientPrescription(ExamplePrescription, ExampleMedication);
            }

            return field;
        }
    }

    public static DoctorMainViewModel DoctorMainViewModel
    {
        get
        {
            if (field == null)
            {
                field = ServiceProvider.GetRequiredService<DoctorMainViewModel>();
            }

            return field;
        }
    }

    public static DoctorPrescribeModalViewModel DoctorPrescribeModalViewModel
    {
        get
        {
            if (field == null)
            {
                field = new DoctorPrescribeModalViewModel(null!, ExampleMedication, ServiceProvider)
                {
                    PatientFirstName = "Jane",
                    PatientLastName = "Doe",
                    PatientEmail = "jdoe@example.com",
                    PrescriptionNotes = "Consume while meowing for best results",
                    DosagePerDay = "5,000,000,000mg",
                    DaysBetweenDosage = 0,
                    DaysPrescribed = ulong.MaxValue
                };
            }

            return field;
        }
    }

    public static DoctorPatientListViewModel DoctorPatientListViewModel
    {
        get
        {
            if (field == null)
            {
                field = ServiceProvider.GetRequiredService<DoctorPatientListViewModel>();
                field.Patients.AddRange(ExamplePatients);
            }

            return field;
        }
    }

    public static DoctorMedicationListViewModel DoctorMedicationListViewModel
    {
        get
        {
            if (field == null)
            {
                field = ServiceProvider.GetRequiredService<DoctorMedicationListViewModel>();
                field.Medications.AddRange(ExampleMedications);
            }

            return field;
        }
    }

    public static DoctorMedicationDetailsPageViewModel DoctorMedicationDetailsPageViewModel
    {
        get
        {
            if (field == null)
            {
                field = ServiceProvider.GetRequiredService<DoctorMedicationDetailsPageViewModel>();
                field.Medication = ExampleMedication;
            }

            return field;
        }
    }

    public static DoctorPatientPrescriptionListModalViewModel DoctorPatientPrescriptionListModalViewModel
    {
        get
        {
            if (field == null)
            {
                field = new DoctorPatientPrescriptionListModalViewModel(null!, ExamplePatient, ServiceProvider);
                field.Prescriptions.AddRange(ExamplePrescriptions.Select(x => new PatientPrescription(x, ExampleMedication)));
            }

            return field;
        }
    }
}