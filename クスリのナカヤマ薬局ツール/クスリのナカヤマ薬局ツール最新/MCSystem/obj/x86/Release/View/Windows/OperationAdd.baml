   M S B A M L   `   `   `  ����   MCSystem(    "MCSystem.View.Windows.OperationAdd   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml      � Title$  マクロ内容追加��$	��350��$	��400�� � Topmost .  � WindowStartupLocation$ CenterScreen=� � Icon$ /MCSystem100x100.ico�����	#FF2F415F � 	Resources  �� 
TargetType �� Property �� Value��%�   '��       '	� ?     '�� ~     �� " ��.���  � ��$ Center=���  � ��
 ������� " 	�.���  � ��$ Center=���  � ��
 ������� " ��.���  � ��$ Center=���  � ��
 �����.��� $��20,30,20,20q� � RowDefinitions �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $��30 ��� 	 � ColumnDefinitions	 �� $
��auto ��� $
��auto �.+�� $��0��$��0��.+�	� -   
 	�Name$
 rbtnClickOperation��$��クリック座標�� 	� 	GroupName$ 0��$��Center=�� $��0��$��1��$��20,0,0,0q�.+���  �� Orientation$ 
Horizontal=�.+��� $��X:���� -    ��Name$ tbClickX��$��50������ *ClickRect.X+�� $��Y:��$��15,0,0,0q��� -   $ tbClickY��$��50������ *ClickRect.Y+d� -    d�Name$ tbtnClick取得��$��Click取得�����	#FFA2AFFF� $��1��$��0��.+��� $ Vertical=�.+�	� -   $
 rbtnDragOperation��$��ドラッグ座標��$ 0��$��Center=�	� -   $
 rbtnDragFastOperation��$��ドラッグ高速座標��$ 0��$��Center=�� $��1��$��1��$��20,0,0,0q� �� $��* ��� $��* �	 �� $��* ��� $
��1.5* �.+�� $��0��$��0��.+��� $ 
Horizontal=�.+��� $��X:���� -   $ tbDragX��$��50������ *Drag.StartX+ �� Mode$ TwoWay=�� $��0��$��1��.+��� $ 
Horizontal=�.+��� $��Y:��$��15,0,0,0q��� -   $ tbDragY��$��50������ *Drag.StartY+$ TwoWay=�d� -	   $! tbtnDragOperationDrag取得��$��
Drag取得��$
��auto��$
��auto�����	#FFC5DAFF� $��1��$��0��.+��� $ 
Horizontal=�.+��� $��W:��$��9(��� -
   $ tbDragWidth��$��50������ *	Drag.EndX+$ TwoWay=�� $��1��$��1��.+��� $ 
Horizontal=�.+��� $��H:��$��9(�$��15,0,0,0q��� -   $ tbDragHeight��$��50������ *	Drag.EndY+$ TwoWay=�� $��2��$��0��.+�	� -   $
 rbtnInputOperation��$��データ入力��$ 0��$��Center=�� $��2��$��1��$��32,0,0,0q�.+��� $ 
Horizontal=�.+��� �� -   $ tbInputData��$	��125�� �� TextAlignment$
 Left=� �� 	MaxLength$ 20��� $��3��$��0��.+�	� -   $
 rbtn待機Operation��$��待機時間(ミリ秒)��$ 0��$��Center=�� $��3��$��1��$��33,0,0,0q�.+��� $ 
Horizontal=�.+��� �� -   $ tbSleepTime��$��10��$��50��� $��4��$��0��.+�	� -   $
 rbtnImageMatchOperation��$��画像一致��$ 0��$��Center=�� $��4��$��1��$��20,0,0,0q�.+��� $ 
Horizontal=�.+��� $
��auto��$
��auto��$��1q�
�������._��� -    ��Name$ imgImageMatch��$	��125��$��20��$��5,5,5,5q�d� -   $ tbtnImageMatchDrag取得��$��
Drag取得��$
��auto��$
��auto�����	#FFC5DAFF� $��5��$��0��.+�	� -   $
 rbtnScreenShotOperation��$!��スクリーンショット��$ 0��$��Center=�� $��5��$��1��$��32,0,0,0q�.+��� $ 
Horizontal=�.+��� �� -   $ tbSaveScreenshotPath��$	��125��$
 Left=�$ 20���� -    ��Name$ btnGetScreenshotPath��$��参照��$
��auto��$
��auto�����	#FFC5DAFF� $��6��$��0��.+�	� -   $
 rbtn終了Operation��$��終了��$ 0��$��Center=�� $��7��$��0��$��2��.+��� -    ��Name$ chkUnMatchTask��$��不一致時の操作��$��Center=�$��Right=�$��0,0,10,0q�� $��8��$��2��.+��� $ 
Horizontal=�$��Right=�$��Bottom=�.+��� -   $ 	btn追加��$��追加��$��80��$��30�����	#FFEAE6AA�� -   $ 	btn中止��$��中止��$��80��$��30�����	#FFEAE6AA