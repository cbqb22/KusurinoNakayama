   M S B A M L   `   `   `  ����   MCSystem&     MCSystem.View.Windows.XYLocation   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    >d2http://schemas.microsoft.com/expression/blend/2008    Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006      � Title$'  !ＸＹ座標確認ウィンドウ�� � Topmost . 
 � Top$ 0�� � Left$ 0�� � Icon$ /MCSystem100x100.ico�� � AllowsTransparency .  � WindowStyle$
 None=����	#FF383838$��1q���Z� .���  � RowDefinitions �� $
��auto � � ColumnDefinitions �� �� $��* �.+��� -   	 ��Name$	 bdTransparent��
�������
 J� Opacity$
 0.275Q�$��0,0,0,0q�$��2��.+�F� 
���   �$��1�����	#BBFFFFFF$��0q����� -   
ScreenArea�� �� GeometryCombineMode$	 Xor=�� $��0��$��0��.+��� -    ��Name$ tblXYPosition��$��	X:0   Y:0��$	��Top=�$
��Left=�
���   �
���  ��$��	10,10,0,0q�$��15(�� $��1��$��0��$��
10,10,10,0q�.+���  �� Orientation$ 
Horizontal=�$	��Top=�$��Right=�.+��� -    ��Name$ btnHide��$��	最小化��
����������	#FF001BFF��Z� �� -   $ btnClose��$��終了��
����������	#FF001BFF��Z� 