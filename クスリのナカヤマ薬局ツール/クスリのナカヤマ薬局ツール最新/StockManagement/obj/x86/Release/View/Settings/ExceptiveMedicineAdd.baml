   M S B A M L   `   `   `  ����   デッド品管理ツール8    2StockManagement.View.Settings.ExceptiveMedicineAdd   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml      � Title$  除外医薬品 追加��$	��200��$	��500�� � WindowStartupLocation$ CenterScreen=����	#FF2F415F � Icon$ /nakayama.ico�� � 	Resources  �� 
TargetType �� Property �� Value   	styButton��%�   '��       &	  %     �� " ��.���  � ��
 ������� " ��.���  � ��$ 80����  � ��$ 40����  � ��$
 12.5(���  � ��
 �������  � ��
 �����.��� .+�� $��0,20,0,0q� � RowDefinitions �� $
��auto ��� $
��auto ��� $��* � � ColumnDefinitions �� $	��150 ��� $��* �.+�� $��0��$��0��$��20,0,0,0q�.+��� $��医薬品名称:��$��Right=�$��Center=�� $��0��$��1��$��20,0,0,0q�.+��� -   	 ��Name$	 tb医薬品名称��$	��150��$
��Left=�$��Center=�
 �� TextAlignment$

 Left=�� $��1��$��0��$��20,5,0,0q�.+��� $"��レセプト電算コード:��$��Right=�$��Center=�� $��1��$��1��$��20,5,0,0q�.+���  �� Orientation$ 
Horizontal=�$
��Left=�.+��� -   $#	 tbレセプト電算コード��$	��150��$
��Left=�$��Center=�$

 Left=��� $��*半角 9文字で入力��
�������$��9(�$��Center=�� $��2��$��2��$��20q�.+��� $ 
Horizontal=�$��Right=�.+��� -    ��Name$ 	btn追加��$��追加��$��75��$��30��#��[  �� -   $ 	btn中止��$��中止��$��75��$��30��#��[  