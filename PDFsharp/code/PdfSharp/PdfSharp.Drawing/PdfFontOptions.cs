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

using PdfSharp.Core.Enums;

namespace PdfSharp.Drawing
{
  /// <summary>
  /// Specifies details about how the font is used in PDF creation.
  /// </summary>
  public class XPdfFontOptions
  {
    internal XPdfFontOptions() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="XPdfFontOptions"/> class.
    /// </summary>
    public XPdfFontOptions(PdfFontEncoding encoding,  PdfFontEmbedding embedding)
    {
      fontEncoding = encoding;
      fontEmbedding = embedding;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XPdfFontOptions"/> class.
    /// </summary>
    public XPdfFontOptions(PdfFontEncoding encoding)
    {
      fontEncoding = encoding;
      fontEmbedding = PdfFontEmbedding.None;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XPdfFontOptions"/> class.
    /// </summary>
    public XPdfFontOptions(PdfFontEmbedding embedding)
    {
      fontEncoding = PdfFontEncoding.WinAnsi;
      fontEmbedding = embedding;
    }

    /// <summary>
    /// Gets a value indicating the font embedding.
    /// </summary>
    public PdfFontEmbedding FontEmbedding
    {
      get { return this.fontEmbedding; }
    }
    PdfFontEmbedding fontEmbedding;

    /// <summary>
    /// Gets a value indicating how the font is encoded.
    /// </summary>
    public PdfFontEncoding FontEncoding
    {
      get { return this.fontEncoding; }
    }
    PdfFontEncoding fontEncoding;
  }
}
