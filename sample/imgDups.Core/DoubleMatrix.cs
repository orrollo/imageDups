using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imgDups.Core
{
	public class DoubleMatrix : ICloneable
	{
		protected readonly double[,] vals;

		public int Rows { get; protected set; }
		public int Cols { get; protected set; }

		public double this[int row, int col]
		{
			get { return vals[row, col]; }
			set { vals[row, col] = value; }
		}

		public DoubleMatrix(int rows, int cols)
		{
			Rows = rows;
			Cols = cols;
			vals = new double[rows,cols];
		}

		public DoubleMatrix(double[,] src)
		{
			Rows = src.GetLength(0);
			Cols = src.GetLength(1);
			vals = new double[Rows, Cols];
			for (int row = 0; row < Rows; row++)
				for (int col = 0; col < Cols; col++)
					vals[row, col] = src[row, col];
		}

		public DoubleMatrix(int[,] src)
		{
			Rows = src.GetLength(0);
			Cols = src.GetLength(1);
			vals = new double[Rows, Cols];
			for (int row = 0; row < Rows; row++)
				for (int col = 0; col < Cols; col++)
					vals[row, col] = src[row, col];
		}

		public double[,] AsArray()
		{
			var ret = new double[Rows, Cols];
			for (int row = 0; row < Rows; row++)
				for (int col = 0; col < Cols; col++)
					ret[row, col] = vals[row, col];
			return ret;
		}

		public static DoubleMatrix operator *(DoubleMatrix a, DoubleMatrix b)
		{
			if (a==null || b==null || a.Cols != b.Rows)
				throw new ArgumentException("bad matricies");

			var ret = new DoubleMatrix(a.Rows, b.Cols);
			for (int row = 0; row < ret.Rows; row++)
				for (int col = 0; col < ret.Cols; col++)
				{
					double sum = 0.0;
					for (int t = 0; t < a.Cols; t++) sum += a[row, t]*b[t, col];
					ret[row, col] = sum;
				}
			return ret;
		}

		public static DoubleMatrix operator +(DoubleMatrix a, DoubleMatrix b)
		{
			if (a == null || b == null || a.Cols != b.Cols || a.Rows != b.Rows)
				throw new ArgumentException("bad matricies");
			var ret = new DoubleMatrix(a.Rows, a.Cols);
			for (int row = 0; row < ret.Rows; row++)
				for (int col = 0; col < ret.Cols; col++)
					ret[row, col] = a[row, col] + b[row, col];
			return ret;
		}

		public static DoubleMatrix operator -(DoubleMatrix a)
		{
			if (a == null) throw new ArgumentException("bad matrix");
			var ret = new DoubleMatrix(a.Rows, a.Cols);
			for (int row = 0; row < ret.Rows; row++)
				for (int col = 0; col < ret.Cols; col++)
					ret[row, col] = -a[row, col];
			return ret;
		}

		public static DoubleMatrix operator -(DoubleMatrix a, DoubleMatrix b)
		{
			if (a == null || b == null || a.Cols != b.Cols || a.Rows != b.Rows)
				throw new ArgumentException("bad matricies");
			var ret = new DoubleMatrix(a.Rows, a.Cols);
			for (int row = 0; row < ret.Rows; row++)
				for (int col = 0; col < ret.Cols; col++)
					ret[row, col] = a[row, col] - b[row, col];
			return ret;
		}

		public static DoubleMatrix operator ~(DoubleMatrix a)
		{
			if (a==null) throw new ArgumentException("bad matrix");
			var ret = new DoubleMatrix(a.Cols, a.Rows);
			for (int row = 0; row < ret.Rows; row++)
				for (int col = 0; col < ret.Cols; col++)
					ret[row, col] = a[col, row];
			return ret;
		}

		public object Clone()
		{
			return new DoubleMatrix(this.vals);
		}
	}
}
