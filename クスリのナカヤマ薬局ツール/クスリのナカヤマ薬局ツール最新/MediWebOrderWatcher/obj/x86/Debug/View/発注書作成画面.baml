   M S B A M L   `   `   `  ����  MediWebOrderWatcher4    .MediWebOrderWatcher.View.発注書作成画面   5      \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    5      >d2http://schemas.microsoft.com/expression/blend/2008    5      .��5      �   � RowDefinitions  5      �� 5      $��30 �6   �� 5	      $��* �6   .+�5      � -   5         �Name$ gdHeader��5      .+�5      �� -   5         ��Name$ 	tblHeader��5      $��0,0,-0.001,2q�6%   $��発注書作成��6;   $
��Wrap=�6H   $��21.333(�6\   $
��Bold"�6n   5      � 5      -   5        $ gdContainer��5      $��1��6!   .+�5      � -   5        $ gdContainerSplitter��5       � ColumnDefinitions 5   	   �� 5   
   �� 5   
   �� 5   
   .+�5   	   � -   5        $ gdOrderList��5      $��0��6#   .+�5   
   �� -   5         ��Name$ lbOrderList��5      �� 5   
   $��ListBoxItem��6   5   
   � 5   	   -   5        $ gdExpire��5      $��1��6    .+�5   
   �� -   5        $ lbExpire��5      .@�5      �� $��ListBoxItem��6   5      5   
   � 5   	   -	   5        $ gdDeadStock��5      $��2��6#   .+�5   
   �� -
   5        $ lbDeadStock��5      .@�5       �� $��ListBoxItem��6   5!      5"   
   5#   	   5$      5(      5)      