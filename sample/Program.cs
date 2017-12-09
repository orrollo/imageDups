using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using imgDups.Core;
using img = imgDups.Core;

namespace sample
{
	class Program
	{
		public static Bitmap ResizeImage(Image image, int width, int height)
		{
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}

		static void Main(string[] args)
		{
			var size = 128;
			var dct = new DCT(size);

            var src = new Bitmap("20171023214635.jpg");
			var tmp = ResizeImage(src, size, size);

			tmp.Save("v000.jpg", ImageFormat.Jpeg);

			DoubleMatrix[] data = dct.Process(tmp);

			var ret = dct.UnProcess(data);
			ret.Save("v001.jpg", ImageFormat.Jpeg);

			var simple = new DoubleMatrix[3];
			for (int i = 0; i < 3; i++)
			{
				simple[i] = (DoubleMatrix)data[i].Clone();
				ClearDCT(simple[i], 16);
			}

		    var vecs = simple.Select(x => x.ToVector(0, 0, 16, 16)).ToArray();


            var smp = dct.UnProcess(simple);
            smp.Save("v002.jpg", ImageFormat.Jpeg);
		}

		private static void ClearDCT(DoubleMatrix src, int start)
		{
			for (int row = 0; row < src.Rows; row++)
			{
				for (int col = 0; col < src.Cols; col++)
				{
					if (row < start || col < start) continue;
					src[row, col] = 0.0;
				}
			}
		}
	}
}
