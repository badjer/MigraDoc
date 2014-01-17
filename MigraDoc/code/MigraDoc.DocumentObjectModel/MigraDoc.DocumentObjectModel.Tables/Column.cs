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

namespace MigraDoc.DocumentObjectModel.Tables
{
  /// <summary>
  /// Represents a column of a table.
  /// </summary>
  public class Column : DocumentObject
  {
    /// <summary>
    /// Initializes a new instance of the Column class.
    /// </summary>
    public Column()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Column class with the specified parent.
    /// </summary>
    internal Column(DocumentObject parent) : base(parent) { }

    #region Methods
    /// <summary>
    /// Creates a deep copy of this object.
    /// </summary>
    public new Column Clone()
    {
      return (Column)DeepCopy();
    }

    /// <summary>
    /// Implements the deep copy of the object.
    /// </summary>
    protected override object DeepCopy()
    {
      Column column = (Column)base.DeepCopy();
      if (column.format != null)
      {
        column.format = column.format.Clone();
        column.format.parent = column;
      }
      if (column.borders != null)
      {
        column.borders = column.borders.Clone();
        column.borders.parent = column;
      }
      if (column.shading != null)
      {
        column.shading = column.shading.Clone();
        column.shading.parent = column;
      }
      return column;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets the table the Column belongs to.
    /// </summary>
    public Table Table
    {
      get
      {
        if (this.table == null)
        {
          Columns clms = this.Parent as Columns;
          if (clms != null)
            this.table = clms.Parent as Table;
        }
        return this.table;
      }
    }
    Table table;

    /// <summary>
    /// Gets the index of the column. First column has index 0.
    /// </summary>
    public int Index
    {
      get
      {
        if (!index.HasValue)
        {
          Columns clms = (Columns)Parent;
          index = clms.IndexOf(this);
        }
        return index.GetValueOrDefault();
      }
    }
    
    internal int? index;

    /// <summary>
    /// Gets a cell by its row index. The first cell has index 0.
    /// </summary>
    public Cell this[int idx]
    {
      get
      {
        //Check.ArgumentOutOfRange(index >= 0 && index < table.Rows.Count, "index");
		  return Table.Rows[idx][index.GetValueOrDefault()];
      }
    }

    /// <summary>
    /// Sets or gets the default style name for all cells of the column.
    /// </summary>
    public string Style
    {
      get { return this.style; }
      set { this.style = value; }
    }

	internal string style;

    /// <summary>
    /// Gets the default ParagraphFormat for all cells of the column.
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
    /// Gets or sets the width of a column.
    /// </summary>
    public Unit Width
    {
      get { return this.width; }
      set { this.width = value; }
    }
    
    internal Unit width = Unit.NullValue;

    /// <summary>
    /// Gets or sets the default left padding for all cells of the column.
    /// </summary>
    public Unit LeftPadding
    {
      get { return this.leftPadding; }
      set { this.leftPadding = value; }
    }
    
    internal Unit leftPadding = Unit.NullValue;

    /// <summary>
    /// Gets or sets the default right padding for all cells of the column.
    /// </summary>
    public Unit RightPadding
    {
      get { return this.rightPadding; }
      set { this.rightPadding = value; }
    }
    
    internal Unit rightPadding = Unit.NullValue;

    /// <summary>
    /// Gets the default Borders object for all cells of the column.
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
    /// Gets or sets the number of columns that should be kept together with
    /// current column in case of a page break.
    /// </summary>
    public int KeepWith
    {
      get { return this.keepWith.GetValueOrDefault(); }
      set { this.keepWith = value; }
    }
    
    internal int? keepWith;

    /// <summary>
    /// Gets or sets a value which define whether the column is a header.
    /// </summary>
    public bool HeadingFormat
    {
		get { return this.headingFormat.GetValueOrDefault(); }
      set { this.headingFormat = value; }
    }

	internal bool? headingFormat;

    /// <summary>
    /// Gets the default Shading object for all cells of the column.
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
    /// Gets or sets a comment associated with this object.
    /// </summary>
    public string Comment
    {
      get { return this.comment; }
      set { this.comment = value; }
    }

	internal string comment;
    #endregion

  }
}
