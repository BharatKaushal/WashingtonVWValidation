Imports PBS.Deals.FormsIntegration
Imports Include7.I7
Imports System.Reflection
Imports System.Xml.Linq
Imports <xmlns="http://www.starstandards.org/STAR">
Imports PBS.Comms.VWCommunication.Contract

Partial Public Class _3LAWWA15VW
    Private vNumber As String = ""
    Private creditAppNumber As String = ""
    Private validated As Boolean = False
    Public Property DataContext As VWCreditProcess
    Private promptView As _3LAWWA15VW_View = Nothing
    Private XMLGen As _3LAWWA15VW_XML
    Private _nationalStandard As NationalStandard

    Private Property State As PBS.Deals.FormsIntegration.ContractStateObject

    Public Overrides Sub SetState(state As ContractStateObject)
        Me.State = state
        If state IsNot Nothing Then
            vNumber = state.ValidationNumber
            validated = state.PreviousValidationResult
            creditAppNumber = state.CreditNumber
        End If
    End Sub

    Public Overrides Function FetchState() As ContractStateObject
        If _DataContext Is Nothing OrElse _DataContext.ContractSpecificBase Is Nothing Then Return Nothing
        GenerateState()
        Return State
    End Function

    Private Sub InitializeValidationObject()
        _promptData = New _3LAWWA15VW_ContractDescription
        Dim vwArgs As New VWCreditProcess.VWCreditProcessArgs With {.ContractSpecificBase = _promptData, .PreviousRun = Me.Prompts.PreviousRun, .CurrentRun = Me.Data}
        _DataContext = VWCreditProcess.Fetch(vwArgs)
        _DataContext.ValidationNumber = vNumber
        If Not String.IsNullOrWhiteSpace(creditAppNumber) Then _DataContext.ProcessInfo.CreditApplicationSourceNumber = creditAppNumber
        _DataContext.ContractSpecificBase.VWValidated = validated

        _DataContext.DealInfo.InsuranceProductCompany.Name = "Madison National Life"
        _DataContext.DealInfo.InsuranceProductCompany.FullAddress = "P.O. Box 5008, Madison, WI 53705"
        _DataContext.DealInfo.GapPrice = Include7.I7.Protection("J").Price


        promptView = New _3LAWWA15VW_View
        promptView.Model = _DataContext
        promptView.Model.CheckRules()
        promptView.Model.GenerateRequirement()
        CheckValidation(True)
        Me.Prompts.AddWPFPrompt(promptView)
    End Sub

    Public Sub GenerateState()
        XMLGen = New _3LAWWA15VW_XML(Me)
        GenerateXML()
    End Sub

    Public Sub CheckValidation(ByVal FormCalc As Boolean)
        If _DataContext.ContractSpecificBase.VWValidated AndAlso State IsNot Nothing Then
            If FormCalc Then Me.Calculate()
            GenerateState()
            _DataContext.ContractSpecificBase.VWValidated = (State.CurrentHash = State.PreviousHash)
        End If
        _DataContext.IsValidated = _DataContext.ContractSpecificBase.PBSValidated AndAlso _DataContext.ContractSpecificBase.VWValidated
    End Sub

    Private Function GenerateXML() As XElement
        Me.Calculate()
        Dim bodId As Guid = Guid.NewGuid

        _nationalStandard = NationalStandard.Fetch(_DataContext.ProcessInfo.IsCanadian)

        Dim indivXML As XElement = XMLGen.FetchIndividualApplicant()
        Dim coAppXML() As XElement = XMLGen.FetchCoApplicant()
        Dim orgXML As XElement = XMLGen.FetchOrganizationApplicant(Include7.Buy.ContactName)
        Dim vehXML As XElement = XMLGen.FetchCreditVehicle()
        Dim finXML As XElement = XMLGen.FetchFinancing()
        Dim addXML() As XElement = XMLGen.FetchAdditionalContractAttributes()
        Dim guarXML As XElement = XMLGen.FetchGuarantor()
        Dim starXML As XElement = <ProcessCreditContract xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" revision="4.4.4" lang=<%= DataContext.ContractSpecificBase.ExecutionLanguage %> xmlns="http://www.starstandards.org/STAR"></ProcessCreditContract>
        starXML.AddAnnotation(SaveOptions.OmitDuplicateNamespaces)
        starXML.Add(XMLGen.ApplicationArea(bodId))
        starXML.Add(<DataArea>
                        <Process/>
                        <CreditContract>
                            <%= XMLGen.Header(D.Contract.DateObj, D.ID) %>
                            <%= XMLGen.FinanceDetail() %>
                            <%= XMLGen.DealerDetail() %>
                            <%= indivXML %>
                            <%= coAppXML %>
                            <%= orgXML %>
                            <%= vehXML %>
                            <%= finXML %>
                            <%= addXML %>
                            <%= guarXML %>
                        </CreditContract>
                    </DataArea>
        )
        If State Is Nothing Then State = New PBS.Deals.FormsIntegration.ContractStateObject
        State.Submit = _DataContext.ContractSpecificBase.ShouldSubmit
        State.Manufacturer = _DataContext.ContractSpecificBase.Manufacturer
        State.CurrentHash = starXML...<DataArea>.First().ToString.GetHashCode
        State.XML = starXML
        State.BodId = bodId

        Return starXML
    End Function

