   M S B A M L   `   `   `  ����  デッド品管理ツール:    4StockManagement.View.Settings.ExceptiveMedicinesList   5   	   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   8x,http://schemas.microsoft.com/winfx/2006/xaml    5   	     � Title$(  "チェック除外医薬品 設定��5   	   $	��500��6   $	��430��6+    � WindowStartupLocation$ CenterScreen=�5   	    � Icon$ /nakayama.ico��5   	   ���	#FF2F415F � 	Resources 5       �� 
TargetType �� Property �� Value   styColumnHeader  dt医薬品名称 ! dtレセプト電算コード��%�  '��       &	  r     &	 �     &	 4    �� 5   
   " ��6   .�5      ��  � ��$ Center=�62   �� 5       � ��
 �����6+   5      �� 5   
   " ��6(   .�5      ��  � ��$ 10(�6)   5      �� 5   
   .R�5      � .+�5      �� $��Right=�63   ��6   �� *医薬品名称+63   5      5      �� 5   
   .R�5      � .+�5      �� $��Right=�67   ��6   �� *レセプト電算コード+67   5      5      5#      .��5%      � $��
20,0,20,20q�6    � RowDefinitions 5&   
   �� 5'      $��* �6   �� 5(      $��80 �6   .+�5,   
   � $��0��6   $��0,20,0,0q�6    5-      �� 5.      $��30 �6    �� 5/      $��* �6    �� 50      $��30 �6    .+�53      � $��0��6   .+�54      �� $��除外医薬品リスト��6   $
��auto��6,   
�������$
��Left=�6M    �� TextAlignment$ Center=�6h   $
��Bold"�6   55      � 56      $��1��6   .+�57      �� -   5        	 ��Name$	 lvExceptiveMedicinesList��57   2   
 �� SelectionMode$
 Single=�6    �� View 58      �� 59      .P�5:      �� $��医薬品名称��6-    �� CellTemplate# [ 6=    �� Width$
 Auto��6e   �� 5;      $!��レセプト電算コード��6-    �� HeaderContainerStyle# [  6A   # [ 6y   $
 auto��6�   5<      5>      5@      � 5A      $��2��6   .+�5B      ��  �� Orientation$ 
Horizontal=�6   $��Right=�66   .+�5C      �� -   5         ��Name$ btn除外追加��5C      $��除外追加��6.   $��60��6S   $��25��6^   $��Right=�6j   ���	#FFFF8B8B6�   �� 5D      -   5        $ btn選択削除��5D      $��選択削除��6.   $��60��6S   $��25��6^   $��Right=�6j   ���	#FFFF8B8B6�   5E      5F      5G      � 5J   
   $��1��6   .+�5K      �� $ 
Horizontal=�6   $��Right=�62   $��Bottom=�6N   .+�5M      �� -   5        $ btn閉じる��5M      $��	閉じる��6)   $��80��6L   $��30��6W   ���	#FFEAE6AA6c   5N      5O      5R      5S      