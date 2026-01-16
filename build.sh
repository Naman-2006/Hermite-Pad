#!/bin/bash
# Build script for Hermite Pad (Linux/macOS - for cross-platform development)

echo "Building Hermite Pad..."
echo ""

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "ERROR: .NET SDK not found!"
    echo "Please install .NET 8.0 SDK from https://dotnet.microsoft.com/download"
    exit 1
fi

echo "Using .NET version:"
dotnet --version
echo ""

# Restore dependencies
echo "Restoring NuGet packages..."
dotnet restore
if [ $? -ne 0 ]; then
    echo "ERROR: Failed to restore packages"
    exit 1
fi

# Build the project
echo ""
echo "Building solution..."
dotnet build -c Release
if [ $? -ne 0 ]; then
    echo "ERROR: Build failed"
    exit 1
fi

echo ""
echo "========================================"
echo "Build completed successfully!"
echo "========================================"
echo ""
echo "Note: This is a Windows-only application (WPF)"
echo "To run on Windows, use:"
echo "  dotnet run --project HermitePad/HermitePad.csproj"
echo ""
