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
  /// Base class of all objects of the MigraDoc Document Object Model.
  /// </summary>
  public abstract class DocumentObject : ICloneable
  {
    /// <summary>
    /// Initializes a new instance of the DocumentObject class.
    /// </summary>
    internal DocumentObject()
    {
    }

    /// <summary>
    /// Initializes a new instance of the DocumentObject class with the specified parent.
    /// </summary>
    internal DocumentObject(DocumentObject parent)
    {
      Debug.Assert(parent != null, "Parent must not be null.");
      this.parent = parent;
    }

    /// <summary>
    /// Creates a deep copy of the DocumentObject. The parent of the new object is null.
    /// </summary>
    public object Clone()
    {
      return DeepCopy();
    }

    /// <summary>
    /// Implements the deep copy of the object.
    /// </summary>
    protected virtual object DeepCopy()
    {
      DocumentObject value = (DocumentObject)MemberwiseClone();
      value.parent = null;
      return value;
    }

    /// <summary>
    /// Gets the parent object.
    /// </summary>
    internal DocumentObject Parent
    {
      get { return parent; }
    }
    
    protected internal DocumentObject parent;

    /// <summary>
    /// Gets the document of the object, or null, if the object is not associated with a document.
    /// </summary>
    public Document Document
    {
      get
      {
        DocumentObject doc = Parent;
        while (doc != null)
        {
          Document document = doc as Document;
          if (document != null)
            return document;
          doc = doc.parent;
        }
        return null;
      }
    }

    /// <summary>
    /// Gets the section of the object, or null, if the object is not associated with a section.
    /// </summary>
    public Section Section
    {
      get
      {
        DocumentObject doc = Parent;
        while (doc != null)
        {
          Section section = doc as Section;
          if (section != null)
            return section;
          doc = doc.parent;
        }
        return null;
      }
    }
	
    /// <summary>
    /// Gets or sets a value that contains arbitrary information about this object.
    /// </summary>
    public object Tag
    {
      get { return this.tag; }
      set { this.tag = value; }
    }
    object tag;

	  /// <summary>
    /// Sets the parent of the specified value.
    /// If a parent is already set, an ArgumentException will be thrown.
    /// </summary>
    protected void SetParent(DocumentObject val)
    {
      if (val != null)
      {
        if (val.Parent != null)
          throw new ArgumentException(DomSR.ParentAlreadySet(val, this));

        val.parent = this;
      }
    }

    /// <summary>
    /// When overridden in a derived class resets cached values
    /// (like column index).
    /// </summary>
    internal virtual void ResetCachedValues()
    {
    }
  }
}
