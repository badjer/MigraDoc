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

#define VERBOSE_

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
#if WPF
using System.Windows.Media;
#endif
using PdfSharp.Core;
using PdfSharp.Core.Enums;
using PdfSharp.Drawing;
using PdfSharp.Internal;
using Fixed = System.Int32;

namespace PdfSharp.Fonts.OpenType
{
	internal class FileExtensions
	{
		public static void WriteFileBytes(byte[] bytes, string filename)
		{
			if (!Directory.Exists(Path.GetDirectoryName(filename)))
				Directory.CreateDirectory(Path.GetDirectoryName(filename));
			using (var fs = File.OpenWrite(filename))
			{
				new BinaryWriter(fs).Write(bytes);
			}
		}
	}

	/// <summary>
	///     public configuration surface for the FontData loader
	/// </summary>
	public static class FontDataConfig
	{
		static FontDataConfig()
		{
			SaveFont = false;
		}

		/// <summary>
		///     Set this to an assembly containing fontdat resources, if you cannot rely on the GetFontData native method. e.g. under Mono
		/// </summary>
		public static Assembly ResourceAssembly { get; set; }

		/// <summary>
		///     Set this to true to create .fontdat files for all requested fonts.
		/// </summary>
		public static bool SaveFont { get; set; }
	}

