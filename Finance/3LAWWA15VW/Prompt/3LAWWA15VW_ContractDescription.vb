Imports Csla
Imports PBS.Deals.FormsIntegration
Imports PBS.Comms.VWCommunication.Contract

Public Class _3LAWWA15VW_ContractDescription
    Inherits PBS.Comms.VWCommunication.Contract.FormSpecificBase

#Region "  Properties "
    Public Overrides ReadOnly Property DealerCodePrefix As String
        Get
            Return "VCI"
        End Get
    End Property
    Public Overrides ReadOnly Property ExecutionLanguage As String
        Get
            Return "en-US"
        End Get
    End Property
    Public Overrides ReadOnly Property ExecutionState As String
        Get
            Return "MN"
        End Get
    End Property
    Public Overrides ReadOnly Property Manufacturer As String
        Get
            Return "Volkswagen"
        End Get
    End Property
    Public Overrides ReadOnly Property FormNumber As String
        Get
            Return "553-MN"
        End Get
    End Property
    Public Overrides ReadOnly Property IsRetailContract As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides ReadOnly Property Revision As String
        Get
            Return "2012-10-01"
        End Get
    End Property

    Public Property FINCHGWaiver As Boolean = False
    Public Property GAPWaiver As Boolean = False
    Public Property SellerAssignment As FinancialAssignment = FinancialAssignment.AssignmentWithoutRecourse
#End Region
End Class
Public Class PromptDataTabItem

End Class
