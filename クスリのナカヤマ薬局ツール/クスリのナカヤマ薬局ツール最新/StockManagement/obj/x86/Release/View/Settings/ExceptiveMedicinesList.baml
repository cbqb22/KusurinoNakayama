   M S B A M L   `   `   `  ����   デッド品管理ツール:    4StockManagement.View.Settings.ExceptiveMedicinesList   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml      � Title$(  "チェック除外医薬品 設定��$	��500��$	��430�� � WindowStartupLocation$ CenterScreen=� � Icon$ /nakayama.ico�����	#FF2F415F � 	Resources  �� 
TargetType �� Property �� Value   styColumnHeader  dt医薬品名称 ! dtレセプト電算コード��%  '��       &	  ?     &	 b     &	 �     �� " ��.���  � ��$ Center=���  � ��
 ������� " ��.���  � ��$ 10(��� .R�� .+��� $��Right=����� *医薬品名称+�� .R�� .+��� $��Right=����� *レセプト電算コード+.��� $��
20,0,20,20q� � RowDefinitions �� $��* ��� $��80 �.+�� $��0��$��0,20,0,0q� �� $��30 ��� $��* ��� $��30 �.+�� $��0��.+��� $��除外医薬品リスト��$
��auto��
�������$
��Left=� �� TextAlignment$ Center=�$
��Bold"�� $��1��.+��� -   	 ��Name$	 lvExceptiveMedicinesList��
 �� SelectionMode$
 Single=� �� View �� .P��� $��医薬品名称�� �� CellTemplate# [  �� Width$
 Auto���� $!��レセプト電算コード�� �� HeaderContainerStyle# [  # [ $
 auto��� $��2��.+���  �� Orientation$ 
Horizontal=�$��Right=�.+��� -    ��Name$ btn除外追加��$��除外追加��$��60��$��25��$��Right=����	#FFFF8B8B�� -   $ btn選択削除��$��選択削除��$��60��$��25��$��Right=����	#FFFF8B8B� $��1��.+��� $ 
Horizontal=�$��Right=�$��Bottom=�.+��� -   $ btn閉じる��$��	閉じる��$��80��$��30�����	#FFEAE6AA