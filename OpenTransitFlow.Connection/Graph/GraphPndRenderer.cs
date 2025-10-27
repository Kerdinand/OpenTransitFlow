// NuGet: QuikGraph, SkiaSharp
using QuikGraph;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace OpenTransitFlow.Connection.Graph
{
    public static class GraphPngRenderer
    {
        public static void Render(
            BidirectionalGraph<NetworkGraphVertex, NetworkGraphEdge> graph,
            string outputPath,
            int width = 1000,
            int height = 800)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(outputPath))!);

            using var surface = SKSurface.Create(new SKImageInfo(width, height));
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.White);

            var vertices = graph.Vertices.ToList();
            int n = vertices.Count;
            if (n == 0)
            {
                Save(surface, outputPath);
                return;
            }

            float margin = 60f;
            float radius = MathF.Min(width, height) * 0.5f - margin;
            var center = new SKPoint(width / 2f, height / 2f);

            // circular layout
            var pos = new Dictionary<NetworkGraphVertex, SKPoint>(n);
            for (int i = 0; i < n; i++)
            {
                float angle = (float)(2 * Math.PI * i / n);
                pos[vertices[i]] = new SKPoint(
                    center.X + radius * MathF.Cos(angle),
                    center.Y + radius * MathF.Sin(angle));
            }

            using var edgePen = new SKPaint { IsAntialias = true, Color = SKColors.Gray, StrokeWidth = 2, Style = SKPaintStyle.Stroke };
            using var vertFill = new SKPaint { IsAntialias = true, Color = SKColors.SteelBlue, Style = SKPaintStyle.Fill };
            using var vertOutline = new SKPaint { IsAntialias = true, Color = SKColors.Black, StrokeWidth = 1, Style = SKPaintStyle.Stroke };
            using var textPaint = new SKPaint { IsAntialias = true, Color = SKColors.Black, TextSize = 16 };

            float nodeR = 18f;

            // draw edges (with small arrowheads to indicate direction)
            foreach (var edge in graph.Edges)
            {
                var e = (IEdge<NetworkGraphVertex>)edge;
                var a = pos[e.Source];
                var b = pos[e.Target];

                if (e.Source == e.Target)
                {
                    // self-loop: small arc near the node
                    var loopRect = new SKRect(a.X - 2 * nodeR, a.Y - 2 * nodeR, a.X - nodeR, a.Y - nodeR);
                    canvas.DrawArc(loopRect, 20, 300, false, edgePen);
                    continue;
                }

                // shorten line so it touches node borders
                var ab = new SKPoint(b.X - a.X, b.Y - a.Y);
                var len = MathF.Sqrt(ab.X * ab.X + ab.Y * ab.Y);
                if (len < 1e-3f) continue;
                var ux = ab.X / len;
                var uy = ab.Y / len;
                var from = new SKPoint(a.X + ux * nodeR, a.Y + uy * nodeR);
                var to = new SKPoint(b.X - ux * nodeR, b.Y - uy * nodeR);

                canvas.DrawLine(from, to, edgePen);

                // arrowhead at 'to'
                DrawArrowhead(canvas, to, new SKPoint(ux, uy), 8, 10, edgePen);
            }

            // draw vertices + labels
            foreach (var v in vertices)
            {
                var p = pos[v];
                canvas.DrawCircle(p, nodeR, vertFill);
                canvas.DrawCircle(p, nodeR, vertOutline);

                var label = v.uuid ?? "";
                var tw = textPaint.MeasureText(label);
                canvas.DrawText(label, p.X - tw / 2f, p.Y + nodeR + 16, textPaint);
            }

            Save(surface, outputPath);
        }

        private static void DrawArrowhead(SKCanvas c, SKPoint tip, SKPoint dirUnit, float wing = 8f, float length = 10f, SKPaint pen = null!)
        {
            // rotate dir by ±θ to get wings
            float nx = -dirUnit.Y, ny = dirUnit.X; // perpendicular
            var basePt = new SKPoint(tip.X - dirUnit.X * length, tip.Y - dirUnit.Y * length);
            var left = new SKPoint(basePt.X + nx * wing, basePt.Y + ny * wing);
            var right = new SKPoint(basePt.X - nx * wing, basePt.Y - ny * wing);
            c.DrawLine(tip, left, pen);
            c.DrawLine(tip, right, pen);
        }

        private static void Save(SKSurface surface, string path)
        {
            using var img = surface.Snapshot();
            using var data = img.Encode(SKEncodedImageFormat.Png, 100);
            using var fs = File.Open(path, FileMode.Create, FileAccess.Write);
            data.SaveTo(fs);
        }
    }
}