	/// <summary>
	///     Represents an Open Type Font font in memory.
	/// </summary>
	internal class FontData
	{
		/// <summary>
		///     Shallow copy for font subset.
		/// </summary>
		private FontData(FontData fontData)
		{
			offsetTable = fontData.offsetTable;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="FontData" /> class.
		/// </summary>
		public FontData(XFont font, XPdfFontOptions options)
		{
#if GDI && !WPF
      CreateGdiFontImage(font, options);
#endif
#if WPF && !GDI
      CreateWpfFontData(font, options);
#endif
#if WPF && GDI
			Font gdiFont = font.RealizeGdiFont();
			if (font.font != null)
				CreateGdiFontImage(font, options);
			else if (font.typeface != null)
				CreateWpfFontData(font, options);
#endif
			if (data == null)
				throw new InvalidOperationException("Cannot allocate font data.");
			Read();
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="FontData" /> class.
		/// </summary>
		public FontData(byte[] data)
		{
			// Always save a copy of the font bytes
			int length = data.Length;
			byte[] copy = new byte[length];
			Array.Copy(data, copy, length);
			this.data = copy;
			Read();
		}

#if GDI
		/// <summary>
		///     Create the font image using GDI+ functionality.
		/// </summary>
		private void CreateGdiFontImage(XFont font, XPdfFontOptions options /*, XPrivateFontCollection privateFontCollection*/)
		{
			Font gdiFont = font.RealizeGdiFont();
#if DEBUG_
      logFont = new NativeMethods.LOGFONT();
      gdiFont.ToLogFont(logFont);
      Debug.WriteLine("FontData: " + logFont.lfFaceName);
#endif
			data = null;

			// PFC
			//if (privateFontCollection != null)
			//{
			//  //XPrivateFont privateFont = privateFontCollection.FindFont(logFont.lfFaceName, logFont.lfWeight >= 700, logFont.lfItalic > 0);
			//  XGlyphTypeface privateFont = privateFontCollection.FindFont(font.Name, font.Bold, font.Italic);
			//  if (privateFont != null)
			//  {
			//    //////int size = privateFont.GetData(ref this.data);
			//    //////if (size > 0)
			//    //////{
			//    //////  this.data = new byte[size];
			//    //////  privateFont.GetData(ref this.data, size);
			//    //////}
			//  }
			//}
			if (data == null)
			{
				var assembly = FontDataConfig.ResourceAssembly;
				var name = string.Format("{0}.FontHacks.{1}{2}{3}.fontdat",
				                         assembly != null ? assembly.GetName().Name : string.Empty,
				                         font.Name,
				                         font.Bold ? ".Bold" : string.Empty,
				                         font.Italic ? ".Italic" : string.Empty);
				if (assembly != null && new List<string>(assembly.GetManifestResourceNames()).Contains(name))
				{
					Debug.WriteLine("*** Reading fontdata from Resource");
					using (var s = assembly.GetManifestResourceStream(name))
					{
						data = new byte[s.Length];
						s.Read(data, 0, (int) s.Length);
					}
				}
				else
				{
					Debug.WriteLine("*** Reading fontdata from GDI+");
					int error;
					IntPtr hfont = gdiFont.ToHfont();
					using (var dcBmp = new Bitmap(100, 100))
					using (var dc = Graphics.FromImage(dcBmp))
					{
						IntPtr hdc = dc.GetHdc();
						error = Marshal.GetLastWin32Error();
						IntPtr oldFont = NativeMethods.SelectObject(hdc, hfont);
						error = Marshal.GetLastWin32Error();
						// size is exactly the size of the font file.
						int size = NativeMethods.GetFontData(hdc, 0, 0, null, 0);
						error = Marshal.GetLastWin32Error();
						if (size > 0)
						{
							data = new byte[size];
							int effectiveSize = NativeMethods.GetFontData(hdc, 0, 0, data, data.Length);
							Debug.Assert(size == effectiveSize);
							if (FontDataConfig.SaveFont)
							{
								FileExtensions.WriteFileBytes(data, string.Format("..\\..\\FontHacks\\{0}{1}{2}.fontdat",
								                                                  font.Name,
								                                                  font.Bold ? ".Bold" : string.Empty,
								                                                  font.Italic ? ".Italic" : string.Empty));
							}
							NativeMethods.SelectObject(hdc, oldFont);
							NativeMethods.ReleaseDC(IntPtr.Zero, hdc);
							error.GetType();
						}
						else
						{
							// Sometimes size is -1 (GDI_ERROR), but I cannot determine why. It happens only with the font 'Symbol'.
							// The issue occurs the first time in early 2005, when I start writing PDFsharp. I could not fix it and after
							// some code refactoring the problem disappears.
							// There was never a report from anyone about this issue.
							// Now I get it again (while debugging QBX 2006). Maybe it is a problem with my PC at my home office.
							// As a work-around I create a new font handle with a different height value. This works. Maybe the
							// font file gets locked somewhere. Very very strange.

							// IF SOMEONE ELSE COMES HERE PLEASE LET ME KNOW!

							// Clean up old handles
							NativeMethods.SelectObject(hdc, oldFont);
							NativeMethods.ReleaseDC(IntPtr.Zero, hdc);

							// Try again with new font handle
							var logFont = new NativeMethods.LOGFONT();
							gdiFont.ToLogFont(logFont);
							logFont.lfHeight += 1; // force new handle
							IntPtr hfont2 = NativeMethods.CreateFontIndirect(logFont);
							hdc = NativeMethods.GetDC(IntPtr.Zero);
							error = Marshal.GetLastWin32Error();
							oldFont = NativeMethods.SelectObject(hdc, hfont2);
							error = Marshal.GetLastWin32Error();
							// size is exactly the size of the font file.
							size = NativeMethods.GetFontData(hdc, 0, 0, null, 0);
							error = Marshal.GetLastWin32Error();
							if (size > 0)
							{
								data = new byte[size];
								int effectiveSize = NativeMethods.GetFontData(hdc, 0, 0, data, data.Length);
								Debug.Assert(size == effectiveSize);
							}
							NativeMethods.SelectObject(hdc, oldFont);
							NativeMethods.ReleaseDC(IntPtr.Zero, hdc);
							NativeMethods.DeleteObject(hfont2);
							error.GetType();
						}
					}
				}
				if (data == null)
					throw new InvalidOperationException("Internal error. Font data could not retrieved.");
			}
		}
#endif

#if WPF
		/// <summary>
		///     Create the font image using WPF functionality.
		/// </summary>
		private void CreateWpfFontData(XFont font, XPdfFontOptions options)
		{
#if !SILVERLIGHT
			GlyphTypeface glyphTypeface;
			if (!font.typeface.TryGetGlyphTypeface(out glyphTypeface))
				throw new InvalidOperationException(PSSR.CannotGetGlyphTypeface(font.Name));

			Stream fontStream = null;
			try
			{
				fontStream = glyphTypeface.GetFontStream();
				int size = (int) fontStream.Length;
				Debug.Assert(size > 0);
				data = new byte[size];
				fontStream.Read(data, 0, size);
			}
			finally
			{
				if (fontStream != null)
					fontStream.Close();
			}
#else
	// AGHACK
#endif
		}
#endif

		/// <summary>
		///     Gets the bytes that represents the font data.
		/// </summary>
		public byte[] Data
		{
			get { return data; }
		}

		private byte[] data;

		internal FontTechnology fontTechnology;

		internal OffsetTable offsetTable;

		/// <summary>
		///     The dictionary of all font tables.
		/// </summary>
		internal Dictionary<string, TableDirectoryEntry> tableDictionary = new Dictionary<string, TableDirectoryEntry>();

		internal CMapTable cmap;
		internal ControlValueTable cvt;
		internal FontProgram fpgm;
		internal MaximumProfileTable maxp;
		internal NameTable name;
		internal ControlValueProgram prep;
		internal FontHeaderTable head;
		internal HorizontalHeaderTable hhea;
		internal HorizontalMetricsTable hmtx;
		internal OS2Table os2;
		internal PostScriptTable post;
		internal GlyphDataTable glyf;
		internal IndexToLocationTable loca;
		internal GlyphSubstitutionTable gsub;
		internal VerticalHeaderTable vhea; // TODO
		internal VerticalMetricsTable vmtx; // TODO

		public bool CanRead
		{
			get { return data != null; }
		}

		public bool CanWrite
		{
			get { return data == null; }
		}

		/// <summary>
		///     Adds the specified table to this font image.
		/// </summary>
		public void AddTable(OpenTypeFontTable fontTable)
		{
			if (!CanWrite)
				throw new InvalidOperationException("Font image cannot be modified.");

			if (fontTable == null)
				throw new ArgumentNullException("fontTable");

			if (fontTable.fontData == null)
			{
				fontTable.fontData = this;
			}
			else
			{
				Debug.Assert(fontTable.fontData.CanRead);
				// Create a reference to this font table
				fontTable = new IRefFontTable(this, fontTable);
			}

			//Debug.Assert(fontTable.FontData == null);
			//fontTable.fontData = this;

			tableDictionary[fontTable.DirectoryEntry.Tag] = fontTable.DirectoryEntry;
			switch (fontTable.DirectoryEntry.Tag)
			{
				case TableTagNames.CMap:
					cmap = fontTable as CMapTable;
					break;

				case TableTagNames.Cvt:
					cvt = fontTable as ControlValueTable;
					break;

				case TableTagNames.Fpgm:
					fpgm = fontTable as FontProgram;
					break;

				case TableTagNames.MaxP:
					maxp = fontTable as MaximumProfileTable;
					break;

				case TableTagNames.Name:
					name = fontTable as NameTable;
					break;

				case TableTagNames.Head:
					head = fontTable as FontHeaderTable;
					break;

				case TableTagNames.HHea:
					hhea = fontTable as HorizontalHeaderTable;
					break;

				case TableTagNames.HMtx:
					hmtx = fontTable as HorizontalMetricsTable;
					break;

				case TableTagNames.OS2:
					os2 = fontTable as OS2Table;
					break;

				case TableTagNames.Post:
					post = fontTable as PostScriptTable;
					break;

				case TableTagNames.Glyf:
					glyf = fontTable as GlyphDataTable;
					break;

				case TableTagNames.Loca:
					loca = fontTable as IndexToLocationTable;
					break;

				case TableTagNames.GSUB:
					gsub = fontTable as GlyphSubstitutionTable;
					break;

				case TableTagNames.Prep:
					prep = fontTable as ControlValueProgram;
					break;
			}
		}

		/// <summary>
		///     Reads all required tables from the font data.
		/// </summary>
		internal void Read()
		{
			try
			{
				// Read offset table
				offsetTable.Version = ReadULong();
				offsetTable.TableCount = ReadUShort();
				offsetTable.SearchRange = ReadUShort();
				offsetTable.EntrySelector = ReadUShort();
				offsetTable.RangeShift = ReadUShort();

				// Move to table dictionary at position 12
				Debug.Assert(pos == 12);
				//this.tableDictionary = (this.offsetTable.TableCount);

				// ReSharper disable InconsistentNaming
				// Determine font technology
				const uint OTTO = 0x4f54544f; // Adobe OpenType CFF data, tag: 'OTTO'
				const uint TTCF = 0x74746366; // TrueType Collection tag: 'ttcf'  
				// ReSharper restore InconsistentNaming
				if (offsetTable.Version == TTCF)
				{
					fontTechnology = FontTechnology.TrueTypeCollection;
					throw new InvalidOperationException("TrueType collection fonts are not supported by PDFsharp.");
				}
				else if (offsetTable.Version == OTTO)
					fontTechnology = FontTechnology.PostscriptOutlines;
				else
					fontTechnology = FontTechnology.TrueTypeOutlines;

				for (int idx = 0; idx < offsetTable.TableCount; idx++)
				{
					TableDirectoryEntry entry = TableDirectoryEntry.ReadFrom(this);
					tableDictionary.Add(entry.Tag, entry);
#if VERBOSE
          Debug.WriteLine(String.Format("Font table: {0}", entry.Tag));
#endif
				}

				// PDFlib checks this, but it is not part of the OpenType spec anymore
				if (tableDictionary.ContainsKey("bhed"))
					throw new NotSupportedException("Bitmap fonts are not supported by PDFsharp.");

				// Read required tables
				if (Seek(CMapTable.Tag) != -1)
					cmap = new CMapTable(this);

				if (Seek(ControlValueTable.Tag) != -1)
					cvt = new ControlValueTable(this);

				if (Seek(FontProgram.Tag) != -1)
					fpgm = new FontProgram(this);

				if (Seek(MaximumProfileTable.Tag) != -1)
					maxp = new MaximumProfileTable(this);

				if (Seek(NameTable.Tag) != -1)
					name = new NameTable(this);

				if (Seek(FontHeaderTable.Tag) != -1)
					head = new FontHeaderTable(this);

				if (Seek(HorizontalHeaderTable.Tag) != -1)
					hhea = new HorizontalHeaderTable(this);

				if (Seek(HorizontalMetricsTable.Tag) != -1)
					hmtx = new HorizontalMetricsTable(this);

				if (Seek(OS2Table.Tag) != -1)
					os2 = new OS2Table(this);

				if (Seek(PostScriptTable.Tag) != -1)
					post = new PostScriptTable(this);

				if (Seek(GlyphDataTable.Tag) != -1)
					glyf = new GlyphDataTable(this);

				if (Seek(IndexToLocationTable.Tag) != -1)
					loca = new IndexToLocationTable(this);

				if (Seek(GlyphSubstitutionTable.Tag) != -1)
					gsub = new GlyphSubstitutionTable(this);

				if (Seek(ControlValueProgram.Tag) != -1)
					prep = new ControlValueProgram(this);
			}
			catch (Exception)
			{
				GetType();
				throw;
			}
		}

		/// <summary>
		///     Creates a new font image that is a subset of this font image containing only the specified glyphs.
		/// </summary>
		public FontData CreateFontSubSet(Dictionary<int, object> glyphs, bool cidFont)
		{
			// Create new font image
			FontData fontData = new FontData(this);

			// Create new loca and glyf table
			IndexToLocationTable loca = new IndexToLocationTable();
			loca.ShortIndex = this.loca.ShortIndex;
			GlyphDataTable glyf = new GlyphDataTable();

			// Add all required tables
			//fontData.AddTable(this.os2);
			if (!cidFont)
				fontData.AddTable(cmap);
			if (cvt != null)
				fontData.AddTable(cvt);
			if (fpgm != null)
				fontData.AddTable(fpgm);
			fontData.AddTable(glyf);
			fontData.AddTable(head);
			fontData.AddTable(hhea);
			fontData.AddTable(hmtx);
			fontData.AddTable(loca);
			if (maxp != null)
				fontData.AddTable(maxp);
			//fontData.AddTable(this.name);
			if (prep != null)
				fontData.AddTable(prep);

			// Get closure of used glyphs
			this.glyf.CompleteGlyphClosure(glyphs);

			// Create a sorted array of all used glyphs
			int glyphCount = glyphs.Count;
			int[] glyphArray = new int[glyphCount];
			glyphs.Keys.CopyTo(glyphArray, 0);
			Array.Sort(glyphArray);

			// Calculate new size of glyph table.
			int size = 0;
			for (int idx = 0; idx < glyphCount; idx++)
				size += this.glyf.GetGlyphSize(glyphArray[idx]);
			glyf.DirectoryEntry.Length = size;

			// Create new loca table
			int numGlyphs = maxp.numGlyphs;
			loca.locaTable = new int[numGlyphs + 1];

			// Create new glyf table
			glyf.glyphTable = new byte[glyf.DirectoryEntry.PaddedLength];

			// Fill new glyf and loca table
			int glyphOffset = 0;
			int glyphIndex = 0;
			for (int idx = 0; idx < numGlyphs; idx++)
			{
				loca.locaTable[idx] = glyphOffset;
				if (glyphIndex < glyphCount && glyphArray[glyphIndex] == idx)
				{
					glyphIndex++;
					byte[] bytes = this.glyf.GetGlyphData(idx);
					int length = bytes.Length;
					if (length > 0)
					{
						Buffer.BlockCopy(bytes, 0, glyf.glyphTable, glyphOffset, length);
						glyphOffset += length;
					}
				}
			}
			loca.locaTable[numGlyphs] = glyphOffset;

			// Compile font tables into byte array
			fontData.Compile();

			return fontData;
		}

		/// <summary>
		///     Compiles the font to its binary representation.
		/// </summary>
		private void Compile()
		{
			MemoryStream stream = new MemoryStream();
			OpenTypeFontWriter writer = new OpenTypeFontWriter(stream);

			int tableCount = tableDictionary.Count;
			int selector = entrySelectors[tableCount];

			offsetTable.Version = 0x00010000;
			offsetTable.TableCount = tableCount;
			offsetTable.SearchRange = (ushort) ((1 << selector)*16);
			offsetTable.EntrySelector = (ushort) selector;
			offsetTable.RangeShift = (ushort) ((tableCount - (1 << selector))*16);
			offsetTable.Write(writer);

			// Sort tables by tag name
			string[] tags = new string[tableCount];
			tableDictionary.Keys.CopyTo(tags, 0);
			Array.Sort(tags, StringComparer.Ordinal);

#if VERBOSE
      Debug.WriteLine("Start Compile");
#endif
			// Write tables in alphabetical order
			int tablePosition = 12 + 16*tableCount;
			for (int idx = 0; idx < tableCount; idx++)
			{
				TableDirectoryEntry entry = tableDictionary[tags[idx]];
#if DEBUG
				if (entry.Tag == "glyf" || entry.Tag == "loca")
					GetType();
#endif
				entry.FontTable.PrepareForCompilation();
				entry.Offset = tablePosition;
				writer.Position = tablePosition;
				entry.FontTable.Write(writer);
				int endPosition = writer.Position;
				tablePosition = endPosition;
				writer.Position = 12 + 16*idx;
				entry.Write(writer);
#if VERBOSE
        Debug.WriteLine(String.Format("  Write Table '{0}', offset={1}, length={2}, checksum={3}, ", entry.Tag, entry.Offset, entry.Length, entry.CheckSum));
#endif
			}
#if VERBOSE
      Debug.WriteLine("End Compile");
#endif
			writer.Stream.Flush();
			int l = (int) writer.Stream.Length;
			l.GetType();
			data = stream.ToArray();
		}

		// 2^entrySelector[n] <= n
		private static readonly int[] entrySelectors = {0, 0, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4};

		public int Position
		{
			get { return pos; }
			set { pos = value; }
		}

		private int pos;

		//public int Seek(int position)
		//{
		//  this.pos = position;
		//  return this.pos;
		//}

		public int Seek(string tag)
		{
			if (tableDictionary.ContainsKey(tag))
			{
				pos = tableDictionary[tag].Offset;
				return pos;
			}
			return -1;
		}

		public int SeekOffset(int offset)
		{
			pos += offset;
			return pos;
		}

		/// <summary>
		///     Reads a System.Byte.
		/// </summary>
		public byte ReadByte()
		{
			return data[pos++];
		}

		/// <summary>
		///     Reads a System.Int16.
		/// </summary>
		public short ReadShort()
		{
			int pos = this.pos;
			this.pos += 2;
			return (short) ((data[pos] << 8) | (data[pos + 1]));
		}

		/// <summary>
		///     Reads a System.UInt16.
		/// </summary>
		public ushort ReadUShort()
		{
			int pos = this.pos;
			this.pos += 2;
			return (ushort) ((data[pos] << 8) | (data[pos + 1]));
		}

		/// <summary>
		///     Reads a System.Int32.
		/// </summary>
		public int ReadLong()
		{
			int pos = this.pos;
			this.pos += 4;
			return ((data[pos] << 24) | (data[pos + 1] << 16) | (data[pos + 2] << 8) | (data[pos + 3]));
		}

		/// <summary>
		///     Reads a System.UInt32.
		/// </summary>
		public uint ReadULong()
		{
			int pos = this.pos;
			this.pos += 4;
			return (uint) ((data[pos] << 24) | (data[pos + 1] << 16) | (data[pos + 2] << 8) | (data[pos + 3]));
		}

		/// <summary>
		///     Reads a System.Int32.
		/// </summary>
		public Fixed ReadFixed()
		{
			int pos = this.pos;
			this.pos += 4;
			return ((data[pos] << 24) | (data[pos + 1] << 16) | (data[pos + 2] << 8) | (data[pos + 3]));
		}

		/// <summary>
		///     Reads a System.Int16.
		/// </summary>
		public short ReadFWord()
		{
			int pos = this.pos;
			this.pos += 2;
			return (short) ((data[pos] << 8) | (data[pos + 1]));
		}

		/// <summary>
		///     Reads a System.UInt16.
		/// </summary>
		public ushort ReadUFWord()
		{
			int pos = this.pos;
			this.pos += 2;
			return (ushort) ((data[pos] << 8) | (data[pos + 1]));
		}

		/// <summary>
		///     Reads a System.Int64.
		/// </summary>
		public long ReadLongDate()
		{
			int pos = this.pos;
			this.pos += 8;
			return (((long) data[pos]) << 56) | (((long) data[pos + 1]) << 48) | (((long) data[pos + 2]) << 40) | (((long) data[pos + 3]) << 32) |
			       (((long) data[pos + 4]) << 24) | (((long) data[pos + 5]) << 16) | (((long) data[pos + 6]) << 8) | data[pos + 7];
		}

		/// <summary>
		///     Reads a System.String with the specified size.
		/// </summary>
		public string ReadString(int size)
		{
			char[] chars = new char[size];
			for (int idx = 0; idx < size; idx++)
				chars[idx] = (char) data[pos++];
			return new string(chars);
		}

		/// <summary>
		///     Reads a System.Byte[] with the specified size.
		/// </summary>
		public byte[] ReadBytes(int size)
		{
			byte[] bytes = new byte[size];
			for (int idx = 0; idx < size; idx++)
				bytes[idx] = data[pos++];
			return bytes;
		}

		/// <summary>
		///     Reads the specified buffer.
		/// </summary>
		public void Read(byte[] buffer)
		{
			Read(buffer, 0, buffer.Length);
		}

		/// <summary>
		///     Reads the specified buffer.
		/// </summary>
		public void Read(byte[] buffer, int offset, int length)
		{
			Buffer.BlockCopy(data, pos, buffer, offset, length);
			pos += length;
		}

		/// <summary>
		///     Reads a System.Char[4] as System.String.
		/// </summary>
		public string ReadTag()
		{
			return ReadString(4);
		}

		/// <summary>
		///     Represents the font offset table.
		/// </summary>
		internal struct OffsetTable
		{
			/// <summary>
			///     Log2(maximum power of 2 ≤ numTables).
			/// </summary>
			public ushort EntrySelector;

			/// <summary>
			///     NumTables x 16-searchRange.
			/// </summary>
			public ushort RangeShift;

			/// <summary>
			///     (Maximum power of 2 ≤ numTables) x 16.
			/// </summary>
			public ushort SearchRange;

			/// <summary>
			///     Number of tables.
			/// </summary>
			public int TableCount;

			/// <summary>
			///     0x00010000 for version 1.0.
			/// </summary>
			public uint Version;

			/// <summary>
			///     Writes the offset table.
			/// </summary>
			public void Write(OpenTypeFontWriter writer)
			{
				writer.WriteUInt(Version);
				writer.WriteShort(TableCount);
				writer.WriteUShort(SearchRange);
				writer.WriteUShort(EntrySelector);
				writer.WriteUShort(RangeShift);
			}
		}
	}
}