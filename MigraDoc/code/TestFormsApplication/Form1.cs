﻿using System;
using System.Windows.Forms;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using MigraDoc.Rendering;
using PdfSharp.Core.Enums;

namespace TestFormsApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

	    private void RenderPdf(object sender, EventArgs e)
        {
            var doc = CreateHelloWorld();
            OpenPdf(doc);
        }

        private static Document CreateHelloWorld()
        {
            var doc = new Document();
            var section = doc.AddSection();
            var p = section.AddParagraph("Hello World");
            p = section.AddParagraph();
            var image = p.AddImage(System.IO.Path.Combine(Environment.CurrentDirectory, @"images\logo.png"));
	        image.ScaleWidth = 0.50;
			image.ScaleHeight = 0.50;

            DefineCharts(doc);

            return doc;
        }

        public static void DefineCharts(Document document)
        {
            var paragraph = document.LastSection.AddParagraph("Chart Overview", "Heading1");
            paragraph.AddBookmark("Charts");
            document.LastSection.AddParagraph("Sample Chart", "Heading2");
            var chart = new Chart();

            chart.Left = 0;
            chart.Width = Unit.FromCentimeter(15);
            chart.Height = Unit.FromCentimeter(12);
            var series = chart.SeriesCollection.AddSeries();
            series.ChartType = ChartType.Column2D;
            series.Add(new double[] { 1, 17, 45, 5, 3, 20, 11, 23, 8, 19 });
            series.HasDataLabel = true;

            series = chart.SeriesCollection.AddSeries();
            series.ChartType = ChartType.Line;
            series.Add(new double[] { 41, 7, 5, 45, 13, 10, 21, 13, 18, 9 });

            var xseries = chart.XValues.AddXSeries();
            xseries.Add("A", "B", "C", "D", "E", "F", "G", "H", "I", "J");

            chart.XAxis.MajorTickMark = TickMarkType.Outside;
            chart.XAxis.Title.Caption = "X-Axis";

            chart.YAxis.MajorTickMark = TickMarkType.Outside;
            chart.YAxis.HasMajorGridlines = true;

            chart.PlotArea.LineFormat.Color = MigraDoc.DocumentObjectModel.Colors.DarkGray;
            chart.PlotArea.LineFormat.Width = 1;

            document.LastSection.Add(chart);
        }


        private static void OpenPdf(Document doc)
        {
            var filename = "c:\\temp\\test.pdf";
            PdfDocumentRenderer pdf = new PdfDocumentRenderer(true, PdfFontEmbedding.None);
            pdf.Document = doc;
            pdf.RenderDocument();
            pdf.Save(filename);
            ShellExecute(filename, "");
        }

	    public static void ShellExecute(string file, string verb)
        {
            if (file == null) throw new ArgumentNullException("file");

            System.Diagnostics.ProcessStartInfo si = new System.Diagnostics.ProcessStartInfo();
            si.UseShellExecute = true;
            si.FileName = file;
            si.Verb = verb;
            System.Diagnostics.Process.Start(si);
        }
    }
}
