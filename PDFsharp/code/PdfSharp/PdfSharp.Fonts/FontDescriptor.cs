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

using PdfSharp.Drawing;

namespace PdfSharp.Fonts
{
	/// <summary>
	///     Base class for all font descriptors.
	/// </summary>
	internal class FontDescriptor
	{
		protected int ascender;
		protected int capHeight;
		protected int descender;
		protected string encodingScheme;
		protected string familyName;
		protected int flags;
		protected string fontFile;
		private XFontMetrics fontMetrics;
		protected string fontName;

		protected string fontType;
		protected string fullName;
		protected bool isFixedPitch;
		protected float italicAngle;
		protected int leading;
		protected int stemV;
		protected int strikeoutPosition;
		protected int strikeoutSize;
		protected int underlinePosition;
		protected int underlineThickness;
		protected int unitsPerEm;
		protected string version;
		protected string weight;
		protected int xHeight;
		protected int xMax;
		protected int xMin;
		protected int yMax;
		protected int yMin;

		/// <summary>
		/// </summary>
		public string FontFile
		{
			get { return fontFile; }
		}

		/// <summary>
		/// </summary>
		public string FontType
		{
			get { return fontType; }
		}

		/// <summary>
		/// </summary>
		public string FontName
		{
			get { return fontName; }
		}

		/// <summary>
		/// </summary>
		public string FullName
		{
			get { return fullName; }
		}

		/// <summary>
		/// </summary>
		public string FamilyName
		{
			get { return familyName; }
		}

		/// <summary>
		/// </summary>
		public string Weight
		{
			get { return weight; }
		}

		/// <summary>
		///     Gets a value indicating whether this instance belongs to a bold font.
		/// </summary>
		public virtual bool IsBoldFace
		{
			get { return false; }
		}

		/// <summary>
		/// </summary>
		public float ItalicAngle
		{
			get { return italicAngle; }
		}

		/// <summary>
		///     Gets a value indicating whether this instance belongs to an italic font.
		/// </summary>
		public virtual bool IsItalicFace
		{
			get { return false; }
		}

		/// <summary>
		/// </summary>
		public int XMin
		{
			get { return xMin; }
		}

		/// <summary>
		/// </summary>
		public int YMin
		{
			get { return yMin; }
		}

		/// <summary>
		/// </summary>
		public int XMax
		{
			get { return xMax; }
		}

		/// <summary>
		/// </summary>
		public int YMax
		{
			get { return yMax; }
		}

		/// <summary>
		/// </summary>
		public bool IsFixedPitch
		{
			get { return isFixedPitch; }
		}

		//Rect FontBBox;

		/// <summary>
		/// </summary>
		public int UnderlinePosition
		{
			get { return underlinePosition; }
		}

		/// <summary>
		/// </summary>
		public int UnderlineThickness
		{
			get { return underlineThickness; }
		}

		/// <summary>
		/// </summary>
		public int StrikeoutPosition
		{
			get { return strikeoutPosition; }
		}

		/// <summary>
		/// </summary>
		public int StrikeoutSize
		{
			get { return strikeoutSize; }
		}

		/// <summary>
		/// </summary>
		public string Version
		{
			get { return version; }
		}

		///// <summary>
		///// 
		///// </summary>
		//public string Notice
		//{
		//  get { return this.Notice; }
		//}
		//protected string notice;

		/// <summary>
		/// </summary>
		public string EncodingScheme
		{
			get { return encodingScheme; }
		}

		/// <summary>
		/// </summary>
		public int UnitsPerEm
		{
			get { return unitsPerEm; }
		}

		/// <summary>
		/// </summary>
		public int CapHeight
		{
			get { return capHeight; }
		}

		/// <summary>
		/// </summary>
		public int XHeight
		{
			get { return xHeight; }
		}

		/// <summary>
		/// </summary>
		public int Ascender
		{
			get { return ascender; }
		}

		/// <summary>
		/// </summary>
		public int Descender
		{
			get { return descender; }
		}

		/// <summary>
		/// </summary>
		public int Leading
		{
			get { return leading; }
		}

		/// <summary>
		/// </summary>
		public int Flags
		{
			get { return flags; }
		}

		/// <summary>
		/// </summary>
		public int StemV
		{
			get { return stemV; }
		}

		/// <summary>
		///     Under Construction
		/// </summary>
		public XFontMetrics FontMetrics
		{
			get
			{
				if (fontMetrics == null)
				{
					fontMetrics = new XFontMetrics(fontName, unitsPerEm, ascender, descender, leading, capHeight,
					                               xHeight, stemV, 0, 0, 0);
				}
				return fontMetrics;
			}
		}
	}
}