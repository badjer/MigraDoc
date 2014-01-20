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

using MigraDoc.DocumentObjectModel.Fields;
using PdfSharp.Core.Enums;

namespace MigraDoc.DocumentObjectModel
{
  /// <summary>
  /// Contains information about document content, author etc.
  /// </summary>
  public class DocumentInfo : DocumentObject
  {
    /// <summary>
    /// Initializes a new instance of the DocumentInfo class.
    /// </summary>
    public DocumentInfo()
    {
    }

    /// <summary>
    /// Initializes a new instance of the DocumentInfo class with the specified parent.
    /// </summary>
    internal DocumentInfo(DocumentObject parent) : base(parent) { }

    #region Methods
    /// <summary>
    /// Creates a deep copy of this object.
    /// </summary>
    public new DocumentInfo Clone()
    {
      return (DocumentInfo)DeepCopy();
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the document title.
    /// </summary>
    public string Title
    {
      get { return this.title; }
      set { this.title = value; }
    }

	internal string title;

    /// <summary>
    /// Gets or sets the document author.
    /// </summary>
    public string Author
    {
      get { return this.author; }
      set { this.author = value; }
    }

	internal string author;

    /// <summary>
    /// Gets or sets keywords related to the document.
    /// </summary>
    public string Keywords
    {
      get { return this.keywords; }
      set { this.keywords = value; }
    }

	internal string keywords;

    /// <summary>
    /// Gets or sets the subject of the document.
    /// </summary>
    public string Subject
    {
      get { return this.subject; }
      set { this.subject = value; }
    }

	internal string subject;

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

	public string GetValueByEnum(InfoFieldType type)
	{
		if (type == InfoFieldType.Author)
		{
			return Author;
		}

		if (type == InfoFieldType.Keywords)
		{
			return Keywords;
		}

		if (type == InfoFieldType.Subject)
		{
			return Subject;
		}

		if (type == InfoFieldType.Title)
		{
			return Title;
		}

		return "";
	}

  }
}
