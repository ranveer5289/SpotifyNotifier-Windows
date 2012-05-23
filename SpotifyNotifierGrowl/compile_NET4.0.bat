@echo off
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe /r:Growl.Connector.dll,Growl.CoreLibrary.dll /out:SpotifyNotifierGrowl.exe *.cs
echo "Compilation successfull now run SpotifyNotifierGrowl.exe"
