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
using MigraDoc.DocumentObjectModel.Internals;

namespace MigraDoc.DocumentObjectModel
{
  /// <summary>
  /// Font represents the formatting of characters in a paragraph.
  /// </summary>
  public sealed class Font : DocumentObject
  {
    /// <summary>
    /// Initializes a new instance of the Font class that can be used as a template.
    /// </summary>
    public Font()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Font class with the specified parent.
    /// </summary>
    internal Font(DocumentObject parent) : base(parent) { }

    /// <summary>
    /// Initializes a new instance of the Font class with the specified name and size.
    /// </summary>
    public Font(string name, Unit size)
    {
      this.name = name;
      this.size.Value = size;
    }

    /// <summary>
    /// Initializes a new instance of the Font class with the specified name.
    /// </summary>
    public Font(string name)
    {
      this.name = name;
    }

    #region Methods
    /// <summary>
    /// Creates a copy of the Font.
    /// </summary>
    public new Font Clone()
    {
      return (Font)DeepCopy();
    }

    /// <summary>
    /// Applies all non-null properties of a font to this font if the given font's property is different from the given refFont's property.
    /// </summary>
    internal void ApplyFont(Font font, Font refFont)
    {
      if (font == null)
        throw new ArgumentNullException("font");

      if ((!string.IsNullOrEmpty(font.name)) && (refFont == null || font.Name != refFont.Name))
        this.Name = font.Name;

      if (!font.size.IsNull && (refFont == null || font.Size != refFont.Size))
        this.Size = font.Size;

      if (font.bold.HasValue && (refFont == null || font.Bold != refFont.Bold))
        this.Bold = font.Bold;

      if (font.italic.HasValue && (refFont == null || font.Italic != refFont.Italic))
        this.Italic = font.Italic;

      if (font.subscript.HasValue && (refFont == null || font.Subscript != refFont.Subscript))
        this.Subscript = font.Subscript;
      else if (font.superscript.HasValue && (refFont == null || font.Superscript != refFont.Superscript))
        this.Superscript = font.Superscript;

      if (font.underline.HasValue && (refFont == null || font.Underline != refFont.Underline))
        this.Underline = font.Underline;

      if (!font.color.IsNull && (refFont == null || font.Color.Argb != refFont.Color.Argb))
        this.Color = font.Color;
    }

    /// <summary>
    /// Applies all non-null properties of a font to this font.
    /// </summary>
    public void ApplyFont(Font font)
    {
      if (font == null)
        throw new ArgumentNullException("font");

      if (!string.IsNullOrEmpty(font.name))
        this.Name = font.Name;

      if (!font.size.IsNull)
        this.Size = font.Size;

      if (font.bold.HasValue)
        this.Bold = font.Bold;

      if (font.italic.HasValue)
        this.Italic = font.Italic;

      if (font.subscript.HasValue)
        this.Subscript = font.Subscript;
      else if (font.superscript.HasValue)
        this.Superscript = font.Superscript;

      if (font.underline.HasValue)
        this.Underline = font.Underline;

      if (!font.color.IsNull)
        this.Color = font.Color;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the name of the font.
    /// </summary>
    public string Name
    {
      get { return this.name; }
      set { this.name = value; }
    }

	internal string name;

    /// <summary>
    /// Gets or sets the size of the font.
    /// </summary>
    public Unit Size
    {
      get { return this.size; }
      set { this.size = value; }
    }
    
    internal Unit size = Unit.NullValue;

    /// <summary>
    /// Gets or sets the bold property.
    /// </summary>
    public bool Bold
    {
		get { return this.bold.GetValueOrDefault(); }
      set { this.bold = value; }
    }

	internal bool? bold;

    /// <summary>
    /// Gets or sets the italic property.
    /// </summary>
    public bool Italic
    {
		get { return this.italic.GetValueOrDefault(); }
      set { this.italic = value; }
    }

	internal bool? italic;

    /// <summary>
    /// Gets or sets the underline property.
    /// </summary>
    public Underline? Underline
    {
      get { return this.underline; }
      set { this.underline = value; }
    }
    
    internal Underline? underline;

    /// <summary>
    /// Gets or sets the color property.
    /// </summary>
    public Color Color
    {
      get { return this.color; }
      set { this.color = value; }
    }
    
    internal Color color = Color.Empty;

    /// <summary>
    /// Gets or sets the superscript property.
    /// </summary>
    public bool Superscript
    {
		get { return this.superscript.GetValueOrDefault(); }
      set
      {
        this.superscript = value;
	      this.subscript = null;
      }
    }

	internal bool? superscript;

    /// <summary>
    /// Gets or sets the subscript property.
    /// </summary>
    public bool Subscript
    {
      get { return this.subscript.GetValueOrDefault(); }
      set
      {
        this.subscript = value;
        this.superscript = null;
      }
    }

	internal bool? subscript;

    //  + .Name = "Verdana"
    //  + .Size = 8
    //  + .Bold = False
    //  + .Italic = False
    //  + .Underline = wdUnderlineDouble
    //  * .UnderlineColor = wdColorOrange
    //    .StrikeThrough = False
    //    .DoubleStrikeThrough = False
    //    .Outline = False
    //    .Emboss = False
    //    .Shadow = False
    //    .Hidden = False
    //  * .SmallCaps = False
    //  * .AllCaps = False
    //  + .Color = wdColorAutomatic
    //    .Engrave = False
    //  + .Superscript = False
    //  + .Subscript = False
    //  * .Spacing = 0
    //  * .Scaling = 100
    //  * .Position = 0
    //    .Kerning = 0
    //    .Animation = wdAnimationNone
    #endregion

    /// <summary>
    /// Gets a value indicating whether the specified font exists.
    /// </summary>
    public static bool Exists(string fontName)
    {
      System.Drawing.FontFamily[] families = System.Drawing.FontFamily.Families;
      foreach (System.Drawing.FontFamily family in families)
      {
        if (String.Compare(family.Name, fontName, true) == 0)
          return true;
      }
      return false;
    }
  }
}