End Class

Public Class _3LAWWA15VW_XML
    Inherits BaseGenerateXML

    Private _form As _3LAWWA15VW

    Public Sub New(form As _3LAWWA15VW)
        MyBase.New(form.DataContext, D.Contract.DateObj, D.AmortTerm, D.Term)
        _form = form
    End Sub

    Public Overrides Function FetchCreditVehicle() As XElement
        Dim xml As XElement = <CreditVehicle></CreditVehicle>
        xml.AddRange(BaseVehicleXML(V(0).Year, V(0).Make, V(0).Model))
        xml.AddRange(DetailVehicleXML(V(0).VIN, V(0).Odometer, V(0).Color))

        For i As Integer = 0 To Include7.I7.LibraryOptions.VehicleOptionCount
            If String.IsNullOrWhiteSpace(V(0).VehicleOption.Item(i).Code) Then Continue For
            xml.Add(VehicleOptionXML(V(0).VehicleOption.Item(i).Code))
        Next

        'Required for leasing
        If IsLease Then
            xml.AddRange(VehicleLeasePricingXML(Include7.I7.Lease.GrossCapitalCost, Include7.I7.Lease.NetLease))
        End If

        xml.Add(<VehicleUse><%= XMLUtility.FormatVehicleUse(XMLContext.VehicleInfo.Use) %></VehicleUse>)

        xml.AddRange(LoadDealerProductXML())

        Return xml
    End Function
    Public Overrides Function FetchTradeXML(balance As Decimal, netTrade As Decimal, grossTrade As Decimal, priorCredit As Decimal) As XElement
        If T.Count = 0 Then Return Nothing
        Dim xml As XElement = <TradeIn></TradeIn>
        xml.AddRange(BaseVehicleXML(T(0).Year, T(0).Make, T(0).Model))
        xml.Add(TradeInFinancingXML(balance, netTrade, grossTrade, priorCredit))
        xml.AddRange(TradeVehicleDeliveryMileageVIN(T(0).VIN, T(0).Odometer))
        Return xml
    End Function

    Public Overrides Function LoadCapitalizedFeeXML() As XElement
        '??? Means the dealership has added a Service Contract without a corresponding request to us.
        Return <root>
                   <%= FeeXML(GeneralFeeType.Documentation, Fee.Admin.Total, False) %>
                   <%= FeeXML(GeneralFeeType.Registration, Fee.Gas.Total, False) %>
                   <%= FeeXML(GeneralFeeType.Title, Fee.Tire.Total, False) %>
                   <%= FeeXML(GeneralFeeType.ElectronicFiling, Fee.Batt.Total, False) %>
                   <%= FeeXML(GeneralFeeType.License, Fee.Lic.Total, False) %>
                   <%= FeeXML(GeneralFeeType.OfficialFeesPaidToGovtAgencies, Fee.Reg.Total, False) %>
               </root>
        'TAX NOT A FEE<%= FeeXML("Taxes Not In Cash Price", Fee.Other.Total, False) %>
        '<%= FeeXML("AcquistionFee", Fee.LeaseAdmin.Total, False) %> Leasing
    End Function
    Public Overrides Function LoadDealerProductXML() As XElement
        'Dealer Products SHOULD ONLY BE A SERVICE CONTRACT OR DEALER PRODUCT NOT BOTH!
        'Etching|Theft Deterrent|Surface Protection|Other
        Return <root>
                   <%= DealerProductXMLUtil(DealerProductType.Unknown, False, FormIndex.B) %>
                   <%= DealerProductXMLUtil(DealerProductType.Etching, False, FormIndex.C) %>
                   <%= DealerProductXMLUtil(DealerProductType.Other, False, FormIndex.D) %>
                   <%= DealerProductXMLUtil(DealerProductType.Unknown, False, FormIndex.E) %>
                   <%= DealerProductXMLUtil(DealerProductType.Unknown, False, FormIndex.F) %>
                   <%= DealerProductXMLUtil(DealerProductType.SurfaceProection, False, FormIndex.G) %>
                   <%= DealerProductXMLUtil(DealerProductType.Unknown, False, FormIndex.H) %>
                   <%= DealerProductXMLUtil(DealerProductType.Unknown, False, FormIndex.I) %>
               </root>
    End Function
    Private Function DealerProductXMLUtil(ByVal t As DealerProductType, InsideCarrierTypeInd As Boolean, ByVal Index As FormIndex) As XElement
        Return DealerProductXML(New DealerProductArguments With {.Type = t,
                                                                     .InsideCarrierTypeInd = InsideCarrierTypeInd,
                                                                     .Provider = MultiWarranty(ConvertFormIndexToLetter(Index)).Provider,
                                                                     .Term = MultiWarranty(ConvertFormIndexToLetter(Index)).Term,
                                                                     .Mileage = MultiWarranty(ConvertFormIndexToLetter(Index)).Mileage,
                                                                     .Price = MultiWarranty(ConvertFormIndexToLetter(Index)).Price,
                                                                     .Capitalized = MultiWarranty(ConvertFormIndexToLetter(Index)).Capitalized <> 0})
    End Function
    Public Overrides Function LoadServiceContractXML() As XElement
        'Service Contracts SHOULD ONLY BE A SERVICE CONTRACT OR DEALER PRODUCT NOT BOTH!
        'ExtendedServiceContract|Maint. Plan|PaintlessDentRepair|RoadAndHazard|Svc. Plan|TheftRecovery|Audi Care|Tint/Pinstripe
        Return <root>
                   <%= ServiceContractXMLUtil(ServiceContractType.ExtendedServiceContract, True, FormIndex.A) %>
                   <%= ServiceContractXMLUtil(ServiceContractType.SvcPlan, True, FormIndex.B) %>
                   <%= ServiceContractXMLUtil(ServiceContractType.MaintPlan, True, FormIndex.C) %>
                   <%= ServiceContractXMLUtil(ServiceContractType.RoadAndHazard, False, FormIndex.D) %>
                   <%= ServiceContractXMLUtil(ServiceContractType.PaintlessDentRepair, False, FormIndex.E) %>
                   <%= ServiceContractXMLUtil(ServiceContractType.TheftRecovery, True, FormIndex.F) %>
                   <%= ServiceContractXMLUtil(ServiceContractType.TintPinstripe, False, FormIndex.G) %>
                   <%= AudiCareXML(MultiWarranty("J").Description, "Audi Care", MultiWarranty("J").Term, MultiWarranty("J").Mileage, MultiWarranty("J").Price, True, True) %>
               </root>
        'H & I Should be an Insurance...
    End Function
    Private Function ServiceContractXMLUtil(ByVal t As ServiceContractType, InsideCarrierTypeInd As Boolean, ByVal Index As FormIndex) As XElement
        Dim condenseArgs As New ServiceContractArguments With {.Type = t,
                                                               .InsideCarrierTypeInd = InsideCarrierTypeInd,
                                                               .Price = MultiWarranty(ConvertFormIndexToLetter(Index)).Price,
                                                               .Capitalized = MultiWarranty(ConvertFormIndexToLetter(Index)).Capitalized <> 0,
                                                               .Provider = MultiWarranty(ConvertFormIndexToLetter(Index)).Provider,
                                                               .Term = MultiWarranty(ConvertFormIndexToLetter(Index)).Term,
                                                               .Mileage = MultiWarranty(ConvertFormIndexToLetter(Index)).Mileage}
        Return ServiceContractXML(condenseArgs)
    End Function
    Public Overrides Function LoadInsuranceProductXML() As XElement
        'Insurance Products
        'Credit Life | Credit Disability | Tire And Wheel | Excess Protection Wear and Tear | GAP | OTHER | SingleInterest | Mechanical Breakdown
        Return <root>
                   <%= InsuranceXML(New InsuranceProductArguments With {.Type = InsuranceProductType.CreditLife, .Capitalized = True, .Provider = XMLContext.DealInfo.InsuranceProductCompany.Name, .Price = Insurance.CreditLife.Premium, .Term = Insurance.CreditLife.Term, .InsideCarrierTypeInd = True}) %>
                   <%= InsuranceXML(New InsuranceProductArguments With {.Type = InsuranceProductType.CreditDisability, .Capitalized = True, .Provider = XMLContext.DealInfo.InsuranceProductCompany.Name, .Price = Insurance.AccidentHealth.Premium, .Term = Insurance.AccidentHealth.Term}) %>
                   <%= InsuranceXMLUtil(InsuranceProductType.ExcessProtectionWearAndTear, False, FormIndex.H) %>
                   <%= InsuranceXMLUtil(InsuranceProductType.MechanicalBreakdown, False, FormIndex.I) %>
                   <%= InsuranceXML(New InsuranceProductArguments With {.Type = InsuranceProductType.GAP, .InsideCarrierTypeInd = True, .Provider = Protection(ConvertFormIndexToLetter(FormIndex.J)).Provider, .Price = Protection(ConvertFormIndexToLetter(FormIndex.J)).Price, .Term = XMLContext.DealInfo.GapTerm, .Capitalized = Protection(ConvertFormIndexToLetter(FormIndex.J)).Capitalized <> 0}) %>
               </root>
        '<%= InsuranceXML("Mechanical Breakdown", MultiWarranty("I").Provider, MultiWarranty("I").Price, True, False, MultiWarranty("I").Term, MultiWarranty("I").Mileage) %>
    End Function
    Private Function InsuranceXMLUtil(ByVal t As InsuranceProductType, InsideCarrierTypeInd As Boolean, ByVal index As FormIndex) As XElement
        Return InsuranceXML(New InsuranceProductArguments With {.Type = t,
                                                                .InsideCarrierTypeInd = InsideCarrierTypeInd,
                                                                .Provider = MultiWarranty(ConvertFormIndexToLetter(index)).Provider,
                                                                .Term = MultiWarranty(ConvertFormIndexToLetter(index)).Term,
                                                                .Mileage = MultiWarranty(ConvertFormIndexToLetter(index)).Mileage,
                                                                .Price = MultiWarranty(ConvertFormIndexToLetter(index)).Price,
                                                                .Capitalized = MultiWarranty(ConvertFormIndexToLetter(index)).Capitalized <> 0})
    End Function

