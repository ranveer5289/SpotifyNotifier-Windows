<b>SpotifyNotifier</b> is an application which notifies current playing song in Spotify
on a Windows system.

Currently supports notification applications like Growl, Snarl, Notifu.

For installation instruction visit http://code.google.com/p/spotifynotifier/

Special Thanks to BrendanMcK(http://stackoverflow.com/users/660175/brendanmck)
for his contribution.


<strong>These are a couple of improvements in the new version of spotifynotifiergrowl:</strong>

1) Now no need to register the application with growl manually. Check
"notifygrowl.cs" for relevant code.

2) Now displays album-cover/album-art of the current playing track in the
notification.

3) Hide the window after running the ".exe" application. Check
"CustomNoWindow.bat" for relevance.

4) Now it doesn't dislpay the track notification when you play again after the
spotify is paused.


Thanks to Matthew Javellana @ mmjavellana@gmail.com for the above improvements.


<strong>Improvements in version 1.2 for spotifynotifiergrowl:</strong>

1) Now uses <b>Growl C# API</b> to register application and to show notifications so dependency on <b>growlnotify.exe</b> removed

2) As some of the methods were not supported for .NET < 4.0 so now the zip includes  

   2.1) SpotifyNotifyGrowl_NET2.0.exe for those who are using .NET version 2.0 (I suppose nobody is).

   2.2) SpotifyNotifyGrowl_NET3.5.exe for those who are using .NET version 3.5.

   2.3) SpotifyNotifyGrowl.exe for those who are using .NET version 4.0.

3) It also includes a compile_NET3.5.bat file to compile the project after making some changes to it.

<strong>Improvements in version 1.1 for spotifynotifiersnarl:</strong>

1) Now displays album-cover/album-art of the current playing track in the
notification.

2) Now uses <b>Snarl C# API</b> to register application and to show notifications so dependency on <b>heysnarl.exe</b> removed.

3) It also includes a compile_NET3.5.bat file to compile the project after making some changes to it.


Author: Ranveer Raghuwanshi
Email: ranveer.raghu@gmail.com

