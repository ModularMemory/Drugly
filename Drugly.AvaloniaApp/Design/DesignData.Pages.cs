using Drugly.AvaloniaApp.ViewModels.Pages;
using Drugly.AvaloniaApp.ViewModels.Pages.Doctor;
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

    public static PatientMainViewModel PatientMainViewModel
    {
        get
        {
            if (field == null)
            {
                field = ServiceProvider.GetRequiredService<PatientMainViewModel>();
            }

            return field;
        }
    }

    public static PatientDetailsPageViewModel PatientDetailsPageViewModel
    {
        get
        {
            if (field == null)
            {
                field = ServiceProvider.GetRequiredService<PatientDetailsPageViewModel>();
                field.Patient = ExamplePatient;
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
                field = new DoctorPrescribeModalViewModel(null!, ExamplePrescription)
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
                field.Medications.AddRange(ExamplePrescriptions);
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
                field.Prescription = ExamplePrescription;
            }

            return field;
        }
    }

    public static PharmacistMainViewModel PharmacistMainViewModel
    {
        get
        {
            if (field == null)
            {
                field = ServiceProvider.GetRequiredService<PharmacistMainViewModel>();
            }

            return field;
        }
    }
}