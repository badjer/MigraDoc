#region PDFsharp - A .NET library for processing PDF

//
// Authors:
//   Stefan Lange (mailto:Stefan.Lange@pdfsharp.com)
//
// Copyright (c) 2005-2009 empira Software GmbH, Cologne (Germany)
//
// http://www.pdfsharp.com
// http://sourceforge.net/projects/pdfsharp
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

#endregion

namespace PdfSharp.Drawing
{
	/// <summary>
	///     Collects information of a font.
	/// </summary>
	public sealed class XFontMetrics
	{
		private readonly int ascent;
		private readonly int averageWidth;
		private readonly int capHeight;
		private readonly int descent;
		private readonly int leading;
		private readonly int maxWidth;
		private readonly string name;
		private readonly int stemH;
		private readonly int stemV;
		private readonly int unitsPerEm;
		private readonly int xHeight;

		internal XFontMetrics(string name, int unitsPerEm, int ascent, int descent, int leading,
		                      int capHeight, int xHeight, int stemV, int stemH, int averageWidth, int maxWidth)
		{
			this.name = name;
			this.unitsPerEm = unitsPerEm;
			this.ascent = ascent;
			this.descent = descent;
			this.leading = leading;
			this.capHeight = capHeight;
			this.xHeight = xHeight;
			this.stemV = stemV;
			this.stemH = stemH;
			this.averageWidth = averageWidth;
			this.maxWidth = maxWidth;
		}

		/// <summary>
		///     Gets the font name.
		/// </summary>
		public string Name
		{
			get { return name; }
		}

		/// <summary>
		///     Gets the ascent value.
		/// </summary>
		public int UnitsPerEm
		{
			get { return unitsPerEm; }
		}

		/// <summary>
		///     Gets the ascent value.
		/// </summary>
		public int Ascent
		{
			get { return ascent; }
		}

		/// <summary>
		///     Gets the descent value.
		/// </summary>
		public int Descent
		{
			get { return descent; }
		}

		/// <summary>
		///     Gets the average width.
		/// </summary>
		/// <value>The average width.</value>
		public int AverageWidth
		{
			get { return averageWidth; }
		}

		/// <summary>
		///     Gets the height of capital letters.
		/// </summary>
		public int CapHeight
		{
			get { return capHeight; }
		}

		/// <summary>
		///     Gets the leading value.
		/// </summary>
		public int Leading
		{
			get { return leading; }
		}

		/// <summary>
		///     Gets the maximum width of a character.
		/// </summary>
		public int MaxWidth
		{
			get { return maxWidth; }
		}

		/// <summary>
		///     Gets an internal value.
		/// </summary>
		public int StemH
		{
			get { return stemH; }
		}

		/// <summary>
		///     Gets an internal value.
		/// </summary>
		public int StemV
		{
			get { return stemV; }
		}

		/// <summary>
		///     Gets the height of a character.
		/// </summary>
		public int XHeight
		{
			get { return xHeight; }
		}
	}
}