namespace CommonLib;

public static class IntExtensions
{
    public static int Mod(this int value, int modulus)
    {
        int r = value % modulus;
        return r < 0 ? r + modulus : r;
    }
}
