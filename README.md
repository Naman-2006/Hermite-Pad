# Hermite Pad - Windows 11 Stylus-First Note App

A powerful Windows 11 desktop application designed for stylus input, featuring advanced ink capabilities, handwriting-to-math conversion, and professional Bezier curve tools.

## Features

### 🖊️ Freehand Ink with Pressure Sensitivity
- Natural writing experience with full pressure sensitivity support
- Smooth, high-quality ink rendering with curve fitting
- Real-time stylus input processing

### 🧹 Advanced Editing Tools
- **Eraser Tool**: Remove strokes with precision
- **Lasso Selection Tool**: Select multiple strokes for batch operations
- Undo/Redo support with intelligent action history

### 🔢 Handwriting-to-Math Conversion
- Draw mathematical equations naturally with stylus
- Convert handwritten math to clean, editable mathematical expressions
- Support for LaTeX and MathML output formats
- Lasso-select strokes and convert to math objects
- Preserve converted math for editing and export

### 📐 Professional Bezier Curve Tool
- Photoshop-like Pen tool with true Bezier curves
- Add and edit anchor points with precision
- Control handles for smooth curve manipulation
- Smooth and corner point types
- Full node editing capabilities

### 🎨 Multi-Layer Canvas System
- Organize content across multiple layers
- Separate layers for ink, math objects, and vector paths
- Layer visibility controls
- Flexible layer management

### 💾 File Operations
- Save and load documents in native .hpd format
- Export to PDF with high-quality rendering
- Preserve all ink, math, and vector data

### 🔍 Navigation Controls
- Smooth zoom with Ctrl + Mouse Wheel
- Pan canvas with middle mouse or Space + drag
- Responsive viewport management

## Requirements

- **Operating System**: Windows 10/11 (64-bit)
- **.NET Runtime**: .NET 8.0 or higher
- **Optional**: Stylus/pen input device for best experience

## Building the Project

### Prerequisites
1. Install [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Visual Studio 2022 (recommended) or Visual Studio Code with C# extension

### Build Steps

1. Clone the repository:
```bash
git clone https://github.com/Naman-2006/Hermite-Pad.git
cd Hermite-Pad
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the solution:
```bash
dotnet build
```

4. Run the application:
```bash
dotnet run --project HermitePad/HermitePad.csproj
```

### Build Release Version
```bash
dotnet publish -c Release -r win-x64 --self-contained
```

## Usage Guide

### Getting Started
1. Launch Hermite Pad
2. Select the **Ink** tool to start drawing with stylus or mouse
3. Use pressure-sensitive input for natural writing

### Tools Overview

#### Ink Tool (✏️)
- Default drawing tool
- Supports pressure sensitivity
- Creates smooth, natural strokes

#### Eraser Tool (🧹)
- Click on strokes to erase them
- Erase by stroke (entire stroke removed)

#### Lasso Tool (⭕)
- Click and drag to select multiple strokes
- Use for batch operations and math conversion

#### Bezier Tool (📐)
- Click to add anchor points
- Creates professional vector curves
- Edit nodes and handles for precise control

### Math Conversion Workflow
1. Draw mathematical equations with the Ink tool
2. Switch to Lasso tool and select the handwritten math
3. Click "Convert to Math" button
4. Review the generated LaTeX/MathML
5. Math objects are preserved in the document

### Keyboard Shortcuts
- **Ctrl + Z**: Undo
- **Ctrl + Y**: Redo
- **Ctrl + S**: Save document
- **Ctrl + O**: Open document
- **Ctrl + Mouse Wheel**: Zoom in/out
- **Space + Drag**: Pan canvas

## Architecture

### Core Components

#### CanvasManager
- Manages the multi-layer canvas system
- Handles zoom and pan transformations
- Coordinates ink, math, and vector content

#### UndoRedoManager
- Implements action-based undo/redo system
- Maintains history stacks for all operations
- Supports unlimited undo (configurable limit)

#### DocumentSerializer
- Handles save/load operations
- Uses ISF (Ink Serialized Format) for ink data
- JSON serialization for math and vector objects

#### PdfExporter
- Exports canvas content to PDF format
- Preserves ink quality and math expressions
- High-resolution rendering

### Tools

#### LassoTool
- Stroke selection and manipulation
- Area-based selection algorithms

#### BezierTool
- Professional Bezier curve creation
- Node and handle management
- Smooth and corner point support

#### MathConverter
- Handwriting recognition for mathematical expressions
- Pattern matching and symbol recognition
- LaTeX and MathML generation

## Development

### Project Structure
```
HermitePad/
├── HermitePad.sln           # Solution file
├── HermitePad/
│   ├── HermitePad.csproj    # Project file
│   ├── App.xaml             # Application definition
│   ├── App.xaml.cs          # Application code-behind
│   ├── MainWindow.xaml      # Main window UI
│   ├── MainWindow.xaml.cs   # Main window code-behind
│   ├── Core/                # Core functionality
│   │   ├── CanvasManager.cs
│   │   ├── UndoRedoManager.cs
│   │   ├── Actions.cs
│   │   ├── DocumentSerializer.cs
│   │   └── PdfExporter.cs
│   ├── Tools/               # Tool implementations
│   │   ├── LassoTool.cs
│   │   ├── BezierTool.cs
│   │   └── MathConverter.cs
│   └── Themes/              # UI themes
│       └── Modern.xaml
```

### Technologies Used
- **WPF (Windows Presentation Foundation)**: UI framework
- **C# / .NET 8.0**: Primary language and runtime
- **InkCanvas API**: Stylus and ink support
- **PdfSharp**: PDF export functionality
- **MathNet.Numerics**: Mathematical computations

## Contributing

Contributions are welcome! Please feel free to submit pull requests or open issues for bugs and feature requests.

## License

This project is open source. Please check the repository for license details.

## Roadmap

Future enhancements planned:
- [ ] Advanced math recognition with ML models
- [ ] Cloud sync and collaboration features
- [ ] Additional export formats (SVG, PNG)
- [ ] Shape recognition and auto-correction
- [ ] Text annotation tools
- [ ] Custom brush and pen styles
- [ ] Tablet-optimized UI
- [ ] Dark mode theme

## Support

For issues, questions, or suggestions, please open an issue on the GitHub repository.

---

Made with ❤️ for Windows 11 stylus users