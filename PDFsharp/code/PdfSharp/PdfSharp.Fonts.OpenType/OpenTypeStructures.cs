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
using System.Diagnostics;
using System.Text;
using PdfSharp.Core;
using Fixed = System.Int32;
using FWord = System.Int16;
using UFWord = System.UInt16;

namespace PdfSharp.Fonts.OpenType
{
	internal enum PlatformId
	{
		Apple,
		Mac,
		Iso,
		Win
	}

	/// <summary>
	///     Only Symbol and Unicode is used by PDFsharp.
	/// </summary>
	internal enum WinEncodingId
	{
		Symbol,
		Unicode
	}

	/// <summary>
	///     CMap format 4: Segment mapping to delta values.
	///     The Windows standard format.
	/// </summary>
	internal class CMap4 : OpenTypeFontTable
	{
		public WinEncodingId encodingId; // Windows encoding ID.
		public ushort[] endCount; // [segCount] / End characterCode for each segment, last=0xFFFF.
		public ushort entrySelector; // log2(searchRange/2)
		public ushort format; // Format number is set to 4.
		public int glyphCount; // = (length - (16 + 4 * 2 * segCount)) / 2;
		public ushort[] glyphIdArray; // Glyph index array (arbitrary length)
		public short[] idDelta; // [segCount] / Delta for all character codes in segment.
		public ushort[] idRangeOffs; // [segCount] / Offsets into glyphIdArray or 0
		public ushort language; // This field must be set to zero for all cmap subtables whose platform IDs are other than Macintosh (platform ID 1). 
		public ushort length; // This is the length in bytes of the subtable. 
		public ushort rangeShift;
		public ushort searchRange; // 2 x (2**floor(log2(segCount)))
		public ushort segCountX2; // 2 x segCount.
		public ushort[] startCount; // [segCount] / Start character code for each segment.

		public CMap4(FontData fontData, WinEncodingId encodingId)
			: base(fontData, "----")
		{
			this.encodingId = encodingId;
			Read();
		}

		internal void Read()
		{
			try
			{
				// m_EncodingID = encID;
				format = fontData.ReadUShort();
				Debug.Assert(format == 4, "Only format 4 expected.");
				length = fontData.ReadUShort();
				language = fontData.ReadUShort(); // Always null in Windows
				segCountX2 = fontData.ReadUShort();
				searchRange = fontData.ReadUShort();
				entrySelector = fontData.ReadUShort();
				rangeShift = fontData.ReadUShort();

				int segCount = segCountX2/2;
				glyphCount = (length - (16 + 8*segCount))/2;

				//ASSERT_CONDITION(0 <= m_NumGlyphIds && m_NumGlyphIds < m_Length, "Invalid Index");

				endCount = new ushort[segCount];
				startCount = new ushort[segCount];
				idDelta = new short[segCount];
				idRangeOffs = new ushort[segCount];

				glyphIdArray = new ushort[glyphCount];

				for (int idx = 0; idx < segCount; idx++)
					endCount[idx] = fontData.ReadUShort();

				//ASSERT_CONDITION(m_EndCount[segs - 1] == 0xFFFF, "Out of Index");

				// Read reserved pad.
				fontData.ReadUShort();

				for (int idx = 0; idx < segCount; idx++)
					startCount[idx] = fontData.ReadUShort();

				for (int idx = 0; idx < segCount; idx++)
					idDelta[idx] = fontData.ReadShort();

				for (int idx = 0; idx < segCount; idx++)
					idRangeOffs[idx] = fontData.ReadUShort();

				for (int idx = 0; idx < glyphCount; idx++)
					glyphIdArray[idx] = fontData.ReadUShort();
			}
			catch (Exception ex)
			{
				throw new PdfSharpException(PSSR.ErrorReadingFontData, ex);
			}
		}
	}

	/// <summary>
	///     This table defines the mapping of character codes to the glyph index values used in the font.
	///     It may contain more than one subtable, in order to support more than one character encoding scheme.
	/// </summary>
	internal class CMapTable : OpenTypeFontTable
	{
		public const string Tag = TableTagNames.CMap;
		public CMap4 cmap4;

