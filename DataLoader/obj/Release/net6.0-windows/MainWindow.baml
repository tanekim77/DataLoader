   M S B A M L   `   `   `  ���� E  ADataLoader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:DataLoader
DataLoader      DataLoader.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:DataLoader       � Title$  
MainWindow��$	��650��$	��800��.��� .+��� .@��� $��Process All��.���  � RowDefinitions �� $��30 ��� $��30 ��� $��40 ��� $
��auto ��� $��* ��� $��60 ��� $��25 �.+�]� $��0��.+��� $��Center=�$��Max File Size (bytes):��$��12(��� -    ��Name$ InputSplit_All��$��20�� �� TextWrapping$
 Wrap=��� $
��Left=�$7��1texts to Replace (from => To) separated by comma:��$��12(�$��1���� -   $ InputReplace_All��$��40��$
 Wrap=�$��2���� -    ��Name$ FilesAndFoldersLabel_All��$��Center=�$�� ��$��3��$��16(��� -    ��Name$ FilesFolders_All��
 � Tag$	 All�� J� 	AllowDrop$
 True��
�������$��	40,0,40,0q�$��4��.+��� $��Drop Files/Folders��$��Center=��� -    ��Name$ Process_All��$	 All��$��5��.��Start�� -   $ 
Status_All��$�� ��$��10(�$��6���� $��Replace illegal Chars��.���  �� $��30 ��� $��40 ��� $
��auto ��� $��* ��� $��60 ��� $��25 �.+��� $
��Left=�$7��1texts to Replace (from => To) separated by comma:��$��0��$��12(��� -   $ InputReplace_Replace��$��40��$
 Wrap=�$��1��.���� -   $" FilesAndFoldersLabel_Replace��$��Center=�$�� ��$��2��$��16(��� -	   $ FilesFolders_Replace��$ Replace��$��3��$
 True��
�������$��	40,0,40,0q�.+��� $��Drop Files/Folders��$��Center=��� -
   $ Process_Replace��$ Replace��$��4��.��Start�� -   $ Status_Replace��$�� ��$��5��$��12(��� $��Split Files��.���  �� $��30 ��� $
��auto ��� $��* ��� $��60 ��� $��25 �.+�]� $��0��.+��� $��Center=�$��Max File Size (bytes):��$��12(��� -   $ InputSplit_Split��$��20��$
 Wrap=��� -   $  FilesAndFoldersLabel_Split��$��Center=�$�� ��$��1��$��16(��� -   $ FilesFolders_Split��$ Split��$
 True��
�������$��	40,0,40,0q�$��2��.+��� $��Drop Files/Folders��$��Center=��� -   $ Process_Split��$ Split��$��3��.��Start�� -   $ Status_Split��$�� ��$��4��$��12(