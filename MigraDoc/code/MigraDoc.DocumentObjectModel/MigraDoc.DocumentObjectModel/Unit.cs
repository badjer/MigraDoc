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

namespace MigraDoc.DocumentObjectModel
{
  /// <summary>
  /// An Unit consist of a numerical value and an UnitType like Centimeter, Millimeter or Inch.
  /// Several conversion between different measures are supported.
  /// </summary>
  public struct Unit : IFormattable
  {
    /// <summary>
    /// Initializes a new instance of the Unit class with type set to point.
    /// </summary>
    public Unit(double point)
    {
      _value = (float)point;
      _type = UnitType.Point;
      _initialized = true;
    }

    /// <summary>
    /// Initializes a new instance of the Unit class.
    /// Throws System.ArgumentException if <code>type</code> is invalid.
    /// </summary>
    public Unit(double value, UnitType type)
    {
      this._value = (float)value;
      this._type = type;
      _initialized = true;
    }

    /// <summary>
    /// Determines whether this instance is empty.
    /// </summary>
    public bool IsEmpty
    {
      get { return IsNull; }
    }

    /// <summary>
    /// Determines whether this instance is null (not set).
    /// </summary>
    internal bool IsNull
    {
      get { return !_initialized; }
    }

    #region Properties
    /// <summary>
    /// Gets or sets the raw value of the object without any conversion.
    /// To determine the UnitType use property <code>Type</code>.
    /// </summary>
    public double Value
    {
      get { return (IsNull ? 0 : _value); }
      set
      {
        this._value = (float)value;
        _initialized = true;
      }
    }

    /// <summary>
    /// Gets the UnitType of the object.
    /// </summary>
    public UnitType Type
    {
      get { return _type; }
    }

    /// <summary>
    /// Gets or sets the value in point.
    /// </summary>
    public double Point
    {
      get
      {
        if (IsNull)
          return 0;

        switch (this._type)
        {
          case UnitType.Centimeter:
            return this._value * 72 / 2.54;

          case UnitType.Inch:
            return this._value * 72;

          case UnitType.Millimeter:
            return this._value * 72 / 25.4;

          case UnitType.Pica:
            return this._value * 12;

          case UnitType.Point:
            return this._value;

          default:
            Debug.Assert(false, "Missing unit type.");
            return 0;
        }
      }
      set
      {
        this._value = (float)value;
        _type = UnitType.Point;
        _initialized = true;
      }
    }

    /// <summary>
    /// Gets or sets the value in centimeter.
    /// </summary>
    public double Centimeter
    {
      get
      {
        if (IsNull)
          return 0;

        switch (this._type)
        {
          case UnitType.Centimeter:
            return this._value;

          case UnitType.Inch:
            return this._value * 2.54;

          case UnitType.Millimeter:
            return this._value / 10;

          case UnitType.Pica:
            return this._value * 12 * 2.54 / 72;

          case UnitType.Point:
            return this._value * 2.54 / 72;

          default:
            Debug.Assert(false, "Missing unit type");
            return 0;
        }
      }
      set
      {
        this._value = (float)value;
        this._type = UnitType.Centimeter;
        this._initialized = true;
      }
    }

    /// <summary>
    /// Gets or sets the value in inch.
    /// </summary>
    public double Inch
    {
      get
      {
        if (IsNull)
          return 0;

        switch (this._type)
        {
          case UnitType.Centimeter:
            return this._value / 2.54;

          case UnitType.Inch:
            return this._value;

          case UnitType.Millimeter:
            return this._value / 25.4;

          case UnitType.Pica:
            return this._value * 12 / 72;

          case UnitType.Point:
            return this._value / 72;

          default:
            Debug.Assert(false, "Missing unit type");
            return 0;
        }
      }
      set
      {
        this._value = (float)value;
        this._type = UnitType.Inch;
        this._initialized = true;
      }
    }

    /// <summary>
    /// Gets or sets the value in millimeter.
    /// </summary>
    public double Millimeter
    {
      get
      {
        if (IsNull)
          return 0;

        switch (this._type)
        {
          case UnitType.Centimeter:
            return this._value * 10;

          case UnitType.Inch:
            return this._value * 25.4;

          case UnitType.Millimeter:
            return this._value;

          case UnitType.Pica:
            return this._value * 12 * 25.4 / 72;

          case UnitType.Point:
            return this._value * 25.4 / 72;

          default:
            Debug.Assert(false, "Missing unit type");
            return 0;
        }
      }
      set
      {
        this._value = (float)value;
        this._type = UnitType.Millimeter;
        this._initialized = true;
      }
    }

