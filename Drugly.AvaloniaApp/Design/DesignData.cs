using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Drugly.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.Design;

/// <summary>Provides static viewmodel instances for Avalonia preview to work with dependency injection.</summary>
public static partial class DesignData
{
    /// <summary>A no-op application for satisfying the <see cref="Application"/> requirement of <see cref="Startup.ConfigureServices"/>.</summary>
    private class DesignApplication : Application
    {
        public DesignApplication()
        {
            ApplicationLifetime = new ClassicDesktopStyleApplicationLifetime();
        }
    }

    static DesignData()
    {
        try
        {
            throw new Exception("Example exception");
        }
        catch (Exception ex)
        {
            ExampleException = ex;
        }
    }

    /// <summary>An example exception.</summary>
    public static Exception ExampleException { get; }

    /// <summary>An array of example patients.</summary>
    public static AccountDetails[] ExamplePatients =>
    [
        new(Guid.NewGuid(), AccountType.Patient, "John", "Doe", "jdoe@example.com"),
        new(Guid.NewGuid(), AccountType.Patient, "Ray", "March", "ray@march.com"),
        new(Guid.NewGuid(), AccountType.Patient, "Jane", "Remover", "jane@gmail.com"),
        new(Guid.NewGuid(), AccountType.Patient, "John", "Carmack", "jcarmack@aol.com"),
    ];

    /// <summary>An example patient.</summary>
    public static AccountDetails ExamplePatient => ExamplePatients[0];

    /// <summary>An array of example medications.</summary>
    public static Medication[] ExampleMedications =>
    [
        new(Guid.NewGuid(), "Estrogen", "Mreowww", "https://i.redd.it/2yp7s912k6m81.jpg"),
        new(Guid.NewGuid(), "Addherall", "Girl, you need to focus.", "https://f4.bcbits.com/img/a4229702017_10.jpg"),
        new(Guid.NewGuid(), "Ibuprofen", "You be what?", "https://ih1.redbubble.net/image.6073234997.2641/raf,360x360,075,t,fafafa:ca443f4786.jpg"),
        new(Guid.NewGuid(), "Water", "Drink. Now.", "https://images.squarespace-cdn.com/content/v1/540e2e30e4b0a9fac1c138ac/27edb5eb-1ae1-4dc7-8440-db186f4e175b/glass_water.jpg"),
    ];

    /// <summary>An example medication.</summary>
    public static Medication ExampleMedication => ExampleMedications[0];

    /// <summary>An example medication.</summary>
    public static Prescription[] ExamplePrescriptions => [
        new(Guid.NewGuid(), Guid.NewGuid(), "1", 1, 10, "no notes","https://upload.wikimedia.org/wikipedia/commons/1/15/Cat_August_2010-4.jpg"),
        new(Guid.NewGuid(), Guid.NewGuid(), "5", 2, 30, "no notes","https://upload.wikimedia.org/wikipedia/commons/1/15/Cat_August_2010-4.jpg")
    ];

    /// <summary>An example medication.</summary>
    public static Prescription ExamplePrescription => ExamplePrescriptions[0];

    /// <summary>The design time <see cref="IServiceProvider"/>.</summary>
    private static IServiceProvider ServiceProvider
        => field ??= new ServiceCollection()
            .ConfigureServices(new DesignApplication())
            .ConfigureViews()
            .BuildServiceProvider();
}