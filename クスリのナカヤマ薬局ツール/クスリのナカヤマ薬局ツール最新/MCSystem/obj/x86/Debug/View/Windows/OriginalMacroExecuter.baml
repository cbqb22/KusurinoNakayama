   M S B A M L   `   `   `  ����C  ?MCSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:MCSystem.ViewModel.Common.Converters$MCSystem.ViewModel.Common.Converters  5   	   1    +MCSystem.View.Windows.OriginalMacroExecuter   5   	   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   8x,http://schemas.microsoft.com/winfx/2006/xaml    5   	   F	converter2clr-namespace:MCSystem.ViewModel.Common.Converters    5   	     � Title$  独自マクロ実行��5   	   $	��200��6   $	��410��6&    � WindowStartupLocation$ CenterScreen=�5   	    � Topmost .  � Icon$ /MCSystem100x100.ico��5   	   ���	#FF2F415F � 	Resources 5
       �� 
TargetType �� Property �� Value��%|   '��       �� 5   
   " ��6   .�5      ��  � ��$ Center=�62   �� 5       � ��
 �����6+   5      5      .��5      � $��20,30,16,20q�6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      .+�5   
   � $��0��6    5      �� 5      $
��auto �6    	 � ColumnDefinitions	 5!      �� 5"      $
��auto �6#   �� 5#      $
��auto �6#   .+�5&      � $��0��6   $��0��6    .+�5'      �� $!��データファイルパス��6   5(      � 5)      $��0��6   $��1��6    .+�5*      �� 
 �� Orientation$
 
Horizontal=�6   $��7,0,0,0q�66   .+�5+      �� -   5         ��Name$! tbデータファイルPath��5+      $	��215��65   $��50��6A    �� TextWrapping$
 Wrap=�6M   �� 5,      -   5         ��Name$$ btnデータファイル参照��5,      $��参照��63   $
��auto��6[   ���	#FFCAFFB96h   5-      5.      50      � 53   
   $��1��6   .+�54      �� $
 
Horizontal=�6   $��Right=�62   $��Bottom=�6N   .+�55      �� -   5        $ btnマクロ実行��55      $��マクロ実行��6+   $��80��6R   $��30��6]   ���	#FFEAE6AA6i   �� 56      -   5        $ btn閉じる��56      $��	閉じる��6)   $��80��6L   $��30��6W   ���	#FFEAE6AA6c   57      58      5;      5<      