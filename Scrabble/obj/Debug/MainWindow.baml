   M S B A M L   `   `   `  ����C  ?Scrabble, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:ScrabbleScrabble  5   	       Scrabble.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Scrabble     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��* �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��Scrabble��6   $��40(�6#   $��0��61   $��0��6>   $��2��6N   $��Center=�6b   $��Center=�6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ LeaderBoard��5      $��1��6(   $��1��65   $��4��6E   
�������   AutoGenerateColumns .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 6�      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 63   �� *Name+  5      $ Score��6%    64   �� *Score1+5      �� 5   
   $��Player 1 Name��6   $��1��6)   $��0��66   $��Center=�6F   $��Center=�6h   �� 5   
   -   5        	 ��Name$	 Player1_Textbox��5      $��2��6+   $��0��68   
�������$��Center=�6_   $��Center=�6�   �� 5   
   $��Player 2 Name��6   $��3��6(   $��0��65   $��Center=�6E   $��Center=�6g   �� 5    
   -   5        $	 Player2_Textbox��5       $��4��6+   $��0��68   
�������$��Center=�6_   $��Center=�6�   �� 5!   
   -   5        
 ��Name$
 Start_button��5!      $��
Start Game��6'   $��5��6<   $��0��6I   $��2��6Y   6m   5$      5%      