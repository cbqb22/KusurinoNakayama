   M S B A M L   `   `   `  ���� C  ?OASystem, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:OASystem.ViewModel.Common.Converters$OASystem.ViewModel.Common.Converters  %    OASystem.View.Windows.OrderLogs   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    F	converter2clr-namespace:OASystem.ViewModel.Common.Converters      � Title$  発注履歴画面��$	��720��$
��1024�� � WindowStartupLocation$ CenterScreen=����	#FF2F415F � Icon$ /OASystemIcon100x100.ico�� � 	Resources @   :OASystem.ViewModel.Common.Converters.BooltoCircleConverter   BooltoCircleConverterG   AOASystem.ViewModel.Common.Converters.ShortDateTimeStringConverter   ShortDateTimeStringConverterF   @OASystem.ViewModel.Common.Converters.ExpDeadInputAmountConverter  ExpDeadInputAmountConverter �� 
TargetType �� Property �� Value �� Triggers ]� 
Conditions	 �� Property
 �� Value  stytblStoreNameOrderTitle  stytblOrderList  stytblExpDead  	styButton  dtOrderLogs発注日付 �� Path �� 	Converter # dtOrderedList発注先店舗名 	 dtOrderedList医薬品名 
 dtOrderedList注文数 �� Orientation �� TextAlignment �� TextAlignment  dtOrderedList薬価  dtOrderedList期迫  dtOrderedListデッド品  dtOrderedList優先移動  HeaderTemplateArrowUp F�Name  HeaderTemplateArrowDown��%�  &	        &	      &	 
     '��      '�� {     &	 �     &	 =    &	 �    &	 �    &	 d    7  &	 �    &		 �    &	
     7  &	 �    &	 "    7   &	 g    7   &	 �    7   &	 �    &	 l          �� " ��.���  � ��
 ��ح���  � ��
 ��  ���  � ��$
 Bold"���  � ��$	 100���� " �� ]�  �� 	 �@��
IsSelected
 . ./���  � ��
 �����.���  � ��$ Stretch=��� " ��.���  � ��$ 25(���  � ��
 �   ���  � ��
 ������� " ��.���  � ��$ 25(���  � ��
 ��ح���  � ��
 �   ��� " ��.���  � ��$ 25(���  � ��
 �  ����  � ��
 �   ��� " ��.���  � ��$ 80����  � ��$ 40����  � ��$
 12.5(���  � ��
 �������  � ��
 ������� .R�� .+��� $
��Left=����� $	 Key�8   �� .R�� .+��� $
��Left=�$��9(����� $ 店名��� .R�� .+��� $
��Left=�$��9(����� $ 医薬品名��� .R�� .+��� $ 
Horizontal=�$
��Left=�.+��� -   $��Center=�$ Right=����� $ 	注文数�8   �� $
 Left=�$��5,0,0,0q�$��Center=����� $ 包装単位表示用��� .R�� .+��� $��Right=�$��9(����� $ 薬価��� .R�� .+��� $��Center=����� $ Is期限切迫�8   �� .R�� .+��� $��Center=����� $ Isデッド品�8   �� .R�� .+��� $��Center=����� $ Is優先移動�8   �� .R�]� .+��� $��Center=����� F� $ arrow��$��1��
�������+���Ѐ�[  � р��  � р��  	= р�[  � 	�� .R�]� .+��� $��Center=����� F� $ arrow��$��1��
�������+���Ѐ�[  	= р��  � р��  	= р�[  	= 	.���  � RowDefinitions �� $��10 ��� $��* �.+�� $��0��� $��1��$��20,10,20,10q� �� $
��auto ��� $��* ��� $	��100 � � ColumnDefinitions �� $
��1.7* ��� $��5* �.+�� $��0��$��0��$��2��� $��1��$��0��.+��� -    ��Name$ lvOrderLogs�� �� SelectionMode$ Single=� �� View �� .P��� $��発注日付�� �� CellTemplate# [  �� Width$	 200��� $��1��$��1��.+��� -   $ lvOrderLogList��$ Single=� �� .P��� # [ $	 150��.���� -   $��発注先店舗名���� # [	 $	 200��.���� -   $��医薬品名���� $��	注文数��# [
 $
 auto���� $��薬価��# [ $ 65���� # [ $ 55��.���� -   $��期迫���� # [ $ 55��.���� -   $��	デッド���� # [ $ 55��.���� -   $��優先��� $��2��$��0��.+��� $��5q� �� Padding$ 5q�$��1q� �� CornerRadius$ 10��$��55������  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ Black�� � Offset$ 0Q�� $ 	#FF3C3C3C��$ 1Q�� $ Black��$ 0.497Q�._��� -	    ��Name$ tblStoreNameOrderTitle��$�� ��#��[ $��Center=�$ Center=�$
��auto����Z� � $��2��$��1��.+��� $ 
Horizontal=�$��Right=�.+��� -
    ��Name$ 	btnDelete��$��削除��#��[ �� -   $ btnSave��$��上書き保存��#��[ $��10,0,0,0q��� -   $ btnPrint��$��	再印刷��#��[ $��10,0,0,0q��� -   $ btnClose��$��	閉じる��#��[ $��10,0,0,0q