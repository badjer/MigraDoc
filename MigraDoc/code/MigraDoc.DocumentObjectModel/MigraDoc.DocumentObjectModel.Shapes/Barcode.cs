#region MigraDoc - Creating Documents on the Fly
//
// Authors:
//   Stefan Lange (mailto:Stefan.Lange@pdfsharp.com)
//   Klaus Potzesny (mailto:Klaus.Potzesny@pdfsharp.com)
//   David Stephensen (mailto:David.Stephensen@pdfsharp.com)
//
// Copyright (c) 2001-2009 empira Software GmbH, Cologne (Germany)
//
// http://www.pdfsharp.com
// http://www.migradoc.com
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

namespace MigraDoc.DocumentObjectModel.Shapes
{
  /// <summary>
  /// Represents a barcode in the document or paragraph. !!!Still under Construction!!!
  /// </summary>
  public class Barcode : Shape
  {
    /// <summary>
    /// Initializes a new instance of the Barcode class.
    /// </summary>
    internal Barcode()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Barcode class with the specified parent.
    /// </summary>
    internal Barcode(DocumentObject parent) : base(parent) { }

    #region Methods
    /// <summary>
    /// Creates a deep copy of this object.
    /// </summary>
    public new Barcode Clone()
    {
      return (Barcode)DeepCopy();
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the text orientation for the barcode content.
    /// </summary>
    public TextOrientation Orientation
    {
      get { return (TextOrientation)this.orientation.GetValueOrDefault(); }
      set { this.orientation = value; }
    }

	internal TextOrientation? orientation;

    /// <summary>
    /// Gets or sets the type of the barcode.
    /// </summary>
    public BarcodeType? Type
    {
      get { return (BarcodeType)this.type; }
      set { this.type = value; }
    }

	internal BarcodeType? type;

    /// <summary>
    /// Gets or sets a value indicating whether bars shall appear beside the barcode
    /// </summary>
    public bool BearerBars
    {
		get { return this.bearerBars.GetValueOrDefault(); }
      set { this.bearerBars = value; }
    }

	internal bool? bearerBars;

    /// <summary>
    /// Gets or sets the a value indicating whether the barcode's code is rendered.
    /// </summary>
    public bool Text
    {
		get { return this.text.GetValueOrDefault(); }
      set { this.text = value; }
    }

	internal bool? text;

    /// <summary>
    /// Gets or sets code the barcode represents.
    /// </summary>
    public string Code
    {
      get { return this.code; }
      set { this.code = value; }
    }

	internal string code;

    /// <summary>
    /// ???
    /// </summary>
    public double LineRatio
    {
		get { return this.lineRatio.GetValueOrDefault(); }
      set { this.lineRatio = value; }
    }

	internal double? lineRatio;

    /// <summary>
    /// ???
    /// </summary>
    public double LineHeight
    {
		get { return this.lineHeight.GetValueOrDefault(); }
      set { this.lineHeight = value; }
    }

	internal double? lineHeight;

    /// <summary>
    /// ???
    /// </summary>
    public double NarrowLineWidth
    {
		get { return this.narrowLineWidth.GetValueOrDefault(); }
      set { this.narrowLineWidth = value; }
    }

	internal double? narrowLineWidth;
    #endregion

  }
}
