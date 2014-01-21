using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel;

namespace DocumentViewer
{
  /// <summary>
  /// Interaction logic for Window1.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      // Create a new MigraDoc document
      Document document = SampleDocuments.CreateSample1();

    }

    private void Sample1_Click(object sender, RoutedEventArgs e)
    {
      Document document = SampleDocuments.CreateSample1();
    }

    private void Sample2_Click(object sender, RoutedEventArgs e)
    {
      Directory.SetCurrentDirectory(GetProgramDirectory());
      Document document = SampleDocuments.CreateSample2();
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void CreatePdf_Click(object sender, RoutedEventArgs e)
    {
      PdfDocumentRenderer printer = new PdfDocumentRenderer();
      printer.DocumentRenderer = preview.Renderer;
      printer.Document = preview.Document;
      printer.RenderDocument();
      preview.Document.BindToRenderer(null);
      printer.Save("test.pdf");

      Process.Start("test.pdf");
    }

    private void OpenDDL_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog dialog = null;
      try
      {
        dialog = new OpenFileDialog();
        dialog.CheckFileExists = true;
        dialog.CheckPathExists = true;
        dialog.Filter = "MigraDoc DDL (*.mdddl)|*.mdddl|All Files (*.*)|*.*";
        dialog.FilterIndex = 1;
        dialog.InitialDirectory = System.IO.Path.Combine(GetProgramDirectory(), "..\\..");
        //dialog.RestoreDirectory = true;
        if (dialog.ShowDialog() == true)
        {
          
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, Title);
        
      }
      finally
      {
        //if (dialog != null)
        //  dialog.Dispose();
      }
      //UpdateStatusBar();
    }

    private string GetProgramDirectory()
    {
      System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
      return System.IO.Path.GetDirectoryName(assembly.Location);
    }
  }
}
