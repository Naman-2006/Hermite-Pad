# Hermite Pad Architecture

## Overview

Hermite Pad is a Windows 11 desktop application built on WPF (Windows Presentation Foundation) that provides a stylus-first note-taking experience with advanced features.

## Architecture Diagram

```
┌──────────────────────────────────────────────────────────────┐
│                      MainWindow (UI)                         │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │  Toolbar     │  │   Canvas     │  │  Layer Panel │      │
│  │  - Tools     │  │  - InkCanvas │  │  - Layers    │      │
│  │  - Actions   │  │  - Viewport  │  │  - Controls  │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└─────────────┬────────────────┬────────────────┬─────────────┘
              │                │                │
              ▼                ▼                ▼
┌─────────────────────┐  ┌────────────────┐  ┌──────────────┐
│   CanvasManager     │  │  UndoRedoMgr   │  │   Tools      │
│  - Layers           │  │  - Undo Stack  │  │  - LassoTool │
│  - Zoom/Pan         │  │  - Redo Stack  │  │  - BezierTool│
│  - Transforms       │  │  - Actions     │  │  - MathConv  │
└─────────────────────┘  └────────────────┘  └──────────────┘
              │                │
              ▼                ▼
┌─────────────────────────────────────────────────────────────┐
│                    Data Models                               │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐   │
│  │  Layer   │  │ MathObj  │  │ BezierP  │  │ Document │   │
│  └──────────┘  └──────────┘  └──────────┘  └──────────┘   │
└─────────────────────────────────────────────────────────────┘
              │                │
              ▼                ▼
┌─────────────────────┐  ┌────────────────┐
│  DocumentSerializer │  │  PdfExporter   │
│  - Save/Load        │  │  - PDF Export  │
└─────────────────────┘  └────────────────┘
```

## Component Descriptions

### UI Layer

#### MainWindow
- Primary user interface
- Manages toolbar, canvas, and layer panel
- Handles user input events
- Coordinates between core components

### Core Components

#### CanvasManager
- **Responsibilities:**
  - Multi-layer canvas management
  - Zoom and pan transformations
  - Layer operations (add, remove, activate)
  - Content organization (ink, math, vectors)
  
- **Key Methods:**
  - `Zoom(factor, center)`: Applies zoom transformation
  - `Pan(delta)`: Translates canvas view
  - `AddLayer()`: Creates new layer
  - `SetActiveLayer(layer)`: Switches active layer

#### UndoRedoManager
- **Responsibilities:**
  - Action history management
  - Undo/redo operations
  - Stack size limiting
  
- **Key Methods:**
  - `AddAction(action)`: Adds action to history
  - `Undo()`: Reverses last action
  - `Redo()`: Reapplies undone action

#### DocumentSerializer
- **Responsibilities:**
  - Save documents to .hpd format
  - Load documents from .hpd format
  - Serialize/deserialize strokes, math, and vectors
  
- **File Format:**
  ```
  [Version: Int32]
  [Stroke Length: Int32]
  [Strokes: ISF Format]
  [Math Objects: JSON]
  [Bezier Paths: JSON]
  ```

#### PdfExporter
- **Responsibilities:**
  - Export canvas to PDF
  - Render ink strokes
  - Include math expressions
  
- **Libraries Used:**
  - PdfSharp for PDF generation
  - WPF RenderTargetBitmap for rendering

### Tools

#### LassoTool
- **Functionality:**
  - Stroke selection
  - Area-based selection
  - Selection management
  
- **Usage Flow:**
  1. User draws lasso around strokes
  2. Tool detects intersecting strokes
  3. Selected strokes highlighted
  4. User can convert to math or manipulate

#### BezierTool
- **Functionality:**
  - Bezier curve creation
  - Anchor point management
  - Control handle editing
  - Smooth/corner point types
  
- **Node Types:**
  - **Smooth**: Continuous tangent through point
  - **Corner**: Independent control handles

#### MathConverter
- **Functionality:**
  - Handwriting recognition
  - Symbol pattern matching
  - LaTeX generation
  - MathML generation
  
- **Recognition Pipeline:**
  ```
  Ink Strokes → Symbol Recognition → Structure Analysis → LaTeX/MathML
  ```

### Data Models

#### Layer
```csharp
class Layer {
    string Name
    bool IsVisible
    List<MathObject> MathObjects
    List<BezierPath> BezierPaths
}
```

#### MathObject
```csharp
class MathObject {
    string LaTeX
    string MathML
    Point Position
    double Width, Height
}
```

#### BezierPath
```csharp
class BezierPath {
    List<BezierNode> Nodes
    bool IsClosed
    double StrokeThickness
    Color StrokeColor
}
```

#### BezierNode
```csharp
class BezierNode {
    Point Position
    Point? ControlPoint1
    Point? ControlPoint2
    NodeType Type  // Smooth or Corner
}
```

## User Interaction Flow

### Drawing Ink
```
User touches stylus → WPF captures input → InkCanvas processes →
Pressure applied → Stroke rendered → Stroke collected → 
Undo action created
```

### Converting to Math
```
User draws equation → Selects with lasso → Clicks convert →
MathConverter analyzes → Patterns recognized → LaTeX generated →
MathML generated → MathObject created → Original strokes removed →
Undo action created
```

### Creating Bezier Curves
```
User selects Bezier tool → Clicks to add anchor → 
Node created → Control handles appear → User drags handles →
Curve updated → Path rendered → Continue or finish
```

### Zoom and Pan
```
Ctrl + Mouse Wheel → Zoom event → CanvasManager.Zoom() →
ScaleTransform updated → Canvas re-rendered

Pan mode + Drag → Mouse move event → CanvasManager.Pan() →
TranslateTransform updated → Canvas repositioned
```

## Technology Stack

- **Framework**: WPF (Windows Presentation Foundation)
- **Language**: C# (.NET 8.0)
- **UI**: XAML
- **Stylus Input**: WPF InkCanvas with StylusPlugIns
- **PDF Export**: PdfSharp 6.0
- **Math**: MathNet.Numerics 5.0
- **Serialization**: System.Text.Json + ISF (Ink Serialized Format)

## Performance Considerations

### Ink Rendering
- Uses hardware-accelerated WPF rendering
- Stroke collection optimized for real-time input
- Curve fitting reduces point count

### Zoom/Pan
- Transform-based (no re-rendering of strokes)
- Smooth interpolation
- Viewport clipping for large canvases

### Undo/Redo
- Action-based (minimal memory per action)
- Stack size limited to 100 actions
- Efficient stroke collection cloning

## Future Enhancements

1. **Advanced Math Recognition**
   - Machine learning integration
   - Cloud-based recognition API
   - Training data collection

2. **Collaboration**
   - Real-time multi-user editing
   - Cloud sync
   - Version history

3. **Advanced Vector Tools**
   - Shape library
   - Boolean operations
   - Path effects

4. **Performance**
   - Virtualization for large documents
   - Incremental PDF export
   - Background auto-save

## Security Considerations

- Local file storage only (no cloud by default)
- No network access required
- User data privacy maintained
- Document encryption (future)

## Testing Strategy

- Unit tests for core components
- Integration tests for file operations
- Manual testing with stylus devices
- Performance profiling for large documents
- Cross-Windows version testing (10 & 11)
