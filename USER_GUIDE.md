# Hermite Pad User Guide

Welcome to Hermite Pad, a Windows 11 stylus-first note-taking application!

## Table of Contents

1. [Getting Started](#getting-started)
2. [Interface Overview](#interface-overview)
3. [Drawing and Writing](#drawing-and-writing)
4. [Tools Reference](#tools-reference)
5. [Math Conversion](#math-conversion)
6. [Bezier Curves](#bezier-curves)
7. [Layer Management](#layer-management)
8. [File Operations](#file-operations)
9. [Keyboard Shortcuts](#keyboard-shortcuts)
10. [Tips and Tricks](#tips-and-tricks)
11. [Troubleshooting](#troubleshooting)

## Getting Started

### System Requirements

- **Operating System**: Windows 10 (build 1809 or later) or Windows 11
- **Processor**: 64-bit processor
- **Memory**: 4 GB RAM minimum (8 GB recommended)
- **Storage**: 100 MB free space
- **Display**: 1280x720 minimum resolution
- **Input**: Mouse/trackpad (stylus/pen highly recommended)

### First Launch

1. Double-click `HermitePad.exe` to launch the application
2. The main window will open with an empty canvas
3. The Ink tool is selected by default
4. You're ready to start creating!

## Interface Overview

### Main Window Layout

```
┌─────────────────────────────────────────────────────────┐
│ [File Buttons] [Undo/Redo] [Tools] [Math] [Clear]      │ Toolbar
├─────────────────────────────────────┬──────────────────┤
│                                     │                  │
│                                     │    Layers        │
│           Canvas Area               │    Panel         │
│                                     │                  │
│                                     │                  │
├─────────────────────────────────────┴──────────────────┤
│ Status Bar: Current tool and information               │
└─────────────────────────────────────────────────────────┘
```

### Toolbar Elements

- **📂 Open**: Open existing document
- **💾 Save**: Save current document
- **📄 Export PDF**: Export to PDF format
- **↶ Undo**: Undo last action
- **↷ Redo**: Redo undone action
- **✏️ Ink**: Freehand drawing tool
- **🧹 Eraser**: Erase strokes
- **⭕ Lasso**: Select multiple strokes
- **📐 Bezier**: Create vector curves
- **🔢 Convert to Math**: Convert selected ink to math
- **🗑️ Clear**: Clear entire canvas

### Canvas Area

- Large white drawing surface
- Scrollable for large documents
- Supports zoom and pan
- Shows all ink, math, and vector content

### Layers Panel

- List of all layers
- Add/remove layer buttons
- Layer properties display
- Click to activate layer

## Drawing and Writing

### Using the Ink Tool

1. Click **✏️ Ink** button in toolbar
2. Use stylus or mouse to draw
3. Pressure sensitivity automatically applied (if supported)
4. Release to complete stroke

### Ink Properties

- **Default color**: Black
- **Default width**: 2 pixels
- **Pressure sensitivity**: Enabled
- **Curve smoothing**: Automatic

### Writing Tips

- Use natural writing motions
- Apply light pressure for thin lines
- Apply heavy pressure for thick lines
- Write smoothly for best results

## Tools Reference

### Ink Tool (✏️)

**Purpose**: Freehand drawing and writing

**Usage**:
1. Select tool from toolbar
2. Click and drag to draw
3. Release to finish stroke

**Features**:
- Pressure-sensitive (with stylus)
- Smooth curve rendering
- Real-time feedback

### Eraser Tool (🧹)

**Purpose**: Remove unwanted strokes

**Usage**:
1. Select eraser tool
2. Click on strokes to remove them
3. Each stroke removed individually

**Features**:
- Stroke-based erasing
- Instant removal
- Undo support

### Lasso Tool (⭕)

**Purpose**: Select multiple strokes

**Usage**:
1. Select lasso tool
2. Click to start selection
3. Draw around desired strokes
4. Close the lasso to select

**Features**:
- Multi-stroke selection
- Area-based selection
- Prepare for math conversion

### Bezier Tool (📐)

**Purpose**: Create precise vector curves

**Usage**:
1. Select Bezier tool
2. Click to add anchor points
3. Drag to create control handles
4. Click finish to complete path

**Features**:
- Professional-grade curves
- Anchor point editing
- Control handle manipulation
- Smooth/corner point types

## Math Conversion

### Converting Handwriting to Math

**Step-by-Step**:

1. **Draw your equation** using the Ink tool
   ```
   Example: Draw "x² + 2x + 1 = 0"
   ```

2. **Select the ink** using the Lasso tool
   - Draw a lasso around the equation
   - All strokes highlighted

3. **Click "Convert to Math"** button
   - Recognition engine analyzes strokes
   - Patterns matched to math symbols
   - LaTeX and MathML generated

4. **Review the result**
   - Dialog shows LaTeX: `x^2 + 2x + 1 = 0`
   - MathML also generated
   - Original ink replaced with math object

### Supported Math Symbols

Currently supported (simplified implementation):
- Basic operators: +, -, ×, ÷
- Fractions: a/b
- Variables: x, y, z
- Numbers: 0-9

**Note**: The current version uses pattern matching. Advanced recognition with ML models is planned for future releases.

### Working with Math Objects

- Math objects are editable
- Stored as LaTeX and MathML
- Preserved in saved documents
- Exported to PDF

## Bezier Curves

### Creating a Bezier Path

1. **Select Bezier Tool**
   - Click 📐 button

2. **Add Anchor Points**
   - Click to place first point
   - Click to add subsequent points
   - Points connected automatically

3. **Adjust Curves**
   - Drag control handles
   - Modify curve shape
   - Smooth or corner points

4. **Finish Path**
   - Double-click last point
   - Or press Escape

### Node Types

#### Smooth Nodes
- Continuous tangent through point
- Symmetrical control handles
- Creates flowing curves
- Default node type

#### Corner Nodes
- Independent control handles
- Sharp direction changes
- Useful for polygonal shapes
- Convert by right-clicking node

### Editing Bezier Paths

**Move Anchor Point**:
- Click and drag the point

**Adjust Control Handles**:
- Click and drag handle endpoints

**Change Node Type**:
- Right-click node
- Select Smooth or Corner

**Delete Node**:
- Select node
- Press Delete key

## Layer Management

### Understanding Layers

Layers organize your content:
- **Ink Layer**: Freehand strokes
- **Math Layer**: Math objects
- **Vector Layer**: Bezier paths

### Working with Layers

**Add New Layer**:
1. Click **➕ Add** in layers panel
2. New layer created
3. Becomes active layer

**Switch Layers**:
1. Click layer name in panel
2. Layer becomes active
3. New content added to this layer

**Remove Layer**:
1. Select layer in panel
2. Click **➖ Remove**
3. Layer and contents deleted

**Note**: Cannot remove last layer

## File Operations

### Saving Documents

**Manual Save**:
1. Click **💾 Save** button
2. Choose location
3. Enter filename
4. Click Save

**File Format**: .hpd (Hermite Pad Document)
- Contains all ink strokes
- Includes math objects
- Preserves Bezier paths
- Stores layer information

**Keyboard Shortcut**: Ctrl + S

### Opening Documents

1. Click **📂 Open** button
2. Browse to .hpd file
3. Select file
4. Click Open

**Keyboard Shortcut**: Ctrl + O

### Exporting to PDF

1. Click **📄 Export PDF** button
2. Choose save location
3. Enter filename
4. Click Save

**PDF Contents**:
- All visible strokes rendered
- Math expressions included
- High-resolution output
- Ready for printing/sharing

## Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| Ctrl + Z | Undo |
| Ctrl + Y | Redo |
| Ctrl + S | Save document |
| Ctrl + O | Open document |
| Ctrl + Mouse Wheel | Zoom in/out |
| Space + Drag | Pan canvas (future) |
| Delete | Delete selected |
| Escape | Cancel current operation |

## Tips and Tricks

### For Best Handwriting Recognition

1. **Write clearly** - Use standard letter forms
2. **Write larger** - Bigger strokes = better recognition
3. **One equation at a time** - Select small groups
4. **Use standard notation** - Stick to common symbols

### Stylus Usage

1. **Calibrate your device** - Use Windows stylus settings
2. **Practice pressure** - Light/heavy for line variation
3. **Palm rejection** - Enable in Windows settings
4. **Hover preview** - Supported devices show cursor

### Performance Tips

1. **Use layers** - Organize complex documents
2. **Save frequently** - Use Ctrl + S regularly
3. **Clear canvas** - Remove old content periodically
4. **Export PDF** - For archival purposes

### Creating Professional Diagrams

1. **Use Bezier tool** - For precise lines
2. **Combine with ink** - Annotations over vectors
3. **Multiple layers** - Separate diagram elements
4. **Math conversion** - For equations in diagrams

## Troubleshooting

### Stylus Not Working

**Solutions**:
1. Check Windows Ink settings
2. Update tablet/stylus drivers
3. Restart application
4. Restart Windows

### Pressure Sensitivity Issues

**Solutions**:
1. Calibrate stylus in Windows settings
2. Check device compatibility
3. Update drivers
4. Test in Windows Ink Workspace

### Math Conversion Not Accurate

**Current Limitation**: The math recognition engine in this version uses basic pattern matching.

**Workarounds**:
1. Write clearly and larger
2. Use simple equations first
3. Select smaller groups
4. Manually edit LaTeX output

**Future**: ML-based recognition coming soon

### Application Won't Start

**Solutions**:
1. Verify .NET 8.0 installed
2. Check Windows version (10/11 required)
3. Run as administrator
4. Check error logs in Event Viewer

### PDF Export Failed

**Solutions**:
1. Check disk space
2. Choose different location
3. Close other PDF viewers
4. Restart application

### Undo Not Working

**Reason**: Undo stack may be full (100 actions max)

**Solution**: No action needed, older actions automatically removed

## Getting Help

### Resources

- **GitHub Repository**: [github.com/Naman-2006/Hermite-Pad](https://github.com/Naman-2006/Hermite-Pad)
- **Issue Tracker**: Report bugs and request features
- **Documentation**: README.md and ARCHITECTURE.md

### Community

- Open issues for bugs
- Submit pull requests for features
- Share your creations

### Contact

For questions or support, please open an issue on GitHub.

---

**Happy Creating with Hermite Pad!** 🎨✏️📐
