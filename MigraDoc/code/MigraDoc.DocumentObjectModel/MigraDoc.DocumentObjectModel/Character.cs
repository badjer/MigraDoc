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

namespace MigraDoc.DocumentObjectModel
{
  /// <summary>
  /// Represents a special character in paragraph text.
  /// </summary>
  // TODO: So ändern, dass symbolName und char in unterschiedlichen Feldern gespeichert wird
  public class Character : DocumentObject
  {
    // \space
    public static readonly Character Blank = new Character(PdfSharp.Core.Enums.SymbolName.Blank);
	public static readonly Character En = new Character(PdfSharp.Core.Enums.SymbolName.En);
	public static readonly Character Em = new Character(PdfSharp.Core.Enums.SymbolName.Em);
	public static readonly Character EmQuarter = new Character(PdfSharp.Core.Enums.SymbolName.EmQuarter);
	public static readonly Character Em4 = new Character(PdfSharp.Core.Enums.SymbolName.Em4);

    // used to serialize as \tab, \linebreak
	public static readonly Character Tab = new Character(PdfSharp.Core.Enums.SymbolName.Tab);
	public static readonly Character LineBreak = new Character(PdfSharp.Core.Enums.SymbolName.LineBreak);
    //public static readonly Character MarginBreak         = new Character(SymbolName.MarginBreak);

    // \symbol
	public static readonly Character Euro = new Character(PdfSharp.Core.Enums.SymbolName.Euro);
	public static readonly Character Copyright = new Character(PdfSharp.Core.Enums.SymbolName.Copyright);
	public static readonly Character Trademark = new Character(PdfSharp.Core.Enums.SymbolName.Trademark);
	public static readonly Character RegisteredTrademark = new Character(PdfSharp.Core.Enums.SymbolName.RegisteredTrademark);
	public static readonly Character Bullet = new Character(PdfSharp.Core.Enums.SymbolName.Bullet);
	public static readonly Character Not = new Character(PdfSharp.Core.Enums.SymbolName.Not);
	public static readonly Character EmDash = new Character(PdfSharp.Core.Enums.SymbolName.EmDash);
	public static readonly Character EnDash = new Character(PdfSharp.Core.Enums.SymbolName.EnDash);
	public static readonly Character NonBreakableBlank = new Character(PdfSharp.Core.Enums.SymbolName.NonBreakableBlank);
	public static readonly Character HardBlank = new Character(PdfSharp.Core.Enums.SymbolName.HardBlank);

    /// <summary>
    /// Initializes a new instance of the Character class.
    /// </summary>
    public Character()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Character class with the specified parent.
    /// </summary>
    internal Character(DocumentObject parent) : base(parent) { }

    /// <summary>
    /// Initializes a new instance of the Character class with the specified SymbolName.
    /// </summary>
    Character(SymbolName name)
      : this()
    {
      symbolName = name;
    }

    #region Properties
    /// <summary>
    /// Gets or sets the SymbolName. Returns 0 if the type is defined by a character.
    /// </summary>
    public SymbolName? SymbolName
    {
      get { return this.symbolName; }
      set { this.symbolName = value; }
    }
    
    internal SymbolName? symbolName;

    /// <summary>
    /// Gets or sets the SymbolName as character. Returns 0 if the type is defined via an enum.
    /// </summary>
    public char Char
    {
      get
      {
        if (((uint)symbolName.Value & 0xF0000000) == 0)
          return (char)symbolName.Value;
        else
          return '\0';
      }
	  //set { this.symbolName = value; }
    }

    /// <summary>
    /// Gets or sets the number of times the character is repeated.
    /// </summary>
    public int Count
    {
      get { return count.GetValueOrDefault(); }
      set { count = value; }
    }
    
    internal int? count = 1;
    #endregion
  }
}
