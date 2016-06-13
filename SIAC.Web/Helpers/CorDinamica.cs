using System;

namespace SIAC.Helpers
{
    public class CorDinamica
    {
        public static string Rgba(float opacidade = 1)
        {
            string rgba = String.Empty;
            int r = Models.Sistema.Random.Next(256);
            int g = Models.Sistema.Random.Next(256);
            int b = Models.Sistema.Random.Next(256);
            rgba = $"rgba({r},{g},{b},{opacidade})";
            return rgba;
        }
    }
}