#Region "   Financing"
    Public Overrides Function FetchFinancing() As XElement
        Dim xml As XElement = BaseFinancingXML(D.Price)
        'Hard Add - Selling price of Accessories that don't increase MSRP
        'Not on contract... not sure what to put in here
        'xml.Add(FormatToOXLOCurrency(<HardAddSellingPrice/>, 0))
        xml.AddRange(BasicFinancingFieldsXML(_form.L1, _form.L2, _form.L2D))

        'All but Canada Retail
        If Rebate.Total.Value <> 0 Then xml.Add(FormatToOXLOCurrency(<ManufacturerRebateAmount/>, Rebate.Total.Value))

        'Retail and Balloon, not lease 
        If IsRetail Or IsBalloon Then
            xml.Add(FormatToOXLOCurrency(<UnpaidBalance/>, _form.L3))
            xml.Add(FormatToOXLOCurrency(<TotalAmountPaidOnYourBehalf/>, _form.L4))
            xml.Add(FormatToOXLOCurrency(<AmountFinanced/>, D.BalanceFinanced))
        End If

        'Lease and Balloon US
        If (IsBalloon And Not IsCanadian) Or IsLease Then
            xml.Add(FormatToOXLOMileage(<AnnualMilesAllowed/>, 0))
            xml.Add(FormatToOXLOCurrency(<ExcessMileageRate/>, 0D))
        End If

        Dim FTIL As XElement = BaseFederalTILDisclosureXML(_form.APR, _form.FINCHG, _form.TOTPMT, Nothing)
        FTIL.AddRange(AdditionalFederalTILDisclosureXML(_form.TOTALSALEPRICE, D.PaymentDate.DateObj))
        FTIL.AddRange(AppendPaymentScheduleXML(D.Payment, Include7.I7.Lease.BasePayment))
        If IsLease Then
            FTIL.Add(<LeaseRateMoneyFactor><%= D.APR %></LeaseRateMoneyFactor>)
        End If
        xml.Add(FTIL)

        'Programs and rates
        xml.Add(ProgramAndRatesXML(New XMLLeaseMileage With {.allowedMiles = 0, .upfrontMileage = 0, .upfrontMileageRate = 0D, .years = (D.Term / D.PaymentsPerYear)}))

        'Add all the taxes that are applicable to the state\Province
        xml.Add(TaxXML(TaxType.Sales, _form.L1A))
        xml.Add(TaxXML(TaxType.TaxesNotInCashPrice, Fee.Other.Total))


        'Fees, add all that matter and what they are
        xml.AddRange(LoadCapitalizedFeeXML)
        'Other Charges
        'xml.Add(OtherChargesXML(_form.OtherCharges, True))

        'Vehicle Insurance - Required
        xml.Add(VehicleInsuranceXML)

        'Other Insurance - State specific
        xml.AddRange(LoadInsuranceProductXML)
        'Service Contracts
        xml.AddRange(LoadServiceContractXML)

        'Trade - Looks like they only allow 1
        If Not String.IsNullOrWhiteSpace(T(0).Year) Then xml.Add(FetchTradeXML(_form.L2B, _form.L2C, _form.L2A, _form.L4H1))

        Dim estFees As Decimal = Fee.Admin.Total
        estFees += Fee.Air.Total
        estFees += Fee.Batt.Total
        estFees += Fee.Freight.Total
        estFees += Fee.Gas.Total
        estFees += Fee.LeaseAdmin.Total
        estFees += Fee.Lic.Total
        estFees += Fee.Other.Total
        estFees += Fee.Reg.Total
        estFees += Fee.Tire.Total

        ''Lease only
        If IsLease Then
            xml.Add(FormatToOXLOCurrency(<SecurityDepositAmount/>, Lease.Deposit))
            xml.Add(FormatToOXLOCurrency(<DepreciationAndAmortizedAmts/>, Lease.NetLease - Lease.NetResidual))

            ''US Lease Only
            If Not IsCanadian Then
                xml.Add(FormatToOXLOCurrency(<TotalAmtOfBaseMonthlyPayments/>, Lease.BasePayment * D.Term))

                Dim estFeesNTax As Decimal = Lease.CapitalizedTaxTotal + Lease.UpfrontTaxTotal

                For Each dc As Decimal In Lease.TaxOnPayment
                    estFeesNTax += dc * D.Term
                Next

                estFeesNTax += estFees

                xml.Add(FormatToOXLOCurrency(<TotalEstimatedFeesAndTaxesAmt/>, estFeesNTax))
            End If
        End If

        'Lease and Balloon
        If IsLease Then
            xml.Add(FormatToOXLOCurrency(<ResidualAmount/>, Lease.NetResidual))
        ElseIf IsBalloon Then
            xml.Add(FormatToOXLOCurrency(<ResidualAmount/>, D.UnpaidBalance))
        End If

        'Automatic Payment, Canada Only not sure if needed
        'TODO automatic payment if needed

        'Applies to some states only, not sure which ones
        xml.Add(FormatToOXLOCurrency(<DisabilityAndLifeSubtotalAmt/>, _form.L4A))

        'Required for Leases
        If IsLease Then
            xml.Add(<TaxExempt>0</TaxExempt>) 'NEED THIS
            xml.Add(FormatToOXLOCurrency(<TotalOfMonthlyPaymentsAmount/>, D.Payment * D.Term))
            xml.Add(FormatToOXLOCurrency(<TotalDueAtSigningAmount/>, Lease.PayableOnDelivery))
            xml.Add(FormatToOXLOCurrency(<PurchaseOptionPrice/>, Lease.NetResidual))
            'xml.Add(<ServiceChargeAmount currency=<%= Currency %>></ServiceChargeAmount>)

            'Canada Lease
            If IsCanadian Then
                xml.Add(FormatToOXLOCurrency(<FeeSubtotal/>, estFees))
                xml.Add(FormatToOXLOCurrency(<TotalCostOfLease/>, 0D)) 'Add in Canadian
            End If
        End If

        ''This field can appear a maximum of 2 times, there are 3 different types and is state specific. 
        ''Too bad I can't find these fields in the state specific bindings
        'xml.Add(<CreditContractFinanceSubtotals><FinanceSubtotalType>Excluding Downpayment</FinanceSubtotalType><FinanceSubtotalAmount currency=<%= Currency %>></FinanceSubtotalAmount></CreditContractFinanceSubtotals>)
        'xml.Add(<CreditContractFinanceSubtotals><FinanceSubtotalType>Other</FinanceSubtotalType><FinanceSubtotalAmount currency=<%= Currency %>></FinanceSubtotalAmount></CreditContractFinanceSubtotals>)
        'xml.Add(<CreditContractFinanceSubtotals><FinanceSubtotalType>N/A</FinanceSubtotalType><FinanceSubtotalAmount currency=<%= Currency %>></FinanceSubtotalAmount></CreditContractFinanceSubtotals>)

        'Canada Lease
        If IsCanadian And IsLease Then
            xml.Add(FormatToOXLOCurrency(<UpfrontCashDownpaymentAmount/>, 0D))
            xml.Add(FormatToOXLOCurrency(<UpfrontMfgRebateAmount/>, 0D))
            xml.Add(FormatToOXLOCurrency(<UpfrontNetTradeAmount/>, 0D))
            xml.Add(<TaxCreditGroup><TaxCredit><TaxCreditAmount currency=<%= Currency %>><TaxCreditTypeCode>Other</TaxCreditTypeCode></TaxCreditAmount></TaxCredit></TaxCreditGroup>)
        End If

        Return xml
    End Function

    'Unfortunately this uses the Include7 heavily and no reason to check this in ContractValidation
    Public Function VehicleInsuranceXML() As XElement
        Dim xml As XElement = GenerateVehicleInsuranceXML(InsuranceCompany.PolicyExpiry.DateObj, InsuranceCompany.PolicyEffective.DateObj)

        ''Required for Canada
        If IsCanadian Then
            xml.Add(<InsuranceDetail><InsuranceDetailType>Collision</InsuranceDetailType><%= FormatToOXLOCurrency(<DeductibleAmount/>, 0D) %></InsuranceDetail>)
            xml.Add(<InsuranceDetail><InsuranceDetailType>Comprehensive</InsuranceDetailType><%= FormatToOXLOCurrency(<DeductibleAmount/>, 0D) %></InsuranceDetail>)
            xml.Add(<InsuranceDetail><InsuranceDetailType>Liability</InsuranceDetailType><%= FormatToOXLOCurrency(<DeductibleAmount/>, 0D) %></InsuranceDetail>)
        End If

        Return xml
    End Function
#End Region

End Class
