   M S B A M L   `   `   `  ���� C  ?OASystem, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:OASystem.ViewModel.Common.Converters$OASystem.ViewModel.Common.Converters  M+clr-namespace:OASystem.ViewModel.BehavioursOASystem.ViewModel.Behaviours  '    !OASystem.View.Windows.OrderCenter   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    F	converter2clr-namespace:OASystem.ViewModel.Common.Converters    >d2http://schemas.microsoft.com/expression/blend/2008    Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    ?	behaviors+clr-namespace:OASystem.ViewModel.Behaviours      � Title$  発注書作成�� � WindowState$ 	Maximized=�$
��1024��$	��720�� � WindowStartupLocation$ CenterScreen=����	#FF2F415F � Icon$ /OASystemIcon100x100.ico�� � 	Resources E   ?OASystem.ViewModel.Common.Converters.BooltoBlackCircleConverter   BooltoBlackCircleConverterE   ?OASystem.ViewModel.Common.Converters.BooltoCircleCrossConverter  BooltoCircleCrossConverter@   :OASystem.ViewModel.Common.Converters.BooltoCircleConverter  BooltoCircleConverterF   @OASystem.ViewModel.Common.Converters.ExpDeadInputAmountConverter  ExpDeadInputAmountConverterH   BOASystem.ViewModel.Common.Converters.DateTimetoDispStringConverter ! DateTimetoDispStringConverterF   @OASystem.ViewModel.Common.Converters.ExpForegroundColorConverter  ExpForegroundColorConverterB   <OASystem.ViewModel.Common.Converters.DoubletoStringConverter  DoubletoStringConverter �� 
TargetType �� Property �� Value �� Triggers	 ^� 
Conditions
 �� Binding �� Value ]� 
Conditions �� Property  styListViewItemDeadExp  styListViewItemOrderAllDisp 	 styListViewItemNormal 
 stytblStoreNameOrderTitle  stytblOrderList  stytblExpDead  	styButton  styColumnHeaderOrderList % !styColumnHeaderOrderListDatDelete " styColumnHeader期限デッド  dtOrder医薬品名 - )dtOrder数量ｘ包装総量_包装単位  dtOrder薬価  dtOrderＪＡＮ  dtOrder帳合先名称  dtOrder伝票No ) %dtOrderSEND01DATから削除するか d� 	IsChecked  dtOrder帳合 �� 	Converter  dtOrder他店該当  dtExpDead店名 �� TextAlignment  dtExpDead医薬品名  dtExpDead在庫数  dtExpDead薬価  dtExpDead注文数 �� TextAlignment  dtExpDead期迫   dtExpDeadデッド ! dtExpDead使用期限 �� Mode " dtExpDead優先移動 � Cursor � ToolTip # HeaderTemplateArrowUp F�Name $ HeaderTemplateArrowDown��%  &	        &	      &	 
     &	      &	      &	      &	      '�� #     '�� �     &	 �    &	 �    &		 �    &	
 �    &	 K    &	 �    &	 �    &	 q    &	 �    &	 �    &	 �    &	     &	 t    &	 �    &	 �    &	 >    &	 �    &	 �    7  &	 $    7  &	 o    &	 �    &	 	    &	 a	    7  &	 �	    7  &	 �	    7  &	  *
    7  &	! v
    7  7  &	" �
    7   &	# k    &	$ �                  �� " ��.���  � ��
 ��ح���  � ��
 ��  ���  � ��$
 Bold"���  � ��$ 75���� " ��.���  � ��$ Stretch=� ^� 	 �� 
 �� *Is帳合一致+	 false.0���  � �� �#FFB6C1]�  ��  �@��
IsSelected . ./���  � ��
 �����^� 	 �� 
 �� *Is注文あり+ true.0���  � �� �	#FFD6FFC9�� " ��.���  � ��$ Stretch=� ^� 	 �� 
 �� *Is帳合一致+	 false.0���  � �� �#FFB6C1]�  ��  �@��
IsSelected . ./���  � ��
 �����^� 	 �� 
 �� *Is注文あり+ true.0���  � �� �	#FFD6FFC9�� " ��.���  � ��$ Stretch=� ^� 	 �� 
 �� *Is帳合一致+	 false.0���  � �� �#FFB6C1]�  ��  �@��
IsSelected . ./���  � ��
 �����^� 	 �� 
 �� *Is注文あり+ true.0���  � �� �	#FFD6FFC9�� " ��.���  � ��$ Stretch=��� " ��.���  � ��$ 25(���  � ��
 �p���  � ��
 ������� " ��.���  � ��$ 25(���  � ��
 �������  � ��
 ������� " ��.���  � ��$ 15(���  � ��
 �r�����  � ��
 ������� " ��.���  � ��$	 120����  � ��$ 60����  � ��$ 20(���  � ��
 �������  � ��
 ������� " ��.���  � ��$ 9(��� " ��.���  � ��$ 9(��� " ��.���  � ��$ 9(��� .R�� .+��� $
��Left=�$	��8.5(����� *医薬品名+�� .R�� .+��� $��Right=�$��8(����� *$"数量ｘ包装総量_包装単位+�� .R�� .+��� $��Right=�$	��8.5(����� *薬価+�� .R�� .+��� $��Center=�$	��8.5(����� *JANコード+�� .R�� .+��� $
��Left=�$��8(����� *帳合先名称+�� .R�� .+��� $��Right=�$	��8.5(����� *レセ発注伝票No+�� .R�� .+��� $��Center=� �� * SEND01DATから削除するか+�� .R�� .+��� $��Center=�$��8(����� *Is帳合一致+8   �� .R�� .+��� $��Center=�$��8(����� *Is他店該当+8   �� .R�� .+��� $
 Left=�$	��7.8(����� *店名+�� .R�� .+��� $
��Left=�$��9(����� * 医薬品名と名称２連結+�� .R�� .+��� $��Right=�$ Right=�$
��auto��$��9(����� *在庫数表示用+�� .R�� .+��� $��Right=�$	��8.5(����� *薬価+8   �� .R��� -   $ Right=����� *	注文数+8   �� .R�� .+��� $��Center=�$��8(����� *Is期限切迫+8   �� .R�� .+��� $��Center=�$��10(����� *Isデッド品+8   �� .R�� .+��� $��Center=�$��10(����� *使用期限+8   ���� *Is期限切迫+8  $ OneWay=��� .R�� .+��� $
 Hand��$��Center=�$��10(����� *Is優先移動+8    �� *優先移動コメント+�� .R�]� .+��� $��Center=����� F� $ arrow��$��1��
�������+���Ѐ�[  � р��  � р��  	= р�[  � 	�� .R�]� .+��� $��Center=����� F� $ arrow��$��1��
�������+���Ѐ�[  	= р��  � р��  	= р�[  	= 	.���  � RowDefinitions �� $��10 ��� $��* �.+�� $��0��� $��1��$��20,10,20,10q� �� $��50 ��� $��* ��� $	��100 � � ColumnDefinitions �� $��1.67* ��� $��3* �.+��� -    ��Name$ bdButtonListBox��$��Right=�$��120.8��$��1q�$��0,0,139.6,-11.8q�$��Bottom=�$
��52.7��$��Disabled=�$��1��$��2��$y�Hidden=�._��� -   .@��� $��発注書印刷��$��10.667(��� $��フリーフォーム��$��10.667(�� -    �Name$
 grid��$��0��$��0��.+��� $��5q� �� Padding$ 5q�$��1q� �� CornerRadius$ 5������  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&� � MappingMode$ RelativeToBoundingBox=�.���  � Color$ 	#FF6260EF��� $ 	#FF6260EF��  � Offset$  1Q�� $ 	#FF161288��$  0.521Q�._��� $��発注予定リスト��#��[ $��Center=�$ Center=�
����������� *ActualWidth+! �� ElementName$
! grid��$ Default=���Z� � -   $ grid1��$��0��$��1��.+��� $��5q�$ 5q�$��1q�$ 5������ $ 0.5,1&�$ 0.5,0&�.��� $ 	#FFFF3E3E��$  0Q�� $ 	#FFFF3E3E��$  1Q�� $ 	#FF9F0C0C��$	  0.5Q�._��� $6��0期限切迫品・デッド品・優先移動品��#��[ $ Center=�$��Center=�$��0q�
����������� *ActualWidth+$! grid1��$ Default=���Z� � $��1��$��0��.+��� -   " ��Name$" tcOrder��# �� SelectedIndex$# 0��.@��� -   $ ��Name$$ tiOrder��$��全て表示��.���� -   % ��Name$% lvOrderAllDisp��& �� SelectionMode$& Single=�#��[ ' �� View' �� .P��� ( �� HeaderContainerStyle#( [ ) �� CellTemplate#) [ * �� Width$	* 140��.���� -	   $��医薬品名���� $��発注数量��#( [ #) [ $* 50���� $��帳合一致��#( [ #) [ $
* auto���� $��他店該当��#( [ #) [ $
* auto���� $��削除��#( [ #) [ $
* auto���� $��帳合不一致のみ��.���� -
   $$% lvOrderOnlyCheckBalancingFalse��$& Single=�' �� .P��� #( [ #) [ $	* 140��.���� $��医薬品名���� $��発注数量��#( [ #) [ $* 50���� #( [ #) [ $* 58��.���� $��	帳合先���� #( [ #) [ $* 49��.���� $��伝票No���� $��削除��#( [ #) [ $
* auto���� $��他店該当(帳合先)��.���� -   $,% &lvOrderDeadAndExpWithBalancingAccounts��$& Single=�' �� .P��� #( [ #) [ $	* 140��.���� -   $��医薬品名���� $��発注数量��#( [ #) [ $* 50���� #( [ #) [ $* 58��.���� -   $��	帳合先���� #( [ #) [ $* 49��.���� -   $��伝票No���� $��削除��#( [ #) [ $
* auto��� $��1��$��1��.+��� -   $" 	tcExpDead��$# 0��.@��� $��全て表示��.���� -   $% lvExpDeadAllDisp��$& Single=�#��[ -  'System.Windows.Input.KeyboardNavigation+   TabNavigation$+ Local=�' �� .P��� #) [ $
* auto��.���� -   $��	店舗名���� #) [ $	* 160��.���� -   $��医薬品名���� $��	在庫数��#) [ $* 65���� $��	注文数��#) [ $
* Auto���� $��薬価��#) [ $* 60���� #( [ #) [ $* 40��.���� -   $��期迫���� #( [ #) [  $* 40��.���� -   $��	デッド���� #( [ #) [" $* 40��.���� -   $��優先���� $��使用期限��#) [! $
* Auto���� $��選択表示��.���� -   $% lvExpDeadSelectMode��$& Single=�#��[ $+ Local=�' �� .P��� #) [ $
* auto��.���� -   $��	店舗名���� #) [ $	* 160��.���� -   $��医薬品名���� $��	在庫数��#) [ $* 65���� $��	注文数��#) [ $
* Auto���� $��薬価��#) [ $* 60���� #( [ #) [ $* 40��.���� -   $��期迫���� #( [ #) [  $* 40��.���� -   $��	デッド���� #( [ #) [" $* 40��.���� -   $��優先���� $��使用期限��#) [! $
* Auto��� $��2��$��0��.+��� $��5q�$ 5q�$��1q�$ 10��$��55������ $ 0.5,1&�$ 0.5,0&�.��� $ 	#FF3C3C3C��$  0Q�� $ 	#FF3C3C3C��$  0.996Q�� $ Black��$  0.528Q�._��� -   , ��Name$, tblStoreNameOrderTitle��$�� ��#��[
 $��Center=�$ Center=�$
��auto����Z� � $��2��$��1��.+��� - �� Orientation$- 
Horizontal=�$��Right=�$	��Top=�$��0,20,0,0q�.+��� -   . ��Name$. btnSave��$��保存��#��[ $��40���� -   $. btnPrint��$��印刷��#��[ $��20,0,0,0q�$��40���� -   $. btnComplate��$��完了��#��[ $��20,0,0,0q�$��40��