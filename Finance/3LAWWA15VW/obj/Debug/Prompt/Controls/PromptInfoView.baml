   M S B A M L   `   `   `  ����E  A3LAWWA15VW, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:_3LAWWA15VW_3LAWWA15VW  5           _3LAWWA15VW.PromptInfoView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      'lclr-namespace:_3LAWWA15VW     5        � 	Resources  5	       �� Source�� 5
   
   $ FormStyle.xaml?�6   %    .��5      �  � IsSharedSizeScope .  � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,    � ColumnDefinitions 5   
   �� 5      $
��Auto �6    z� SharedSizeGroup$ First��6,   6F   �� 5      $��* �6   6+   .+�5   
   �� $��0��6   $��0��6   .��6.   Primary Usage6=   �� 5   
   -   5         ��Name$ PrimaryUseCombo��5   1   $��0��6   $��1��6     �� SelectedValue 6J   ��  �� Path$ VehicleInfo.Use�   VehicleUsageToString	 �� 	Converter#	 [  6�   �� 5    
   $��1��6   $��0��6   .��6.   Other Usage (if applicable)6K   �� 5!   
   $��1��6   $��1��6   ��60   �� $% VehicleInfo.OtherUseDescription��� 5"   
   $��2��6   $��0��6   .��6.   Waive Finance Charge6D   � 5#   
   $��2��6   $��1��6    5$      �� 5%      $
��Auto �6#   $ FirstColumn��60   6P   �� 5&      $��* �6#   6/   .+�5(      �� -   5        $ FINCHGWaiverComboYN��5(      $��0��64   $��0��6A   $��80��6Q    6]   �� $ DealInfo.WaiveFINCHG�  BooleanToYesNoType#	 [ � 5)      $��0��6   $��1��6     5*      �� 5+      $��60 �6'   64   �� 5,      6)   .+�5.      �� $��0��6   $��0��6%   .��65   Date6;   �� 5/      $��0��6   $��1��6'   ��67   �� $ DealInfo.FINCHGWaiveDate�6h   50      51      �� 52   
   $��4��6   $��0��6    $��2��60   $��Late Charge��6D   .��53      �  54      �� 55      6&   �� 56      6&   �� 57      6&    59      �� 5:      $ First��6'   6A   �� 5;      6)   .+�5=      �� $��0��6   $��0��6%   .��65   # of Days Grace6F   �� 5>      $��0��6   $��1��6'   ��67   �� $! VehicleInfo.NumberDaysGrace�6k   �� 5?      $��1��6   $��0��6%   .��65   Amount6=   �� 5@      $��1��6   $��1��6'   ��67   �� $  VehicleInfo.LateChargeFlat�6j   �� 5A      $��2��6   $��0��6%   .��65   
Daily Rate6A   �� 5B      $��2��6   $��1��6'   ��67   �� $  VehicleInfo.LateChargeRate�6j   5C      5D      �� 5E   
   $��6��6   $��0��6   .��6.   Seller Assignment6A   �� 5F   
   -   5        $ SellerAssignmentCombo��5F   1   $��6��6   $��1��6     6P   �� $+ %ContractSpecificBase.SellerAssignment�  FinancialAssignmentToString#	 [ 6�   �� 5G   
   $��7��6   $��0��6   y�6-   �� *DealInfo.GapVisibility+.��6[   
Gap Term6e   �� 5H   
   $��7��6   $��1��6   ��6/   �� *DealInfo.GapTerm+y�6Q   �� *DealInfo.GapVisibility+6�   5I      5J      