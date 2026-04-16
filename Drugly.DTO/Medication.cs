using System.Diagnostics.CodeAnalysis;

namespace Drugly.DTO;

/// <summary>A container class for all the information needed for a medication</summary>
public class Medication
{
    [SetsRequiredMembers]
    public Medication(Guid id, string name, string description, string imageUri)
    {
        Id = id;
        Name = name;
        Description = description;
        ImageUri = imageUri;
    }

    /// <summary>
    /// The ID of the medication
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// The name of the medication
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The description of the medication
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// The Uri for the image of the medication
    /// </summary>
    public required string ImageUri { get; set; }
}