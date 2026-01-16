using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Ink;

namespace HermitePad.Core
{
    public class Document
    {
        public StrokeCollection Strokes { get; set; } = new StrokeCollection();
        public List<MathObject> MathObjects { get; set; } = new List<MathObject>();
        public List<BezierPath> BezierPaths { get; set; } = new List<BezierPath>();
    }

    public static class DocumentSerializer
    {
        public static void Save(Document document, string filePath)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    // Save strokes using ISF format
                    using (var strokeStream = new MemoryStream())
                    {
                        document.Strokes.Save(strokeStream);
                        var strokeBytes = strokeStream.ToArray();
                        var strokeLength = strokeBytes.Length;
                        
                        using (var writer = new BinaryWriter(fs))
                        {
                            // Write version
                            writer.Write(1); // Version number
                            
                            // Write strokes
                            writer.Write(strokeLength);
                            writer.Write(strokeBytes);
                            
                            // Write math objects as JSON
                            var mathJson = JsonSerializer.Serialize(document.MathObjects);
                            writer.Write(mathJson);
                            
                            // Write bezier paths as JSON
                            var bezierJson = JsonSerializer.Serialize(document.BezierPaths);
                            writer.Write(bezierJson);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save document: {ex.Message}", ex);
            }
        }

        public static Document Load(string filePath)
        {
            try
            {
                var document = new Document();
                
                using (var fs = new FileStream(filePath, FileMode.Open))
                using (var reader = new BinaryReader(fs))
                {
                    // Read version
                    var version = reader.ReadInt32();
                    
                    // Read strokes
                    var strokeLength = reader.ReadInt32();
                    var strokeBytes = reader.ReadBytes(strokeLength);
                    document.Strokes = new StrokeCollection(new MemoryStream(strokeBytes));
                    
                    // Read math objects
                    var mathJson = reader.ReadString();
                    document.MathObjects = JsonSerializer.Deserialize<List<MathObject>>(mathJson) ?? new List<MathObject>();
                    
                    // Read bezier paths
                    var bezierJson = reader.ReadString();
                    document.BezierPaths = JsonSerializer.Deserialize<List<BezierPath>>(bezierJson) ?? new List<BezierPath>();
                }
                
                return document;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load document: {ex.Message}", ex);
            }
        }
    }
}
