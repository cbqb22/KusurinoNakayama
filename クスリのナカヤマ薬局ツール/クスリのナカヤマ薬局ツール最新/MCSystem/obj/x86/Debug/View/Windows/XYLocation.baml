   M S B A M L   `   `   `  ����  MCSystem&     MCSystem.View.Windows.XYLocation   5   	   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   8x,http://schemas.microsoft.com/winfx/2006/xaml    5   	   >d2http://schemas.microsoft.com/expression/blend/2008    5   	   Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    6F     � Title$'  !ＸＹ座標確認ウィンドウ��5   	    � Topmost . 
 � Top$ 0��5   	    � Left$ 0��5   	    � Icon$ /MCSystem100x100.ico��5	   	    � AllowsTransparency .  � WindowStyle$
 None=�5   	   ���	#FF383838$��1q�64   ��6H   Z� .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6    � ColumnDefinitions 5   
   �� 5      �� 5      $��* �6   .+�5   
   �� -   5        	 ��Name$	 bdTransparent��5      
�������
 J� Opacity$
 0.275Q�6<   $��0,0,0,0q�6L   $��2��6]   .+�5      F� 
���   �$��1��6"   ���	#BBFFFFFF$��0q�6G   ��5       �� 5!      -   5        
ScreenArea�� �� GeometryCombineMode$	 Xor=�5!   ;   5#      5$      � 5'   
   $��0��6   $��0��6   .+�5(      �� -   5         ��Name$ tblXYPosition��5(      $��	X:0   Y:0��6/   $	��Top=�6@   $
��Left=�6X   
���   �
���  ��$��	10,10,0,0q�6�   $��15(�6�   5*      � 5+   
   $��1��6   $��0��6   $��
10,10,10,0q�6,   .+�5,      ��  �� Orientation$ 
Horizontal=�6   $	��Top=�62   $��Right=�6J   .+�5-      �� -   5         ��Name$ btnHide��5-      $��	最小化��6*   
����������	#FF001BFF��6x   Z� �� 5.      -   5        $ btnClose��5.      $��終了��6+   
����������	#FF001BFF��6y   Z� 5/      50      53      54      