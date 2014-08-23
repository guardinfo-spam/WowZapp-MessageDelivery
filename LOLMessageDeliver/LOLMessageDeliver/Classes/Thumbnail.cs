using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ReliantWellnessCenter.Helpers
{
	public class Thumbnail
	{
		public static void Generate(int thumbWidth, Stream inputStream, Stream outputStream)
		{
			using (Image image = Image.FromStream(inputStream))
			{
				float srcWidth = image.Width;
				float srcHeight = image.Height;
				int thumbHeight = (int)((srcHeight/srcWidth)*thumbWidth);
				using (var bmp = new Bitmap(thumbWidth, thumbHeight))
				{
					Graphics gr = Graphics.FromImage(bmp);
					gr.SmoothingMode = SmoothingMode.HighQuality;
					gr.CompositingQuality = CompositingQuality.HighQuality;
					gr.InterpolationMode = InterpolationMode.High;
					gr.PixelOffsetMode = PixelOffsetMode.HighQuality;

					var rectDestination = new Rectangle(0, 0, thumbWidth, thumbHeight);
					gr.DrawImage(image, rectDestination, 0, 0, (int)srcWidth, (int)srcHeight, GraphicsUnit.Pixel);

					bmp.Save(outputStream, ImageFormat.Jpeg);
				}
			}
		}

		public static byte[] Generate(int thumbWidth, byte[] source)
		{
			using(var inputStream = new MemoryStream(source))
			using(var outputStream = new MemoryStream())
			{
				Generate(thumbWidth, inputStream, outputStream);
				return outputStream.GetBuffer();
			}			
		}
	}
}