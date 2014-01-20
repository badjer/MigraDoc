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

using System.Collections;
using PdfSharp.Core.Enums;

namespace MigraDoc.DocumentObjectModel
{
  /// <summary>
  /// A Borders collection represents the eight border objects used for paragraphs, tables etc.
  /// </summary>
  public class Borders : DocumentObject, IEnumerable
  {
    /// <summary>
    /// Initializes a new instance of the Borders class.
    /// </summary>
    public Borders()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Borders class with the specified parent.
    /// </summary>
    internal Borders(DocumentObject parent) : base(parent) { }

    /// <summary>
    /// Determines whether a particular border exists.
    /// </summary>
    public bool HasBorder(BorderType type)
    {
	    Border border = GetByType(type);

	    return border != null;
    }

	public Border GetByType(BorderType type)
	{
		if (type == BorderType.Bottom)
		{
			return bottom;
		}

		if (type == BorderType.Top)
		{
			return top;
		}

		if (type == BorderType.Left)
		{
			return left;
		}

		if (type == BorderType.Right)
		{
			return right;
		}

		if (type == BorderType.DiagonalDown)
		{
			return diagonalDown;
		}

		if (type == BorderType.DiagonalUp)
		{
			return diagonalUp;
		}

		return null;
	}

    #region Methods
    /// <summary>
    /// Creates a deep copy of this object.
    /// </summary>
    public new Borders Clone()
    {
      return (Borders)DeepCopy();
    }

    /// <summary>
    /// Implements the deep copy of the object.
    /// </summary>
    protected override object DeepCopy()
    {
      Borders borders = (Borders)base.DeepCopy();
      if (borders.top != null)
      {
        borders.top = borders.top.Clone();
        borders.top.parent = borders;
      }
      if (borders.left != null)
      {
        borders.left = borders.left.Clone();
        borders.left.parent = borders;
      }
      if (borders.right != null)
      {
        borders.right = borders.right.Clone();
        borders.right.parent = borders;
      }
      if (borders.bottom != null)
      {
        borders.bottom = borders.bottom.Clone();
        borders.bottom.parent = borders;
      }
      if (borders.diagonalUp != null)
      {
        borders.diagonalUp = borders.diagonalUp.Clone();
        borders.diagonalUp.parent = borders;
      }
      if (borders.diagonalDown != null)
      {
        borders.diagonalDown = borders.diagonalDown.Clone();
        borders.diagonalDown.parent = borders;
      }
      return borders;
    }

    /// <summary>
    /// Gets an enumerator for the borders object.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator()
    {
      Hashtable ht = new Hashtable();
      ht.Add("Top", this.top);
      ht.Add("Left", this.left);
      ht.Add("Bottom", this.bottom);
      ht.Add("Right", this.right);
      ht.Add("DiagonalUp", this.diagonalUp);
      ht.Add("DiagonalDown", this.diagonalDown);

      return new BorderEnumerator(ht);
    }

    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the top border.
    /// </summary>
    public Border Top
    {
      get
      {
        if (this.top == null)
          this.top = new Border(this);

        return this.top;
      }
      set
      {
        SetParent(value);
        this.top = value;
      }
    }
    
    internal Border top;

    /// <summary>
    /// Gets or sets the left border.
    /// </summary>
    public Border Left
    {
      get
      {
        if (this.left == null)
          this.left = new Border(this);

        return this.left;
      }
      set
      {
        SetParent(value);
        this.left = value;
      }
    }
    
    internal Border left;

    /// <summary>
    /// Gets or sets the bottom border.
    /// </summary>
    public Border Bottom
    {
      get
      {
        if (this.bottom == null)
          this.bottom = new Border(this);

        return this.bottom;
      }
      set
      {
        SetParent(value);
        this.bottom = value;
      }
    }
    
    internal Border bottom;

    /// <summary>
    /// Gets or sets the right border.
    /// </summary>
    public Border Right
    {
      get
      {
        if (this.right == null)
          this.right = new Border(this);

        return this.right;
      }
      set
      {
        SetParent(value);
        this.right = value;
      }
    }
    
    internal Border right;

    /// <summary>
    /// Gets or sets the diagonalup border.
    /// </summary>
    public Border DiagonalUp
    {
      get
      {
        if (this.diagonalUp == null)
          this.diagonalUp = new Border(this);

        return this.diagonalUp;
      }
      set
      {
        SetParent(value);
        this.diagonalUp = value;
      }
    }
    
    internal Border diagonalUp;

    /// <summary>
    /// Gets or sets the diagonaldown border.
    /// </summary>
    public Border DiagonalDown
    {
      get
      {
        if (this.diagonalDown == null)
          this.diagonalDown = new Border(this);

        return diagonalDown;
      }
      set
      {
        SetParent(value);
        this.diagonalDown = value;
      }
    }
    
