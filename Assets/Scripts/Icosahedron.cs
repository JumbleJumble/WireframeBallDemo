using UnityEngine;

public class Icosahedron : Tetrahedron
{
    private readonly float _radius;

    private Vector3 _northPole;
    private Vector3 _southPole;
    private Vector3[] _northTropic;
    private Vector3[] _southTropic;

    public Icosahedron(float radius)
    {
        _radius = radius;
        GenerateIcosahedronVertices();
        GenerateTriangles();
    }

    private void GenerateIcosahedronVertices()
    {
        _northPole = Vector3.up * _radius;
        _southPole = -Vector3.up * _radius;

        var arctanHalf = Mathf.Atan(0.5f) * Mathf.Rad2Deg;
        _northTropic = FivePoints(-arctanHalf, 0);
        _southTropic = FivePoints(arctanHalf, 36);

        Vector3[] FivePoints(float latitude, float offsetAngle)
        {
            var result = new Vector3[5];
            for (int r = 0; r < 5; r++)
            {
                float longDeg = offsetAngle + r * (360 / 5);
                var direction = Quaternion.Euler(latitude, longDeg, 0);
                result[r] = (direction * Vector3.forward) * _radius;
            }

            return result;
        }
    }

    private void GenerateTriangles()
    {
        Triangles = new TriangleFace[20];

        // add north 5 triangles
        for (int i = 0; i < 5; i++)
        {
            Triangles[i] = new TriangleFace(
                _northPole,
                _northTropic[i],
                _northTropic[(i + 1) % 5]);
        }

        // add south 5 triangles
        for (int i = 0; i < 5; i++)
        {
            Triangles[5 + i] = new TriangleFace(
                _southPole,
                _southTropic[i],
                _southTropic[(i + 1) % 5]);
        }

        // add 5 triangles with tips on north tropic
        for (int i = 0; i < 5; i++)
        {
            Triangles[10 + i] = new TriangleFace(
                _northTropic[i],
                _southTropic[(i + 4) % 5],
                _southTropic[i]);
        }

        // add 5 triangles with tips on south tropic
        for (int i = 0; i < 5; i++)
        {
            Triangles[15 + i] = new TriangleFace(
                _southTropic[i],
                _northTropic[i],
                _northTropic[(i + 1) % 5]);
        }
    }
}
