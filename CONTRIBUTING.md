# Contributing to Hermite Pad

Thank you for your interest in contributing to Hermite Pad! This document provides guidelines and instructions for contributing.

## Table of Contents

1. [Code of Conduct](#code-of-conduct)
2. [How Can I Contribute?](#how-can-i-contribute)
3. [Development Setup](#development-setup)
4. [Coding Standards](#coding-standards)
5. [Commit Guidelines](#commit-guidelines)
6. [Pull Request Process](#pull-request-process)
7. [Bug Reports](#bug-reports)
8. [Feature Requests](#feature-requests)

## Code of Conduct

### Our Pledge

We are committed to providing a welcoming and inclusive environment for all contributors.

### Expected Behavior

- Be respectful and considerate
- Welcome newcomers and help them learn
- Focus on constructive feedback
- Accept responsibility for mistakes

### Unacceptable Behavior

- Harassment or discrimination
- Trolling or insulting comments
- Personal attacks
- Publishing private information

## How Can I Contribute?

### Reporting Bugs

Found a bug? Please create an issue with:
- Clear, descriptive title
- Steps to reproduce
- Expected vs. actual behavior
- System information (OS, .NET version)
- Screenshots if applicable

### Suggesting Features

Have an idea? Open an issue with:
- Clear description of the feature
- Use cases and benefits
- Possible implementation approach
- Mockups or examples if relevant

### Code Contributions

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

### Documentation

- Fix typos and errors
- Improve clarity
- Add examples
- Translate to other languages

## Development Setup

### Prerequisites

1. **Windows 10/11** (required for WPF development)
2. **.NET 8.0 SDK** or later
3. **Visual Studio 2022** (recommended) or VS Code
4. **Git** for version control

### Getting Started

1. **Fork and clone**:
   ```bash
   git clone https://github.com/YOUR-USERNAME/Hermite-Pad.git
   cd Hermite-Pad
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Build the project**:
   ```bash
   dotnet build
   ```

4. **Run the application**:
   ```bash
   dotnet run --project HermitePad/HermitePad.csproj
   ```

### Project Structure

```
Hermite-Pad/
├── HermitePad/           # Main application
│   ├── Core/             # Core functionality
│   ├── Tools/            # Tool implementations
│   ├── Themes/           # UI themes
│   ├── *.xaml            # UI definitions
│   └── *.cs              # Code-behind
├── README.md
├── ARCHITECTURE.md
├── USER_GUIDE.md
└── CONTRIBUTING.md
```

## Coding Standards

### C# Style

Follow Microsoft's C# coding conventions:

#### Naming

```csharp
// Classes and methods: PascalCase
public class CanvasManager { }
public void SaveDocument() { }

// Private fields: _camelCase with underscore
private InkCanvas _inkCanvas;

// Parameters and locals: camelCase
public void AddLayer(string layerName) { }

// Constants: PascalCase
private const int MaxStackSize = 100;
```

#### Formatting

```csharp
// Braces on new line
public void Method()
{
    if (condition)
    {
        // Code
    }
}

// Single line if statements - use braces
if (condition)
{
    return;
}
```

#### Comments

```csharp
// Single line comments for brief explanations
private int _count; // Number of items

/// <summary>
/// XML comments for public APIs
/// </summary>
/// <param name="value">Description</param>
public void PublicMethod(int value)
{
    // Implementation
}
```

### XAML Style

```xaml
<!-- Clear hierarchy with indentation -->
<Window>
    <Grid>
        <Button Content="Click" 
                Padding="10,5" 
                Margin="5,0" />
    </Grid>
</Window>

<!-- Use meaningful names -->
<Button x:Name="SaveButton" Content="Save" />

<!-- Resource references -->
<Button Style="{StaticResource ToolbarButtonStyle}" />
```

### Code Quality

- **Keep it simple**: Prefer clarity over cleverness
- **Single responsibility**: Each class/method does one thing
- **Error handling**: Use try-catch appropriately
- **Null checks**: Use null-conditional operators
- **LINQ**: Use for collection operations
- **Async/await**: For I/O operations

### Example Good Code

```csharp
public class CanvasManager
{
    private readonly InkCanvas _inkCanvas;
    private readonly List<Layer> _layers;

    public CanvasManager(InkCanvas inkCanvas)
    {
        _inkCanvas = inkCanvas ?? throw new ArgumentNullException(nameof(inkCanvas));
        _layers = new List<Layer>();
    }

    public void Zoom(double factor, Point center)
    {
        if (factor <= 0)
            throw new ArgumentException("Zoom factor must be positive", nameof(factor));

        // Implementation
    }
}
```

## Commit Guidelines

### Commit Message Format

```
<type>: <subject>

<body>

<footer>
```

### Types

- **feat**: New feature
- **fix**: Bug fix
- **docs**: Documentation changes
- **style**: Code style changes (formatting)
- **refactor**: Code refactoring
- **test**: Adding tests
- **chore**: Build process or tooling changes

### Examples

```
feat: Add pressure sensitivity support

Implemented pressure-sensitive ink rendering using StylusPoint data.
Strokes now vary in thickness based on stylus pressure.

Closes #123
```

```
fix: Resolve PDF export crash on empty canvas

Added null check before rendering canvas to PDF.
Empty canvases now export successfully.

Fixes #456
```

### Best Practices

- Use present tense ("Add feature" not "Added feature")
- Keep first line under 50 characters
- Provide detailed description in body
- Reference issues and PRs

## Pull Request Process

### Before Submitting

1. **Test your changes**
   - Build successfully
   - Run the application
   - Test all affected features
   - Check for regressions

2. **Update documentation**
   - Update README if needed
   - Add inline comments
   - Update user guide for features

3. **Follow coding standards**
   - Run code formatter
   - Check for warnings
   - Review your own code

### PR Template

```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Tested locally
- [ ] Added/updated tests
- [ ] All tests passing

## Screenshots
(if applicable)

## Related Issues
Closes #XXX
```

### Review Process

1. Maintainers will review your PR
2. Address any feedback
3. Keep PR updated with main branch
4. Once approved, PR will be merged

### After Merge

- Your contribution will be credited
- Appears in next release notes
- You become a contributor!

## Bug Reports

### Good Bug Report Template

```markdown
**Describe the bug**
Clear description of the bug

**To Reproduce**
Steps to reproduce:
1. Open application
2. Click on '...'
3. See error

**Expected behavior**
What should happen

**Actual behavior**
What actually happens

**Screenshots**
(if applicable)

**Environment**
- OS: Windows 11
- .NET Version: 8.0
- App Version: 1.0.0

**Additional context**
Any other relevant information
```

## Feature Requests

### Good Feature Request Template

```markdown
**Feature description**
Clear description of proposed feature

**Problem it solves**
What user need does this address?

**Proposed solution**
How should it work?

**Alternatives considered**
Other approaches you've thought about

**Additional context**
Mockups, examples, etc.
```

## Questions?

If you have questions about contributing:
1. Check existing issues
2. Read the documentation
3. Open a discussion on GitHub
4. Ask in issue comments

## Recognition

Contributors will be:
- Listed in CONTRIBUTORS.md
- Mentioned in release notes
- Credited in commits

Thank you for contributing to Hermite Pad! 🎉
