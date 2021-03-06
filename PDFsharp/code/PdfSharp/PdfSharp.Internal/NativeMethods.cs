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

using System;
using System.Runtime.InteropServices;

namespace PdfSharp.Internal
{
	/// <summary>
	///     Required native Win32 calls. Don't know what to do under Mono.
	/// </summary>
	internal static class NativeMethods
	{
		public const int HORZSIZE = 4; // Horizontal size in millimeters
		public const int VERTSIZE = 6; // Vertical size in millimeters
		public const int HORZRES = 8; // Horizontal width in pixels
		public const int VERTRES = 10; // Vertical height in pixels
		public const int LOGPIXELSX = 88; // Logical pixels/inch in X
		public const int LOGPIXELSY = 90; // Logical pixels/inch in Y

		[DllImport("user32.dll")]
		public static extern IntPtr GetDC(IntPtr hwnd);

		[DllImport("user32.dll")]
		public static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr hdc);

		[DllImport("gdi32.dll", SetLastError = true)]
		public static extern int GetFontData(
			IntPtr hdc, // handle to DC
			uint dwTable, // metric table name
			uint dwOffset, // offset into table
			byte[] lpvBuffer, // buffer for returned data
			int cbData // length of data
			);

		[DllImport("gdi32.dll", EntryPoint = "CreateFontIndirectW")]
		public static extern IntPtr CreateFontIndirect(LOGFONT lpLogFont);

		[DllImport("gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hgdiobj);

		[DllImport("gdi32.dll")]
		public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

		/// <summary>
		///     Reflected from System.Drawing.SafeNativeMethods+LOGFONT
		/// </summary>
		//[SuppressUnmanagedCodeSecurity]
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LOGFONT
		{
			// Preserve us for warning CS0649...
			private LOGFONT(int dummy)
			{
				lfHeight = 0;
				lfWidth = 0;
				lfEscapement = 0;
				lfOrientation = 0;
				lfWeight = 0;
				lfItalic = 0;
				lfUnderline = 0;
				lfStrikeOut = 0;
				lfCharSet = 0;
				lfOutPrecision = 0;
				lfClipPrecision = 0;
				lfQuality = 0;
				lfPitchAndFamily = 0;
				lfFaceName = "";
			}

			public int lfHeight;
			public int lfWidth;
			public int lfEscapement;
			public int lfOrientation;
			public int lfWeight;
			public byte lfItalic;
			public byte lfUnderline;
			public byte lfStrikeOut;
			public byte lfCharSet;
			public byte lfOutPrecision;
			public byte lfClipPrecision;
			public byte lfQuality;
			public byte lfPitchAndFamily;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string lfFaceName;

			public override string ToString()
			{
				object[] objArray1 = new object[0x1c]
					{
						"lfHeight=", lfHeight,
						", lfWidth=", lfWidth,
						", lfEscapement=", lfEscapement,
						", lfOrientation=", lfOrientation,
						", lfWeight=", lfWeight,
						", lfItalic=", lfItalic,
						", lfUnderline=", lfUnderline,
						", lfStrikeOut=", lfStrikeOut,
						", lfCharSet=", lfCharSet,
						", lfOutPrecision=", lfOutPrecision,
						", lfClipPrecision=", lfClipPrecision,
						", lfQuality=", lfQuality,
						", lfPitchAndFamily=", lfPitchAndFamily,
						", lfFaceName=", lfFaceName
					};
				return string.Concat(objArray1);
			}

			public LOGFONT()
			{
			}
		}
	}
}