    internal Border diagonalDown;

    /// <summary>
    /// Gets or sets a value indicating whether the borders are visible.
    /// </summary>
    public bool Visible
    {
		get { return this.visible.GetValueOrDefault(); }
      set { this.visible = value; }
    }

	internal bool? visible;

    /// <summary>
    /// Gets or sets the line style of the borders.
    /// </summary>
	public BorderStyle? Style
    {
      get { return this.style; }
      set { this.style = value; }
    }
    
    internal BorderStyle? style;

    /// <summary>
    /// Gets or sets the standard width of the borders.
    /// </summary>
    public Unit Width
    {
      get { return this.width; }
      set { this.width = value; }
    }
    
    internal Unit width = Unit.NullValue;

    /// <summary>
    /// Gets or sets the color of the borders.
    /// </summary>
    public Color Color
    {
      get { return this.color; }
      set { this.color = value; }
    }
    
    internal Color color = Color.Empty;

    /// <summary>
    /// Gets or sets the distance between text and the top border.
    /// </summary>
    public Unit DistanceFromTop
    {
      get { return this.distanceFromTop; }
      set { this.distanceFromTop = value; }
    }
    
    internal Unit distanceFromTop = Unit.NullValue;

    /// <summary>
    /// Gets or sets the distance between text and the bottom border.
    /// </summary>
    public Unit DistanceFromBottom
    {
      get { return this.distanceFromBottom; }
      set { this.distanceFromBottom = value; }
    }
    
    internal Unit distanceFromBottom = Unit.NullValue;

    /// <summary>
    /// Gets or sets the distance between text and the left border.
    /// </summary>
    public Unit DistanceFromLeft
    {
      get { return this.distanceFromLeft; }
      set { this.distanceFromLeft = value; }
    }
    
    internal Unit distanceFromLeft = Unit.NullValue;

    /// <summary>
    /// Gets or sets the distance between text and the right border.
    /// </summary>
    public Unit DistanceFromRight
    {
      get { return this.distanceFromRight; }
      set { this.distanceFromRight = value; }
    }
    
    internal Unit distanceFromRight = Unit.NullValue;

    /// <summary>
    /// Sets the distance to all four borders to the specified value.
    /// </summary>
    public Unit Distance
    {
      set
      {
        this.DistanceFromTop = value;
        this.DistanceFromBottom = value;
        this.DistanceFromLeft = value;
        this.distanceFromRight = value;
      }
    }

    /// <summary>
    /// Gets the information if the collection is marked as cleared. Additionally 'Borders = null'
    /// is written to the DDL stream when serialized.
    /// </summary>
    public bool BordersCleared
    {
      get { return this.clearAll; }
      set { this.clearAll = value; }
    }
    protected bool clearAll = false;
    #endregion

    #region Internal

    /// <summary>
    /// Gets a name of a border.
    /// </summary>
    internal string GetMyName(Border border)
    {
      if (border == this.top)
        return "Top";
      else if (border == this.bottom)
        return "Bottom";
      else if (border == this.left)
        return "Left";
      else if (border == this.right)
        return "Right";
      else if (border == this.diagonalUp)
        return "DiagonalUp";
      else if (border == this.diagonalDown)
        return "DiagonalDown";
      return null;
    }

    /// <summary>
    /// Returns an enumerator that can iterate through the Borders.
    /// </summary>
    public class BorderEnumerator : IEnumerator
    {
      int index;
      Hashtable ht;

      /// <summary>
      /// Creates a new BorderEnumerator.
      /// </summary>
      public BorderEnumerator(Hashtable ht)
      {
        this.ht = ht;
        index = -1;
      }

      /// <summary>
      /// Sets the enumerator to its initial position, which is before the first element in the border collection.
      /// </summary>
      public void Reset()
      {
        index = -1;
      }

      /// <summary>
      /// Gets the current element in the border collection.
      /// </summary>
      public Border Current
      {
        get
        {
          IEnumerator enumerator = ht.GetEnumerator();
          enumerator.Reset();
          for (int idx = 0; idx < index + 1; idx++)
            enumerator.MoveNext();
          return ((DictionaryEntry)enumerator.Current).Value as Border;
        }
      }

      /// <summary>
      /// Gets the current element in the border collection.
      /// </summary>
      object IEnumerator.Current
      {
        get
        {
          return this.Current;
        }
      }

      /// <summary>
      /// Advances the enumerator to the next element of the border collection.
      /// </summary>
      public bool MoveNext()
      {
        index++;
        return (index < ht.Count);
      }
    }

    #endregion
  }
}
