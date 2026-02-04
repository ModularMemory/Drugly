# Drugly

A patient-centric way to fill prescriptions.

## How to Run

1. Install [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

2. Clone the repository:
    ```sh
    git clone https://github.com/ModularMemory/Drugly.git
    ```

3. Navigate to the cloned repository:
     ```sh
    cd Drugly
    ```

4. Build the solution:
    ```sh
    dotnet build
   ```

5. Run the Server:
    ```sh
    dotnet run --project Drugly.Server
    ```

6. Run the GUI:
    ```sh
    dotnet run --project Drugly.AvaloniaApp
    ```

7. Run the tests:
    ```sh
    .\TestWithResults.bat
    ```