		public ushort numTables;

		/// <summary>
		///     Is true for symbol font encoding.
		/// </summary>
		public bool symbol;

		public ushort version;

		/// <summary>
		///     Initializes a new instance of the <see cref="CMapTable" /> class.
		/// </summary>
		public CMapTable(FontData fontData)
			: base(fontData, Tag)
		{
			Read();
		}

		internal void Read()
		{
			try
			{
				int tableOffset = fontData.Position;

				version = fontData.ReadUShort();
				numTables = fontData.ReadUShort();

				bool success = false;
				for (int idx = 0; idx < numTables; idx++)
				{
					PlatformId platformId = (PlatformId) fontData.ReadUShort();
					WinEncodingId encodingId = (WinEncodingId) fontData.ReadUShort();
					int offset = fontData.ReadLong();

					int currentPosition = fontData.Position;

					// Just read Windows stuff
					if (platformId == PlatformId.Win && (encodingId == WinEncodingId.Symbol || encodingId == WinEncodingId.Unicode))
					{
						symbol = encodingId == WinEncodingId.Symbol;

						fontData.Position = tableOffset + offset;
						cmap4 = new CMap4(fontData, encodingId);
						fontData.Position = currentPosition;
						// We have found what we are looking for, so break.
						success = true;
						break;
					}
				}
				if (!success)
					throw new InvalidOperationException("Font has no usable platform or encoding ID. It cannot be used with PDFsharp.");
			}
			catch (Exception ex)
			{
				throw new PdfSharpException(PSSR.ErrorReadingFontData, ex);
			}
		}
	}

	/// <summary>
	///     This table gives global information about the font. The bounding box values should be computed using
	///     only glyphs that have contours. Glyphs with no contours should be ignored for the purposes of these calculations.
	/// </summary>
	internal class FontHeaderTable : OpenTypeFontTable
	{
		public const string Tag = TableTagNames.Head;

		public uint checkSumAdjustment;
		public long created;
		public ushort flags;
		public short fontDirectionHint;
		public Fixed fontRevision;
		public short glyphDataFormat; // 0 for current format
		public short indexToLocFormat; // 0 for short offsets, 1 for long
		public ushort lowestRecPPEM;
		public ushort macStyle;
		public uint magicNumber; // Set to 0x5F0F3CF5
		public long modified;
		public ushort unitsPerEm; // Valid range is from 16 to 16384. This value should be a power of 2 for fonts that have TrueType outlines.
		public Fixed version; // 0x00010000 for version 1.0.
		public short xMax; // For all glyph bounding boxes.
		public short xMin; // For all glyph bounding boxes.
		public short yMax; // For all glyph bounding boxes.
		public short yMin; // For all glyph bounding boxes.

		public FontHeaderTable(FontData fontData)
			: base(fontData, Tag)
		{
			Read();
		}

		public void Read()
		{
			try
			{
				version = fontData.ReadFixed();
				fontRevision = fontData.ReadFixed();
				checkSumAdjustment = fontData.ReadULong();
				magicNumber = fontData.ReadULong();
				flags = fontData.ReadUShort();
				unitsPerEm = fontData.ReadUShort();
				created = fontData.ReadLongDate();
				modified = fontData.ReadLongDate();
				xMin = fontData.ReadShort();
				yMin = fontData.ReadShort();
				xMax = fontData.ReadShort();
				yMax = fontData.ReadShort();
				macStyle = fontData.ReadUShort();
				lowestRecPPEM = fontData.ReadUShort();
				fontDirectionHint = fontData.ReadShort();
				indexToLocFormat = fontData.ReadShort();
				glyphDataFormat = fontData.ReadShort();
			}
			catch (Exception ex)
			{
				throw new PdfSharpException(PSSR.ErrorReadingFontData, ex);
			}
		}
	}

