   M S B A M L   `   `   `  ���� C  ?OASystem, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:OASystem.ViewModel.Common.Converters$OASystem.ViewModel.Common.Converters  :    4OASystem.View.Windows.BalancingAccountsCheckRegister   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    F	converter2clr-namespace:OASystem.ViewModel.Common.Converters    >d2http://schemas.microsoft.com/expression/blend/2008    Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006      � Title$*  $帳合先チェックマスタ登録��$	��700��$	��768�� � WindowStartupLocation$ CenterScreen=����	#FF2F415F � Icon$ /OASystemIcon100x100.ico�� � 	Resources M   GOASystem.ViewModel.Common.Converters.VanCodeToBalancingAccountConverter &  "VanCodeToBalancingAccountConverter �� 
TargetType . *stytblBalancingAccountsCheckRegisterHeader �� Property �� Value  stytblOtherMakerHeader  styOtherMakerCombobox  	styButton �� Triggers ]� 
Conditions	 �� Property
 �� Value  dtMakerSortメーカー名  dtMakerSort帳合先 �� RelativeSource �� AncestorType �� SelectedValue  dtMedicineSortJANコード ��Name  dtMedicineSort医薬品名 	 dtMedicineSort包装 !
 dtMedicineSortメーカー名  dtMedicineSort帳合先 �� 	Converter $  dtMedicineSort修正後帳合先 ��Name �� Mode��%�  &	        &	      &	 X     &	     &	 �    '��     &	 u    &	 �    &	 
    &	 ]    &		 �    &	
     &	 `    7   &	 �    7     �� " ��.���  � ��$ 25(���  � ��
 ������  � ��
 �   ��� " ��.���  � ��$ 19(���  � ��
 �������  � ��
 �������  �@��TextAlignment$ Center=���  � ��$ Center=���  � ��$ 30����  � ��$
 auto���� " ��.���  � ��$ 15(���  � ��$ Center=���  � ��$ 30����  � ��$	 150���� " ��.���  � ��$ 60����  � ��$ 40����  � ��$
 12.5(���  � ��
 �������  � ��
 ������� " ��.���  � ��$ Stretch=� ]�  �� 	 �@��
IsSelected
 . ./���  � ��
 ������� .R�� .+��� $
��Left=����� *Key+�� .R�� .+��� -   ���� *DataContext+ �� " �� �� *Value.帳合先名称+�� .R�� .+��� $ tblJAN��$��Center=�$	��8.5(����� *JANコード+�� .R�� .+��� $ tbl医薬品名��$
��Left=�$��9(����� *医薬品名+�� .R�� .+��� $ 	tbl包装��$
��Left=�$	��8.5(����� *包装+�� .R�� .+��� $ tblメーカー名��$��Center=�$��9(����� *販売会社+�� .R�� .+��� $ tbl帳合先��$��Center=����� *	帳合先+8   �� .R�� .+��� $ cmb修正後帳合先������ *DataContext+ �� " �� �� *修正後帳合先+8   $ TwoWay=�.��� .+�� $��20,10,20,10q� � RowDefinitions �� $
��auto ��� $��* �.+�� $��0��$��0,0,0,20q�� $��1��.+�� .+��� -    ��Name$ tcBalancingAccountCheck�����	#FF465A7D.@��� $��メーカー別��$	��100��.���  �� $��* ��� $	��100 �.+�� $��0��.+��� -    ��Name$" lvBACRegisterメーカー別�� �� SelectionMode$ Single=� �� View �� .P��� $��メーカー名�� �� CellTemplate# [  �� Width$	 200���� $��	帳合先��# [ $	 200��� $��1�� � ColumnDefinitions �� $��* ��� $��* �.+�� $��0��$	��Top=�$��0,25,0,0q�.+���  �� Orientation$ 
Horizontal=�$
��Left=�$��15,0,0,0q�.+��� $��2q� �� Padding$ 2q�$��1q����	#FFB3A5A5 �� CornerRadius$ 5��$	��158��$��30��._��� -   $ 	textBlock��$��その他のメーカー��#��[ ���	#FFB3A5A5$��0q�$��16(�$
��Auto���� -   $* $cmbその他のメーカーリスト��#��[ $��10,0,0,0q�
���   ����	#FF5C7FC9���� � $��1��$	��Top=�$��0,20,0,0q�.+��� -    ��Name$$ bdMakerSortUpdateButtonListBox��$��Right=�$��0,0,29.3,-45q�$	��100��$��45��$��Bottom=�$��Disabled=�$y�Hidden=�._��� -    ��Name$ lbMakerSortUpdate��$��Hidden=�$��Hidden=�.@��� $��表示リストからUP��$��10.667(��� $��ファイルからUP��$��10.667(��� $ 
Horizontal=�$��Right=�.+��� -    ��Name$ btnMakerSortCSV��$��	ＣＳＶ��#��[ �� -	   $ btnMakerSortDelete��$��選択を削除��#��[ $��10,0,0,0q�$��9(��� -
   $ btnMakerSortAdd��$��追加��#��[ $��10,0,0,0q��� -   $ btnMakerSortUpload��$��アップロード��#��[ $��10,0,0,0q�$	��8.5(��� -   $ btnClose��$��	閉じる��#��[ $��10,0,0,0q��� $��医薬品別��$	��100��.���  �� $��* ��� $	��100 �.+�� $��0��.+��� -   $ lvBACRegister医薬品別��$ Single=� �� .P��� $��JANコード��# [ $ 80���� $��医薬品名��# [ $	 200���� $��包装��# [	 $	 100���� $��メーカー名��# [
 $ 80���� $��	帳合先��# [ $ 80���� $��修正後帳合先��# [ $	 120��� $��1��.+�� $	��Top=�$��0,20,0,0q�.+��� -   $' !bdMedicineSortUpdateButtonListBox��$��Right=�$��
0,0,28,-44q�$	��100��$��45��$��Bottom=�$��Disabled=�$y�Hidden=�._��� -   $ lbMedicineSortUpdate��$��Hidden=�$��Hidden=�.@��� $��表示リストからUP��$��10.667(��� $��ファイルからUP��$��10.667(��� $ 
Horizontal=�$��Right=�.+��� -   $ btnMedicineSortCSV��$��	ＣＳＶ��#��[ �� -   $ btnMedicineSortDelete��$��選択を削除��#��[ $��10,0,0,0q�$��9(��� -   $ btnMedicineSortAdd��$��追加��#��[ $��10,0,0,0q��� -   $ btnMedicineSortUpload��$��アップロード��#��[ $��10,0,0,0q�$	��8.5(��� -   $ 	btnClose2��$��	閉じる��#��[ $��10,0,0,0q�� $��1��