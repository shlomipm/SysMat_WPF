   M S B A M L   `   `   `  ����  	SysMatWPF    SysMatWPF.MyLog   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   X TXceed.Wpf.Toolkit, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   ;toolkit-http://schemas.microsoft.com/wpf/2008/toolkit  5   	     � 
ResizeMode$  NoResize=�5   	    � WindowStyle$
 None=�6    � WindowStartupLocation$ Manual=�62    � AllowsTransparency . 
������ $	��285��5   	   $	��650��6    � ShowInTaskbar .   J� Opacity$	 0.8Q�68    � 	Resources 5	       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ Resources\ScrollBar.xaml?�6%   �� 5      $ Resources\Buttons.xaml?�6%   %    5      .��5      � .+�5   
   �� 
�������$	��0.5q�6(   ���#e4e4e4$��0q�6S   	 �� CornerRadius$	 0 3 3 0��6^   $	��645��6u   
 J� Effect
 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         ShadowDepth$ 0Q�6#      
BlurRadius$ 5Q�63      Color$ DarkGray��6B      Opacity$
 0.85Q�6S      	Direction$	 270Q�6b   ._�5      �  � RowDefinitions 5      �� 5      $��40 �6$   �� 5      $��* �6$   �� 5      $��25 �6$   .+�5#      � $	��270��6   $��0,3,0,5q�6#   $��0��64   $��0��6D    � ColumnDefinitions 5$      �� 5%      $��90 �6+   �� 5&      $��* �6+   �� 5'      $��30 �6+   .+�5*      �� $��0��6   $��5,0,0,0q�6,   $��Center=�6=   .��6Y   My Log File6f   �� 5,      -   5        $��
rectangle2��5,   0   $��1��6    $��30��6B   
������ 5-   7   �� 5.      $��2��6!    �� Orientation$ 
Horizontal=�61   $
��Left=�6J   $��0,0,0,0q�6e   .+�5/      �� -   5        $��25��5/   !   $��20��6,   $��Right=�68   $��0,0,3,0q�6T      SquareBackgroundButton#��[  50       � ToolTip$ Close��68   .��51      �� $��images\close.png��6$   52      54      55      &   System.Windows.Controls.DataGrid  57      -   5          Name$ logDataGrid��57      $	��555��60   $	��265��6=      AutoGenerateColumns .  $��0��6e   $��1��6u   $	��Top=�58      $��Center=�63   "   VerticalScrollBarVisibility$
 Auto=�59      $   HorizontalScrollBarVisibility$ Hidden=�6>      CanUserAddRows .  $��0q�5:         GridLinesVisibility$
 None=�6/      HeadersVisibility$
 None=�6J   ���#e4e4e4��5;      �� *logDataTable+ � DataContext 5<      ��    	CellStyle 5>      �� 5?       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6     �� Triggers 5@      P� 5A   "    P� Property �@ 
IsSelected  P� Value  . . �5B   &   �� ! �� Property! � ��" �� Value
" ���� 6C   �� 5C   &   ! � ��
" �   �6C   �� 5D   &   ! � ��
" ���� 6D   �� 5E   &   ! � ��$
" Wrap=�6O   5F   #   5H      #   RowStyle# 5K      �� 5L      )  #System.Windows.Controls.DataGridRow"  6    .�5M      �� ! � ��
" ���� 6<   �� 5N      ! � ��$" 1q�6@   �� 5O      ! � ��" �#e4e4e46;   5\      $   Columns$ 5_      0  *System.Windows.Controls.DataGridTextColumn  5`      ,  &System.Windows.Controls.DataGridColumn%   Width5  /System.Windows.Controls.DataGridLengthConverter$	% 250 6-   1  +System.Windows.Controls.DataGridBoundColumn&   Binding& 69   �� *	logLine+' �� NotifyOnTargetUpdated' . ( �� NotifyOnSourceUpdated( . ) �� Mode$) OneWay=�*   ElementStyle* 5b      �� 5c   "   .�5d   &   �� ! � ��$
" Wrap=�6O   �� 5e   &   ! �@��TextAlignment$
" Left=�6P   �� 5f   &   ! � ��$" 5,0,0,0q�6I   5g   #   5i      5l      �� 5n      $ 
Horizontal=�6   $��0,5,0,0q�66   $��0��6G   $��2��6W   5p      � 5r      $��Stretch=�6   $��0��65   $��4��6E   ���#0072c6 5s      �� 5t      $��50 �6+   �� 5u      $��50 �6+   �� 5v      $��* �6+   .+�5y      �� -   5        + ��Name$+ clearLog��5y      $��0��6/   $��40��6?   $��20��6J   $
��Left=�6V   $��5 0 0 0q�6q     SquareBackgroundButtonDark#��[ 5z      $ 	Clear Log��6A   .��5{      �� $��Clear��6$   
�������61   5|      �� 5~      -   5        $+ savaLog��5~      $��1��6.   $��40��6>   $��20��6I   $
��Left=�6U   #��[ 5      $ Save Log��6A   .��5�      �� $
��Save��6$   
�������60   5�      5�      5�      5�      5�      5�      