	/// <summary>
	///     This table contains information for horizontal layout. The values in the minRightSidebearing,
	///     minLeftSideBearing and xMaxExtent should be computed using only glyphs that have contours.
	///     Glyphs with no contours should be ignored for the purposes of these calculations.
	///     All reserved areas must be set to 0.
	/// </summary>
	internal class HorizontalHeaderTable : OpenTypeFontTable
	{
		public const string Tag = TableTagNames.HHea;
		public UFWord advanceWidthMax;

		public FWord ascender; // Typographic ascent. (Distance from baseline of highest ascender) 
		public short caretSlopeRise;
		public short caretSlopeRun;
		public FWord descender; // Typographic descent. (Distance from baseline of lowest descender) 
		public FWord lineGap; // Typographic line gap. Negative LineGap values are treated as zero in Windows 3.1, System 6, and System 7.
		public short metricDataFormat;
		public FWord minLeftSideBearing;
		public FWord minRightSideBearing;
		public ushort numberOfHMetrics;
		public short reserved1;
		public short reserved2;
		public short reserved3;
		public short reserved4;
		public short reserved5;
		public Fixed version; // 0x00010000 for version 1.0.
		public FWord xMaxExtent;

		public HorizontalHeaderTable(FontData fontData)
			: base(fontData, Tag)
		{
			Read();
		}

		public void Read()
		{
			try
			{
				version = fontData.ReadFixed();
				ascender = fontData.ReadFWord();
				descender = fontData.ReadFWord();
				lineGap = fontData.ReadFWord();
				advanceWidthMax = fontData.ReadUFWord();
				minLeftSideBearing = fontData.ReadFWord();
				minRightSideBearing = fontData.ReadFWord();
				xMaxExtent = fontData.ReadFWord();
				caretSlopeRise = fontData.ReadShort();
				caretSlopeRun = fontData.ReadShort();
				reserved1 = fontData.ReadShort();
				reserved2 = fontData.ReadShort();
				reserved3 = fontData.ReadShort();
				reserved4 = fontData.ReadShort();
				reserved5 = fontData.ReadShort();
				metricDataFormat = fontData.ReadShort();
				numberOfHMetrics = fontData.ReadUShort();
			}
			catch (Exception ex)
			{
				throw new PdfSharpException(PSSR.ErrorReadingFontData, ex);
			}
		}
	}

	internal class HorizontalMetrics : OpenTypeFontTable
	{
		public const string Tag = "----";

		public ushort advanceWidth;
		public short lsb;

		public HorizontalMetrics(FontData fontData)
			: base(fontData, Tag)
		{
			Read();
		}

		public void Read()
		{
			try
			{
				advanceWidth = fontData.ReadUFWord();
				lsb = fontData.ReadFWord();
			}
			catch (Exception ex)
			{
				throw new PdfSharpException(PSSR.ErrorReadingFontData, ex);
			}
		}
	}

	/// <summary>
	///     The type longHorMetric is defined as an array where each element has two parts:
	///     the advance width, which is of type USHORT, and the left side bearing, which is of type SHORT.
	///     These fields are in font design units.
	/// </summary>
	internal class HorizontalMetricsTable : OpenTypeFontTable
	{
		public const string Tag = TableTagNames.HMtx;

		public FWord[] leftSideBearing;
		public HorizontalMetrics[] metrics;

		public HorizontalMetricsTable(FontData fontData)
			: base(fontData, Tag)
		{
			Read();
		}

		public void Read()
		{
			try
			{
				HorizontalHeaderTable hhea = fontData.hhea;
				MaximumProfileTable maxp = fontData.maxp;
				if (hhea != null && maxp != null)
				{
					int numMetrics = hhea.numberOfHMetrics; //->NumberOfHMetrics();
					int numLsbs = maxp.numGlyphs - numMetrics;

					Debug.Assert(numMetrics != 0);
					Debug.Assert(numLsbs >= 0);

					metrics = new HorizontalMetrics[numMetrics];
					for (int idx = 0; idx < numMetrics; idx++)
						metrics[idx] = new HorizontalMetrics(fontData);

					if (numLsbs > 0)
					{
						leftSideBearing = new FWord[numLsbs];
						for (int idx = 0; idx < numLsbs; idx++)
							leftSideBearing[idx] = fontData.ReadFWord();
					}
				}
			}
			catch (Exception ex)
			{
				throw new PdfSharpException(PSSR.ErrorReadingFontData, ex);
			}
		}
	}

