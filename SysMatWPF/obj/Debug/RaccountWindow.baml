   M S B A M L   `   `   `  ����  	SysMatWPF    SysMatWPF.RaccountWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   I EWpfAnimatedGif, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null,gif"http://wpfanimatedgif.codeplex.com  5   	   X TXceed.Wpf.Toolkit, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   ;toolkit-http://schemas.microsoft.com/wpf/2008/toolkit  5   	   -   5        $��70��5   	   $	��250��6     � 
ResizeMode$  NoResize=�5	   	    � WindowStyle$
 None=�6    � AllowsTransparency . 
������  � WindowStartupLocation$ Manual=�6e    � 	Resources 5   
    �� MergedDictionaries �� Source�� 5       5      �� 5      $ Resources\ScrollBar.xaml?�6)   �� 5      $ Resources\Buttons.xaml?�6)   %    5      .��5      � .+�5   
   �� 
�������$��1q�6(   
�������$��2q�6O    �� CornerRadius$ 5��6Z    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5      	   ShadowDepth$	 0Q�6#   
   
BlurRadius$
 10Q�63      Color$ DarkGray��6C   5      �� 5    
   $��7,8,0,0q�6    �� Orientation$ 
Horizontal=�6&   $	��Top=�6?   .+�5!      �� .+�5#      �� -   5        $��raccountPassTextbox��5#      $��3,0,0,3q�65   $	��190��6F   $
��Left=�6R   $��0q�6m   $��14(�6�   $
��Bold"�6�   .��6�    � ContextMenu 5$      �� 5%      .@�5&      m�  m� Command$
 Copy��6'    m� Icon 5'   "   �� 5(   &   5*      5+      .��5,   +   5-      �� 5/      $ 
Horizontal=�6   .+�50      (  "Xceed.Wpf.Toolkit.WatermarkTextBox  -   5        $��SendTo��50   ,   $	��3,5q�6:   $	��130��6G      	Watermark$ Send Via SMS��6S   $
��Left=�6l   6�   �� 52      -   5        $��btnSend��52   4   $��43��6   $��20��6(   $��5,0,0,0q�6C    �� 	IsDefault .  � ToolTip$  Send SMS With The Raccount��53         myFirstbutton#��[  6>   .��6f   SEND54      55      57      �� 59      $	��Top=�6   $��10,3,0,0q�61   .+�5:      �� -   5        $��btnExit��5:   0   $��20��6   $��15��6$   $ Close Window��6U      roundButtonTemplateVerySmall#��[ 5;   !   .��5<      �� $��images\clear.png��6   $��12��66   5=      �� 5?      -   5        $��btnCopy��5?      $��Right=�6)   $��0,5,0,0q�6E   $��30��6V   $��30��6b     SquareBackgroundButton#��[ 5@      $  Copy Raccount To Clipboard��68   .��5A      �� $��images\copy.png��6   $��20��65   $��20��6@   $��0,-3,0,0q�6L   5B      5D      5E      5G      5I      