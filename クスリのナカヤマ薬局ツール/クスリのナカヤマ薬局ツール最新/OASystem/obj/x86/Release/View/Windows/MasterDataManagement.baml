   M S B A M L   `   `   `  ����   OASystem0    *OASystem.View.Windows.MasterDataManagement   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml      � Title$  マスタ管理��$	��700��$	��768�� � WindowStartupLocation$ CenterScreen=����	#FF2F415F � Icon$ /OASystemIcon100x100.ico�� � 	Resources  �� 
TargetType �� Triggers ]� 
Conditions �� Property �� Value	 �� Property
 �� Value $   stytblMasterDataManagementHeader  stytblOtherMakerHeader  styOtherMakerCombobox  
stytblIBMM  	styButton  styListViewHeader 2 .dtBalancingAccountsMaster卸ＶＡＮコード �� Path , (dtBalancingAccountsMaster帳合先名称 9 5dtIndividualBasedManagementMedicineＪＡＮコード 3	 /dtIndividualBasedManagementMedicine医薬品名 3
 /dtIndividualBasedManagementMedicine包装形態 �� TextAlignment 3 /dtIndividualBasedManagementMedicine包装単位 6 2dtIndividualBasedManagementMedicine包装単位数 3 /dtIndividualBasedManagementMedicine包装総量 3 /dtIndividualBasedManagementMedicine製薬会社 3 /dtIndividualBasedManagementMedicine販売会社��%!  '��       &	  o     &	 �     &	 �    &	 �    &	     &	 �    &	 �    &	 �    &	 9    7  &		 |    7  &	
 �    7  &	     7  &	 P    7  &	 �    7  &	 �    7  &	     7  �� " �� ]�  ��  �@��
IsSelected . ./��� 	 � ��

 �����.��� 	 � ��$
 Stretch=��� " ��.��� 	 � ��$
 25(��� 	 � ��

 ������� 	 � ��

 �   ��� " ��.��� 	 � ��$
 22(��� 	 � ��

 ������� 	 � ��

 ������� 	 �@��TextAlignment$
 Center=��� 	 � ��$
 Center=��� 	 � ��$
 35���� 	 � ��$	
 200���� " ��.��� 	 � ��$
 20(��� 	 � ��$
 Center=��� 	 � ��$
 40���� 	 � ��$	
 200���� " ��.��� 	 � ��$
 9(��� " ��.��� 	 � ��$
 80���� 	 � ��$
 40���� 	 � ��$

 12.5(��� 	 � ��

 ������� 	 � ��

 ������� " ��.��� 	 � ��$	
 8.5(��� .R�� .+��� $
��Left=����� $ 卸ＶＡＮコード��� .R�� .+��� $
��Left=����� $ 帳合先名称��� .R�� .+��� $��Center=�8��  ���� $ JANコード��� .R�� .+��� $
��Left=�8��  ���� $ 医薬品名��� .R�� .+��� $��Center=�$ Center=�8��  ���� $ 包装形態��� .R�� .+��� $��Center=�8��  ���� $ 包装単位��� .R�� .+��� $
��Left=�8��  ���� $ 包装単位数��� .R�� .+��� $
��Left=�8��  ���� $ 包装総量��� .R�� .+��� $
��Left=�8��  ���� $ 製薬会社��� .R�� .+��� $
��Left=�8��  ���� $ 販売会社�.��� .+��  � RowDefinitions �� $
��auto ��� $��* �.+�� $��0��$��20,10,20,10q�� $��1��$��
20,0,20,10q�.+�� .+��� -    ��Name$ tcBalancingAccountsMaster�����	#FF465A7D.@��� $��帳合先マスタ��$	��150��.���  �� $��* ��� $	��100 �.+�� $��0��.+��� -    ��Name$ lvBalancingAccountsMaster�� �� SelectionMode$ Single=� �� View �� .P��� $��卸ＶＡＮコード�� �� CellTemplate# [  �� Width$	 200���� $��帳合先名称��# [ $	 200��� $��1��.+�� .+���  �� Orientation$ 
Horizontal=�$��Right=�$��0,0,10,0q�.+��� -    ��Name$&  btnBalancingAccountsMasterDelete��$��選択を削除��#��[ $��10,0,0,0q��� -   $# btnBalancingAccountsMasterAdd��$��追加��#��[ $��10,0,0,0q��� -   $&  btnBalancingAccountsMasterUpload��$��アップロード��#��[ $��10,0,0,0q��� -   $ btnClose��$��	閉じる��#��[ $��10,0,0,0q��� $$��個別管理医薬品マスタ��$	��150��.���  �� $��* ��� $	��100 �.+�� $��0��.+��� -   $) #lvIndividualBasedManagementMedicine��$ Single=� �� .P��� $��ＪＡＮコード�� �� HeaderContainerStyle# [ # [ $ 75���� $��医薬品名��# [	 $	 150���� $��包装形態��# [ # [
 $ 50���� $��包装単位��# [ # [ $ 50���� $��包装単位数��# [ # [ $ 70���� $��包装総量��# [ # [ $ 70���� $��製薬会社��# [ $	 100���� $��販売会社��# [ $	 100��� $��1��.+�� .+��� $ 
Horizontal=�$��Right=�$��0,0,10,0q�.+��� -   $0 *btnIndividualBasedManagementMedicineDelete��$��選択を削除��#��[ $��10,0,0,0q��� -	   $- 'btnIndividualBasedManagementMedicineAdd��$��追加��#��[ $��10,0,0,0q��� -
   $0 *btnIndividualBasedManagementMedicineUpload��$��アップロード��#��[ $��10,0,0,0q��� -   $ 	btnClose2��$��	閉じる��#��[ $��10,0,0,0q�� $��1��