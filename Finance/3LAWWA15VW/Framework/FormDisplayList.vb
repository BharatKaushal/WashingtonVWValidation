Public Class FormDisplayList
    Public Shared Function ToDisplayList(ByVal strList As List(Of String)) As List(Of FormDisplayValue)
        Dim l As New List(Of FormDisplayValue)
        For Each str As String In strList
            l.Add(New FormDisplayValue(str))
        Next
        Return l
    End Function
End Class
