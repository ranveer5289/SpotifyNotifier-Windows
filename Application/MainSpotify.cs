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
        private const uint WINEVENT_OUTOFCONTEXT = 0;

        public static string track = null;
        public static string artist = null;
        public static Notify notify = null;
        public static TrackMetadata TMD = null;


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
                TMD = new TrackMetadata();
                track = TMD.getTrack();
                artist  = TMD.getArtist();
                notify = new Notify();
        }


        private static WinEventDelegate procDelegate = new WinEventDelegate(NameChangeTracker.WinEventProc);

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


        public static void Main()
        {
                ProcessInformation PSI = new ProcessInformation();


                if(PSI.isAvailable())
                {         
                        IntPtr hWnd = PSI.getSpotify();
                        int processid = PSI.getProcessId(hWnd);

                        IntPtr hWinEventHook = SetWinEventHook(0x800c, 0x800c, IntPtr.Zero, procDelegate, Convert.ToUInt32(processid), 0, 0);
                        //MessageBox.Show("Tracking name changes on HWNDs, close message box to exit.");

                        Message msg = new Message();

                        while(GetMessage(ref msg,hWnd,0,0))


                                UnhookWinEvent(hWinEventHook);
                }

                else
                        MessageBox.Show("check spotify running or not");
        }
}
