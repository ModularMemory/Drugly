using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Drugly.AvaloniaApp.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.Design;

public static partial class DesignData
{
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

    private static Exception ExampleException { get; }

    public static PatientModel[] ExamplePatients =>
    [
        new("John", "Doe", "jdoe@example.com"),
        new("Ray", "March", "ray@march.com"),
        new("Jane", "Remover", "jane@gmail.com"),
        new("John", "Carmack", "jcarmack@aol.com"),
    ];

    public static PatientModel ExamplePatient => ExamplePatients[0];

    public static MedicationModel[] ExampleMedications =>
    [
        new("Estrogen", "Mreowww", "https://i.redd.it/2yp7s912k6m81.jpg"),
        new("Addherall", "Girl, you need to focus.", "https://f4.bcbits.com/img/a4229702017_10.jpg"),
        new("Ibuprofen", "You be what?", "https://ih1.redbubble.net/image.6073234997.2641/raf,360x360,075,t,fafafa:ca443f4786.jpg"),
        new("Water", "Drink. Now.", "https://images.squarespace-cdn.com/content/v1/540e2e30e4b0a9fac1c138ac/27edb5eb-1ae1-4dc7-8440-db186f4e175b/glass_water.jpg"),
    ];

    public static MedicationModel ExampleMedication => ExampleMedications[0];

    private static IServiceProvider ServiceProvider
        => field ??= new ServiceCollection()
            .ConfigureServices(new DesignApplication())
            .ConfigureViews()
            .BuildServiceProvider();
}