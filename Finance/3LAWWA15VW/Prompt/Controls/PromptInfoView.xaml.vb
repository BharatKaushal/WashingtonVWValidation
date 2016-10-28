Imports PBS.Comms.VWCommunication.Contract

Public Class PromptInfoView
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        PrimaryUseCombo.SetDataBinding(FormDisplayList.ToDisplayList(VehicleUsageType.FetchList))
        FINCHGWaiverComboYN.SetEnumBinding(GetType(YesNo))
        SellerAssignmentCombo.SetDataBinding(FormDisplayList.ToDisplayList(FinancialAssignmentType.FetchList))
    End Sub
End Class
