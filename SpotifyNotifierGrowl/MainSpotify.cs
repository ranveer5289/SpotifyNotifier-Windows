/* Author: Ranveer Raghuwanshi
 * Email: ranveer.raghu@gmail.com
 * Stackoverflow: http://stackoverflow.com/users/776084/ranrag */

using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ProcessInfo;
using Metadata;
using Notification;

class NameChangeTracker

{
        // Initializing Fields
        private const uint EVENT_OBJECT_NAMECHANGE = 0x800c;
        private const uint EVENT_OBJECT_CREATE = 0x00008000;
        private const uint WINEVENT_OUTOFCONTEXT = 0;

        public static string track = null;
        public static string artist = null;
        public static string oldTrack = null;
        public static string oldArtist = null;
        public static Notify notify = null;
        public static TrackMetadata TMD = null;
        public static ProcessInformation PSI = null;
        public static IntPtr hwnd_spotify = IntPtr.Zero;
        public static int processid = 0;
        //Dll Imports

        [DllImport("user32")]
                internal static extern  bool GetMessage(ref Message lpMsg, IntPtr handle, uint mMsgFilterInMain, uint mMsgFilterMax);

        [DllImport("user32.dll")]
                private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, 
                                WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
                private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll")]
                public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.Dll")]
                static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // Methods

        //Constructor for NameChangeTracker
        public NameChangeTracker()
        {
                nameChanger();
        }

        public static void nameChanger()
        {
                hwnd_spotify = TMD.hWnd;
                processid = TMD.processid;
        }


        //get track and artist name.
        public static void get_Track_and_Artist()
        {
                track = TMD.getTrack();
                artist  = TMD.getArtist();
        }

        //get process information specifically when spotify gets restarted.
        public static void spotify_start_event()
        {
                hwnd_spotify = PSI.getSpotify();
                processid = PSI.getProcessId(hwnd_spotify);

        }

        //Create object.
        public static void create_object()
        {
                PSI = new ProcessInformation();
                TMD = new TrackMetadata();
                notify = new Notify();

                notify.register();
        }

        // Need to ensure delegate is not collected while we're using it,
        // storing it in a class field is simplest way to do this.

        private static WinEventDelegate procDelegate = new WinEventDelegate(NameChangeTracker.WinEventProc);
        private static WinEventDelegate procDelegate_start = new WinEventDelegate(NameChangeTracker.WinEventProc_start);

        private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int 
                        idChild, uint dwEventThread, uint dwmsEventTime);


        private static void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint 
                        dwEventThread, uint dwmsEventTime)
        {
                if ((idObject == 0) && (idChild == 0))

                {
                        //Method call to get current playing track and artist as
                        //soon as EVENT_OBJECT_NAMECHANGE occured.
                        get_Track_and_Artist();

                        //Making sure no internal name change event get caught.

                        if(hwnd.ToInt32() == hwnd_spotify.ToInt32())
                        {

                                if((!String.IsNullOrWhiteSpace(track) && track != oldTrack) || (!String.IsNullOrWhiteSpace(artist) && artist != oldArtist))
                                {
                                        notify.sendNotification(track,artist);
                                        Console.WriteLine(track);
                                        Console.WriteLine(artist);

                                        oldTrack = track;
                                        oldArtist = artist;
                                }
                        }
                }
        }

        private static void WinEventProc_start(IntPtr hWinEventHook_start, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)

        {
                if(idObject == 0 && idChild == 0)
                {
                        //Method call to get new processid and window
                        //handle(hwnd) for spotify after restarting it OR
                        //as soon as EVENT_OBJECT_CREATE occured.

                        spotify_start_event();

                        //specifically looking for spotify via hwnd_spotify
                        //across all process(hwnd).

                        if(hwnd.ToInt32() == hwnd_spotify.ToInt32() )
                        {
                                if(eventType == EVENT_OBJECT_CREATE)
                                {              
                                        create_object();
                                        IntPtr hWinEventHook = SetWinEventHook(0x0800c, 0x800c, IntPtr.Zero, procDelegate, Convert.ToUInt32(processid), 0, 0);
                                }
                        }
                }
        }


        public static void Main(string[] args)
        {
                Console.Title = "SpotifyNotifier";

                //Thanks to Matthew Javellana @ mmjavellana@gmail.com
                for( int i = 0; i < args.Length; i ++)
                {                
                    if(args[i].StartsWith("-w"))
                    {
                        IntPtr hWnd = FindWindow(null, Console.Title);

                        if(hWnd != IntPtr.Zero)
                        {
                            ShowWindow(hWnd, 0);
                        }
                    }
                }


                ProcessInformation PSI = new ProcessInformation();

                //Checking spotify available.
                if(PSI.isAvailable())
                { 
                        NameChangeTracker.create_object(); //Method call to create objects.
                        NameChangeTracker NCT = new NameChangeTracker();//constructor call;
                        IntPtr hWnd = NameChangeTracker.hwnd_spotify;
                        int pid = NameChangeTracker.processid;

                        // Listen for name change changes for spotify(check pid!=0).
                        IntPtr hWinEventHook = SetWinEventHook(0x0800c, 0x800c, IntPtr.Zero, procDelegate, Convert.ToUInt32(pid), 0, 0);
                        // Listen for create window event across all processes/threads on current desktop.(check pid=0)
                        IntPtr hWinEventHook_start = SetWinEventHook(0x00008000,0x00008000,IntPtr.Zero, procDelegate_start, 0, 0, 0);

                        Message msg = new Message();

                        //GetMessage provides the necessary mesage loop that SetWinEventHook requires.

                        while(GetMessage(ref msg,hWnd,0,0))
                        {
                                UnhookWinEvent(hWinEventHook);
                                UnhookWinEvent(hWinEventHook_start);
                        }

                }

                else
                        //Executed if spotify not running.
                        MessageBox.Show("check spotify running or not");
        }
}

