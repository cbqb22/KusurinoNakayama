   M S B A M L   `   `   `  ����   OASystem$    OASystem.View.Windows.Settings   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    >d2http://schemas.microsoft.com/expression/blend/2008    Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006      � Title$  設定��$	��600��$	��800�� � WindowStartupLocation$ CenterScreen=����	#FF2F415F � Icon$ /OASystemIcon100x100.ico�� � 	Resources  �� 
TargetType   stytblSettingsHeader �� Property �� Value  stytblDetailSettingHeader  stytbOwnerStoreName  stycmbPrinterSettings  	styButton��%�  &	        &	 S     &	 �     &	 �    &	 �    �� " ��.���  � ��$ 20(���  � ��
 �������  � ��
 �   ��� " ��.���  � ��$ 15(���  �@��TextAlignment$ Center=���  � ��$ Center=���  � ��$
 auto����  � ��$	 200���� " ��.���  � ��$ 15(���  �@��TextAlignment$
 Left=���  � ��$ Center=���  � ��$
 Left=���  � ��$ 35����  � ��$	 200���� " ��.���  � ��$ Center=���  � ��$
 Left=���  � ��$ 40����  � ��$	 200���� " ��.���  � ��$ 80����  � ��$ 40����  � ��$
 12.5(���  � ��
 �������  � ��
 �����.��� $��20,20,20,10q� � RowDefinitions �� $��60 ��� $��* ��� $	��100 �.+�� $��0��$��0,0,20,0q�.+��� $��5q� �� Padding$ 5q�$��1q����	#FFFFF7B6	 �� CornerRadius$	 5��$	��250��$
��Left=�._��� $��設定画面��#��[  $��Center=�
 �� TextAlignment$
 Center=�$	��200��$��Center=����	#FF6A4747��Z� � $��1����. $��0,20,0,0q� �� $��50 ��� $��50 ��� $��50 � � ColumnDefinitions �� $
��Auto ��� $��* �.+�� $��0��$��0��$��0q�.+��� $��33��$	��200�����	#FFE3E0E0$��Center=�$��Center=�$	 2��$��10,0,0,0q���Z� ._��� -    ��Name$ 	textBlock��$��自店舗名称��#��[ ���	#FF4A3A3A$��0q�$��Center=�$
��Auto��$��Center=�$
 Center=�� $��0��$��1��$��20,0,0,0q�.+��� -    ��Name$ tbOwnerStoreName��$�� ��#��[ $	��300��$
��Left=�� $��1��$��0��.+��� $��33��$	��200�����	#FFE3E0E0$	 2��$��10,0,0,0q���Z� ._��� $��プリンター設定��#��[ ���	#FF4A3A3A$��Center=����	#FFE3E0E0$
��Auto��� $��1��$��1��$��20,0,0,0q�.+���  �� Orientation$ 
Horizontal=�.+��� -    ��Name$ cmbPrinterSettings��#��[ $
��Left=�$	��300���� -   $ cmbPrinterトレイ選択��#��[ $
��Left=�$	��175��$��25,0,0,0q�� $��2��$��0��.+��� $��33��$	��200�����	#FFE3E0E0$	 2��$��10,0,0,0q���Z� ._��� $��MEDICODE-Web SRパス��#��[ ���	#FF4A3A3A$��Center=�$
��Auto��� $��2��$��1��$��20,0,0,0q�.+��� -   $ tbMEDICODEWebSRPGPath��$�� ��$��35�� �� TextWrapping$
 Wrap=�$	��500��$
��Left=�� $��2��$��1��.+��� $ 
Horizontal=�$��Right=�.+��� -    ��Name$ btnSave��$��保存��#��[ �� -   $ btnClose��$��	閉じる��#��[ $��10,0,0,0q