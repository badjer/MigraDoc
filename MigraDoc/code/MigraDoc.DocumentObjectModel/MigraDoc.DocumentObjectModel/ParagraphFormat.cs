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
  /// A ParagraphFormat represents the formatting of a paragraph.
  /// </summary>
  public class ParagraphFormat : DocumentObject
  {
    /// <summary>
    /// Initializes a new instance of the ParagraphFormat class that can be used as a template.
    /// </summary>
    public ParagraphFormat()
    {
    }

    /// <summary>
    /// Initializes a new instance of the ParagraphFormat class with the specified parent.
    /// </summary>
    internal ParagraphFormat(DocumentObject parent) : base(parent) { }

    #region Methods
    /// <summary>
    /// Creates a deep copy of this object.
    /// </summary>
    public new ParagraphFormat Clone()
    {
      return (ParagraphFormat)DeepCopy();
    }

    /// <summary>
    /// Implements the deep copy of the object.
    /// </summary>
    protected override object DeepCopy()
    {
      ParagraphFormat format = (ParagraphFormat)base.DeepCopy();
      if (format.font != null)
      {
        format.font = format.font.Clone();
        format.font.parent = format;
      }
      if (format.shading != null)
      {
        format.shading = format.shading.Clone();
        format.shading.parent = format;
      }
      if (format.borders != null)
      {
        format.borders = format.borders.Clone();
        format.borders.parent = format;
      }
      if (format.tabStops != null)
      {
        format.tabStops = format.tabStops.Clone();
        format.tabStops.parent = format;
      }
      if (format.listInfo != null)
      {
        format.listInfo = format.listInfo.Clone();
        format.listInfo.parent = format;
      }
      return format;
    }

    /// <summary>
    /// Adds a TabStop object to the collection.
    /// </summary>
    public TabStop AddTabStop(Unit position)
    {
      return this.TabStops.AddTabStop(position);
    }

    /// <summary>
    /// Adds a TabStop object to the collection and sets its alignment and leader.
    /// </summary>
    public TabStop AddTabStop(Unit position, TabAlignment alignment, TabLeader leader)
    {
      return this.TabStops.AddTabStop(position, alignment, leader);
    }

    /// <summary>
    /// Adds a TabStop object to the collection and sets its leader.
    /// </summary>
    public TabStop AddTabStop(Unit position, TabLeader leader)
    {
      return this.TabStops.AddTabStop(position, leader);
    }

    /// <summary>
    /// Adds a TabStop object to the collection and sets its alignment.
    /// </summary>
    public TabStop AddTabStop(Unit position, TabAlignment alignment)
    {
      return this.TabStops.AddTabStop(position, alignment);
    }

    /// <summary>
    /// Adds a TabStop object to the collection marked to remove the tab stop at
    /// the given position.
    /// </summary>
    public void RemoveTabStop(Unit position)
    {
      this.TabStops.RemoveTabStop(position);
    }

    /// <summary>
    /// Adds a TabStop object to the collection.
    /// </summary>
    public void Add(TabStop tabStop)
    {
      this.TabStops.AddTabStop(tabStop);
    }

    /// <summary>
    /// Clears all TapStop objects from the collection. Additionally 'TabStops = null'
    /// is written to the DDL stream when serialized.
    /// </summary>
    public void ClearAll()
    {
      this.TabStops.ClearAll();
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the Alignment of the paragraph.
    /// </summary>
	public ParagraphAlignment? Alignment
    {
      get { return this.alignment; }
      set { this.alignment = value; }
    }
    
    internal ParagraphAlignment? alignment;

    /// <summary>
    /// Gets the Borders object.
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
    /// Gets or sets the indent of the first line in the paragraph.
    /// </summary>
    public Unit FirstLineIndent
    {
      get { return this.firstLineIndent; }
      set { this.firstLineIndent = value; }
    }
    
    internal Unit firstLineIndent = Unit.NullValue;

    /// <summary>
    /// Gets or sets the Font object.
    /// </summary>
    public Font Font
    {
      get
      {
        if (this.font == null)
          this.font = new Font(this);

        return this.font;
      }
      set
      {
        SetParent(value);
        this.font = value;
      }
    }
    
    internal Font font;

    /// <summary>
    /// Gets or sets a value indicating whether to keep all the paragraph's lines on the same page.
    /// </summary>
    public bool KeepTogether
    {
		get { return this.keepTogether.GetValueOrDefault(); }
      set { this.keepTogether = value; }
    }

	internal bool? keepTogether;

    /// <summary>
    /// Gets or sets a value indicating whether this and the next paragraph stay on the same page.
    /// </summary>
    public bool KeepWithNext
    {
		get { return this.keepWithNext.GetValueOrDefault(); }
      set { this.keepWithNext = value; }
    }

	internal bool? keepWithNext;

    /// <summary>
    /// Gets or sets the left indent of the paragraph.
    /// </summary>
    public Unit LeftIndent
    {
      get { return this.leftIndent; }
      set { this.leftIndent = value; }
    }
    
    internal Unit leftIndent = Unit.NullValue;

    /// <summary>
    /// Gets or sets the space between lines on the paragraph.
    /// </summary>
    public Unit LineSpacing
    {
      get { return this.lineSpacing; }
      set { this.lineSpacing = value; }
    }
    
    internal Unit lineSpacing = Unit.NullValue;

    /// <summary>
    /// Gets or sets the rule which is used to define the line spacing.
    /// </summary>
    public LineSpacingRule? LineSpacingRule
    {
      get { return this.lineSpacingRule; }
      set { this.lineSpacingRule = value; }
    }
    
    internal LineSpacingRule? lineSpacingRule;

    /// <summary>
    /// Gets or sets the ListInfo object of the paragraph.
    /// </summary>
    public ListInfo ListInfo
    {
      get
      {
        if (this.listInfo == null)
          this.listInfo = new ListInfo(this);

        return this.listInfo;
      }
      set
      {
        SetParent(value);
        this.listInfo = value;
      }
    }
    
    internal ListInfo listInfo;

    /// <summary>
    /// Gets or sets the out line level of the paragraph.
    /// </summary>
    public OutlineLevel? OutlineLevel
    {
      get { return this.outlineLevel; }
      set { this.outlineLevel = value; }
    }
    
    internal OutlineLevel? outlineLevel;

    /// <summary>
    /// Gets or sets a value indicating whether a page break is inserted before the paragraph.
    /// </summary>
    public bool PageBreakBefore
    {
		get { return this.pageBreakBefore.GetValueOrDefault(); }
      set { this.pageBreakBefore = value; }
    }

	internal bool? pageBreakBefore;

    /// <summary>
    /// Gets or sets the right indent of the paragraph.
    /// </summary>
    public Unit RightIndent
    {
      get { return this.rightIndent; }
      set { this.rightIndent = value; }
    }
    
    internal Unit rightIndent = Unit.NullValue;

    /// <summary>
    /// Gets the shading object.
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
    /// Gets or sets the space that's inserted after the paragraph.
    /// </summary>
    public Unit SpaceAfter
    {
      get { return this.spaceAfter; }
      set { this.spaceAfter = value; }
    }
    
    internal Unit spaceAfter = Unit.NullValue;

    /// <summary>
    /// Gets or sets the space that's inserted before the paragraph.
    /// </summary>
    public Unit SpaceBefore
    {
      get { return this.spaceBefore; }
      set { this.spaceBefore = value; }
    }
    
    internal Unit spaceBefore = Unit.NullValue;

    /// <summary>
    /// Indicates whether the ParagraphFormat has a TabStops collection.
    /// </summary>
    public bool HasTabStops
    {
      get { return this.tabStops != null; }
    }

    /// <summary>
    /// Get the TabStops collection.
    /// </summary>
    public TabStops TabStops
    {
      get
      {
        if (this.tabStops == null)
          this.tabStops = new TabStops(this);

        return this.tabStops;
      }
      set
      {
        SetParent(value);
        this.tabStops = value;
      }
    }
    
    internal TabStops tabStops;

    /// <summary>
    /// Gets or sets a value indicating whether a line from the paragraph stays alone in a page.
    /// </summary>
    public bool WidowControl
    {
		get { return this.widowControl.GetValueOrDefault(); }
      set { this.widowControl = value; }
    }

	internal bool? widowControl;
    #endregion

    #region Internal

	  
    #endregion
  }
}