	// UNDONE
	internal class VerticalHeaderTable : OpenTypeFontTable
	{
		public const string Tag = TableTagNames.VHea;
		public UFWord advanceWidthMax;

		// code comes from HorizontalHeaderTable
		public FWord ascender; // Typographic ascent. (Distance from baseline of highest ascender) 
		public short caretSlopeRise;
		public short caretSlopeRun;
		public FWord descender; // Typographic descent. (Distance from baseline of lowest descender) 
		public FWord lineGap; // Typographic line gap. Negative LineGap values are treated as zero in Windows 3.1, System 6, and System 7.
		public short metricDataFormat;
		public FWord minLeftSideBearing;
		public FWord minRightSideBearing;
		public ushort numberOfHMetrics;
		public short reserved1;
		public short reserved2;
		public short reserved3;
		public short reserved4;
		public short reserved5;
		public Fixed version; // 0x00010000 for version 1.0.
		public FWord xMaxExtent;

		public VerticalHeaderTable(FontData fontData)
			: base(fontData, Tag)
		{
			Read();
		}

		public void Read()
		{
			try
			{
				version = fontData.ReadFixed();
				ascender = fontData.ReadFWord();
				descender = fontData.ReadFWord();
				lineGap = fontData.ReadFWord();
				advanceWidthMax = fontData.ReadUFWord();
				minLeftSideBearing = fontData.ReadFWord();
				minRightSideBearing = fontData.ReadFWord();
				xMaxExtent = fontData.ReadFWord();
				caretSlopeRise = fontData.ReadShort();
				caretSlopeRun = fontData.ReadShort();
				reserved1 = fontData.ReadShort();
				reserved2 = fontData.ReadShort();
				reserved3 = fontData.ReadShort();
				reserved4 = fontData.ReadShort();
				reserved5 = fontData.ReadShort();
				metricDataFormat = fontData.ReadShort();
				numberOfHMetrics = fontData.ReadUShort();
			}
			catch (Exception ex)
			{
				throw new PdfSharpException(PSSR.ErrorReadingFontData, ex);
			}
		}
	}

	internal class VerticalMetrics : OpenTypeFontTable
	{
		public const string Tag = "----";

		// code comes from HorizontalMetrics
		public ushort advanceWidth;
		public short lsb;

		public VerticalMetrics(FontData fontData)
			: base(fontData, Tag)
		{
			Read();
		}

		public void Read()
		{
			try
			{
				advanceWidth = fontData.ReadUFWord();
				lsb = fontData.ReadFWord();
			}
			catch (Exception ex)
			{
				throw new PdfSharpException(PSSR.ErrorReadingFontData, ex);
			}
		}
	}

	/// <summary>
	///     The vertical metrics table allows you to specify the vertical spacing for each glyph in a
	///     vertical font. This table consists of either one or two arrays that contain metric
	///     information (the advance heights and top sidebearings) for the vertical layout of each
	///     of the glyphs in the font.
	/// </summary>
	internal class VerticalMetricsTable : OpenTypeFontTable
	{
		// UNDONE
		public const string Tag = TableTagNames.VMtx;

		// code comes from HorizontalMetricsTable
		public FWord[] leftSideBearing;
		public HorizontalMetrics[] metrics;

		public VerticalMetricsTable(FontData fontData)
			: base(fontData, Tag)
		{
			Read();
			throw new NotImplementedException("VerticalMetricsTable");
		}

