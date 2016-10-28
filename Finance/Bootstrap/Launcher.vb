Public Class Launcher
    Shared Sub main()
        Dim h As New PBS.Deals.FormsIntegration.FormHost
        ''Dim fs As PBS.Deals.FormsIntegration.OKIFormBaseV = New _3LAWMN12VW._3LAWMN12VW
        Dim fs As PBS.Deals.FormsIntegration.LaserFormBaseV = New _3LAWWA15VW._3LAWWA15VW
        'Dim fs As PBS.Deals.FormsIntegration.OKIFormBaseV = New _3LAWMN12VW._3LAWMN12VW
        'Dim fs As PBS.Deals.FormsIntegration.LaserFormBaseV = New _3VCIAB11AD._3VCIAB11AD
        'Dim fs As PBS.Deals.FormsIntegration.LaserFormBaseV = New _3VCIAB11VW._3VCIAB11VW
        h.Execute(fs, "")
        fs.FetchState()
    End Sub

End Class
