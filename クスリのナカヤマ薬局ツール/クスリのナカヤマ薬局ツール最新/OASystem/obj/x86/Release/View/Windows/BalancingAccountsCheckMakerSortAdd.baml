   M S B A M L   `   `   `  ����   OASystem>    8OASystem.View.Windows.BalancingAccountsCheckMakerSortAdd   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml      � Title$=  7帳合先チェックマスタ：メーカー別 追加��$	��200��$	��500�� � WindowStartupLocation$ CenterScreen=����	#FF2F415F � Icon$ /OASystemIcon100x100.ico�� � 	Resources  �� 
TargetType   styButtonKeyword �� Property �� Value  	styButton��%�   &	        &	 A     �� " ��.���  � ��$
 Left=���  � �� �#9CAAC1�� " ��.���  � ��$ 80����  � ��$ 40����  � ��$
 12.5(���  � ��
 �������  � ��
 �����.���  � RowDefinitions �� $��20 ��� $
��auto ��� $
��auto ��� $��* � � ColumnDefinitions �� $
��auto ��� $��* �.+�� $��1��$��0��$��20,0,0,0q�.+��� $��メーカー名:��$��Center=�� $��1��$��1��.+��� 	 �� Orientation$	 
Horizontal=�.+��� -   
 ��Name$
 tbxメーカー名��$0��*キーワード検索してください。��$	��150��$��Center=� J� IsHitTestVisible .  
�������$��9(��� -    ��Name$ btn検索メーカー名��$��キーワード検索��$
��auto��$��10(�#��[  � $��2��$��0��$��20,0,0,0q�.+��� $��
帳合先:��$��Center=�� $��2��$��1��.+��� -    ��Name$ cmb帳合先��$	��150��$��Center=�$
��Left=�� $��3��$��20q�$��2��.+��� $	 
Horizontal=�$��Right=�$��Bottom=�.+��� -   $ 	btn中止��$��中止��$��75��$��30��#��[ �� -   $ 	btn追加��$��追加��$��75��$��30��#��[ 