		public void Read()
		{
			try
			{
				HorizontalHeaderTable hhea = fontData.hhea;
				MaximumProfileTable maxp = fontData.maxp;
				if (hhea != null && maxp != null)
				{
					int numMetrics = hhea.numberOfHMetrics; //->NumberOfHMetrics();
					int numLsbs = maxp.numGlyphs - numMetrics;

					Debug.Assert(numMetrics != 0);
					Debug.Assert(numLsbs >= 0);

					metrics = new HorizontalMetrics[numMetrics];
					for (int idx = 0; idx < numMetrics; idx++)
						metrics[idx] = new HorizontalMetrics(fontData);

					if (numLsbs > 0)
					{
						leftSideBearing = new FWord[numLsbs];
						for (int idx = 0; idx < numLsbs; idx++)
							leftSideBearing[idx] = fontData.ReadFWord();
					}
				}
			}
			catch (Exception ex)
			{
				throw new PdfSharpException(PSSR.ErrorReadingFontData, ex);
			}
		}
	}

	/// <summary>
	///     This table establishes the memory requirements for this font.
	///     Fonts with CFF data must use Version 0.5 of this table, specifying only the numGlyphs field.
	///     Fonts with TrueType outlines must use Version 1.0 of this table, where all data is required.
	///     Both formats of OpenType require a 'maxp' table because a number of applications call the
	///     Windows GetFontData() API on the 'maxp' table to determine the number of glyphs in the font.
	/// </summary>
	internal class MaximumProfileTable : OpenTypeFontTable
	{
		public const string Tag = TableTagNames.MaxP;
		public ushort maxComponentDepth;
		public ushort maxComponentElements;

		public ushort maxCompositeContours;
		public ushort maxCompositePoints;
		public ushort maxContours;
		public ushort maxFunctionDefs;
		public ushort maxInstructionDefs;
		public ushort maxPoints;
		public ushort maxSizeOfInstructions;
		public ushort maxStackElements;
		public ushort maxStorage;
		public ushort maxTwilightPoints;
		public ushort maxZones;
		public ushort numGlyphs;
		public Fixed version;

		public MaximumProfileTable(FontData fontData)
			: base(fontData, Tag)
		{
			Read();
		}

		public void Read()
		{
			try
			{
				version = fontData.ReadFixed();
				numGlyphs = fontData.ReadUShort();
				maxPoints = fontData.ReadUShort();
				maxContours = fontData.ReadUShort();
				maxCompositePoints = fontData.ReadUShort();
				maxCompositeContours = fontData.ReadUShort();
				maxZones = fontData.ReadUShort();
				maxTwilightPoints = fontData.ReadUShort();
				maxStorage = fontData.ReadUShort();
				maxFunctionDefs = fontData.ReadUShort();
				maxInstructionDefs = fontData.ReadUShort();
				maxStackElements = fontData.ReadUShort();
				maxSizeOfInstructions = fontData.ReadUShort();
				maxComponentElements = fontData.ReadUShort();
				maxComponentDepth = fontData.ReadUShort();
			}
			catch (Exception ex)
			{
				throw new PdfSharpException(PSSR.ErrorReadingFontData, ex);
			}
		}
	}

	/// <summary>
	///     The naming table allows multilingual strings to be associated with the OpenTypeTM font file.
	///     These strings can represent copyright notices, font names, family names, style names, and so on.
	///     To keep this table short, the font manufacturer may wish to make a limited set of entries in some
	///     small set of languages; later, the font can be "localized" and the strings translated or added.
	///     Other parts of the OpenType font file that require these strings can then refer to them simply by
	///     their index number. Clients that need a particular string can look it up by its platform ID, character
	///     encoding ID, language ID and name ID. Note that some platforms may require single byte character
	///     strings, while others may require double byte strings.
	///     For historical reasons, some applications which install fonts perform version control using Macintosh
	///     platform (platform ID 1) strings from the 'name' table. Because of this, we strongly recommend that
	///     the 'name' table of all fonts include Macintosh platform strings and that the syntax of the version
	///     number (name id 5) follows the guidelines given in this document.
	/// </summary>
	internal class NameTable : OpenTypeFontTable
	{
		public const string Tag = TableTagNames.Name;

		public string Name = String.Empty;
		public string Style = String.Empty;
		private byte[] bytes;

		public ushort count;
		public ushort format;
		public ushort stringOffset;

		public NameTable(FontData fontData)
			: base(fontData, Tag)
		{
			Read();
		}

