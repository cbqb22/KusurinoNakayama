   M S B A M L   `   `   `  ����   OASystemB    <OASystem.View.Windows.OrderMedicineManagementPriorityMoveAdd   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml      � Title$  医薬品 検索��$	��520��$	��500�� � WindowStartupLocation$ CenterScreen=����	#FF2F415F � Icon$ /OASystemIcon100x100.ico�� � 	Resources  �� 
TargetType �� Triggers ]� 
Conditions �� Property �� Value	 �� Property
 �� Value !  dtレセプト電算コード  dt医薬品名  styButtonKeyword  	styButton��%�  '��       &	  o     &	 �     &	 �     &	 3    �� " �� ]�  ��  �@��
IsSelected . ./��� 	 � ��

 �����.��� 	 � ��$
 Stretch=��� .R�� .+��� $
��Left=����� *レセプト電算コード+�� .R�� .+��� $
��Left=����� *医薬品名+�� " ��.��� 	 � ��$

 Left=��� 	 � ��
 �#9CAAC1�� " ��.��� 	 � ��$
 80���� 	 � ��$
 40���� 	 � ��$

 12.5(��� 	 � ��

 ������� 	 � ��

 �����.��� $��20,20,20,20q� � RowDefinitions �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto �.+�� $��0��.+���  �� Orientation$ 
Horizontal=�$
��Left=�.+��� -    ��Name$ tbキーワード��$	��200��$��Center=� �� TextAlignment$
 Left=��� -    ��Name$ btnキーワード検索��$��キーワード検索��$
��auto��$��25��#��[ � $��1��.+��� -    ��Name$ lvMedicineInfo�� �� SelectionMode$ Single=�$	��280�� �� View �� .P��� $!��レセプト電算コード�� �� CellTemplate# [   �� Width$	 120���� $��医薬品名��# [ $	 280��� $��2��.+��� $ Vertical=�$��0,10,0,0q�.+��� -    ��Name$ tblコメント��$#��コメント(100文字まで)��
�������$
��Bold"��� -   $ tbコメント��$��50��$
��auto�� �� TextWrapping$
 Wrap=�.��� $��3��$��0,20,0,0q�.+��� $ 
Horizontal=�$��Right=�.+��� -   $ 	btn追加��$��追加��$	��100��$��30��#��[ �� -   $ 	btn中止��$��中止��$	��100��$��30��#��[ 