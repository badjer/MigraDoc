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

using System;
using System.Diagnostics;
using System.Reflection;

namespace MigraDoc.DocumentObjectModel.Shapes
{
  /// <summary>
  /// A PictureFormat object.
  /// Used to set more detailed image attributes
  /// </summary>
  public class PictureFormat : DocumentObject
  {
    /// <summary>
    /// Initializes a new instance of the PictureFormat class.
    /// </summary>
    public PictureFormat()
    {
    }

    /// <summary>
    /// Initializes a new instance of the PictureFormat class with the specified parent.
    /// </summary>
    internal PictureFormat(DocumentObject parent) : base(parent) { }

    #region Methods
    /// <summary>
    /// Creates a deep copy of this object.
    /// </summary>
    public new PictureFormat Clone()
    {
      return (PictureFormat)DeepCopy();
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the part cropped from the left of the image.
    /// </summary>
    public Unit CropLeft
    {
      get { return this.cropLeft; }
      set { this.cropLeft = value; }
    }
    
    protected Unit cropLeft = Unit.NullValue;

    /// <summary>
    /// Gets or sets the part cropped from the right of the image.
    /// </summary>
    public Unit CropRight
    {
      get { return this.cropRight; }
      set { this.cropRight = value; }
    }
    
    protected Unit cropRight = Unit.NullValue;

    /// <summary>
    /// Gets or sets the part cropped from the top of the image.
    /// </summary>
    public Unit CropTop
    {
      get { return this.cropTop; }
      set { this.cropTop = value; }
    }
    
    protected Unit cropTop = Unit.NullValue;

    /// <summary>
    /// Gets or sets the part cropped from the bottom of the image.
    /// </summary>
    public Unit CropBottom
    {
      get { return this.cropBottom; }
      set { this.cropBottom = value; }
    }
    
    protected Unit cropBottom = Unit.NullValue;
    #endregion

    #region Internal

	  
    #endregion
  }
}
