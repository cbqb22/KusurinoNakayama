   M S B A M L   `   `   `  ����  MCSystem1    +MCSystem.View.Windows.BalanceChangeSettings   5   	   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   8x,http://schemas.microsoft.com/winfx/2006/xaml    5   	     � Title$!  帳合変更マクロ設定��5   	    � WindowStartupLocation$ CenterScreen=�5   	    � Icon$ /MCSystem100x100.ico��5   	    � Topmost . ���	#FF2F415F$	��380��5   /   $	��400��6;    � 	Resources 5	       �� 
TargetType �� Property �� Value��%�  '��       '�� �     '�� �     'd� #    �� 5
   
   " ��6   .�5      ��  � ��$ Center=�62   �� 5       � ��$ 8(�6)   �� 5       � ��
 �����6+   5      �� 5   
   " ��6   .�5      ��  � ��$ 9(�6)   5      �� 5   
   " ��6   .�5      ��  � ��$ 8(�6)   5      �� 5   
   " d�6   .�5      ��  � ��$ 50��6&   �� 5       � ��$ 20��6'   �� 5       � ��$ 9(�6)   5      5      .��5       � $��20q�6   $
��Left=�6   $	��Top=�62    � RowDefinitions 5!   
   �� 5"      $
��auto �6   �� 5#      $��20 �6   �� 5$      $
��auto �6   �� 5%      $��20 �6   �� 5&      $
��auto �6   �� 5(      $��50 �6   .+�5+   
   � $��0��6    5,      �� 5-      $
��auto �6    �� 5.      $
��auto �6    �� 5/      $
��auto �6    �� 50      $
��auto �6    	 � ColumnDefinitions	 53      �� 54      $
��auto �6#   �� 55      $
��auto �6#   .+�58      � $��0��6   $��0��6    .+�59      �� $��検索名称ＸＹ座標��6   $��0,0,10,0q�6,   5:      � 5;      $��0��6   $��1��6    .+�5<      �� 
 �� Orientation$
 
Horizontal=�6   .+�5=      �� $��X:��6    �� 5>      -   5         ��Name$ tb検索名称X��5>      $��50��60    �� 	MaxLength$ 4��6;   �� 5?      $��Y:��6    �� 5@      -   5        $ tb検索名称Y��5@      $��50��60   $ 4��6;   d� 5A      -   5         d�Name$( "tbtn検索名称クリック取得��5A   #   $��Click取得��6;   ���	#FFC5DAFF6�   5B      5C      � 5E      $��1��6   $��0��6    .+�5F      �� $0��*検索名称F12完了ボタンＸＹ座標��6   $��0,0,10,0q�64   5G      � 5H      $��1��6   $��1��6    .+�5I      �� $
 
Horizontal=�6   .+�5J      �� $��X:��6    �� 5K      -   5        $$ tb検索名称完了ボタンX��5K      $��50��65   $ 4��6@   �� 5L      $��Y:��6    �� 5M      -   5        $$ tb検索名称完了ボタンY��5M      $��50��65   $ 4��6@   d� 5N      -   5        $7 1tbtn検索名称完了ボタンクリック取得��5N   #   $��Click取得��6@   ���	#FFC5DAFF6�   5O      5P      � 5T      $��2��6   $��0��6    .+�5U      �� $!��通常仕入先ＸＹ座標��6   $��0,0,10,0q�6-   5V      � 5W      $��2��6   $��1��6    .+�5X      �� $
 
Horizontal=�6   .+�5Y      �� $��X:��6    �� 5Z      -   5        $ tb通常仕入先X��5Z      $��50��61   $ 4��6<   �� 5[      $��Y:��6    �� 5\      -   5        $ tb通常仕入先Y��5\      $��50��61   $ 4��6<   d� 5]      -	   5        $+ %tbtn通常仕入先クリック取得��5]   #   $��Click取得��6<   ���	#FFC5DAFF6�   5^      5_      � 5d      $��3��6   $��0��6    .+�5e      �� $3��-通常仕入先F12完了ボタンＸＹ座標��6   $��0,0,10,0q�65   5f      � 5g      $��3��6   $��1��6    .+�5h      �� $
 
Horizontal=�6   .+�5i      �� $��X:��6    �� 5j      -
   5        $$ tb個別入力完了ボタンX��5j      $��50��65   $ 4��6@   �� 5k      $��Y:��6    �� 5l      -   5        $$ tb個別入力完了ボタンY��5l      $��50��65   $ 4��6@   d� 5m      -   5        $7 1tbtn個別入力完了ボタンクリック取得��5m   #   $��Click取得��6A   ���	#FFC5DAFF6�   5n      5o      5r      � 5s   
   $��2��6    5t      �� 5u      $
��auto �6    �� 5v      $
��auto �6    	 5y      �� 5z      $
��auto �6#   �� 5{      $
��auto �6#   .+�5~      � $��0��6   $��0��6    .+�5      �� $2��,在庫メンテナンス(受付)座標範囲��6   $��0,0,22,0q�64   5�      � 5�      $��0��6   $��1��6    .+�5�      �� $
 
Horizontal=�6   .+�5�      �� $
��auto��6   $
��auto��6*   $��1q�68   
�������._�5�      �� -   5         ��Name$' !img在庫メンテナンス受付��5�       $	��100��67   $��20��6C   $��5,5,5,5q�6O   5�      d� 5�      -   5        $2 ,tbtn在庫メンテナンス受付Drag取得��5�   #   $��
Drag取得��6A   $
��auto��6R   ���	#FFC5DAFF6�   5�      5�      � 5�      $��1��6   $��0��6    .+�5�      �� $*��$在庫メンテナンス座標範囲��6   $
��auto��60   $��30��6=   $��0,0,0,0q�6I   5�      � 5�      $��1��6   $��1��6    .+�5�      �� $
 
Horizontal=�6   .+�5�      �� $
��auto��6   $
��auto��6*   $��1q�69   
�������._�5�      �� -   5        $! img在庫メンテナンス��5�       $	��100��65   $��20��6A   $��5,5,5,5q�6M   5�      d� 5�      -   5        $, &tbtn在庫メンテナンスDrag取得��5�   #   $��
Drag取得��6?   $
��auto��6P   ���	#FFC5DAFF6�   5�      5�      5�      � 5�   
   $��4��6   $
��Left=�6   $��Center=�67    5�      �� 5�      $
��auto �6    �� 5�      $
��auto �6    	 5�      �� 5�      $
��auto �6#   �� 5�      $
��auto �6#   .+�5�      � $��0��6   $��0��6    .+�5�      �� $!��在庫テーブルCSVパス��6   5�      � 5�      $��0��6   $��1��6    .+�5�      �� $
 
Horizontal=�6   .+�5�      �� -   5        $% tb在庫テーブルCSVFilePath��5�      $	��200��6;   $��40��6G    �� TextWrapping$
 Wrap=�6S   �� 5�      -   5         ��Name$! btn在庫テーブル参照��5�      $��参照��62   $
��auto��6?   ���	#FFE4FFCF6f   5�      5�      � 5�      $��1��6   $��0��6    .+�5�      �� $'��!新帳合変更データ表パス��6    5�      � 5�      $��1��6   $��1��6    .+�5�      �� $
 
Horizontal=�6   .+�5�      �� -   5        $+ %tb新帳合変更データ表FilePath��5�      $	��200��6;   $��40��6G   $
 Wrap=�6S   �� 5�      -   5        $ btn新帳合変更参照��5�      $��参照��61   $
��auto��6>   ���	#FFE4FFCF6d   5�      5�      5�      � 5�   
   $��6��6   $��Right=�6   $��Bottom=�68   .+�5�      �� $
 
Horizontal=�6   $��Right=�62   .+�5�      �� -   5        $ 	btn保存��5�      $��保存��6(   $��80��6I   $��30��6T   ���	#FFFFFECD6`   �� 5�      -   5        $ btn閉じる��5�      $��	閉じる��6)   $��80��6L   $��30��6W   ���	#FFFFFECD6c   5�      5�      5�      5�      