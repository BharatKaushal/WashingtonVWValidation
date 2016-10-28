Imports PBS.Deals.FormsIntegration
Imports PBS.Comms.VWCommunication.Contract
Imports System.ComponentModel
Imports System.Windows.Controls

Partial Public Class _3LAWWA15VW_View
    Implements IPromptResponseWPF

    Public Property Model As VWCreditProcess
        Get
            Return DataContext
        End Get
        Set(value As VWCreditProcess)
            DataContext = value
        End Set
    End Property

    Private Sub Validate(sender As Object, e As Windows.RoutedEventArgs)
        Model.CheckRules()
        Model.GenerateRequirement()
        validationPnl.IsOpen = True
    End Sub

    Private Sub Submit(sender As Object, e As Windows.RoutedEventArgs)
        Model.ContractSpecificBase.ShouldSubmit = True
        Dim cd As _3LAWWA15VW_ContractDescription = TryCast(Model.ContractSpecificBase, _3LAWWA15VW_ContractDescription)
        If cd IsNot Nothing Then OperationResult = 1
        Me.Close()
    End Sub

    Private Sub Cancel(sender As Object, e As Windows.RoutedEventArgs)
        Dim cd As _3LAWWA15VW_ContractDescription = TryCast(Model.ContractSpecificBase, _3LAWWA15VW_ContractDescription)
        If cd IsNot Nothing Then OperationResult = 2
        Me.Close()
    End Sub
    Private Sub Preview(sender As Object, e As Windows.RoutedEventArgs)
        Dim cd As _3LAWWA15VW_ContractDescription = TryCast(Model.ContractSpecificBase, _3LAWWA15VW_ContractDescription)
        If cd IsNot Nothing Then OperationResult = 1
        Me.Close()
    End Sub
    Public Shadows Sub OnClosing(ByVal sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Model Is Nothing Then
            Dim procArgs As New VWCreditProcess.VWCreditProcessArgs With {.ContractSpecificBase = New _3LAWWA15VW_ContractDescription, .PreviousRun = Nothing, .CurrentRun = Nothing}
            Model = VWCreditProcess.Fetch(procArgs)
            Model.ContractSpecificBase.ShouldSubmit = False
            OperationResult = 2
        End If
    End Sub

    Public ReadOnly Property DataResponse As Dictionary(Of String, Object) Implements IPromptResponseWPF.DataResponse
        Get
            Dim d As New Dictionary(Of String, Object)
            Model.ReplicateCurrentState(d)
            Return d
        End Get
    End Property

    Public Property OperationResult As Object Implements IPromptResponseWPF.OperationResult
    Private Sub OnSelectionChanged(ByVal sender As Object, ByVal rEvt As EventArgs)
        If TypeOf sender Is TabControl Then DirectCast(sender, TabControl).Focus()
    End Sub

End Class
