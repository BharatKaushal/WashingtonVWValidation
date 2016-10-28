Imports Include7.I7
Imports PBS.Comms.VWCommunication.Contract

Partial Public Class _3LAWWA15VW
    Private Class FormTuple
        Implements Include7.IFormMapItem

        Public Property Name As String Implements Include7.IFormMapItem.Name
        Public Property Value As Decimal Implements Include7.IFormMapItem.Value
        Public Property ForPS As String

        Public Sub New(ByVal val As Decimal, toProvider As String, PS As String)
            Value = val
            Name = toProvider
            ForPS = PS
        End Sub
    End Class
    Public Overrides Sub Calculate()

        L1A = D.TotalTax(0) + D.TotalTax(1) + D.TotalTax(2) + D.TotalTax(3) + D.TotalTax(4)
        L1 = D.Price + L1A + Accessory.Total.Price + Fee.Freight.Total + Fee.Air.Total
        L1 = L1 + Include7.I7.MultiWarranty.Total.Price - Include7.I7.MultiWarranty("A").Price - Include7.I7.MultiWarranty("B").Price - Include7.I7.MultiWarranty("C").Price - Include7.I7.MultiWarranty("D").Price - Include7.I7.MultiWarranty("E").Price - Include7.I7.MultiWarranty("F").Price - Include7.I7.MultiWarranty("G").Price - Include7.I7.MultiWarranty("H").Price - Include7.I7.MultiWarranty("I").Price - Include7.I7.MultiWarranty("J").Price
        L1 = L1 + Include7.I7.Protection.Total.Price - Include7.I7.Protection("A").Price - Include7.I7.Protection("B").Price - Include7.I7.Protection("C").Price - Include7.I7.Protection("D").Price - Include7.I7.Protection("E").Price - Include7.I7.Protection("F").Price - Include7.I7.Protection("G").Price - Include7.I7.Protection("H").Price - Include7.I7.Protection("I").Price - Include7.I7.Protection("J").Price
        L2A = Trade.TotalAllowance
        L2B = Lien.Total
        L2C = L2A - L2B
        L2D = COD.Value
        L2EDESC = String.Empty
        If Rebate.Total.Value <> 0 Then L2EDESC = "/REBATE(S)"
        If D.Deposit <> 0 Then L2EDESC = "/DEPOSIT"
        If L2EDESC.StartsWith("/") Then L2EDESC = L2EDESC.Remove(0, 1)
        L2E = Rebate.Total.Value + D.Deposit
        Dim TotalDownPayment As Decimal = L2C + L2D + L2E
        If TotalDownPayment < 0 Then
            L2 = 0
            L4HTo1 = T(0).Lien.Name
            L4H1 = -TotalDownPayment
        Else
            L2 = TotalDownPayment
            L4H1 = 0
        End If
        L3 = L1 - L2
        L4Ai = Insurance.CreditLife.Premium
        L4Aii = Insurance.AccidentHealth.Premium
        L4A = L4Ai + L4Aii
        L4B = 0D
        L4C = Fee.Reg.Total
        L4D = Protection("J").Price + Insurance.LossOfEmployment.Premium
        L4E = Fee.Other.Total
        L4F = Fee.Lic.Total + Fee.Gas.Total
        L4FDESC = String.Empty
        If Fee.Lic.Total <> 0 Then L4FDESC = L4FDESC & "/Plate " & Fee.Lic.Total.ToString("$#.00")
        If Fee.Gas.Total <> 0 Then L4FDESC = L4FDESC & "/Registration " & Fee.Gas.Total.ToString("$#.00")
        If L4F <> 0 Then L4FDESC = L4FDESC & " = " & L4F.ToString("$#.00")
        If L4FDESC.StartsWith("/") Then L4FDESC = L4FDESC.Remove(0, 1)
        L4G = Fee.Tire.Total
        'L4H1 See above for Negative Equity Calculations L2
        Dim x As New Include7.FormMap(Of FormTuple)(9)
        x.Add(New FormTuple(Fee.Admin.Total, Dlr.Name, "Doc Fee"))
        x.Add(New FormTuple(Fee.Batt.Total, Dlr.Name, "E-Transfer"))
        x.Add(New FormTuple(MultiWarranty("A").Price, MultiWarranty("A").Provider, MultiWarranty("A").Description))
        x.Add(New FormTuple(MultiWarranty("B").Price, MultiWarranty("B").Provider, MultiWarranty("B").Description))
        x.Add(New FormTuple(MultiWarranty("C").Price, MultiWarranty("C").Provider, MultiWarranty("C").Description))
        x.Add(New FormTuple(MultiWarranty("D").Price, MultiWarranty("D").Provider, MultiWarranty("D").Description))
        x.Add(New FormTuple(MultiWarranty("E").Price, MultiWarranty("E").Provider, MultiWarranty("E").Description))
        x.Add(New FormTuple(MultiWarranty("F").Price, MultiWarranty("F").Provider, MultiWarranty("F").Description))
        x.Add(New FormTuple(MultiWarranty("G").Price, MultiWarranty("G").Provider, MultiWarranty("G").Description))
        x.Add(New FormTuple(MultiWarranty("H").Price, MultiWarranty("H").Provider, MultiWarranty("H").Description))
        x.Add(New FormTuple(MultiWarranty("I").Price, MultiWarranty("I").Provider, MultiWarranty("I").Description))
        x.Add(New FormTuple(MultiWarranty("J").Price, MultiWarranty("J").Provider, MultiWarranty("J").Description))
        x.Add(New FormTuple(Protection("B").Price, Protection("B").Provider, Protection("B").Name))
        x.Add(New FormTuple(Protection("C").Price, Protection("C").Provider, Protection("C").Name))
        x.Add(New FormTuple(Protection("D").Price, Protection("D").Provider, Protection("D").Name))
        x.Add(New FormTuple(Protection("E").Price, Protection("E").Provider, Protection("E").Name))
        x.Add(New FormTuple(Protection("F").Price, Protection("F").Provider, Protection("F").Name))
        x.Add(New FormTuple(Protection("G").Price, Protection("G").Provider, Protection("G").Name))
        x.Add(New FormTuple(Protection("H").Price, Protection("H").Provider, Protection("H").Name))
        x.Add(New FormTuple(Protection("I").Price, Protection("I").Provider, Protection("I").Name))

        If Not String.IsNullOrWhiteSpace(x.ExceptionText) Then Throw New ApplicationException("Number of items of extra items cannot exceed 9")

        L4H2 = If(x.ItemExists(0), x.Item(0).Value, 0D)
        If L4H2 <> 0D Then
            L4HTo2 = x.Item(0).Name
            L4HFor2 = x.Item(0).ForPS
        End If
        L4H3 = If(x.ItemExists(1), x.Item(1).Value, 0D)
        If L4H3 <> 0D Then
            L4HTo3 = x.Item(1).Name
            L4HFor3 = x.Item(1).ForPS
        End If
        L4H4 = If(x.ItemExists(2), x.Item(2).Value, 0D)
        If L4H4 <> 0D Then
            L4HTo4 = x.Item(2).Name
            L4HFor4 = x.Item(2).ForPS
        End If
        L4H5 = If(x.ItemExists(3), x.Item(3).Value, 0D)
        If L4H5 <> 0D Then
            L4HTo5 = x.Item(3).Name
            L4HFor5 = x.Item(3).ForPS
        End If
        L4H6 = If(x.ItemExists(4), x.Item(4).Value, 0D)
        If L4H6 <> 0D Then
            L4HTo6 = x.Item(4).Name
            L4HFor6 = x.Item(4).ForPS
        End If
        L4H7 = If(x.ItemExists(5), x.Item(5).Value, 0D)
        If L4H7 <> 0D Then
            L4HTo7 = x.Item(5).Name
            L4HFor7 = x.Item(5).ForPS
        End If
        L4H8 = If(x.ItemExists(6), x.Item(6).Value, 0D)
        If L4H8 <> 0D Then
            L4HTo8 = x.Item(6).Name
            L4HFor8 = x.Item(6).ForPS
        End If
        L4H9 = If(x.ItemExists(7), x.Item(7).Value, 0D)
        If L4H9 <> 0D Then
            L4HTo9 = x.Item(7).Name
            L4HFor9 = x.Item(7).ForPS
        End If
        L4H10 = If(x.ItemExists(8), x.Item(8).Value, 0D)
        If L4H10 <> 0D Then
            L4HTo10 = x.Item(8).Name
            L4HFor10 = x.Item(8).ForPS
        End If
        L4 = L4A + L4B + L4C + L4D + L4E + L4F + L4G
        L4 = L4 + L4H1 + L4H2 + L4H3 + L4H4 + L4H5 + L4H6 + L4H7 + L4H8 + L4H9 + L4H10
        L5 = L3 + L4
        AMTFIN = L5
        DOWNPMT = L2

        APR = IIf(D.APR < 0.01, 0D, D.APR)
        FINCHG = IIf(D.APR < 0.01, 0D, D.FinanceCharge)
        TOTPMT = 0D
        If _DataContext.PaymentSchedule.PaymentDictionary(PaymentSchedule.FIRSTLINE).IsValid Then
            TOTPMT += (D.Payment * _DataContext.PaymentSchedule.PaymentDictionary(PaymentSchedule.FIRSTLINE).PaymentPeriod)
        End If
        If _DataContext.PaymentSchedule.PaymentDictionary(PaymentSchedule.SECONDLINE).IsValid Then
            TOTPMT += (_DataContext.PaymentSchedule.PaymentDictionary(PaymentSchedule.SECONDLINE).Payment * _DataContext.PaymentSchedule.PaymentDictionary(PaymentSchedule.SECONDLINE).PaymentPeriod)
        End If
        If _DataContext.PaymentSchedule.PaymentDictionary(PaymentSchedule.THIRDLINE).IsValid Then
            TOTPMT += (_DataContext.PaymentSchedule.PaymentDictionary(PaymentSchedule.THIRDLINE).Payment * _DataContext.PaymentSchedule.PaymentDictionary(PaymentSchedule.THIRDLINE).PaymentPeriod)
        End If
        TOTALSALEPRICE = TOTPMT + DOWNPMT
    End Sub
End Class
