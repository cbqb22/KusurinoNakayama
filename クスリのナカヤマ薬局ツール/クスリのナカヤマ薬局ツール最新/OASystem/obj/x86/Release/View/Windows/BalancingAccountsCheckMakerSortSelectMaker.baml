   M S B A M L   `   `   `  ����   OASystemF    @OASystem.View.Windows.BalancingAccountsCheckMakerSortSelectMaker   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml      � Title$  メーカー名 検索��$	��500��$	��500�� � WindowStartupLocation$ CenterScreen=����	#FF2F415F � Icon$ /OASystemIcon100x100.ico�� � 	Resources  �� 
TargetType �� Triggers ]� 
Conditions �� Property �� Value	 �� Property
 �� Value 9  5dtBalancingAccountsCheckMakerSortSelectMaker会社名  styButtonKeyword  	styButton��%�  '��       &	  o     &	 �     &	 �     �� " �� ]�  ��  �@��
IsSelected . ./��� 	 � ��

 �����.��� 	 � ��$
 Stretch=��� .R�� .+��� $
��Left=����� �� " ��.��� 	 � ��$

 Left=��� 	 � ��
 �#9CAAC1�� " ��.��� 	 � ��$
 80���� 	 � ��$
 40���� 	 � ��$

 12.5(��� 	 � ��

 ������� 	 � ��

 �����.��� $��20,20,20,20q� � RowDefinitions �� $
��auto ��� $
��auto ��� $
��auto �.+�� $��0��.+���  �� Orientation$ 
Horizontal=�$
��Left=�.+��� -    ��Name$ tbキーワード��$	��200��$��Center=� �� TextAlignment$
 Left=��� -    ��Name$ btnキーワード検索��$��キーワード検索��$
��auto��$��25��#��[ � $��1��.+��� -    ��Name$ lvCompanyName�� �� SelectionMode$ Single=�$	��330�� �� View �� .P��� $��メーカー名�� �� CellTemplate# [   �� Width$	 430��� $��2��$��0,20,0,0q�.+��� $ 
Horizontal=�$��Right=�.+��� -   $ 	btn中止��$��中止��$��75��$��30��#��[ �� -   $ 	btn選択��$��選択��$��75��$��30��#��[ 