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

namespace MigraDoc.DocumentObjectModel.Tables
{
  /// <summary>
  /// Represents the columns of a table.
  /// </summary>
  public class Columns : DocumentObjectCollection, IVisitable
  {
    /// <summary>
    /// Initializes a new instance of the Columns class.
    /// </summary>
    public Columns()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Columns class containing columns of the specified widths.
    /// </summary>
    public Columns(params Unit[] widths)
    {
      foreach (Unit width in widths)
      {
        Column clm = new Column();
        clm.Width = width;
        this.Add(clm);
      }
    }

    /// <summary>
    /// Initializes a new instance of the Columns class with the specified parent.
    /// </summary>
    internal Columns(DocumentObject parent) : base(parent) { }

    #region Methods
    /// <summary>
    /// Creates a deep copy of this object.
    /// </summary>
    public new Columns Clone()
    {
      return (Columns)base.DeepCopy();
    }

    /// <summary>
    /// Adds a new column to the columns collection. Allowed only before any row was added.
    /// </summary>
    public Column AddColumn()
    {
      if (Table.Rows.Count > 0)
        throw new InvalidOperationException("Cannot add column because rows collection is not empty.");

      Column column = new Column();
      Add(column);
      return column;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets the table the columns collection belongs to.
    /// </summary>
    public Table Table
    {
      get { return this.parent as Table; }
    }

    /// <summary>
    /// Gets a column by its index.
    /// </summary>
    public new Column this[int index]
    {
      get { return base[index] as Column; }
    }

    /// <summary>
    /// Gets or sets the default width of all columns.
    /// </summary>
    public Unit Width
    {
      get { return this.width; }
      set { this.width = value; }
    }
    
    internal Unit width = Unit.NullValue;

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

    #region Internal

    /// <summary>
    /// Allows the visitor object to visit the document object and it's child objects.
    /// </summary>
    void IVisitable.AcceptVisitor(DocumentObjectVisitor visitor, bool visitChildren)
    {
      visitor.VisitColumns(this);
    }
    #endregion
  }
}
