   M S B A M L   `   `   `  ���� C  ?OASystem, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:OASystem.ViewModel.Common.Converters$OASystem.ViewModel.Common.Converters  8    2OASystem.View.Windows.BalancingAccountsCheckResult   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    F	converter2clr-namespace:OASystem.ViewModel.Common.Converters      � Title$!  帳合先チェック結果��$	��720��$
��1024�� � WindowStartupLocation$ CenterScreen=� � Icon$ /OASystemIcon100x100.ico�����	#FF2F415F � 	Resources M   GOASystem.ViewModel.Common.Converters.VanCodeToBalancingAccountConverter &  "VanCodeToBalancingAccountConverter �� 
TargetType . *stytblBalancingAccountsCheckRegisterHeader �� Property �� Value  	styButton  styListViewHeader  dtCheckResultJANコード �� Path  dtCheckResult医薬品名  dtCheckResult包装 �� TextAlignment   dtCheckResultメーカー名   dtCheckResult注文帳合先	 �� 	Converter  	 dtCheckResult設定帳合先  
 dtCheckResultエラー内容��%>  &	        &	      &	 @     &	 �     &	     &	 h    &	 �    &	     &	 h    7   &		 �    7   &	
 T    7     �� " ��.���  � ��$ 13(���  � ��
 �   ��� " ��.���  � ��$	 100����  � ��$ 50����  � ��$ 15(���  � ��
 �������  � ��
 ������� " ��.���  � ��
 �������  � ��
 �������  � ��$
 Bold"��� .R�� .+��� $��tblJAN��$��Center=�$��9(����� $ JANコード��� .R�� .+��� $��tbl医薬品名��$
��Left=����� $ 医薬品名��� .R�� .+��� $��	tbl包装��$
��Left=�$
 Left=�$��9(����� $ 包装��� .R�� .+��� $��tblメーカー名��$
��Left=����� $ 販売会社��� .R�� .+��� $��tbl注文帳合先��$��Center=�
����  ����� $! 注文帳合先VANコード�8	   �� .R�� .+��� $��tbl設定帳合先��$��Center=�
���  ������ $! 設定帳合先VANコード�8	   �� .R�� .+��� $��tblエラー内容��$
��Left=�$��9(�$
��Wrap=����� $ エラー内容�8	   .��� .+�� 
 � RowDefinitions
 �� $
��auto ��� $��* �.+�� -    �Name$
 grid��$��0��$��
20,10,20,0q�.+��� -    ��Name$! tbl帳合チェック結果��$!��帳合先チェック結果��#��[ $��Center=�$
��Left=�
�������� $��1��$��20,10,20,10q�.+�� 
 �� $��* ��� $	��100 �.+�� $��0��.+��� -    ��Name$ lvBACheckResult�� �� SelectionMode$ Single=� �� AlternationCount$ 2������ " ��.���  � ��$ Stretch=� �� Triggers P�  P� Property �@��AlternationIndex P� Value$ 0��. ���  � ��
 �����P�  �@��AlternationIndex$ 1��. ���  � ��
 ����� �� View �� .P��� $��JANコード�� �� HeaderContainerStyle# [  �� CellTemplate# [  �� Width$ 85���� $��医薬品名��# [ # [ $	 250���� $��包装��# [ # [ $	 100���� $��メーカー名��# [ # [ $
 Auto���� $��今回注文先��# [ # [ $
 Auto���� $��正しい帳合先��# [ # [	 $
 auto���� $��エラー内容��# [ # [
 $
 auto��� $��1��.+�� .+���  �� Orientation$ 
Horizontal=�$��Right=�.+��� -    ��Name$ btnCSV��$��	ＣＳＶ��#��[ �� -   $ btnClose��$��	閉じる��#��[ $��20,0,0,0q�� $��1��