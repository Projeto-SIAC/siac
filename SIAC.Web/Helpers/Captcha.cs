using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Helpers
{
    public class Captcha
    {
        public static string Novo()
        {
            string strBase64 = String.Empty;
            System.Drawing.Bitmap objBMP = new System.Drawing.Bitmap(100, 30);
            System.Drawing.Graphics objGraphics = System.Drawing.Graphics.FromImage(objBMP);
            objGraphics.Clear(System.Drawing.Color.White);
            objGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            System.Drawing.Font objFont = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Strikeout);
            string captchaValue = "";
            int[] valuesArray = new int[8];
            int x;
            Random autoRand = new Random();

            for (x = 0; x < 8; x++)
            {
                valuesArray[x] = Convert.ToInt32(autoRand.Next(0, 9));
                captchaValue += (valuesArray[x].ToString());
            }
            Sessao.Inserir("Captcha", captchaValue);
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            objGraphics.DrawString(captchaValue, objFont, System.Drawing.Brushes.Black, 3, 3);
            objBMP.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            strBase64 = Convert.ToBase64String(stream.ToArray());
            stream.Close();
            objFont.Dispose();
            objGraphics.Dispose();
            objBMP.Dispose();
            return strBase64;
        }
    }
}