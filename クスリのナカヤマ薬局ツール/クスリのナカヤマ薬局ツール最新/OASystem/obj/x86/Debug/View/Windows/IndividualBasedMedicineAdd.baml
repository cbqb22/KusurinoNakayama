   M S B A M L   `   `   `  ����C  ?OASystem, Version=1.1.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:OASystem.Model.ValidatorOASystem.Model.Validator  5   	   6    0OASystem.View.Windows.IndividualBasedMedicineAdd   5   	   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   8x,http://schemas.microsoft.com/winfx/2006/xaml    5   	   :	validator&clr-namespace:OASystem.Model.Validator    5   	     � Title$*  $個別管理医薬品マスタ追加��5   	   $	��350��6   $	��500��6+    � WindowStartupLocation$ CenterScreen=�5   	   ���	#FF2F415F � Icon$ /OASystemIcon100x100.ico��5       � 	Resources 5	      3   -OASystem.Model.Validator.JanCodeValidatorRule   JanCodeValidatorRule2   ,OASystem.Model.Validator.StringValidatorRule  StringValidatorRule2   ,OASystem.Model.Validator.DoubleValidatorRule  DoubleValidatorRule �� 
TargetType  	styHeader �� Property �� Value  
styExample  styButtonKeyword  	styButton��%�  &	        &	      &	 &     &	 9     &	 �     '�� �    &	 L    &	 �      5
   
   6)     5   
   6(     5   
   6(   �� 5   
   " ��6   .�5      ��  � ��$ Right=�64   �� 5       �@��TextAlignment$ Right=�6.   �� 5       � ��
 �����6+   5      �� 5   
   " ��6   .�5      ��  � ��$
 Left=�64   �� 5       �@��TextAlignment$
 Left=�6.   �� 5       � ��$	 8.5(�6)   �� 5       � ��
 �����6+   5      �� 5   
   " ��6   .�5      ��  � ��$
 auto��6&   �� 5       �@��TextAlignment$
 Left=�6.   �� 5       � ��$
 Left=�64   5      �� 5   
   " ��6   .�5       ��  � ��$
 Left=�64   �� 5!       � �� �#9CAAC16+   5"      �� 5$   
   " ��6   .�5%      ��  � ��$ 80��6&   �� 5&       � ��$ 40��6'   �� 5'       � ��$
 12.5(�6)   �� 5(       � ��
 �����6+   �� 5)       � ��
 �����6+   5*      5-      .��5.      � $��20,20,20,20q�6    � RowDefinitions 5/   
   �� 50      $
��auto �6   �� 51      $
��auto �6   �� 52      $
��auto �6   �� 53      $
��auto �6   �� 54      $
��auto �6   �� 55      $
��auto �6   �� 56      $
��auto �6   �� 57      $
��auto �6   �� 58      $
��auto �6   �� 59      $��* �6    � ColumnDefinitions 5<   
   �� 5=      $
��auto �6   �� 5>      $��* �6   �� 5?      $��* �6   .+�5B   
   � $��0��6   $��0��6   .+�5C      �� $��ＪＡＮコード:��6   $��Center=�6'   #��[ 6B   5D      � 5E   
   $��0��6   $��1��6   .+�5F      �� -   5        	 ��Name$	 tbＪＡＮコード��5F      $	��150��6(   $��Center=�64   .��6O   ��5G      �� 5H      
 �� Path$
 JANコード�6    �� Mode$ TwoWay=�6,    �� NotifyOnValidationError .  �� ValidatesOnExceptions .  �� ValidationRules 5I        5J      5L      .��5M       5N      5O      � 5P   
   $��0��6   $��2��6   .+�5Q      �� $��例) 4987*********��6   $��Center=�60   #��[ 6K   5R      � 5T   
   $��1��6   $��0��6   .+�5U      �� $��医薬品名:��6   $��Center=�6%   #��[ 6@   5V      � 5X   
   $��1��6   $��1��6   .+�5Y      �� -   5        $	 tb医薬品名��5Y      $	��150��6&   $��Center=�62   .��6M   ��5Z      �� 5[      $
 医薬品名�6    5\        5]         	MaxLength$ 50��6<      	MinLength$ 1��6K   5_      .��5`       5a      5b      � 5c   
   $��1��6   $��2��6   .+�5d      �� $4��.例) マイクロファインプラス 31G 5mm��6   $��Center=�66   #��[ 6Q   5e      � 5i   
   $��2��6   $��0��6   .+�5j      �� $��包装形態:��6   $��Center=�6%   #��[ 6@   5k      � 5l   
   $��2��6   $��1��6   .+�5m      �� -   5        $	 tb包装形態��5m      $	��150��6&   $��Center=�62   .��6M   ��5n      �� 5o      $
 包装形態�6    5p        5q      $ 10��6<   $ 1��6K   5s      .��5t       5u      5w      � 5x   
   $��2��6   $��2��6   .+�5y      �� $��例)袋��6   $��Center=�6#   #��[ 6>   5z      � 5}   
   $��3��6   $��0��6   .+�5~      �� $��包装単位:��6   $��Center=�6%   #��[ 6@   5      � 5�   
   $��3��6   $��1��6   .+�5�      �� -   5        $	 tb包装単位��5�      $	��150��6&   $��Center=�62   .��6M   ��5�      �� 5�      $
 包装単位�6    5�        5�      $ 10��6<   $ 1��6K   5�      .��5�       5�      5�      � 5�   
   $��3��6   $��2��6   .+�5�      �� $��例)本��6   $��Center=�6#   #��[ 6>   5�      � 5�   
   $��4��6   $��0��6   .+�5�      �� $��包装単位数:��6   $��Center=�6&   #��[ 6A   5�      � 5�   
   $��4��6   $��1��6   .+�5�      �� -   5        $	 tb包装単位数��5�      $	��150��6'   $��Center=�63   .��6N   ��5�      �� 5�      $
 包装単位数�6    5�        5�      $ 10��6<   $ 1��6K   5�      .��5�       5�      5�      � 5�   
   $��4��6   $��2��6   .+�5�      �� $��例)14本x5��6   $��Center=�6'   #��[ 6B   5�      � 5�   
   $��5��6   $��0��6   .+�5�      �� $��包装総量:��6   $��Center=�6%   #��[ 6@   5�      � 5�   
   $��5��6   $��1��6   .+�5�      �� -   5        $	 tb包装総量��5�      $	��150��6&   $��Center=�62   .��6M   ��5�      �� 5�      $
 包装総量�6    5�        5�      5�      .��5�       5�      5�      � 5�   
   $��5��6   $��2��6   .+�5�      �� $"��例)70 ※半角数字入力��6   $��Center=�6,   #��[ 6G   5�      � 5�   
   $��6��6   $��0��6   .+�5�      �� $��剤形区分:��6   $��Center=�6%   #��[ 6@   5�      � 5�   
   $��6��6   $��1��6   .+�5�      �� -   5         ��Name$ cmb剤形区分��5�      $	��150��6(   $��Center=�64   $
��Left=�6O   5�      � 5�   
   $��6��6   $��2��6   .+�5�      �� $#��例) 医療材料=0.その他��6   $��Center=�6-   #��[ 6H   5�      � 5�   
   $��7��6   $��0��6   .+�5�      �� $��製薬会社:��6   $��Center=�6%   #��[ 6@   5�      � 5�   
   $��7��6   $��1��6   .+�5�      �� -   5        $	 tb製薬会社��5�      $	��150��6&   $��Center=�62   $��9(�6M   .��6Z   ��5�      �� 5�      $
 製薬会社�6    5�        5�      $ 1��6<   $ 30��6J   5�      .��5�       5�      5�      � 5�   
   $��7��6   $��2��6   .+�5�      �� -	   5         ��Name$* $btn製薬会社キーワード検索��5�      $��キーワード検索��6-   $	��100��6?   #��[ 6K   6u   5�      � 5�   
   $��8��6   $��0��6   .+�5�      �� $��販売会社:��6   $��Center=�6%   #��[ 6@   5�      � 5�   
   $��8��6   $��1��6   .+�5�      �� -
   5        $	 tb販売会社��5�      $	��150��6&   $��Center=�62   $��9(�6M   .��6Z   ��5�      �� 5�      $
 販売会社�6    5�        5�      $ 1��6<   $ 30��6J   5�      .��5�       5�      5�      � 5�   
   $��8��6   $��2��6   .+�5�      �� -   5        $* $btn販売会社キーワード検索��5�      $��キーワード検索��6-   $	��100��6?   #��[ 6K   6u   5�      � 5�   
   $��9��6   $��2��6   $��0,20,0,0q�6,   .+�5�      ��  �� Orientation$ 
Horizontal=�6   $��Right=�62   .+�5�      �� -   5        $ 	btn中止��5�      $��中止��6(   $��75��65   $��30��6@   #��[ 6`   �� 5�      -   5        $ 	btn追加��5�      $��追加��6(   $��75��65   $��30��6@   #��[ 6`   5�      5�      5�      5�      