   M S B A M L   `   `   `  ���� V  Rデッド品管理ツール, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:StockManagement.ViewStockManagement.View  a ]PresentationFramework.Aero, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35gJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.AeroMicrosoft.Windows.Themes S.clr-namespace:StockManagement.Common.Converter StockManagement.Common.Converter  )    #StockManagement.View.DeadStockFrame   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml    Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    >d2http://schemas.microsoft.com/expression/blend/2008    0nud"clr-namespace:StockManagement.View    mMicrosoft_Windows_ThemesJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero    B	converter.clr-namespace:StockManagement.Common.Converter    $	��800��$	��600��  � 	Resources  A   ;StockManagement.Common.Converter.CreateSheetButtonConverter   CreateSheetButtonConverter:   4StockManagement.Common.Converter.StopButtonConverter  StopButtonConverter  ButtonFocusVisual �� Property �� Value J� SnapsToDevicePixels �� StrokeDashArray  ButtonNormalBackground �� EndPoint �� 
StartPoint � Color � Offset  ButtonNormalBorder	 �� Color  ButtonStyle1
 �� 
TargetType �� 
TargetType �Name'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup  Name   States   System.Windows.VisualState  Name   
Storyboard �� TargetProperty �� 
TargetName �� KeyTime �� Value ��Name �� RadiusX �� RadiusY ��Name �� Triggers P� Property P� Value �� 
TargetName��%5  &	        &	      &	 
     &	 \     &	 �     &	 �     7  7  7      �� .���  � �� �� .R��� $��2q� . 
���   �$��1��$	 1 2R��� $	 0,1&�$	 0,0&�.��� $ #F3F3F3��$ 0Q�� $ #EBEBEB��$	 0.5Q�� $ #DDDDDD��$	 0.5Q�� $ #CDCDCD��$ 1Q��� $	 	#FF707070���� "
 ��.���  �@�FocusVisualStyle8   ��  � ��8  ��  � ��8  ��  � ��$ 1q���  � ��# � ����  � ��$ Center=���  � ��$ Center=���  � ��$ 1q���  � �� �� " ��.R�� $
 grid��   $ MouseStates��.   $ 
MouseEnter��. �� .���� $6 0(TextElement.Foreground).(SolidColorBrush.Color)�$ 	textBlock��.g��� $ 0��$ 	#FF104B0E��  $ 
MouseLeave��  $ MouseLeftButtonDown��. �� .���� $* $(Shape.Fill).(SolidColorBrush.Color)�$ 	rectangle��.g��� $ 0��$ 	#591A9D07��.+��� $ 	rectangle�����	#FF1A9D07
�������$��1��$ 10��$ 11���� $ 	textBlock��$��0q�$
��Wrap=�$��ファイル作成開始��
�������$��Center=�$��Center=� P�  �@J�IsKeyboardFocused . P�  �@d�	IsChecked$
 trueY�. ��� $ 	textBlock�� Z�  � ���� $ 	textBlock�� � �� �	#FF024D0AP�  � {� .  . ���  � �� �#ADADAD�� $
 grid�� Z�  � ��.��� ���	#FFF5CBF5 � RowDefinitions �� $
��Auto ��� $��30 ��� $��75 ��� $��75 ��� $��75 ��� $��75 ��� $��75 ��� $��* ��� $��20 �.+�� $��0��.+���  �� TextAlignment$ Center=�$'��!デッド品管理ツール画面��$��30(�$	��Top=����	#FF7A4EF7$��0q����	#FFA2C5FF�� -     ��Name$  btnSettings��$��設定��$��50��$��25��$��Center=�$��Right=�$��0,0,15,0q�! � Cursor$
! Hand��� $��2��" � ColumnDefinitions" �� $	��250 ��� $��* �.+�� $��0��.+��� $��操作種別��$��20(�$
��Bold"����	#FFF32A2A$��Center=�$��Center=�� $��1��.+��� -   # ��Name$# cmbOperationType��$	��400��$
��Left=�$��15,0,0,0q�$��20(�$ �� SelectedIndex$$ 0��$��Center=�$��Center=�$��75��$
! Hand��.@��� $!��貰い受け依頼書作成���� $'��!貰い受け可能リスト作成���� $3��-全店使用無しデッド品リスト作成���� $$��期限切迫品依頼書作成��� $��3��" �� $	��250 ��� $��* �.+�� $��0��.+��� $��作成元店舗��$��20(�$
��Bold"����	#FFF32A2A$��Center=�$��Center=�� $��1��.+��� -   $# cmbOrderShopName��$	��400��$
��Left=�$��15,0,0,0q�$��Center=�$��75��$��20(�$
! Hand��� $��4��" �� $	��250 ��� $��* �.+�� $��0��.+��� $��使用量期間��$��Center=�$��Center=�$��20(�$
��Bold"�$��	10,0,10,0q����	#FF3133CA� $��1��$��15,0,0,0q�" �� $	��180 ��� $��30 ��� $	��180 �.+�� $��0��.+�(   "StockManagement.View.NumericUpDown  -   %  Name$% nudFrom��$��Center=�$��Center=�� $��1��.+��� $	��～��$��Center=�$��Center=�$��20(�� $��2��.+�  -   $% nudTo��$��Center=�$��Center=�� $��5��" �� $	��250 ��� $��* �.+�� $��0��.+��� $��出力形式��$��Center=�$��Center=�$��20(�$
��Bold"�$��	10,0,10,0q����	#FFE06131� $��1��.+��� -   $# cmbOutputType��$	��400��$
��Left=�$��15,0,0,0q�$��20(�$$ 1��$��Center=�$��Center=�$��75��$
! Hand��.@��� $��Excel 2003 (.xls)���� $��Excel 2007 (.xlsx)��� $��6��" �� $	��250 ��� $��* �.+�� $��0��.+��� $��自動印刷��$��Center=�$��Center=�$��20(�$
��Bold"�$��	10,0,10,0q����	#FF3CCA58� $��1��.+��� -   $# cmbAutoPrint��$	��400��$��75��$
��Left=�$��Center=�$��15,0,0,0q�$��20(�$$ 0��$��Center=�$
! Hand��.@��� $��	しない���� $��する��� $��7��$��Center=�$��Center=�" �� $	��200 �.+�� $��0��.+��� -   $  btnStop��
���  ��$��80��$��中断��
�������$
��Bold"�$��15(�$
! Hand��y��� & �� Path$& IsBusy�' �� 	Converter#' [ ( �� Mode$( OneWay=�� $��0��.+��� -	   $  btnCreateOrderSheet��
��� � �$��80��$��ファイル作成開始��
�������$
��Bold"�$��15(�#���  $
! Hand��y��� $& IsBusy�#' [  $( OneWay=�� $��8��.+��� -
   $ 
tblVersion��$�� ��
�������$��12(�$
��Bold"�$��Right=�$��0,0,10,0q