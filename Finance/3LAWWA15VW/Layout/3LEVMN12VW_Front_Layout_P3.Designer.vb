<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class _3LEVMN12VW_Front_Layout_P3
    Inherits GrapeCity.ActiveReports.SectionReport

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
        End If
        MyBase.Dispose(disposing)
    End Sub

    'NOTE: The following procedure is required by the ActiveReports Designer
    'It can be modified using the ActiveReports Designer.
    'Do not modify it using the code editor.
    Private WithEvents Detail1 As GrapeCity.ActiveReports.SectionReportModel.Detail
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(_3LEVMN12VW_Front_Layout_P3))
        Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
        Me.Background = New GrapeCity.ActiveReports.SectionReportModel.Picture()
        CType(Me.Background, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'Detail1
        '
        Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Background})
        Me.Detail1.Height = 10.5!
        Me.Detail1.Name = "Detail1"
        '
        'Background
        '
        Me.Background.Height = 10.5!
        Me.Background.ImageData = CType(resources.GetObject("Background.ImageData"), System.IO.Stream)
        Me.Background.Left = 0!
        Me.Background.Name = "Background"
        Me.Background.SizeMode = GrapeCity.ActiveReports.SectionReportModel.SizeModes.Zoom
        Me.Background.Top = 0!
        Me.Background.Width = 8.0!
        '
        '_3LEVMN12VW_Front_Layout_P3
        '
        Me.MasterReport = False
        Me.PageSettings.DefaultPaperSize = False
        Me.PageSettings.Margins.Bottom = 0.25!
        Me.PageSettings.Margins.Left = 0.24!
        Me.PageSettings.Margins.Right = 0.24!
        Me.PageSettings.Margins.Top = 0.25!
        Me.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Portrait
        Me.PageSettings.PaperHeight = 10.5!
        Me.PageSettings.PaperName = "Letter"
        Me.PageSettings.PaperWidth = 8.0!
        Me.PrintWidth = 8.0!
        Me.Sections.Add(Me.Detail1)
        CType(Me.Background, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Public WithEvents Background As GrapeCity.ActiveReports.SectionReportModel.Picture
End Class
