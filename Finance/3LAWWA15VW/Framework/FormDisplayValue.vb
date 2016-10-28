Public Class FormDisplayValue

    Private m_display As String = String.Empty
    Private m_value As Object = String.Empty
#Region "  Properties "
    Public ReadOnly Property Display() As String
        Get
            Return m_display
        End Get
    End Property
    Public ReadOnly Property Value() As Object
        Get
            Return m_value
        End Get
    End Property
#End Region

#Region "  Data Access "
    Sub New(ByVal dField As String, ByVal value As Object)
        m_display = dField
        m_value = value
    End Sub

    Sub New(dField As Object)
        m_display = dField.ToString
        m_value = dField
    End Sub
#End Region

    Public Shared Function ConvertToDisplayValueCollection(l As List(Of String)) As List(Of FormDisplayValue)
        Dim lfdv = From s In l
                   Select New FormDisplayValue(l)
        Return lfdv.ToList
    End Function
End Class