    /// <summary>
    /// Gets or sets the value in pica.
    /// </summary>
    public double Pica
    {
      get
      {
        if (IsNull)
          return 0;

        switch (this._type)
        {
          case UnitType.Centimeter:
            return this._value * 72 / 2.54 / 12;

          case UnitType.Inch:
            return this._value * 72 / 12;

          case UnitType.Millimeter:
            return this._value * 72 / 25.4 / 12;

          case UnitType.Pica:
            return this._value;

          case UnitType.Point:
            return this._value / 12;

          default:
            Debug.Assert(false, "Missing unit type");
            return 0;
        }
      }
      set
      {
        this._value = (float)value;
        this._type = UnitType.Pica;
        this._initialized = true;
      }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Returns the object as string using the format information.
    /// Measure will be added to the end of the string.
    /// </summary>
    public string ToString(IFormatProvider formatProvider)
    {
      if (IsNull)
        return 0.ToString(formatProvider); // TODO: ?? can it be anything other than "0"??

      string valuestring;
      valuestring = this._value.ToString(formatProvider) + GetSuffix();
      return valuestring;
    }

    /// <summary>
    /// Returns the object as string using the format.
    /// Measure will be added to the end of the string.
    /// </summary>
    public string ToString(string format)
    {
      if (IsNull)
        return 0.ToString(format); // TODO: ?? can it be anything other than "0"??

      string valuestring;
      valuestring = this._value.ToString(format) + GetSuffix();
      return valuestring;
    }

    /// <summary>
    /// Returns the object as string using the specified format and format information.
    /// Measure will be added to the end of the string.
    /// </summary>
    string IFormattable.ToString(string format, IFormatProvider formatProvider)
    {
      if (IsNull)
        return 0.ToString(format, formatProvider);

      string valuestring;
      valuestring = this._value.ToString(format, formatProvider) + GetSuffix();
      return valuestring;
    }

    /// <summary>
    /// Returns the object as string. Measure will be added to the end of the string.
    /// </summary>
    public override string ToString()
    {
      if (IsNull)
        return 0.ToString(System.Globalization.CultureInfo.InvariantCulture);

      string valuestring;
      valuestring = this._value.ToString(System.Globalization.CultureInfo.InvariantCulture) + GetSuffix();
      return valuestring;
    }

    /// <summary>
    /// Returns the type of the object as a string like 'pc', 'cm', or 'in'. Empty string is equal to 'pt'.
    /// </summary>
    string GetSuffix()
    {
      switch (this._type)
      {
        case UnitType.Centimeter:
          return "cm";

        case UnitType.Inch:
          return "in";

        case UnitType.Millimeter:
          return "mm";

        case UnitType.Pica:
          return "pc";

        case UnitType.Point:
          //Point is default, so leave this blank.
          return "";

        default:
          Debug.Assert(false, "Missing unit type");
          return "";
      }
    }

    /// <summary>
    /// Returns an Unit object. Sets type to centimeter.
    /// </summary>
    public static Unit FromCentimeter(double value)
    {
      Unit unit = Unit.Zero;
      unit._value = (float)value;
      unit._type = UnitType.Centimeter;
      return unit;
    }

    /// <summary>
    /// Returns an Unit object. Sets type to millimeter.
    /// </summary>
    public static Unit FromMillimeter(double value)
    {
      Unit unit = Unit.Zero;
      unit._value = (float)value;
      unit._type = UnitType.Millimeter;
      return unit;
    }

    /// <summary>
    /// Returns an Unit object. Sets type to point.
    /// </summary>
    public static Unit FromPoint(double value)
    {
      Unit unit = Unit.Zero;
      unit._value = (float)value;
      unit._type = UnitType.Point;
      return unit;
    }

    /// <summary>
    /// Returns an Unit object. Sets type to inch.
    /// </summary>
    public static Unit FromInch(double value)
    {
      Unit unit = Unit.Zero;
      unit._value = (float)value;
      unit._type = UnitType.Inch;
      return unit;
    }

    /// <summary>
    /// Returns an Unit object. Sets type to pica.
    /// </summary>
    public static Unit FromPica(double value)
    {
      Unit unit = Unit.Zero;
      unit._value = (float)value;
      unit._type = UnitType.Pica;
      return unit;
    }
    #endregion

    /// <summary>
    /// Converts a string to an Unit object.
    /// If the string contains a suffix like 'cm' or 'in' the object will be converted
    /// to the appropriate type, otherwise point is assumed.
    /// </summary>
    public static implicit operator Unit(string value)
    {
      Unit unit = Unit.Zero;
      value = value.Trim();

      // For Germans...
      value = value.Replace(',', '.');

      int count = value.Length;
      int valLen = 0;
      for (; valLen < count; )
      {
        char ch = value[valLen];
        if (ch == '.' || ch == '-' || ch == '+' || Char.IsNumber(ch))
          valLen++;
        else
          break;
      }

      unit._value = 1;
      try
      {
        unit._value = float.Parse(value.Substring(0, valLen).Trim(), System.Globalization.CultureInfo.InvariantCulture);
      }
      catch (FormatException ex)
      {
        throw new ArgumentException(DomSR.InvalidUnitValue(value), ex);
      }

      string typeStr = value.Substring(valLen).Trim().ToLower();
      unit._type = UnitType.Point;
      switch (typeStr)
      {
        case "cm":
          unit._type = UnitType.Centimeter;
          break;

        case "in":
          unit._type = UnitType.Inch;
          break;

        case "mm":
          unit._type = UnitType.Millimeter;
          break;

        case "pc":
          unit._type = UnitType.Pica;
          break;

        case "":
        case "pt":
          unit._type = UnitType.Point;
          break;

        default:
          throw new ArgumentException(DomSR.InvalidUnitType(typeStr));
      }

      return unit;
    }

    /// <summary>
    /// Converts an int to an Unit object with type set to point.
    /// </summary>
    public static implicit operator Unit(int value)
    {
      Unit unit = Unit.Zero;
      unit._value = value;
      unit._type = UnitType.Point;
      return unit;
    }

    /// <summary>
    /// Converts a float to an Unit object with type set to point.
    /// </summary>
    public static implicit operator Unit(float value)
    {
      Unit unit = Unit.Zero;
      unit._value = value;
      unit._type = UnitType.Point;
      return unit;
    }

    /// <summary>
    /// Converts a double to an Unit object with type set to point.
    /// </summary>
    public static implicit operator Unit(double value)
    {
      Unit unit = Unit.Zero;
      unit._value = (float)value;
      unit._type = UnitType.Point;
      return unit;
    }

    /// <summary>
    /// Returns a double value as point.
    /// </summary>
    public static implicit operator double(Unit value)
    {
      return value.Point;
    }

    /// <summary>
    /// Returns a float value as point.
    /// </summary>
    public static implicit operator float(Unit value)
    {
      return (float)value.Point;
    }

    /// <summary>
    /// Memberwise comparison. To compare by value, 
    /// use code like Math.Abs(a.Point - b.Point) &lt; 1e-5.
    /// </summary>
    public static bool operator ==(Unit l, Unit r)
    {
      return (l._initialized == r._initialized && l._type == r._type && l._value == r._value);
    }

    /// <summary>
    /// Memberwise comparison. To compare by value, 
    /// use code like Math.Abs(a.Point - b.Point) &lt; 1e-5.
    /// </summary>
    public static bool operator !=(Unit l, Unit r)
    {
      return !(l == r);
    }

	  /// <summary>
    /// This member is intended to be used by XmlDomainObjectReader only.
    /// </summary>
    public static Unit Parse(string value)
    {
      Unit unit = Unit.Zero;
      unit = value;
      return unit;
    }

    /// <summary>
    /// Converts an existing object from one unit into another unit type.
    /// </summary>
    public void ConvertType(UnitType type)
    {
      if (_type == type)
        return;

      switch (type)
      {
        case UnitType.Centimeter:
          _value = (float)this.Centimeter;
          _type = UnitType.Centimeter;
          break;

        case UnitType.Inch:
          this._value = (float)this.Inch;
          this._type = UnitType.Inch;
          break;

        case UnitType.Millimeter:
          this._value = (float)this.Millimeter;
          this._type = UnitType.Millimeter;
          break;

        case UnitType.Pica:
          this._value = (float)this.Pica;
          this._type = UnitType.Pica;
          break;

        case UnitType.Point:
          this._value = (float)this.Point;
          this._type = UnitType.Point;
          break;

        default:
          //Remember missing unit type!!!
          Debug.Assert(false, "Missing unit type");
          break;
      }
    }

    /// <summary>
    /// Represents the uninitialized Unit object.
    /// </summary>
    public static readonly Unit Empty = new Unit();

    /// <summary>
    /// Represents an initialized Unit object with value 0 and unit type point.
    /// </summary>
    public static readonly Unit Zero = new Unit(0);

    /// <summary>
    /// Represents the uninitialized Unit object. Same as Unit.Empty.
    /// </summary>
    internal static readonly Unit NullValue = Empty;

    bool _initialized;
    float _value;
    UnitType _type;
  }
}
