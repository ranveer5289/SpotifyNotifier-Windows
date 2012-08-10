@echo off
C:\Windows\Microsoft.NET\Framework\v3.5\csc.exe /win32icon:SpotifyGrowl.ico /r:Growl.Connector.dll,Growl.CoreLibrary.dll /out:SpotifyNotifierGrowl.exe *.cs
echo "Compilation successful now run SpotifyNotifierGrowl.exe"
