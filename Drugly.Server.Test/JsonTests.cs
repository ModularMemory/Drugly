using System.Text.Json;
using Drugly.DTO;
using Drugly.Server.Data;

namespace Drugly.Server.Test;

public class JsonTests : IDisposable
{
    private record TestObject
    {
        public string? Name { get; init; }
        public int Value { get; init; }
    }

    private readonly string _testDir;

    /// <summary>Setup.</summary>
    public JsonTests()
    {
        _testDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDir);
    }

    /// <summary>Cleanup method.</summary>
    public void Dispose()
    {
        if (Directory.Exists(_testDir))
            Directory.Delete(_testDir, true);
    }

    private string GetFilePath(string fileName) => Path.Combine(_testDir, fileName);

    [Fact]
    public async Task JsonWriter_And_Reader_RoundTrip_Generic()
    {
        // Arrange
        var filePath = GetFilePath("generic.json");
        var data = new TestObject
        {
            Name = "Test",
            Value = 42
        };

        // Act
        await JsonWriter.SaveAsync(data, filePath);
        var result = await JsonReader.LoadAsync<TestObject>(filePath);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(data.Name, result!.Name);
        Assert.Equal(data.Value, result.Value);
    }

    [Fact]
    public async Task JsonReader_FileDoesNotExist_ReturnsDefault()
    {
        // Arrange
        var filePath = GetFilePath("missing.json");

        // Act
        var result = await JsonReader.LoadAsync<TestObject>(filePath);

        // Assert
        Assert.Null(result);
    }

}