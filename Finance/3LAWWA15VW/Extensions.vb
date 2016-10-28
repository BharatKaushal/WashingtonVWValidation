Imports System.Runtime.CompilerServices
Imports PBS.Comms.VWCommunication.Contract

Public Module Extensions
    <Extension>
    Public Sub SetDataBinding(cbo As System.Windows.Controls.ComboBox, list As IList, Optional display As String = "Display", Optional value As String = "Value")
        cbo.ItemsSource = list
        cbo.DisplayMemberPath = display
        cbo.SelectedValuePath = value
    End Sub

    <Extension>
    Public Sub SetEnumBinding(cbo As System.Windows.Controls.ComboBox, t As Type)
        Dim col As New List(Of FormDisplayValue)

        For Each value As Object In [Enum].GetValues(t)
            Dim dv As New FormDisplayValue([Enum].GetName(t, value), value)

            col.Add(dv)
        Next

        cbo.ItemsSource = col
        cbo.DisplayMemberPath = "Display"
        cbo.SelectedValuePath = "Value"
    End Sub

    <Extension>
    Public Sub AddRange(ByVal xml As XElement, ByVal x As IEnumerable(Of XElement))
        For Each xElem As XElement In x
            xml.Add(xElem)
        Next
    End Sub
    <Extension>
    Public Sub AddRange(ByVal dest As XElement, ByVal src As XElement)
        If src.HasElements Then
            dest.AddRange(src.Elements)
        End If
    End Sub
End Module
