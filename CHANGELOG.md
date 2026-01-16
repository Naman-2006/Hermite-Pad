# Changelog

All notable changes to Hermite Pad will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-01-16

### Added

#### Core Features
- **Freehand Ink Drawing**
  - Pressure-sensitive stylus support
  - Smooth curve rendering with automatic fitting
  - Real-time ink feedback
  - Hardware-accelerated rendering

- **Advanced Editing Tools**
  - Eraser tool for stroke removal
  - Lasso selection tool for multi-stroke selection
  - Undo/redo system with 100-action history
  - Clear canvas functionality

- **Handwriting-to-Math Conversion**
  - Pattern-based math recognition
  - LaTeX output generation
  - MathML output generation
  - Lasso-select and convert workflow
  - Math object preservation

- **Bezier Curve Tool**
  - Professional Pen tool implementation
  - Anchor point creation and editing
  - Control handle manipulation
  - Smooth and corner point types
  - Visual node representation
  - Path preview rendering

- **Multi-Layer Canvas System**
  - Layer creation and deletion
  - Layer activation/switching
  - Separate storage for ink, math, and vectors
  - Layer visibility controls
  - Default layer always present

- **File Operations**
  - Save to .hpd format (Hermite Pad Document)
  - Load .hpd documents
  - Export to PDF with high-quality rendering
  - Stroke serialization using ISF format
  - JSON serialization for math and vector data

- **Navigation and View Controls**
  - Smooth zoom with Ctrl + Mouse Wheel
  - Canvas panning
  - Transform-based viewport management
  - Zoom range limiting (10%-1000%)

#### User Interface
- Modern Windows 11 design
- Toolbar with tool selection
- Layer management panel
- Status bar with tool information
- Responsive layout
- Custom button styles
- Professional color scheme

#### Documentation
- Comprehensive README with build instructions
- Architecture documentation (ARCHITECTURE.md)
- User guide (USER_GUIDE.md)
- Build scripts for Windows and Linux
- Inline code comments

### Technical Details

#### Technologies Used
- WPF (Windows Presentation Foundation)
- C# / .NET 8.0
- InkCanvas API for stylus support
- PdfSharp 6.0 for PDF export
- MathNet.Numerics 5.0
- System.Text.Json for serialization

#### Project Structure
- Core components for canvas and undo/redo management
- Tool implementations (Lasso, Bezier, Math conversion)
- Data models for layers and content
- Serialization and export utilities
- Modern XAML theme

### Known Limitations

- Math recognition uses basic pattern matching (ML-based recognition planned)
- Bezier tool node editing partially implemented
- Some advanced features in development

### Dependencies

- .NET 8.0 Runtime (Windows)
- PdfSharp 6.0.0
- MathNet.Numerics 5.0.0

## [Unreleased]

### Planned Features

- Advanced math recognition with machine learning
- Cloud synchronization
- Collaboration features
- Shape recognition
- Text annotation tools
- Custom brush styles
- Dark mode theme
- Additional export formats (SVG, PNG)
- Touch gesture support
- Template library

### Planned Improvements

- Enhanced Bezier tool with more editing options
- Better math symbol recognition
- Performance optimization for large documents
- Auto-save functionality
- Document encryption
- Multi-language support

---

## Version History

### Version Numbering

- **Major version**: Breaking changes or major feature additions
- **Minor version**: New features, backward compatible
- **Patch version**: Bug fixes and minor improvements

### Release Schedule

Releases are made when significant features are complete and tested. Check the GitHub releases page for the latest version.
