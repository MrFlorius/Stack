using System.Linq;
using UnityEngine;

public class Gradient : MonoBehaviour
{

    private Color[] _colors;

    private MeshFilter _meshFilter;

    public float ColorChangeSpeed;

    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _colors = new Color[_meshFilter.mesh.vertices.Length];
        _colors[0] = Colors.Color1;
        _colors[2] = Colors.Color2;
        _colors[1] = Colors.Color1;
        _colors[3] = Colors.Color2;
    }

    private void Update()
    {
        var mesh = _meshFilter.mesh;
        _colors[0] = Color.Lerp(_colors[0], Colors.Color1, ColorChangeSpeed * Time.deltaTime);
        _colors[1] = Color.Lerp(_colors[1], Colors.Color1, ColorChangeSpeed * Time.deltaTime);
        _colors[2] = Color.Lerp(_colors[2], Colors.Color2, ColorChangeSpeed * Time.deltaTime);
        _colors[3] = Color.Lerp(_colors[3], Colors.Color2, ColorChangeSpeed * Time.deltaTime);
        mesh.colors = _colors;
    }
}