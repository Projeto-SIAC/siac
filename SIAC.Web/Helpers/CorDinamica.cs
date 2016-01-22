using System;

namespace SIAC.Helpers
{
    public class CorDinamica
    {
        public static string Rgba(Random random, float opacidade = 1)
        {
            string rgba = String.Empty;
            int r = random.Next(256);
            int g = random.Next(256);
            int b = random.Next(256);
            rgba = $"rgba({r},{g},{b},{opacidade})";
            return rgba;
        }
    }
}