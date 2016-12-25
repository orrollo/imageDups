using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace imgDups.Core
{
	public class DCT
	{
		protected static Dictionary<int,DoubleMatrix> coefs = new Dictionary<int, DoubleMatrix>();
		protected static Dictionary<int, DoubleMatrix> tcoefs = new Dictionary<int, DoubleMatrix>();

		public int Size { get; protected set; }

		public DCT(int size)
		{
			if (!coefs.ContainsKey(size))
			{
				lock (coefs)
				{
					if (!coefs.ContainsKey(size))
					{
						var matrix = BuildCoefs(size);
						coefs.Add(size, matrix);
						tcoefs.Add(size, ~matrix);
					}
				}
			}
			Size = size;
		}

		private static DoubleMatrix BuildCoefs(int size)
		{
			var ret = new DoubleMatrix(size, size);
			//
			double k1 = Math.Sqrt(1.0/size), k2 = Math.Sqrt(2.0/size);
			for (int row = 0; row < ret.Rows; row++)
				for (int col = 0; col < ret.Cols; col++)
					ret[row, col] = row == 0 ? k1 : k2*Math.Cos(Math.PI*(2*col + 1)*row/(2*size));
			//
			return ret;
		}

		public DoubleMatrix[] Process(Bitmap src)
		{
			if (src.Width != Size || src.Height != Size) 
				throw new ArgumentException("bad bitmap size");
			var rgb = new DoubleMatrix[3];
			for (int i = 0; i < 3; i++) rgb[i] = new DoubleMatrix(Size, Size);
				for (int row = 0; row < Size; row++)
					for (int col = 0; col < Size; col++)
					{
						var color = src.GetPixel(col, row);
						rgb[0][row, col] = color.R;
						rgb[1][row, col] = color.G;
						rgb[2][row, col] = color.B;
					}
			// calculate
			DoubleMatrix c1 = coefs[Size], c2 = tcoefs[Size];
			for (int i = 0; i < 3; i++) rgb[i] = (c1*rgb[i])*c2;
			return rgb;
		}

		public Bitmap UnProcess(DoubleMatrix[] src)
		{
			if (src == null || src.Length !=3) throw new ArgumentException("bad matrix array");
			for (int i = 0; i < 3; i++) if (src[i].Cols != Size || src[i].Rows != Size) 
				throw new ArgumentException("bad matrix in array");
			var idct = new DoubleMatrix[3];
			DoubleMatrix c1 = coefs[Size], c2 = tcoefs[Size];
			for (int i = 0; i < 3; i++) idct[i] = (c2*src[i])*c1;
			return MatrixesToBitmap(idct);
		}

		public Bitmap MatrixesToBitmap(DoubleMatrix[] idct)
		{
			var ret = new Bitmap(Size, Size);
			for (int row = 0; row < Size; row++)
				for (int col = 0; col < Size; col++)
				{
					int r = GetIDCTColor(idct[0][row, col]);
					int g = GetIDCTColor(idct[1][row, col]);
					int b = GetIDCTColor(idct[2][row, col]);
					ret.SetPixel(col, row, Color.FromArgb(r, g, b));
				}
			return ret;
		}

		public DoubleMatrix[] NormalizeMatrixes(DoubleMatrix[] src)
		{
			var ret = new DoubleMatrix[src.Length];
			for (int i = 0; i < src.Length; i++)
			{
				
			}
			return ret;
		}

		private static int GetIDCTColor(double value)
		{
			var r = (int) Math.Round(value);
			if (r < 0) r = 0;
			if (r > 255) r = 255;
			return r;
		}
	}
}
