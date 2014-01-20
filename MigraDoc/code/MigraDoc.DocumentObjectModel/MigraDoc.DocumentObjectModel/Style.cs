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
using MigraDoc.DocumentObjectModel.Visitors;
using PdfSharp.Core.Enums;

namespace MigraDoc.DocumentObjectModel
{
  /// <summary>
  /// Represents style templates for paragraph or character formatting
  /// </summary>
  public sealed class Style : DocumentObject, IVisitable
  {
    /// <summary>
    /// Initializes a new instance of the Style class.
    /// </summary>
    internal Style()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Style class with the specified parent.
    /// </summary>
    internal Style(DocumentObject parent) : base(parent) { }

    /// <summary>
    /// Initializes a new instance of the Style class with name and base style name.
    /// </summary>
    public Style(string name, string baseStyleName)
      : this()
    {
      // baseStyleName can be null or empty
      if (name == null)
        throw new ArgumentNullException("name");
      if (name == "")
        throw new ArgumentException("name");

      this.name = name;
      this.baseStyle = baseStyleName;
    }

    #region Methods
    /// <summary>
    /// Creates a deep copy of this object.
    /// </summary>
    public new Style Clone()
    {
      return (Style)DeepCopy();
    }

    /// <summary>
    /// Implements the deep copy of the object.
    /// </summary>
    protected override object DeepCopy()
    {
      Style style = (Style)base.DeepCopy();
      if (style.paragraphFormat != null)
      {
        style.paragraphFormat = style.paragraphFormat.Clone();
        style.paragraphFormat.parent = style;
      }
      return style;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Indicates whether the style is read-only. 
    /// </summary>
    public bool IsReadOnly
    {
      get { return this.readOnly; }
    }
    internal bool readOnly;

    /// <summary>
    /// Gets the font of ParagraphFormat. 
    /// Calling style.Font is just a shortcut to style.ParagraphFormat.Font.
    /// </summary>
    
    public Font Font
    {
      get { return ParagraphFormat.Font; }
      // SetParent will be called inside ParagraphFormat.
      set { ParagraphFormat.Font = value; }
    }

    /// <summary>
    /// Gets the name of the style.
    /// </summary>
    public string Name
    {
      get { return this.name; }
    }
    
    internal string name;

    /// <summary>
    /// Gets the ParagraphFormat. To prevent read-only styles from being modified, a copy of its ParagraphFormat
    /// is returned in this case.
    /// </summary>
    public ParagraphFormat ParagraphFormat
    {
      get
      {
        if (this.paragraphFormat == null)
          this.paragraphFormat = new ParagraphFormat(this);
        if (this.readOnly)
          return this.paragraphFormat.Clone();
        return this.paragraphFormat;
      }
      set
      {
        SetParent(value);
        this.paragraphFormat = value;
      }
    }
    
    internal ParagraphFormat paragraphFormat;

    /// <summary>
    /// Gets or sets the name of the base style.
    /// </summary>
    public string BaseStyle
    {
      get { return baseStyle; }
      set
      {
        if (value == null || value == "" && baseStyle != "") //!!!modTHHO 17.07.2007: Self assignment is allowed
          throw new ArgumentException(DomSR.EmptyBaseStyle);

        // Self assignment is allowed
        if (String.Compare(baseStyle, value, true) == 0)
        {
          baseStyle = value;  // character case may change...
          return;
        }

        if (String.Compare(this.name, Style.DefaultParagraphName, true) == 0 ||
            String.Compare(this.name, Style.DefaultParagraphFontName, true) == 0)
        {
          string msg = String.Format("Style '{0}' has no base style and that cannot be altered.", this.name);
          throw new ArgumentException(msg);
        }

        Styles styles = (Styles)this.parent;
        // The base style must exists
        int idxBaseStyle = styles.GetIndex(value);
        if (idxBaseStyle == -1)
        {
          string msg = String.Format("Base style '{0}' does not exist.", value);
          throw new ArgumentException(msg);
        }
        if (idxBaseStyle > 1)
        {
          // Is this style in the base style chain of the new base style
          Style style = styles[idxBaseStyle] as Style;
          while (style != null)
          {
            if (style == this)
            {
              string msg = String.Format("Base style '{0}' leads to a circular dependency.", value);
              throw new ArgumentException(msg);
            }
            style = styles[style.BaseStyle];
          }
        }

        // Now setting new base style is save
        baseStyle = value;
      }
    }
    
    internal string baseStyle;

    /// <summary>
    /// Gets the StyleType of the style.
    /// </summary>
    public StyleType Type
    {
      get
      {
        if (!this.styleType.HasValue)
        {
          if (String.Compare(this.baseStyle, DefaultParagraphFontName, true) == 0)
            this.styleType = StyleType.Character;
          else
          {
            Style baseStyle = GetBaseStyle();
            if (baseStyle == null)
              throw new InvalidOperationException("User defined style has no valid base Style.");

            this.styleType = baseStyle.Type;
          }
        }
        return this.styleType.Value;
      }
    }
    
    internal StyleType? styleType;

    /// <summary>
    /// Determines whether the style is the style Normal or DefaultParagraphFont.
    /// </summary>
    internal bool IsRootStyle
    {
      get
      {
        return String.Compare(this.Name, DefaultParagraphFontName, true) == 0 ||
               String.Compare(this.Name, DefaultParagraphName, true) == 0;
      }
    }

    /// <summary>
    /// Get the BaseStyle of the current style.
    /// </summary>
    public Style GetBaseStyle()
    {
      if (IsRootStyle)
        return null;

      Styles styles = Parent as Styles;
      if (styles == null)
        //??? 'owner of a parent'? eher 'owned by a parent' oder einfach: "A parent object is required for this operation."
        throw new InvalidOperationException("This instance of 'style' is currently not owner of a parent; access failed");
      if (this.baseStyle == "")
        throw new ArgumentException("User defined Style defined without a BaseStyle");

      //REVIEW KlPo4StLa Spezialbehandlung f¸r den DefaultParagraphFont kr¸ppelig(DefaultParagraphFont wird bei zugrif ¸ber styles["name"] nicht zur¸ckgeliefert).
      //Da hast Du Recht -> siehe IsReadOnly
      if (this.baseStyle == DefaultParagraphFontName)
        return styles[0];

      return styles[this.baseStyle];
    }

    /// <summary>
    /// Indicates whether the style is a predefined (build in) style.
    /// </summary>
    public bool BuildIn
    {
      get { return this.buildIn; }
    }
    
    internal bool buildIn;
    // THHO: muss dass nicht builtIn heiﬂen?!?!?!?

    /// <summary>
    /// Gets or sets a comment associated with this object.
    /// </summary>
    public string Comment
    {
      get { return this.comment; }
      set { this.comment = value; }
    }

	  internal string comment;
    #endregion

    // Names of the root styles. Root styles have no BaseStyle.

    /// <summary>
    /// Name of the default character style.
    /// </summary>
    public const string DefaultParagraphFontName = "DefaultParagraphFont";

    /// <summary>
    /// Name of the default paragraph style.
    /// </summary>
    public const string DefaultParagraphName = "Normal";

    #region Internal

	  /// <summary>
    /// Allows the visitor object to visit the document object and it's child objects.
    /// </summary>
    void IVisitable.AcceptVisitor(DocumentObjectVisitor visitor, bool visitChildren)
    {
      visitor.VisitStyle(this);
    }

	  
    #endregion
  }
}
