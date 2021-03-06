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
using PdfSharp.Core.Enums;

namespace MigraDoc.DocumentObjectModel.Shapes
{
  /// <summary>
  /// Represents the left position in a shape.
  /// </summary>
  public struct LeftPosition
  {
    /// <summary>
    /// Initializes a new instance of the LeftPosition class from Unit.
    /// </summary>
    private LeftPosition(Unit value)
    {
      shapePosition = ShapePosition.Undefined;
      position = value;
      notNull = !value.IsNull;
    }

    /// <summary>
    /// Initializes a new instance of the LeftPosition class from ShapePosition.
    /// </summary>
    private LeftPosition(ShapePosition value)
    {
      if (!(value == ShapePosition.Undefined || IsValid(value)))
        throw new ArgumentException(DomSR.InvalidEnumForLeftPosition);

      shapePosition = value;
      position = Unit.NullValue;
      notNull = (value != ShapePosition.Undefined);
    }

	  /// <summary>
    /// Determines whether this instance is null (not set).
    /// </summary>
    public bool IsNull
    {
      get { return !notNull; }
    }

    /// <summary>
    /// Gets the value of the position in unit.
    /// </summary>
    public Unit Position
    {
      get { return position; }
    }

    /// <summary>
    /// Gets the value of the position.
    /// </summary>
    public ShapePosition ShapePosition
    {
      get { return shapePosition; }
    }
    internal ShapePosition shapePosition;
    internal Unit position;
    private bool notNull;

    /// <summary>
    /// Indicates the given shapePosition is valid for LeftPosition.
    /// </summary>
    private static bool IsValid(ShapePosition shapePosition)
    {
      return shapePosition == ShapePosition.Left ||
             shapePosition == ShapePosition.Center ||
             shapePosition == ShapePosition.Right ||
             shapePosition == ShapePosition.Inside ||
             shapePosition == ShapePosition.Outside;
    }

    /// <summary>
    /// Converts a ShapePosition to a LeftPosition.
    /// </summary>
    public static implicit operator LeftPosition(ShapePosition value)
    {
      return new LeftPosition(value);
    }

    /// <summary>
    /// Converts a Unit to a LeftPosition.
    /// </summary>
    public static implicit operator LeftPosition(Unit value)
    {
      return new LeftPosition(value);
    }

    /// <summary>
    /// Converts a string to a LeftPosition.
    /// The string is interpreted as a Unit.
    /// </summary>
    public static implicit operator LeftPosition(string value)
    {
      Unit unit = value;
      return new LeftPosition(unit);
    }

    /// <summary>
    /// Converts a double to a LeftPosition.
    /// The double is interpreted as a Unit in Point.
    /// </summary>
    public static implicit operator LeftPosition(double value)
    {
      Unit unit = value;
      return new LeftPosition(unit);
    }

    /// <summary>
    /// Converts an integer to a LeftPosition. 
    /// The integer is interpreted as a Unit in Point.
    /// </summary>
    public static implicit operator LeftPosition(int value)
    {
      Unit unit = value;
      return new LeftPosition(unit);
    }

    /// <summary>
    /// Parses the specified value.
    /// </summary>
    public static LeftPosition Parse(string value)
    {
      if (string.IsNullOrEmpty(value))
        throw new ArgumentNullException("value");

      value = value.Trim();
      char ch = value[0];
      if (ch == '+' || ch == '-' || Char.IsNumber(ch))
        return Unit.Parse(value);
      else
        return (ShapePosition)Enum.Parse(typeof(ShapePosition), value, true);
    }

    /// <summary>
    /// Returns the unitialized LeftPosition object.
    /// </summary>
    internal static readonly LeftPosition NullValue = new LeftPosition();
  }
}
