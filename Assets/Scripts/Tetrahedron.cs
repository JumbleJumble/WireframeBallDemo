using System.Collections.Generic;
using UnityEngine;

public abstract class Tetrahedron
{
    public TriangleFace[] Triangles { get; protected set; }

    public void Subdivide(int n)
    {
        var newTriangles = new List<TriangleFace>(Triangles.Length * n * n);
        foreach (var triangle in Triangles)
        {
            newTriangles.AddRange(SubdivideFace(triangle, n));
        }
        
        Triangles = newTriangles.ToArray();
    }

    private IEnumerable<TriangleFace> SubdivideFace(TriangleFace triangle, int n)
    {
        var result = new List<TriangleFace>(n * n);
        int rowTris = 1;
        var verts = triangle.GetVertices(VertexDirection.Clockwise);
        var prev = verts[0];
        var sphereRadius = prev.magnitude;
        var trileft = (verts[1] - verts[0]) / n;
        var triright = (verts[2] - verts[0]) / n;
        var tribase = (verts[2] - verts[1]) / n;
        for (int row = 0; row < n; row++)
        {
            for (int x = 0; x < rowTris; x++)
            {
                if (x > 0)
                {
                    var udorig = prev + tribase * (x - 1);
                    AddTriangle(udorig, udorig + tribase, udorig + triright);
                }

                var start = prev + tribase * x;
                AddTriangle(start, start + trileft, start + triright);
            }

            rowTris += 1;
            prev += trileft;
        }

        void AddTriangle(Vector3 a, Vector3 b, Vector3 c)
        {
            result.Add(new TriangleFace(Stretch(a), Stretch(b), Stretch(c)));
        }

        Vector3 Stretch(Vector3 v) => v.normalized * sphereRadius;

        return result;
    }

}
