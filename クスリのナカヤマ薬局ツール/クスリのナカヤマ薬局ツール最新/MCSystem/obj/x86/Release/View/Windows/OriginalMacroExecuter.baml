   M S B A M L   `   `   `  ���� C  ?MCSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:MCSystem.ViewModel.Common.Converters$MCSystem.ViewModel.Common.Converters  1    +MCSystem.View.Windows.OriginalMacroExecuter   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    F	converter2clr-namespace:MCSystem.ViewModel.Common.Converters      � Title$  独自マクロ実行��$	��200��$	��410�� � WindowStartupLocation$ CenterScreen=� � Topmost .  � Icon$ /MCSystem100x100.ico�����	#FF2F415F � 	Resources  �� 
TargetType �� Property �� Value��%I   '��       �� " ��.���  � ��$ Center=���  � ��
 �����.��� $��20,30,16,20q� � RowDefinitions �� $
��auto ��� .+�� $��0�� �� $
��auto �	 � ColumnDefinitions	 �� $
��auto ��� $
��auto �.+�� $��0��$��0��.+��� $!��データファイルパス��� $��0��$��1��.+��� 
 �� Orientation$
 
Horizontal=�$��7,0,0,0q�.+��� -    ��Name$! tbデータファイルPath��$	��215��$��50�� �� TextWrapping$
 Wrap=��� -    ��Name$$ btnデータファイル参照��$��参照��$
��auto�����	#FFCAFFB9� $��1��.+��� $
 
Horizontal=�$��Right=�$��Bottom=�.+��� -   $ btnマクロ実行��$��マクロ実行��$��80��$��30�����	#FFEAE6AA�� -   $ btn閉じる��$��	閉じる��$��80��$��30�����	#FFEAE6AA