		public void Read()
		{
			try
			{
#if DEBUG
				fontData.Position = DirectoryEntry.Offset;
#endif
				bytes = new byte[DirectoryEntry.PaddedLength];
				Buffer.BlockCopy(fontData.Data, DirectoryEntry.Offset, bytes, 0, DirectoryEntry.Length);

				format = fontData.ReadUShort();
				count = fontData.ReadUShort();
				stringOffset = fontData.ReadUShort();

				for (int idx = 0; idx < count; idx++)
				{
					NameRecord nrec = ReadNameRecord();
					byte[] value = new byte[nrec.length];
					Buffer.BlockCopy(fontData.Data, DirectoryEntry.Offset + stringOffset + nrec.offset, value, 0, nrec.length);

					//Debug.WriteLine(nrec.platformID.ToString());

					// Read font name and style
					if (nrec.platformID == 0 || nrec.platformID == 3)
					{
						if (nrec.nameID == 1 && nrec.languageID == 0x0409)
						{
							if (String.IsNullOrEmpty(Name))
								Name = Encoding.BigEndianUnicode.GetString(value, 0, value.Length);
						}
						if (nrec.nameID == 2 && nrec.languageID == 0x0409)
						{
							if (String.IsNullOrEmpty(Style))
								Style = Encoding.BigEndianUnicode.GetString(value, 0, value.Length);
						}
					}
					//string s1 = Encoding.Default.GetString(name);
					//string s2 = Encoding.BigEndianUnicode.GetString(name);
					//Debug.WriteLine(s1);
					//Debug.WriteLine(s2);
				}
				Debug.Assert(!String.IsNullOrEmpty(Name));
			}
			catch (Exception ex)
			{
				throw new PdfSharpException(PSSR.ErrorReadingFontData, ex);
			}
		}

		private NameRecord ReadNameRecord()
		{
			NameRecord nrec = new NameRecord();
			nrec.platformID = fontData.ReadUShort();
			nrec.encodingID = fontData.ReadUShort();
			nrec.languageID = fontData.ReadUShort();
			nrec.nameID = fontData.ReadUShort();
			nrec.length = fontData.ReadUShort();
			nrec.offset = fontData.ReadUShort();
			return nrec;
		}

		private class NameRecord
		{
			public ushort encodingID;
			public ushort languageID;
			public ushort length;
			public ushort nameID;
			public ushort offset;
			public ushort platformID;
		}
	}

	/// <summary>
	///     The OS/2 table consists of a set of metrics that are required in OpenType fonts.
	/// </summary>
	internal class OS2Table : OpenTypeFontTable
	{
		[Flags]
		public enum FontSelectionFlags : ushort
		{
			Italic = 1 << 0,
			Bold = 1 << 5,
			Regular = 1 << 6,
		}

		public const string Tag = TableTagNames.OS2;
		public string achVendID; // = "";
		public ushort fsSelection;

		public ushort fsType;
		public byte[] panose; // = new byte[10];
		public short sCapHeight;
		public short sFamilyClass;
		public short sTypoAscender;
		public short sTypoDescender;
		public short sTypoLineGap;
		public short sxHeight;
		public uint ulCodePageRange1; // Bits 0-31
		public uint ulCodePageRange2; // Bits 32-63
		public uint ulUnicodeRange1; // Bits 0-31
		public uint ulUnicodeRange2; // Bits 32-63
		public uint ulUnicodeRange3; // Bits 64-95
		public uint ulUnicodeRange4; // Bits 96-127
		public ushort usBreakChar;
		public ushort usDefaultChar;
		public ushort usFirstCharIndex;
		public ushort usLastCharIndex;
		public ushort usMaxContext;
		public ushort usWeightClass;
		public ushort usWidthClass;
		public ushort usWinAscent;
		public ushort usWinDescent;
		public ushort version;
		public short xAvgCharWidth;
		public short yStrikeoutPosition;
		public short yStrikeoutSize;
		public short ySubscriptXOffset;
		public short ySubscriptXSize;
		public short ySubscriptYOffset;
		public short ySubscriptYSize;
		public short ySuperscriptXOffset;
		public short ySuperscriptXSize;
		public short ySuperscriptYOffset;
		public short ySuperscriptYSize;
		// version >= 1

