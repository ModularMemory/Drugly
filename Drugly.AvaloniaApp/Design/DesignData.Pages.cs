using Drugly.AvaloniaApp.ViewModels.Pages;
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