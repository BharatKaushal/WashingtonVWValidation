Imports PBS.Deals.FormsIntegration
Imports Include7.I7
Imports PBS.Comms.VWCommunication.Contract

''' <summary>
''' Created On 04/10/2015 by JDM
''' Modified On 04/14/2015 by JDM re: Created specifically for CountrySide VW (4104).
''' </summary>
''' <remarks></remarks>
Partial Public Class _3LAWWA15VW
    Inherits LaserFormBaseV

    Public Overrides Sub SetProgramID()
        Me.Name = "WA VW Retail Installment Contact 2015 (Previewable-OKI)"
    End Sub

    Public Overrides Sub SetProgramName()
        Me.Id = "LAW 553-WA 2015"
    End Sub

    Public Sub New()
        Me.Init()
    End Sub

    Protected Overrides Sub OnAddPrompts()
        Include7.I7.Initialize(Me.Data)
        InitializeValidationObject()
    End Sub

    Protected Overrides Function OnExecute() As GrapeCity.ActiveReports.Document.SectionDocument
        CheckValidation(False)

        _process.BeginFormExecution(Me)
        Using ar As New _3LEVMN12VW_Front_Layout_P1
            Dim form As New DisplayLibraryAR7(ar, Format.FormatData.GenerateConstantFontTypeAndSize(ar.Buyer.Font), False, -999)

            form.PrintText(ar.DealerNo, Dlr.Code(_promptData.DealerCodePrefix))
            If _DataContext.IsValidated Then form.PrintText(ar.ContractNo, _DataContext.ValidationNumber)

            'Buyer Info
            form.PrintText(ar.Buyer, Buy.Name.LastFirstInit)
            form.PrintText(ar.Buy_USAddr, Buy.Contact.USShortAddress)
            form.PrintText(ar.Buy_C_P_PC, Buy.Contact.CityProvincePostalCode + " " + Buy.Contact.County)
            'Cobuyer Info
            form.PrintText(ar.CoBuyer, Cob(0).Name.LastFirstInit)
            form.PrintText(ar.Cob_USAddr, Cob(0).Contact.USShortAddress)
            form.PrintText(ar.Cob_C_P_PC, Cob(0).Contact.CityProvincePostalCode + " " + Cob(0).Contact.County)
            'Dealer Info
            form.PrintText(ar.Dlr_Name, Dlr.FullName)
            form.PrintText(ar.Dlr_USAddr, Dlr.Contact.Address)
            form.PrintText(ar.Dlr_C_P_PC, Dlr.Contact.CityProvincePostalCode)

            'VehicleInfo Info
            If V(0).Status.StartsWith("N") Then
                form.PrintText(ar.V_Type, "NEW")
            Else
                form.PrintText(ar.V_Type, "USED")
            End If

            form.PrintText(ar.V_Year, V(0).Year)
            form.PrintText(ar.V_Make, V(0).Make)
            form.PrintText(ar.V_Model, V(0).Model)
            form.PrintText(ar.V_Vin, V(0).VIN)
            form.PrintFlag(ar.Business_Tick, VehicleUtility.IsBusinessUse(_DataContext.VehicleInfo))
            form.PrintFlag(ar.Agriculture_Tick, VehicleUtility.IsAgricultureUse(_DataContext.VehicleInfo))
            If VehicleUtility.IsOtherUse(_DataContext.VehicleInfo) Then
                form.PrintFlag(ar.Other_Tick, True)
                form.PrintText(ar.OtherUse, _DataContext.VehicleInfo.OtherUseDescription)
            End If

            'FED BOX
            form.PrintNumber(ar.FED_L1, APR, 2, "-0-")
            form.PrintNumber(ar.FED_L2, FINCHG, 2, "-0-")
            form.PrintNumber(ar.FED_L3, AMTFIN, 2)
            form.PrintNumber(ar.FED_L4, TOTPMT, 2)
            form.PrintNumber(ar.FED_L5A, DOWNPMT, 2, "-0-")
            form.PrintNumber(ar.FED_L5B, TOTALSALEPRICE, 2)

            form.PrintNumber(ar.FED_L6A, _DataContext.PaymentSchedule.PaymentDictionary(PaymentSchedule.FIRSTLINE).PaymentPeriod)
            form.PrintNumber(ar.FED_L6B, _DataContext.PaymentSchedule.PaymentDictionary(PaymentSchedule.FIRSTLINE).Payment, 2)
            form.PrintText(ar.FED_L6C, _DataContext.PaymentSchedule.PaymentDictionary(PaymentSchedule.FIRSTLINE).PaymentStartDate)

            If _DataContext.PaymentSchedule.PaymentDictionary(PaymentSchedule.SECONDLINE).IsValid Then
                form.PrintText(ar.FED_L7, _DataContext.PaymentSchedule.PaymentDictionary(PaymentSchedule.SECONDLINE).PaymentLine())
                If _DataContext.PaymentSchedule.PaymentDictionary(PaymentSchedule.THIRDLINE).IsValid Then form.PrintText(ar.FED_L8, _DataContext.PaymentSchedule.PaymentDictionary(PaymentSchedule.THIRDLINE).PaymentLine())
            End If
            form.PrintText(ar.FED_L9, "")

            ''Late Charge
            form.PrintFlag(ar.BusLateCharge_Tick, Buy.Name.IsBusiness)
            If Buy.Name.IsBusiness Then
                form.PrintNumber(ar.BusLateCharge, _DataContext.VehicleInfo.LateChargeFlat, 2, NA)
                form.PrintNumber(ar.BusLateDays, _DataContext.VehicleInfo.NumberDaysGrace, NA)
                form.PrintNumber(ar.BusLateRate, _DataContext.VehicleInfo.LateChargeRate, 2, NA)
            Else
                form.PrintText(ar.BusLateCharge, NA)
                form.PrintText(ar.BusLateDays, NA)
                form.PrintText(ar.BusLateRate, NA)
            End If
            _process.RegisterLayout(ar)
        End Using
        Using ar As New _3LEVMN12VW_Front_Layout_P2
            Dim form As New DisplayLibraryAR7(ar, Format.FormatData.GenerateConstantFontTypeAndSize(ar.L1A.Font), False, -999)
            'ITEMIZATION OF AMOUNTS
            form.PrintNumber(ar.L1A, L1A, 2, "-0-")
            form.PrintNumber(ar.L1, L1, 2)
            If Not String.IsNullOrWhiteSpace(T(0).Year) Then
                form.PrintText(ar.T_Year, T(0).Year)
            Else
                form.PrintText(ar.T_Year, NA)
            End If
            If Not String.IsNullOrWhiteSpace(T(0).Make) Then
                form.PrintText(ar.T_Make, T(0).Make)
            Else
                form.PrintText(ar.T_Make, NA)
            End If
            If Not String.IsNullOrWhiteSpace(T(0).Model) Then
                form.PrintText(ar.T_Model, T(0).Model)
            Else
                form.PrintText(ar.T_Model, NA)
            End If

            form.PrintNumber(ar.L2A, L2A, 2, NA)
            form.PrintNumber(ar.L2B, L2B, 2, NA)
            form.PrintNumber(ar.L2C, L2C, 2, NA)
            form.PrintNumber(ar.L2D, L2D, 2, NA)
            form.PrintText(ar.L2EDESC, L2EDESC)
            form.PrintNumber(ar.L2E, L2E, 2, NA)
            form.PrintNumber(ar.L2, L2, 2, NA)

            form.PrintNumber(ar.L3, L3, 2, NA)
            form.PrintNumber(ar.L4Ai, L4Ai, 2, NA)
            form.PrintNumber(ar.L4Aii, L4Aii, 2, NA)
            form.PrintNumber(ar.L4A, L4A, 2, NA)
            form.PrintNumber(ar.L4B, L4B, 2, NA)
            form.PrintNumber(ar.L4C, L4C, 2, NA)
            form.PrintNumber(ar.L4D, L4D, 2, NA)
            form.PrintNumber(ar.L4E, L4E, 2, NA)
            form.PrintText(ar.L4FDESC, L4FDESC)
            form.PrintNumber(ar.L4F, L4F, 2, NA)
            form.PrintNumber(ar.L4G, L4G, 2, NA)

            form.PrintText(ar.L4HTo1, L4HTo1)
            form.PrintNumber(ar.L4H1, L4H1, 2, NA)
            form.PrintText(ar.L4HTo2, L4HTo2)
            form.PrintText(ar.L4HFor2, L4HFor2)
            form.PrintNumber(ar.L4H2, L4H2, 2, NA)
            form.PrintText(ar.L4HTo3, L4HTo3)
            form.PrintText(ar.L4HFor3, L4HFor3)
            form.PrintNumber(ar.L4H3, L4H3, 2, NA)
            form.PrintText(ar.L4HTo4, L4HTo4)
            form.PrintText(ar.L4HFor4, L4HFor4)
            form.PrintNumber(ar.L4H4, L4H4, 2, NA)
            form.PrintText(ar.L4HTo5, L4HTo5)
            form.PrintText(ar.L4HFor5, L4HFor5)
            form.PrintNumber(ar.L4H5, L4H5, 2, NA)
            form.PrintText(ar.L4HTo6, L4HTo6)
            form.PrintText(ar.L4HFor6, L4HFor6)
            form.PrintNumber(ar.L4H6, L4H6, 2, NA)
            form.PrintText(ar.L4HTo7, L4HTo7)
            form.PrintText(ar.L4HFor7, L4HFor7)
            form.PrintNumber(ar.L4H7, L4H7, 2, NA)
            form.PrintText(ar.L4HTo8, L4HTo8)
            form.PrintText(ar.L4HFor8, L4HFor8)
            form.PrintNumber(ar.L4H8, L4H8, 2, NA)
            form.PrintText(ar.L4HTo9, L4HTo9)
            form.PrintText(ar.L4HFor9, L4HFor9)
            form.PrintNumber(ar.L4H9, L4H9, 2, NA)
            form.PrintText(ar.L4HTo10, L4HTo10)
            form.PrintText(ar.L4HFor10, L4HFor10)
            form.PrintNumber(ar.L4H10, L4H10, 2, NA)

            form.PrintNumber(ar.L4, L4, 2, NA)
            form.PrintNumber(ar.L5, L5, 2, NA)

            'Insurance
            form.PrintFlag(ar.CreditLife_Tick, (Insurance.CreditLife.Premium <> 0))
            form.PrintFlag(ar.CL_Buy_Tick, Insurance.CreditLife.Status.ToUpper.StartsWith("B") Or Insurance.CreditLife.Status.ToUpper.StartsWith("S"))
            form.PrintFlag(ar.CL_Cob_Tick, Insurance.CreditLife.Status.ToUpper.StartsWith("C"))
            form.PrintFlag(ar.CL_Joint_Tick, Insurance.CreditLife.Status.ToUpper.StartsWith("J"))
            form.PrintFlag(ar.AH_Tick, (Insurance.AccidentHealth.Premium <> 0))
            form.PrintFlag(ar.AH_Buy_Tick, Insurance.AccidentHealth.Status.ToUpper.StartsWith("B") Or Insurance.AccidentHealth.Status.ToUpper.StartsWith("S"))
            form.PrintFlag(ar.AH_Cob_Tick, Insurance.AccidentHealth.Status.ToUpper.StartsWith("C"))
            form.PrintFlag(ar.AH_Joint_Tick, Insurance.AccidentHealth.Status.ToUpper.StartsWith("J"))
            form.PrintNumber(ar.CL_Premium, Insurance.CreditLife.Premium, 2, NA)
            form.PrintNumber(ar.AH_Premium, Insurance.AccidentHealth.Premium, 2, NA)
            'Set based on INSCODE
            'Do Something clever with multiple line Company Name for Insurance Products
            If Insurance.CreditLife.Premium + Insurance.AccidentHealth.Premium <> 0 Then
                form.PrintText(ar.InsCo1L1, _DataContext.DealInfo.InsuranceProductCompany.Name)
                form.PrintText(ar.InsCo1_AddrL1, _DataContext.DealInfo.InsuranceProductCompany.FullAddress)
            Else
                form.PrintText(ar.InsCo1L1, NA)
                form.PrintText(ar.InsCo1_AddrL1, NA)
            End If

            'Other Insurance
            form.PrintFlag(ar.OtherIns1_Tick, (OtherIns1Premium <> 0))
            form.PrintText(ar.OtherIns1Type, OtherIns1Type)
            form.PrintNumber(ar.OtherIns1Term, OtherIns1Term, NA)
            form.PrintNumber(ar.OtherIns1Premium, OtherIns1Premium, 2, NA)
            form.PrintText(ar.OtherInsCo1L1, OtherInsCo1L1)
            form.PrintText(ar.OtherInsCo1_AddrL1, OtherIns1_AddrL1)
            form.PrintFlag(ar.OtherIns2_Tick, (OtherIns2Premium <> 0))
            form.PrintText(ar.OtherIns2Type, OtherIns2Type)
            form.PrintNumber(ar.OtherIns2Term, OtherIns2Term, NA)
            form.PrintNumber(ar.OtherIns2Premium, OtherIns2Premium, 2, NA)
            form.PrintText(ar.OtherInsCo2L1, OtherInsCo2L1)
            form.PrintText(ar.OtherInsCo2_AddrL1, OtherIns2_AddrL1)

            'Optional Finance Charge
            form.PrintFlag(ar.NoFinanceDate_TICK, _DataContext.DealInfo.WaiveFINCHG)
            If _DataContext.DealInfo.WaiveFINCHG Then
                form.PrintText(ar.NoFinanceDate_MMDD, _DataContext.DealInfo.FINCHGWaiveDate.Substring(0, 5))
                form.PrintText(ar.NoFinanceDate_YY, _DataContext.DealInfo.FINCHGWaiveDate.Substring(6, 2))
            Else
                form.PrintText(ar.NoFinanceDate_MMDD, NA)
                form.PrintText(ar.NoFinanceDate_YY, NA)
            End If
            'Optional Gap Contract
            If L4D <> 0 Then
                form.PrintText(ar.GapProvider, Protection("J").Provider)
                form.PrintNumber(ar.GapTerm, DataContext.DealInfo.GapTerm, NA)
            Else
                form.PrintText(ar.GapProvider, NA)
                form.PrintText(ar.GapTerm, NA)
            End If
            _process.RegisterLayout(ar)
        End Using
        Using ar As New _3LEVMN12VW_Front_Layout_P3
            Dim form As New DisplayLibraryAR7(ar, Format.FormatData.GenerateOKIFontAndSize, False, -999)
            _process.RegisterLayout(ar)
        End Using
        Using ar As New _3LEVMN12VW_Front_Layout_P4
            Dim form As New DisplayLibraryAR7(ar, Format.FormatData.GenerateConstantFontTypeAndSize(ar.Dlr_Name_Sign.Font), False, -999)
            'Signatures
            form.PrintText(ar.Buy_Sign_Date, D.Contract.ToString("MM/dd/yy"))
            If Not String.IsNullOrWhiteSpace(Cob(0).Name.LastFirstInit) Then form.PrintText(ar.Cob_Sign_Date, D.Contract.ToString("MM/dd/yy"))
            form.PrintText(ar.Dlr_Name_Sign, Dlr.Name)
            form.PrintText(ar.Dlr_Sign_Date, D.Contract.ToString("MM/dd/yy"))
            If Not String.IsNullOrWhiteSpace(Cob(1).Name.LastFirstInit) Then
                form.PrintText(ar.Cob2_USAddr, Cob(1).Contact.USShortAddress)
            Else
                form.PrintText(ar.Cob2_USAddr, NA)
            End If
            form.PrintText(ar.Dlr_Title, SalesStaff.SalesManager(0).Title)
            _process.RegisterLayout(ar)
        End Using
        Return _process.EndFormExecution
    End Function
End Class