		public OS2Table(FontData fontData)
			: base(fontData, Tag)
		{
			Read();
		}

		public void Read()
		{
			try
			{
				version = fontData.ReadUShort();
				xAvgCharWidth = fontData.ReadShort();
				usWeightClass = fontData.ReadUShort();
				usWidthClass = fontData.ReadUShort();
				fsType = fontData.ReadUShort();
				ySubscriptXSize = fontData.ReadShort();
				ySubscriptYSize = fontData.ReadShort();
				ySubscriptXOffset = fontData.ReadShort();
				ySubscriptYOffset = fontData.ReadShort();
				ySuperscriptXSize = fontData.ReadShort();
				ySuperscriptYSize = fontData.ReadShort();
				ySuperscriptXOffset = fontData.ReadShort();
				ySuperscriptYOffset = fontData.ReadShort();
				yStrikeoutSize = fontData.ReadShort();
				yStrikeoutPosition = fontData.ReadShort();
				sFamilyClass = fontData.ReadShort();
				panose = fontData.ReadBytes(10);
				ulUnicodeRange1 = fontData.ReadULong();
				ulUnicodeRange2 = fontData.ReadULong();
				ulUnicodeRange3 = fontData.ReadULong();
				ulUnicodeRange4 = fontData.ReadULong();
				achVendID = fontData.ReadString(4);
				fsSelection = fontData.ReadUShort();
				usFirstCharIndex = fontData.ReadUShort();
				usLastCharIndex = fontData.ReadUShort();
				sTypoAscender = fontData.ReadShort();
				sTypoDescender = fontData.ReadShort();
				sTypoLineGap = fontData.ReadShort();
				usWinAscent = fontData.ReadUShort();
				usWinDescent = fontData.ReadUShort();

				if (version >= 1)
				{
					ulCodePageRange1 = fontData.ReadULong();
					ulCodePageRange2 = fontData.ReadULong();

					if (version >= 2)
					{
						sxHeight = fontData.ReadShort();
						sCapHeight = fontData.ReadShort();
						usDefaultChar = fontData.ReadUShort();
						usBreakChar = fontData.ReadUShort();
						usMaxContext = fontData.ReadUShort();
					}
				}
			}
			catch (Exception ex)
			{
				throw new PdfSharpException(PSSR.ErrorReadingFontData, ex);
			}
		}
	}

	/// <summary>
	///     This table contains additional information needed to use TrueType or OpenTypeTM fonts
	///     on PostScript printers.
	/// </summary>
	internal class PostScriptTable : OpenTypeFontTable
	{
		public const string Tag = TableTagNames.Post;

		public Fixed formatType;
		public ulong isFixedPitch;
		public float italicAngle;
		public ulong maxMemType1;
		public ulong maxMemType42;
		public ulong minMemType1;
		public ulong minMemType42;
		public FWord underlinePosition;
		public FWord underlineThickness;

		public PostScriptTable(FontData fontData)
			: base(fontData, Tag)
		{
			Read();
		}

		public void Read()
		{
			try
			{
				formatType = fontData.ReadFixed();
				italicAngle = fontData.ReadFixed()/65536f;
				underlinePosition = fontData.ReadFWord();
				underlineThickness = fontData.ReadFWord();
				isFixedPitch = fontData.ReadULong();
				minMemType42 = fontData.ReadULong();
				maxMemType42 = fontData.ReadULong();
				minMemType1 = fontData.ReadULong();
				maxMemType1 = fontData.ReadULong();
			}
			catch (Exception ex)
			{
				throw new PdfSharpException(PSSR.ErrorReadingFontData, ex);
			}
		}
	}

	/// <summary>
	///     This table contains a list of values that can be referenced by instructions.
	///     They can be used, among other things, to control characteristics for different glyphs.
	///     The length of the table must be an integral number of FWORD units.
	/// </summary>
	internal class ControlValueTable : OpenTypeFontTable
	{
		public const string Tag = TableTagNames.Cvt;

