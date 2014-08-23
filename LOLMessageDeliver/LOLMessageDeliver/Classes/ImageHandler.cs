using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;

namespace LOLMessageDelivery
{
    static public class ImageHandler
    {

        static public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            if (imageIn != null)
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            }
            return ms.ToArray();
        }

        static public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = null;
            try
            {
                returnImage = Image.FromStream(ms);
            }
            catch { }

            return returnImage;
        }

    }
}