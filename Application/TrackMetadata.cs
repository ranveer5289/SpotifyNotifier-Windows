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

namespace Metadata
{
        internal class DLL_Methods
        {
                [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
                        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

                [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
                        internal static extern int GetWindowTextLength(IntPtr hWnd);
        }

        class TrackMetadata
        {
                public ProcessInformation PSI ;
                public IntPtr hWnd ;
                public int processid;

                public TrackMetadata()
                {
                        PSI = new ProcessInformation();
                        hWnd = PSI.getSpotify();
                        processid = PSI.getProcessId(hWnd);
                }

                public string GetCurrentTrack()
                {
                        //Retrieves the length, in characters, of the specified window's title bar text 
                        int length = DLL_Methods.GetWindowTextLength(hWnd);
                        StringBuilder sb = new StringBuilder(length + 1);
                        //Copies the text of the specified window's title bar (if it has one) into a buffer
                        //If the target window is owned by the current process, GetWindowText causes a (WM_GETTEXT) message to be sent to the specified window or control
                        DLL_Methods.GetWindowText(hWnd, sb, sb.Capacity);
                        return sb.ToString().Replace("Spotify", "").TrimStart(' ', '-').Trim();
                }

                public string[] getCurrentTrackInfo()
                {
                        string[] strArray;
                        string currentTrack = GetCurrentTrack();

                        if (!string.IsNullOrEmpty(currentTrack))
                        {

                                strArray = currentTrack.Split('\u2013');
                        }
                        else
                                return null;

                        return strArray;
                }


                public string getTrack()
                {
                        if (getCurrentTrackInfo() == null)
                                return null;
                        else
                                return getCurrentTrackInfo()[1].Trim();

                }

                public string getArtist()
                {
                        if (getCurrentTrackInfo() == null)
                                return null;
                        else
                                return getCurrentTrackInfo()[0].Trim();
                }







        }

}
