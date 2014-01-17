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

using MigraDoc.DocumentObjectModel.Internals;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.Visitors;

namespace MigraDoc.DocumentObjectModel.Shapes.Charts
{
  /// <summary>
  /// An area object in the chart which contain text or legend.
  /// </summary>
  public class TextArea : ChartObject, IVisitable
  {
    /// <summary>
    /// Initializes a new instance of the TextArea class.
    /// </summary>
    internal TextArea()
    {
    }

    /// <summary>
    /// Initializes a new instance of the TextArea class with the specified parent.
    /// </summary>
    internal TextArea(DocumentObject parent) : base(parent) { }

    #region Methods
    /// <summary>
    /// Creates a deep copy of this object.
    /// </summary>
    public new TextArea Clone()
    {
      return (TextArea)DeepCopy();
    }

    /// <summary>
    /// Implements the deep copy of the object.
    /// </summary>
    protected override object DeepCopy()
    {
      TextArea textArea = (TextArea)base.DeepCopy();
      if (textArea.format != null)
      {
        textArea.format = textArea.format.Clone();
        textArea.format.parent = textArea;
      }
      if (textArea.lineFormat != null)
      {
        textArea.lineFormat = textArea.lineFormat.Clone();
        textArea.lineFormat.parent = textArea;
      }
      if (textArea.fillFormat != null)
      {
        textArea.fillFormat = textArea.fillFormat.Clone();
        textArea.fillFormat.parent = textArea;
      }
      if (textArea.elements != null)
      {
        textArea.elements = textArea.elements.Clone();
        textArea.elements.parent = textArea;
      }
      return textArea;
    }

    /// <summary>
    /// Adds a new paragraph to the text area.
    /// </summary>
    public Paragraph AddParagraph()
    {
      return this.Elements.AddParagraph();
    }

    /// <summary>
    /// Adds a new paragraph with the specified text to the text area.
    /// </summary>
    public Paragraph AddParagraph(string paragraphText)
    {
      return this.Elements.AddParagraph(paragraphText);
    }

    /// <summary>
    /// Adds a new table to the text area.
    /// </summary>
    public Table AddTable()
    {
      return this.Elements.AddTable();
    }

    /// <summary>
    /// Adds a new Image to the text area.
    /// </summary>
    public Image AddImage(string fileName)
    {
      return this.Elements.AddImage(fileName);
    }

    /// <summary>
    /// Adds a new legend to the text area.
    /// </summary>
    public Legend AddLegend()
    {
      return this.Elements.AddLegend();
    }

    /// <summary>
    /// Adds a new paragraph to the text area.
    /// </summary>
    public void Add(Paragraph paragraph)
    {
      this.Elements.Add(paragraph);
    }

    /// <summary>
    /// Adds a new table to the text area.
    /// </summary>
    public void Add(Table table)
    {
      this.Elements.Add(table);
    }

    /// <summary>
    /// Adds a new image to the text area.
    /// </summary>
    public void Add(Image image)
    {
      this.Elements.Add(image);
    }

    /// <summary>
    /// Adds a new legend to the text area.
    /// </summary>
    public void Add(Legend legend)
    {
      this.Elements.Add(legend);
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the height of the area.
    /// </summary>
    public Unit Height
    {
      get { return this.height; }
      set { this.height = value; }
    }
    
    internal Unit height = Unit.NullValue;

    /// <summary>
    /// Gets or sets the width of the area.
    /// </summary>
    public Unit Width
    {
      get { return this.width; }
      set { this.width = value; }
    }
    
    internal Unit width = Unit.NullValue;

    /// <summary>
    /// Gets or sets the default style name of the area.
    /// </summary>
    public string Style
    {
      get { return this.style; }
      set { this.style = value; }
    }

	internal string style;

    /// <summary>
    /// Gets or sets the default paragraph format of the area.
    /// </summary>
    public ParagraphFormat Format
    {
      get
      {
        if (this.format == null)
          this.format = new ParagraphFormat(this);

        return this.format;
      }
      set
      {
        SetParent(value);
        this.format = value;
      }
    }
    
    internal ParagraphFormat format;

    /// <summary>
    /// Gets the line format of the area's border.
    /// </summary>
    public LineFormat LineFormat
    {
      get
      {
        if (this.lineFormat == null)
          this.lineFormat = new LineFormat(this);

        return this.lineFormat;
      }
      set
      {
        SetParent(value);
        this.lineFormat = value;
      }
    }
    
    internal LineFormat lineFormat;

    /// <summary>
    /// Gets the background filling of the area.
    /// </summary>
    public FillFormat FillFormat
    {
      get
      {
        if (this.fillFormat == null)
          this.fillFormat = new FillFormat(this);

        return this.fillFormat;
      }
      set
      {
        SetParent(value);
        this.fillFormat = value;
      }
    }
    
    internal FillFormat fillFormat;

    /// <summary>
    /// Gets or sets the left padding of the area.
    /// </summary>
    public Unit LeftPadding
    {
      get { return this.leftPadding; }
      set { this.leftPadding = value; }
    }
    
    internal Unit leftPadding = Unit.NullValue;

    /// <summary>
    /// Gets or sets the right padding of the area.
    /// </summary>
    public Unit RightPadding
    {
      get { return this.rightPadding; }
      set { this.rightPadding = value; }
    }
    
    internal Unit rightPadding = Unit.NullValue;

    /// <summary>
    /// Gets or sets the top padding of the area.
    /// </summary>
    public Unit TopPadding
    {
      get { return this.topPadding; }
      set { this.topPadding = value; }
    }
    
    internal Unit topPadding = Unit.NullValue;

    /// <summary>
    /// Gets or sets the bottom padding of the area.
    /// </summary>
    public Unit BottomPadding
    {
      get { return this.bottomPadding; }
      set { this.bottomPadding = value; }
    }
    
    internal Unit bottomPadding = Unit.NullValue;

    /// <summary>
    /// Gets or sets the Vertical alignment of the area.
    /// </summary>
    public VerticalAlignment? VerticalAlignment
    {
      get { return this.verticalAlignment; }
      set { this.verticalAlignment = value; }
    }
    
    internal VerticalAlignment? verticalAlignment;

    /// <summary>
    /// Gets the document objects that creates the text area.
    /// </summary>
    public DocumentElements Elements
    {
      get
      {
        if (this.elements == null)
          this.elements = new DocumentElements(this);

        return this.elements;
      }
      set
      {
        SetParent(value);
        this.elements = value;
      }
    }
    
    internal DocumentElements elements;
    #endregion

    #region Internal

	  
    #endregion

    void IVisitable.AcceptVisitor(DocumentObjectVisitor visitor, bool visitChildren)
    {
      visitor.VisitTextArea(this);
      if (this.elements != null && visitChildren)
        ((IVisitable)this.elements).AcceptVisitor(visitor, visitChildren);
    }
  }
}
