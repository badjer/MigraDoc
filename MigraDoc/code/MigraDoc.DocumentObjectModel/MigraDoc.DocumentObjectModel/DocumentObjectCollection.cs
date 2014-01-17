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
using System.Collections.Generic;
using MigraDoc.DocumentObjectModel.Visitors;

namespace MigraDoc.DocumentObjectModel
{
	/// <summary>
	/// Base class of all collections of the MigraDoc Document Object Model.
	/// </summary>
	public abstract class DocumentObjectCollection : DocumentObject, IList<DocumentObject>, IVisitable
	{
		private List<DocumentObject> _elements;
		/// <summary>
		/// Initializes a new instance of the DocumentObjectCollection class.
		/// </summary>
		internal DocumentObjectCollection()
		{
			_elements = new List<DocumentObject>();
		}

		/// <summary>
		/// Initializes a new instance of the DocumentObjectCollection class with the specified parent.
		/// </summary>
		internal DocumentObjectCollection(DocumentObject parent) : base(parent)
		{
			_elements = new List<DocumentObject>();
		}

		/// <summary>
		/// Gets the first value in the Collection, if there is any, otherwise null.
		/// </summary>
		public DocumentObject First
		{
			get { return Count > 0 ? this[0] : null; }
		}

		/// <summary>
		/// Creates a deep copy of this object.
		/// </summary>
		public new DocumentObjectCollection Clone()
		{
			return (DocumentObjectCollection)DeepCopy();
		}

		/// <summary>
		/// Implements the deep copy of the object.
		/// </summary>
		protected override object DeepCopy()
		{
			DocumentObjectCollection coll = (DocumentObjectCollection)base.DeepCopy();

			int count = Count;
			coll._elements = new List<DocumentObject>(count);
			for (int index = 0; index < count; ++index)
			{
				DocumentObject doc = this[index];
				if (doc != null)
				{
					doc = (DocumentObject)doc.Clone();
					doc.parent = coll;
				}
				coll._elements.Add(doc);
			}
			return coll;
		}

		public bool Remove(DocumentObject item)
		{
			return _elements.Remove(item);
		}

		/// <summary>
		/// Gets the number of elements actually contained in the collection.
		/// </summary>
		public int Count
		{
			get { return _elements.Count; }
		}

		public bool IsReadOnly { get { return false; } }

		/// <summary>
		/// Removes all elements from the collection.
		/// </summary>
		public void Clear()
		{
			_elements.Clear();
		}

		public bool Contains(DocumentObject item)
		{
			return _elements.Contains(item);
		}

		/// <summary>
		/// Copies the entire collection to a compatible one-dimensional System.Array,
		/// starting at the specified index of the target array.
		/// </summary>
		public void CopyTo(DocumentObject[] array, int arrayIndex)
		{
			_elements.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Inserts an object at the specified index.
		/// </summary>
		public virtual void InsertObject(int index, DocumentObject val)
		{
			SetParent(val);
			_elements.Insert(index, val);
			// Call ResetCachedValues for all objects moved by the Insert operation.
			int count = _elements.Count;
			for (int idx = index + 1; idx < count; ++idx)
			{
				DocumentObject obj = _elements[idx];
				obj.ResetCachedValues();
			}
		}

		/// <summary>
		/// Determines the index of a specific item in the collection.
		/// </summary>
		public int IndexOf(DocumentObject val)
		{
			return _elements.IndexOf(val);
		}

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		public virtual DocumentObject this[int index]
		{
			get { return _elements[index]; }
			set
			{
				SetParent(value);
				_elements[index] = value;
			}
		}

		/// <summary>
		/// Gets the last element or null, if no such element exists.
		/// </summary>
		public DocumentObject LastObject
		{
			get
			{
				int count = _elements.Count;
				if (count > 0)
					return _elements[count - 1];
				return null;
			}
		}

		/// <summary>
		/// Removes the element at the specified index.
		/// </summary>
		public void RemoveObjectAt(int index)
		{
			_elements.RemoveAt(index);
			// Call ResetCachedValues for all objects moved by the RemoveAt operation.
			int count = _elements.Count;
			for (int idx = index; idx < count; ++idx)
			{
				DocumentObject obj = _elements[idx];
				obj.ResetCachedValues();
			}
		}

		/// <summary>
		/// Inserts the object into the collection and sets it's parent.
		/// </summary>
		public virtual void Add(DocumentObject value)
		{
			SetParent(value);
			_elements.Add(value);
		}

		/// <summary>
		/// Allows the visitor object to visit the document object and it's child objects.
		/// </summary>
		void IVisitable.AcceptVisitor(DocumentObjectVisitor visitor, bool visitChildren)
		{
			visitor.VisitDocumentObjectCollection(this);

			foreach (DocumentObject docobj in this)
			{
				IVisitable visitable = docobj as IVisitable;
				if (visitable != null)
					visitable.AcceptVisitor(visitor, visitChildren);
			}
		}

		IEnumerator<DocumentObject> IEnumerable<DocumentObject>.GetEnumerator()
		{
			return _elements.GetEnumerator();
		}

		/// <summary>
		/// Inserts an object at the specified index.
		/// </summary>
		public void Insert(int index, DocumentObject item)
		{
			_elements.Insert(index, item);
		}

		/// <summary>
		/// Removes the item at the specified index from the Collection.
		/// </summary>
		public void RemoveAt(int index)
		{
			_elements.RemoveAt(index);
		}

		public IEnumerator GetEnumerator()
		{
			return _elements.GetEnumerator();
		}
	}
}
