using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class Geodesic : MonoBehaviour
{
    public enum VisibilityDirection
    {
        Inside,
        Outside
    }
    
    private const float Radius = 1;
    public int subdivisions = 1;
    
    private bool isDirty;
    private MeshFilter meshFilter;
    public VisibilityDirection visibilityDirection;

    private void OnEnable()
    {
        meshFilter = GetComponent<MeshFilter>();
        isDirty = true;
    }

    private void Update()
    {
        if (isDirty)
        {
            Regenerate();
        }
    }

    private void OnValidate()
    {
        isDirty = true;
    }

    private void Regenerate()
    {
        var ico = new Icosahedron(Radius);
        ico.Subdivide(subdivisions);
        var mesh = new Mesh();
        int ofs = 0;
        var vertCount = ico.Triangles.Length * 3;
        var vertices = new Vector3[vertCount];
        var normals = new Vector3[vertCount];
        var triangles = new int[vertCount];
        var colors = new Color[vertCount];
        foreach (var triangle in ico.Triangles)
        {
            var vertexDirection = visibilityDirection == VisibilityDirection.Outside
                ? VertexDirection.Clockwise
                : VertexDirection.Anticlockwise;
            
            var triangleVerts = triangle.GetVertices(vertexDirection);
            triangleVerts.CopyTo(vertices, ofs);
            triangles[ofs] = ofs;
            triangles[ofs + 1] = ofs + 1;
            triangles[ofs + 2] = ofs + 2;
            normals[ofs] = triangle.PlaneNormal;
            normals[ofs + 1] = triangle.PlaneNormal;
            normals[ofs + 2] = triangle.PlaneNormal;
            
            // add barycentric coordinates in color data
            colors[ofs] = new Color(1, 0, 0);
            colors[ofs + 1] = new Color(0, 1, 0);
            colors[ofs + 2] = new Color(0, 0, 1);
            ofs += 3;
        }

        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.normals = normals;
        mesh.triangles = triangles;
        meshFilter.mesh = mesh;

        isDirty = false;
    }
}
