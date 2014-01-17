#region MigraDoc - Creating Documents on the Fly
//
// Authors:
//   Klaus Potzesny (mailto:Klaus.Potzesny@pdfsharp.com)
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MigraDoc.DocumentObjectModel;

namespace MigraDoc.Rendering.Windows
{
	/// <summary>
	/// Interaction logic for DocumentPreview.xaml
	/// </summary>
	public partial class DocumentPreview : UserControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentPreview"/> class.
		/// </summary>
		public DocumentPreview()
		{
			InitializeComponent();
			Width = Double.NaN;
			Height = Double.NaN;
			//this.preview.SetRenderEvent(new PdfSharp.Forms.PagePreview.RenderEvent(RenderPage));
		}

		///////// <summary>
		///////// Sets a delegate that is invoked when the preview needs to be painted.
		///////// </summary>
		//////public void SetRenderEvent(RenderEvent renderEvent)
		//////{
		//////  this.renderEvent = renderEvent;
		//////  Invalidate();
		//////}
		//////RenderEvent renderEvent;

		/// <summary>
		/// Gets or sets the current page.
		/// </summary>
		public int Page
		{
			get { return page; }
			set
			{
				try
				{
					if (this.viewer != null)
					{
						if (this.page != value)
						{
							this.page = value;
							//PageInfo pageInfo = this.renderer.formattedDocument.GetPageInfo(this.page);
							//if (pageInfo.Orientation == PdfSharp.PageOrientation.Portrait)
							//  this.viewer.PageSize = new Size((int)pageInfo.Width, (int)pageInfo.Height);
							//else
							//  this.viewer.PageSize = new Size((int)pageInfo.Height, (int)pageInfo.Width);

							//this.viewer.Invalidate();
							//OnPageChanged(new EventArgs());
						}
					}
					else
						this.page = -1;
				}
				catch { }
			}
		}
		int page;

		/// <summary>
		/// Gets the number of pages of the underlying formatted document.
		/// </summary>
		public int PageCount
		{
			get
			{
				if (this.renderer != null)
					return this.renderer.FormattedDocument.PageCount;
				return 0;
			}
		}

		/// <summary>
		/// Goes to the first page.
		/// </summary>
		public void FirstPage()
		{
			if (this.renderer != null)
			{
				Page = 1;
				this.viewer.GoToPage(1);
			}
		}

		/// <summary>
		/// Goes to the next page.
		/// </summary>
		public void NextPage()
		{
			if (this.renderer != null && this.page < PageCount)
			{
				Page++;
				//this.preview.Invalidate();
				//OnPageChanged(new EventArgs());
			}
		}

		/// <summary>
		/// Goes to the previous page.
		/// </summary>
		public void PrevPage()
		{
			if (this.renderer != null && this.page > 1)
			{
				Page--;
			}
		}

		/// <summary>
		/// Goes to the last page.
		/// </summary>
		public void LastPage()
		{
			if (this.renderer != null)
			{
				Page = PageCount;
				//this.preview.Invalidate();
				//OnPageChanged(new EventArgs());
			}
		}

		/// <summary>
		/// Gets or sets the MigraDoc document that is previewed in this control.
		/// </summary>
		public Document Document
		{
			get { return this.document; }
			set
			{
				if (value != null)
				{
					this.document = value;
					this.renderer = new DocumentRenderer(value);
					this.renderer.PrepareDocument();
					Page = 1;
					//this.preview.Invalidate();
				}
				else
				{
					this.document = null;
					this.renderer = null;
					//this.preview.Invalidate();
				}
			}
		}
		Document document;

		/// <summary>
		/// Gets the underlying DocumentRenderer of the document currently in preview, or null, if no rederer exists.
		/// You can use this renderer for printing or creating PDF file. This evades the necessity to format the
		/// document a second time when you want to print it and convert it into PDF.
		/// </summary>
		public DocumentRenderer Renderer
		{
			get { return this.renderer; }
		}
		DocumentRenderer renderer;


		/// <summary>
		/// Helper class to render a single visual.
		/// </summary>
		public class DrawingVisualPresenter : FrameworkElement
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="DrawingVisualPresenter"/> class.
			/// </summary>
			public DrawingVisualPresenter(DrawingVisual visual)
			{
				this.visual = visual;
			}

			/// <summary>
			/// Gets the number of visual child elements within this element, which is 1 in this class.
			/// </summary>
			protected override int VisualChildrenCount
			{
				get { return 1; }
			}

			/// <summary>
			/// Overrides <see cref="M:System.Windows.Media.Visual.GetVisualChild(System.Int32)"/>, and returns a child at the specified index from a collection of child elements.
			/// </summary>
			protected override Visual GetVisualChild(int index)
			{
				if (index != 0)
					throw new ArgumentOutOfRangeException("index");
				return visual;
			}

			readonly DrawingVisual visual;
		}
	}
}
