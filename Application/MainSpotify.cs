using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using ProcessInfo;
using Metadata;
using Notification;


class NameChangeTracker

{
        // Fields
        private const uint EVENT_OBJECT_NAMECHANGE = 0x800c;
        private const uint EVENT_OBJECT_CREATE = 0x00008000;
        private const uint WINEVENT_OUTOFCONTEXT = 0;

        public static string track = null;
        public static string artist = null;
        public static Notify notify = null;
        public static TrackMetadata TMD = null;
        public static ProcessInformation PSI = null;
        public static IntPtr hwnd_spotify;
        public static int processid;

        //Dll Imports

        [DllImport("user32")]
                internal static extern  bool GetMessage(ref Message lpMsg, IntPtr handle, uint mMsgFilterInMain, uint mMsgFilterMax);

        [DllImport("user32.dll")]
                private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, 
                                WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
                private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        // Methods

        public static void nameChanger()
        {
                hwnd_spotify = PSI.getSpotify();
                processid = PSI.getProcessId(hwnd_spotify);
                track = TMD.getTrack();
                artist  = TMD.getArtist();
        }

        public static void create_object()
        {
                PSI = new ProcessInformation();
                TMD = new TrackMetadata();
                notify = new Notify();

        }


        private static WinEventDelegate procDelegate = new WinEventDelegate(NameChangeTracker.WinEventProc);
        private static WinEventDelegate procDelegate_start = new WinEventDelegate(NameChangeTracker.WinEventProc_start);

        private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int 
                        idChild, uint dwEventThread, uint dwmsEventTime);


        private static void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint 
                        dwEventThread, uint dwmsEventTime)
        {

                if ((idObject == 0) && (idChild == 0))
                {
                        nameChanger();

                        if(track !=null || artist !=null)
                        {
                                notify.sendNotification(track,artist);
                                Console.WriteLine(track);
                                Console.WriteLine(artist);
                        }
                }
        }

        private static void WinEventProc_start(IntPtr hWinEventHook_start, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)

        {
                if(idObject==0 && idChild==0)
                {
                        nameChanger();
                        if(hwnd.ToInt32() == hwnd_spotify.ToInt32() )
                        {
                                Console.WriteLine("checking hwnd");
                                if(eventType == EVENT_OBJECT_CREATE)
                                {              
                                        Console.WriteLine("create event");
                                        create_object();
                                        IntPtr hWinEventHook = SetWinEventHook(0x0800c, 0x800c, IntPtr.Zero, procDelegate, Convert.ToUInt32(processid), 0, 0);
                                }
                        }
                }
        }


        public static void Main()
        {
                ProcessInformation PSI = new ProcessInformation();



                if(PSI.isAvailable())
                { 

                        NameChangeTracker.create_object();
                        NameChangeTracker.nameChanger();
                        IntPtr hWnd = NameChangeTracker.hwnd_spotify;
                        int pid = NameChangeTracker.processid;

                        IntPtr hWinEventHook = SetWinEventHook(0x0800c, 0x800c, IntPtr.Zero, procDelegate, Convert.ToUInt32(pid), 0, 0);
                        IntPtr hWinEventHook_start = SetWinEventHook(0x00008000,0x00008000,IntPtr.Zero, procDelegate_start, 0, 0, 0);
                        //MessageBox.Show("Tracking name changes on HWNDs, close message box to exit.");

                        Message msg = new Message();

                        while(GetMessage(ref msg,hWnd,0,0))
                        {}


                        // UnhookWinEvent(hWinEventHook);
                        // UnhookWinEvent(hWinEventHook_start);

                }

                else
                        MessageBox.Show("check spotify running or not");
        }
}

