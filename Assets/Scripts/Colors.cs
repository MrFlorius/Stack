using UnityEngine;

public static class Colors
{
    public static Color Color1 = new Color(Random.Range(0, 1f),
        Random.Range(0, 1f), Random.Range(0, 1f));

    public static Color Color2 = new Color(Random.Range(0, 1f),
        Random.Range(0, 1f), Random.Range(0, 1f));

    private static int _idx;

    public static Color GetColor()
    {
        _idx += 1;
        if (_idx > 15) UpdateColors();
        return Color.Lerp(Color1, Color2, Mathf.Clamp(_idx / 15f, 0, 1));
    }

    public static void UpdateColors()
    {
        _idx = 0;
        Color1 = new Color(Random.Range(0, 1f),
            Random.Range(0, 1f), Random.Range(0, 1f));
        Color2 = new Color(Random.Range(0, 1f),
            Random.Range(0, 1f), Random.Range(0, 1f));
    }
}