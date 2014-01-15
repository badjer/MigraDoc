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
using MigraDoc.DocumentObjectModel.Visitors;

namespace MigraDoc.DocumentObjectModel.Tables
{
  /// <summary>
  /// Represents a row of a table.
  /// </summary>
  public class Row : DocumentObject, IVisitable
  {
    /// <summary>
    /// Initializes a new instance of the Row class.
    /// </summary>
    public Row()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Row class with the specified parent.
    /// </summary>
    internal Row(DocumentObject parent) : base(parent) { }

    #region Methods
    /// <summary>
    /// Creates a deep copy of this object.
    /// </summary>
    public new Row Clone()
    {
      return (Row)DeepCopy();
    }

    /// <summary>
    /// Implements the deep copy of the object.
    /// </summary>
    protected override object DeepCopy()
    {
      Row row = (Row)base.DeepCopy();
      if (row.format != null)
      {
        row.format = row.format.Clone();
        row.format.parent = row;
      }
      if (row.borders != null)
      {
        row.borders = row.borders.Clone();
        row.borders.parent = row;
      }
      if (row.shading != null)
      {
        row.shading = row.shading.Clone();
        row.shading.parent = row;
      }
      if (row.cells != null)
      {
        row.cells = row.cells.Clone();
        row.cells.parent = row;
      }
      return row;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets the table the row belongs to.
    /// </summary>
    public Table Table
    {
      get
      {
        if (this.table == null)
        {
          Rows rws = this.Parent as Rows;
          if (rws != null)
            this.table = rws.Table;
        }
        return this.table;
      }
    }
    Table table;

    /// <summary>
    /// Gets the index of the row. First row has index 0.
    /// </summary>
    public int Index
    {
      get
      {
        if (index.IsNull)
        {
          Rows rws = this.parent as Rows;
          index = rws.IndexOf(this);
        }
        return index;
      }
    }
    
    internal NInt index = NInt.NullValue;

    /// <summary>
    /// Gets a cell by its column index. The first cell has index 0.
    /// </summary>
    public Cell this[int index]
    {
      get { return Cells[index]; }
    }

    /// <summary>
    /// Gets or sets the default style name for all cells of the row.
    /// </summary>
    public string Style
    {
      get { return this.style.Value; }
      set { this.style.Value = value; }
    }
    
    internal NString style = NString.NullValue;

    /// <summary>
    /// Gets the default ParagraphFormat for all cells of the row.
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
    /// Gets or sets the default vertical alignment for all cells of the row.
    /// </summary>
    public VerticalAlignment VerticalAlignment
    {
      get { return (VerticalAlignment)this.verticalAlignment.Value; }
      set { this.verticalAlignment.Value = (int)value; }
    }
    
    internal NEnum verticalAlignment = NEnum.NullValue(typeof(VerticalAlignment));

    /// <summary>
    /// Gets or sets the height of the row.
    /// </summary>
    public Unit Height
    {
      get { return this.height; }
      set { this.height = value; }
    }
    
    internal Unit height = Unit.NullValue;

    /// <summary>
    /// Gets or sets the rule which is used to determine the height of the row.
    /// </summary>
    public RowHeightRule HeightRule
    {
      get { return (RowHeightRule)this.heightRule.Value; }
      set { this.heightRule.Value = (int)value; }
    }
    
    internal NEnum heightRule = NEnum.NullValue(typeof(RowHeightRule));

    /// <summary>
    /// Gets or sets the default value for all cells of the row.
    /// </summary>
    public Unit TopPadding
    {
      get { return this.topPadding; }
      set { this.topPadding = value; }
    }
    
    internal Unit topPadding = Unit.NullValue;

    /// <summary>
    /// Gets or sets the default value for all cells of the row.
    /// </summary>
    public Unit BottomPadding
    {
      get { return this.bottomPadding; }
      set { this.bottomPadding = value; }
    }
    
    internal Unit bottomPadding = Unit.NullValue;

    /// <summary>
    /// Gets or sets a value which define whether the row is a header.
    /// </summary>
    public bool HeadingFormat
    {
      get { return this.headingFormat.Value; }
      set { this.headingFormat.Value = value; }
    }
    
    internal NBool headingFormat = NBool.NullValue;

    /// <summary>
    /// Gets the default Borders object for all cells of the row.
    /// </summary>
    public Borders Borders
    {
      get
      {
        if (this.borders == null)
          this.borders = new Borders(this);

        return this.borders;
      }
      set
      {
        SetParent(value);
        this.borders = value;
      }
    }
    
    internal Borders borders;

    /// <summary>
    /// Gets the default Shading object for all cells of the row.
    /// </summary>
    public Shading Shading
    {
      get
      {
        if (this.shading == null)
          this.shading = new Shading(this);

        return this.shading;
      }
      set
      {
        SetParent(value);
        this.shading = value;
      }
    }
    
    internal Shading shading;

    /// <summary>
    /// Gets or sets the number of rows that should be
    /// kept together with the current row in case of a page break.
    /// </summary>
    public int KeepWith
    {
      get { return this.keepWith.Value; }
      set { this.keepWith.Value = value; }
    }
    
    internal NInt keepWith = NInt.NullValue;

    /// <summary>
    /// Gets the Cells collection of the table.
    /// </summary>
    public Cells Cells
    {
      get
      {
        if (this.cells == null)
          this.cells = new Cells(this);

        return this.cells;
      }
      set
      {
        SetParent(value);
        this.cells = value;
      }
    }
    
    internal Cells cells;

    /// <summary>
    /// Gets or sets a comment associated with this object.
    /// </summary>
    public string Comment
    {
      get { return this.comment.Value; }
      set { this.comment.Value = value; }
    }
    
    internal NString comment = NString.NullValue;
    #endregion

    #region Internal

    /// <summary>
    /// Allows the visitor object to visit the document object and it's child objects.
    /// </summary>
    void IVisitable.AcceptVisitor(DocumentObjectVisitor visitor, bool visitChildren)
    {
      visitor.VisitRow(this);

      foreach (Cell cell in this.cells)
        ((IVisitable)cell).AcceptVisitor(visitor, visitChildren);
    }

	  
    #endregion
  }
}
