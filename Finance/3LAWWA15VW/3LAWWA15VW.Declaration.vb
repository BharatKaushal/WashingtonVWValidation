Imports PBS.Deals.FormsIntegration
Imports PBS.Comms.VWCommunication.Contract

Partial Public Class _3LAWWA15VW

    Private _process As New FormEngineSlick
    Private _promptData As _3LAWWA15VW_ContractDescription

#Region "Form Variables "
    Public License As Decimal = 0D
    Public OtherCharges As Decimal = 0D

    'Prompt Response
    Public _CLSingleSelectionResponse As String = String.Empty
    Public _AHSingleSelectionResponse As String = String.Empty
    Public _FINCHGRESPONSE As Boolean = True
    Public _FINCHGRESPONSE_DATE As Include7.Forms.Critical.DateInc7
    Public _LATECHARGE As Decimal = 0D
    Public _LATEDAYS As Integer = 0
    Public _LATERATE As Decimal = 0D
    Public _GAPPROVIDER As String = NA
    Public _GAPTERM As Integer = 0
    Public _GAP As Decimal = 0
    Public _ASSIGNCHOICERESPONSE As String = "Assigned W/Recourse"
    Public _AssigneeResponse As String = NA
    Public _AssigneeTitleResponse As String = NA
    Public _AssigneeTitleReponse As String = NA

    'FED BOX
    Public APR As Decimal = 0D
    Public FINCHG As Decimal = 0D
    Public AMTFIN As Decimal = 0D
    Public TOTPMT As Decimal = 0D
    Public DOWNPMT As Decimal = 0D
    Public TOTALSALEPRICE As Decimal = 0D

    'Itemized Variables
    Public L1A As Decimal = 0D
    Public L1 As Decimal = 0D
    Public L2A As Decimal = 0D
    Public L2B As Decimal = 0D
    Public L2C As Decimal = 0D
    Public L2D As Decimal = 0D
    Public L2EDESC As String = NA
    Public L2E As Decimal = 0D
    Public L2 As Decimal = 0D
    Public L3 As Decimal = 0D
    Public L4Ai As Decimal = 0D
    Public L4Aii As Decimal = 0D
    Public L4A As Decimal = 0D
    Public L4B As Decimal = 0D
    Public L4C As Decimal = 0D
    Public L4D As Decimal = 0D
    Public L4E As Decimal = 0D
    Public L4FDESC As String = NA
    Public L4F As Decimal = 0D
    Public L4G As Decimal = 0D
    Public L4HTo1 As String = NA
    Public L4HFor1 As String = NA
    Public L4H1 As Decimal = 0D
    Public L4HTo2 As String = NA
    Public L4HFor2 As String = NA
    Public L4H2 As Decimal = 0D
    Public L4HTo3 As String = NA
    Public L4HFor3 As String = NA
    Public L4H3 As Decimal = 0D
    Public L4HTo4 As String = NA
    Public L4HFor4 As String = NA
    Public L4H4 As Decimal = 0D
    Public L4HTo5 As String = NA
    Public L4HFor5 As String = NA
    Public L4H5 As Decimal = 0D
    Public L4HTo6 As String = NA
    Public L4HFor6 As String = NA
    Public L4H6 As Decimal = 0D
    Public L4HTo7 As String = NA
    Public L4HFor7 As String = NA
    Public L4H7 As Decimal = 0D
    Public L4HTo8 As String = NA
    Public L4HFor8 As String = NA
    Public L4H8 As Decimal = 0D
    Public L4HTo9 As String = NA
    Public L4HFor9 As String = NA
    Public L4H9 As Decimal = 0D
    Public L4HTo10 As String = NA
    Public L4HFor10 As String = NA
    Public L4H10 As Decimal = 0D
    Public L4 As Decimal = 0D
    Public L5 As Decimal = 0D

    'INSCO INFO
    Public INSCO1L1 As String = NA
    Public INSCO1L2 As String = NA
    Public INSCO1_ADDRL1 As String = NA
    Public INSCO1_ADDRL2 As String = NA
    Public OtherIns1Premium As Decimal = 0D
    Public OtherIns1Type As String = NA
    Public OtherIns1Term As Integer = 0
    Public OtherInsCo1L1 As String = NA
    Public OtherInsCo1L2 As String = NA
    Public OtherIns1_AddrL1 As String = NA
    Public OtherIns1_AddrL2 As String = NA
    Public OtherIns2Premium As Decimal = 0D
    Public OtherIns2Type As String = NA
    Public OtherIns2Term As Integer = 0
    Public OtherInsCo2L1 As String = NA
    Public OtherInsCo2L2 As String = NA
    Public OtherIns2_AddrL1 As String = NA
    Public OtherIns2_AddrL2 As String = NA
#End Region
End Class
