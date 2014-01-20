#region MigraDoc - Creating Documents on the Fly
//
// Authors:
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
using PdfSharp.Charting;
using PdfSharp.Core.Enums;

namespace MigraDoc.Rendering.ChartMapper
{
  /// <summary>
  /// The AxisMapper class.
  /// </summary>
  public class AxisMapper
  {
	  void MapObject(Axis axis, DocumentObjectModel.Shapes.Charts.Axis domAxis)
    {
      if (!string.IsNullOrEmpty(domAxis.TickLabels.format))
        axis.TickLabels.Format = domAxis.TickLabels.Format;
      if (!string.IsNullOrEmpty(domAxis.TickLabels.style))
        FontMapper.Map(axis.TickLabels.Font, domAxis.TickLabels.Document, domAxis.TickLabels.Style);
      if (domAxis.TickLabels.font != null)
        FontMapper.Map(axis.TickLabels.Font, domAxis.TickLabels.Font);

      if (domAxis.majorTickMark.HasValue)
        axis.MajorTickMark = domAxis.MajorTickMark;
      if (domAxis.minorTickMark.HasValue)
        axis.MinorTickMark = domAxis.MinorTickMark;

      if (domAxis.majorTick.HasValue)
        axis.MajorTick = domAxis.MajorTick;
	  if (domAxis.minorTick.HasValue)
        axis.MinorTick = domAxis.MinorTick;

      if (domAxis.title != null)
      {
        axis.Title.Caption = domAxis.Title.Caption;
        if (!string.IsNullOrEmpty(domAxis.title.style))
          FontMapper.Map(axis.Title.Font, domAxis.Title.Document, domAxis.Title.Style);
        if (domAxis.title.font != null)
          FontMapper.Map(axis.Title.Font, domAxis.Title.Font);
        axis.Title.Orientation = domAxis.Title.Orientation.Value;
        axis.Title.Alignment = (HorizontalAlignment)domAxis.Title.Alignment.GetValueOrDefault();
        axis.Title.VerticalAlignment = (VerticalAlignment)domAxis.Title.VerticalAlignment.GetValueOrDefault();
      }

      axis.HasMajorGridlines = domAxis.HasMajorGridlines;
      axis.HasMinorGridlines = domAxis.HasMinorGridlines;

      if (domAxis.majorGridlines != null && domAxis.MajorGridlines.lineFormat != null)
        LineFormatMapper.Map(axis.MajorGridlines.LineFormat, domAxis.MajorGridlines.LineFormat);
      if (domAxis.minorGridlines != null && domAxis.MinorGridlines.lineFormat != null)
        LineFormatMapper.Map(axis.MinorGridlines.LineFormat, domAxis.MinorGridlines.LineFormat);

	  if (domAxis.maximumScale.HasValue)
        axis.MaximumScale = domAxis.MaximumScale;
	  if (domAxis.minimumScale.HasValue)
        axis.MinimumScale = domAxis.MinimumScale;

      if (domAxis.lineFormat != null)
        LineFormatMapper.Map(axis.LineFormat, domAxis.LineFormat);
    }

    internal static void Map(Axis axis, MigraDoc.DocumentObjectModel.Shapes.Charts.Axis domAxis)
    {
      AxisMapper mapper = new AxisMapper();
      mapper.MapObject(axis, domAxis);
    }
  }
}
