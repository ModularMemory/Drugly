using Drugly.DTO;
using Drugly.Server.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Drugly.Tests
{
    [TestClass]
    public class JsonTests
    {
        private string _testDir = null!;

        [TestInitialize]
        public void Setup()
        {
            _testDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDir);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(_testDir))
                Directory.Delete(_testDir, true);
        }

        private string GetFilePath(string fileName) => Path.Combine(_testDir, fileName);

        [TestMethod]
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
            Assert.IsNotNull(result);
            Assert.AreEqual(data.Name, result!.Name);
            Assert.AreEqual(data.Value, result.Value);
        }

        [TestMethod]
        public async Task JsonReader_FileDoesNotExist_ReturnsDefault()
        {
            // Arrange
            var filePath = GetFilePath("missing.json");

            // Act
            var result = await JsonReader.LoadAsync<TestObject>(filePath);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task JsonWritePrescription_And_ReadPrescription_RoundTrip()
        {
            // Arrange
            var filePath = GetFilePath("prescription.json");
            var prescription = new Prescription
            {
                // Populate required fields
            };

            // Act
            await JsonWritePrescription.SavePrescription(prescription, filePath);
            var result = await JsonReadPrescription.LoadPrescription(filePath);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task JsonWriteMedication_And_ReadMedication_RoundTrip()
        {
            // Arrange
            var filePath = GetFilePath("medication.json");
            var medication = new Medication
            {
                // Populate required fields
            };

            // Act
            await JsonWriteMedication.SaveMedication(medication, filePath);
            var result = await JsonReadMedication.LoadMedication(filePath);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task JsonWriteAccountDetails_And_ReadAccountDetails_RoundTrip()
        {
            // Arrange
            var filePath = GetFilePath("accountDetails.json");
            var details = new AccountDetails
            {
                // Populate required fields
            };

            // Act
            await JsonWriteAccountDetails.SaveAccountDetails(details, filePath);
            var result = await JsonReadAccountDetails.LoadAccountDetails(filePath);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task JsonWriteAccountDatabaseEntry_CreatesDirectory_And_SavesFile()
        {
            // Arrange
            var filePath = Path.Combine(_testDir, "nested", "account.json");
            var credentials = new AccountCredentials
            {
                // Populate required fields
            };

            // Act
            await JsonWriteAccountDatabaseEntry.SaveAccount(credentials, filePath);

            // Assert
            Assert.IsTrue(File.Exists(filePath));
        }

        [TestMethod]
        public void JsonReadAccountDatabaseEntry_FileDoesNotExist_ReturnsNull()
        {
            // Arrange
            var filePath = GetFilePath("missingAccount.json");

            // Act
            var result = JsonReadAccountDatabaseEntry.LoadAccount(filePath);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void JsonReadAccountDatabaseEntry_ValidFile_ReturnsObject()
        {
            // Arrange
            var filePath = GetFilePath("account.json");
            var credentials = new AccountCredentials
            {
                // Populate required fields
            };

            var json = System.Text.Json.JsonSerializer.Serialize(credentials);
            File.WriteAllText(filePath, json);

            // Act
            var result = JsonReadAccountDatabaseEntry.LoadAccount(filePath);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}