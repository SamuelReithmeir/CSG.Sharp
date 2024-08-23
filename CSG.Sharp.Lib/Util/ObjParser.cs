using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CSG.Sharp.Util;

public static class ObjParser
{
    private static readonly char[] Separator = [' '];

    public static CSG FromObjString(string objContent)
    {
        var vertices = new List<Vertex>();
        var polygons = new List<Polygon>();

        var lines = objContent.Split('\n');
        foreach (var line in lines)
        {
            var parts = line.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            switch (parts[0])
            {
                case "v":
                    var x = double.Parse(parts[1], CultureInfo.InvariantCulture);
                    var y = double.Parse(parts[2], CultureInfo.InvariantCulture);
                    var z = double.Parse(parts[3], CultureInfo.InvariantCulture);
                    vertices.Add(new Vertex(new Vector(x, y, z), new Vector(0, 0, 0)));
                    break;

                case "f":
                    var faceVertices = parts.Skip(1).Select(p => vertices[int.Parse(p.Split('/')[0]) - 1])
                        .ToArray();
                    polygons.Add(new Polygon(faceVertices));
                    break;
            }
        }

        return CSG.FromPolygons(polygons.ToArray());
    }

    public static string WavefrontObjFromPolygons(Polygon[] polygons)
    {
        var sb = new StringBuilder();
        var vertices = new List<Vertex>();
        var vertexIndexMap = new Dictionary<Vertex, int>();

        // Collect unique vertices and map them to indices
        foreach (var polygon in polygons)
        {
            foreach (var vertex in polygon.Vertices)
            {
                if (!vertexIndexMap.ContainsKey(vertex))
                {
                    vertexIndexMap[vertex] = vertices.Count + 1; // 1-based index
                    vertices.Add(vertex);
                }
            }
        }
        
        
        // Write vertices to the StringBuilder
        foreach (var vertex in vertices)
        {
            sb.AppendLine(
                $"v {vertex.Pos.x.ToString(CultureInfo.InvariantCulture)} {vertex.Pos.y.ToString(CultureInfo.InvariantCulture)} {vertex.Pos.z.ToString(CultureInfo.InvariantCulture)}");
        }

        // Write faces to the StringBuilder
        foreach (var polygon in polygons)
        {
            var faceIndices = polygon.Vertices.Select(v => vertexIndexMap[v].ToString()).ToArray();
            sb.AppendLine($"f {string.Join(" ", faceIndices)}");
        }

        return sb.ToString();
    }
}