		private FWord[] array; // List of n values referenceable by instructions. n is the number of FWORD items that fit in the size of the table.

		public ControlValueTable(FontData fontData)
			: base(fontData, Tag)
		{
			DirectoryEntry.Tag = TableTagNames.Cvt;
			DirectoryEntry = fontData.tableDictionary[TableTagNames.Cvt];
			Read();
		}

		public void Read()
		{
			try
			{
				int length = DirectoryEntry.Length/2;
				array = new FWord[length];
				for (int idx = 0; idx < length; idx++)
					array[idx] = fontData.ReadFWord();
			}
			catch (Exception ex)
			{
				throw new PdfSharpException(PSSR.ErrorReadingFontData, ex);
			}
		}
	}

	/// <summary>
	///     This table is similar to the CVT Program, except that it is only run once, when the font is first used.
	///     It is used only for FDEFs and IDEFs. Thus the CVT Program need not contain function definitions.
	///     However, the CVT Program may redefine existing FDEFs or IDEFs.
	/// </summary>
	internal class FontProgram : OpenTypeFontTable
	{
		public const string Tag = TableTagNames.Fpgm;

		private byte[] bytes; // Instructions. n is the number of BYTE items that fit in the size of the table.

		public FontProgram(FontData fontData)
			: base(fontData, Tag)
		{
			DirectoryEntry.Tag = TableTagNames.Fpgm;
			DirectoryEntry = fontData.tableDictionary[TableTagNames.Fpgm];
			Read();
		}

		public void Read()
		{
			try
			{
				int length = DirectoryEntry.Length;
				bytes = new byte[length];
				for (int idx = 0; idx < length; idx++)
					bytes[idx] = fontData.ReadByte();
			}
			catch (Exception ex)
			{
				throw new PdfSharpException(PSSR.ErrorReadingFontData, ex);
			}
		}
	}

	/// <summary>
	///     The Control Value Program consists of a set of TrueType instructions that will be executed whenever the font or
	///     point size or transformation matrix change and before each glyph is interpreted. Any instruction is legal in the
	///     CVT Program but since no glyph is associated with it, instructions intended to move points within a particular
	///     glyph outline cannot be used in the CVT Program. The name 'prep' is anachronistic.
	/// </summary>
	internal class ControlValueProgram : OpenTypeFontTable
	{
		public const string Tag = TableTagNames.Prep;

		private byte[] bytes; // Set of instructions executed whenever point size or font or transformation change. n is the number of BYTE items that fit in the size of the table.

		public ControlValueProgram(FontData fontData)
			: base(fontData, Tag)
		{
			DirectoryEntry.Tag = TableTagNames.Prep;
			DirectoryEntry = fontData.tableDictionary[TableTagNames.Prep];
			Read();
		}

		public void Read()
		{
			try
			{
				int length = DirectoryEntry.Length;
				bytes = new byte[length];
				for (int idx = 0; idx < length; idx++)
					bytes[idx] = fontData.ReadByte();
			}
			catch (Exception ex)
			{
				throw new PdfSharpException(PSSR.ErrorReadingFontData, ex);
			}
		}
	}

	/// <summary>
	///     This table contains information that describes the glyphs in the font in the TrueType outline format.
	///     Information regarding the rasterizer (scaler) refers to the TrueType rasterizer.
	/// </summary>
	internal class GlyphSubstitutionTable : OpenTypeFontTable
	{
		public const string Tag = TableTagNames.GSUB;

		public GlyphSubstitutionTable(FontData fontData)
			: base(fontData, Tag)
		{
			DirectoryEntry.Tag = TableTagNames.GSUB;
			DirectoryEntry = fontData.tableDictionary[TableTagNames.GSUB];
			Read();
		}

		public void Read()
		{
			try
			{
			}
			catch (Exception ex)
			{
				throw new PdfSharpException(PSSR.ErrorReadingFontData, ex);
			}
		}
	}
}