   M S B A M L   `   `   `  ����   OASystemR    LOASystem.View.Windows.BalancingAccountsCheckMedicineSortSelectMedicineDetail   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml      � Title$%  ＪＡＮ管理 医薬品検索��$	��460��$	��500�� � WindowStartupLocation$ CenterScreen=����	#FF2F415F � Icon$ /OASystemIcon100x100.ico�� � 	Resources  �� 
TargetType   styTextBlock �� Property �� Value �� Triggers ]� 
Conditions	 �� Property
 �� Value  	styButton $  dtMedicineSortDetailJANコード �� Path $  dtMedicineSortDetail医薬品名  dtMedicineSortDetail包装 �� TextAlignment ' #dtMedicineSortDetailメーカー名��%�  &	        '�� "     &	 �     &	     7   &	 `    7   &	 �    7   &	     7   �� " ��.���  � ��$ 9(��� " �� ]�  �� 	 �@��
IsSelected
 . ./���  � ��
 �����.���  � ��$ Stretch=��� " ��.���  � ��$ 80����  � ��$ 40����  � ��$
 12.5(���  � ��
 �������  � ��
 ������� .R�� .+��� $��tblJAN��8��  $
��Left=����� $ JANコード��� .R�� .+��� $��tbl医薬品名��8��  $
��Left=����� $ 医薬品名��� .R�� .+��� $��	tbl包装��8��  $
��Left=�$
 Left=����� $ 包装��� .R�� .+��� $��tblメーカー名��8��  $
��Left=����� $ 販売会社�.��� $��20,20,20,20q� � RowDefinitions �� $
��auto ��� $
��auto ��� $
��auto �.+�� $��0��� $��1��.+��� -    ��Name$ lvMedicineInfo�� �� SelectionMode$ Single=�$	��330�� �� View �� .P��� $��JANコード�� �� CellTemplate# [  �� Width$ 90���� $��医薬品名��# [ $	 150���� $��包装��# [ $	 100���� $��メーカー名��# [ $ 90��� $��2��$��0,20,0,0q�.+���  �� Orientation$ 
Horizontal=�$��Right=�.+��� -    ��Name$ 	btn中止��$��中止��$��75��$��30��#��[ �� -   $ 	btn追加��$��追加��$��75��$��30��#��[ 