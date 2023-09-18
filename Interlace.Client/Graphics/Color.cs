using JetBrains.Annotations;

namespace Interlace.Client.Graphics;

[PublicAPI]
public readonly struct Color
{
    public readonly byte R;
    public readonly byte G;
    public readonly byte B;
    public readonly byte A;

    public Color(int r, int g, int b, int a = 255)
    {
        R = (byte)r;
        G = (byte)g;
        B = (byte)b;
        A = (byte)a;
    }
}
