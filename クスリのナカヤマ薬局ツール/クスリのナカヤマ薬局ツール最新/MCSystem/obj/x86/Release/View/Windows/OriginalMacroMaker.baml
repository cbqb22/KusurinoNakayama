   M S B A M L   `   `   `  ���� C  ?MCSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:MCSystem.ViewModel.Common.Converters$MCSystem.ViewModel.Common.Converters  .    (MCSystem.View.Windows.OriginalMacroMaker   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    F	converter2clr-namespace:MCSystem.ViewModel.Common.Converters      � Title$  マクロ作成��$	��500��$	��430�� � WindowStartupLocation$ CenterScreen=� � Topmost .  � Icon$ /MCSystem100x100.ico�����	#FF2F415F � 	Resources @   :MCSystem.ViewModel.Common.Converters.RectToStringConverter   RectToStringConverter �� 
TargetType �� Property �� Value  styColumnHeader  dt操作  dt操作座標 �� 	Converter	 �� Mode  dt入力データ列��%t  &	        '��      &	 D     &	 f     &	 �     7   &	 �       �� " ��.���  � ��$ Center=���  � ��
 ������� " ��.���  � ��$ 9(��� .R�� .+��� $��Right=����� *OperationEnum+�� .R�� .+��� $��Right=����� *操作座標+8   $	 OneTime=��� .R�� .+��� $��Right=����� *InputDataColumnNumber+.��� $��20,30,20,20q�
 � RowDefinitions
 �� $
��auto ��� $��* ��� $	��100 �.+�� $��0��
 �� $
��auto ��� $
��auto � � ColumnDefinitions �� $
��auto ��� $
��auto �.+�� $��0��$��0��.+��� $!��データファイルパス��� $��0��$��1��.+���  �� Orientation$ 
Horizontal=�$��13,0,0,0q�.+��� -    ��Name$! tbデータファイルPath��$	��215��$��50�� �� TextWrapping$
 Wrap=��� -    ��Name$$ btnデータファイル参照��$��参照��$
��auto�����	#FFCAFFB9� $��1��$��0��.+��� $!��保存先フォルダパス��� $��1��$��1��.+��� $ 
Horizontal=�$��13,0,0,0q�.+��� -   $! tb保存先フォルダPath��$	��215��$��50��$
 Wrap=��� -   $$ btn保存先フォルダ参照��$��参照��$
��auto�����	#FFCAFFB9� $��1��$��0,20,0,0q�
 �� $��30 ��� $��* ��� $��30 �.+�� $��0��.+��� $��マクロ内容�����	#FF3CE6D2$��70��$
��Left=� �� TextAlignment$ Center=�� $��1��.+��� -    ��Name$ lvMacroContents�� �� SelectionMode$ Single=� �� View �� .P���  �� CellTemplate# [  �� Width$ 85��.���� $��操作���� $��操作座標��# [ $	 230���� $��入力データ列�� �� HeaderContainerStyle# [ # [ $
 auto��� $��2��.+��� -   $ btnマクロ内容追加��$��内容追加��$��75��$��30��$��Right=����	#FFFF8B8B� $��4��.+��� $ 
Horizontal=�$��Right=�$��Bottom=�.+��� -   $ 	btn保存��$��保存��$��80��$��30�����	#FFEAE6AA�� -   $ btn閉じる��$��	閉じる��$��80��$��30�����	#FFEAE6AA