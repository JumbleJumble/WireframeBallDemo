using UnityEngine;

public enum VertexDirection
{
    Clockwise,
    Anticlockwise
}

public class TriangleFace
{
    private readonly Vector3[] _vertices;
    public Vector3 PlaneNormal { get; }

    public TriangleFace(Vector3 a, Vector3 b, Vector3 c)
    {
        _vertices = new[] { a, b, c };
        PlaneNormal = ((a + b + c) / 3f).normalized;
    }

    public Vector3[] GetVertices(VertexDirection direction)
    {
        var cross = Vector3.Cross(_vertices[1] - _vertices[0], _vertices[2] - _vertices[0]).normalized;
        var dot = Vector3.Dot(cross, PlaneNormal);
        var add = (int)Mathf.Sign(dot) * (direction == VertexDirection.Clockwise ? -1 : 1);
        return new[] { _vertices[1 + add], _vertices[1], _vertices[1 - add] };
    }
}
