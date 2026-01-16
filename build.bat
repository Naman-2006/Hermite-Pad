@echo off
REM Build script for Hermite Pad

echo Building Hermite Pad...
echo.

REM Check if .NET is installed
dotnet --version > nul 2>&1
if errorlevel 1 (
    echo ERROR: .NET SDK not found!
    echo Please install .NET 8.0 SDK from https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

REM Restore dependencies
echo Restoring NuGet packages...
dotnet restore
if errorlevel 1 (
    echo ERROR: Failed to restore packages
    pause
    exit /b 1
)

REM Build the project
echo.
echo Building solution...
dotnet build -c Release
if errorlevel 1 (
    echo ERROR: Build failed
    pause
    exit /b 1
)

echo.
echo ========================================
echo Build completed successfully!
echo ========================================
echo.
echo To run the application:
echo   dotnet run --project HermitePad/HermitePad.csproj
echo.
echo Or navigate to:
echo   HermitePad/bin/Release/net8.0-windows/HermitePad.exe
echo.
pause
