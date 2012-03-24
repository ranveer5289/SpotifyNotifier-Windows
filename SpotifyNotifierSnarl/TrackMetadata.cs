using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ProcessInfo;

namespace Metadata
{
        //DLL Imports.
        internal class DLL_Methods
        {
                [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
                        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

                [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
                        internal static extern int GetWindowTextLength(IntPtr hWnd);
        }


        class TrackMetadata
        {
                //Initializing Fields
                public ProcessInformation PSI = null ;
                public IntPtr hWnd  = IntPtr.Zero;
                public int processid = 0;

                //Constructor
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
                        DLL_Methods.GetWindowText(hWnd, sb, sb.Capacity);

                        return sb.ToString().Replace("Spotify", "").TrimStart(' ', '-').Trim();
                }

                //Returns track info in an array.
                public string[] getCurrentTrackInfo()
                {
                        string[] strArray = null;
                        string currentTrack = null;
                        currentTrack = GetCurrentTrack();

                        if (!string.IsNullOrEmpty(currentTrack))
                        {

                                strArray = currentTrack.Split('\u2013');
                        }
                        else
                                return null;

                        return strArray;
                }

                //Returns track/song name.
                public string getTrack()
                {
                        if (getCurrentTrackInfo() == null || getCurrentTrackInfo().Length == 0)
                                return null;
                        else
                                return getCurrentTrackInfo()[1].Trim();

                }

                //Returns artist name.
                public string getArtist()
                {
                        if (getCurrentTrackInfo() == null || getCurrentTrackInfo().Length==0)
                                return null;
                        else
                                return getCurrentTrackInfo()[0].Trim();
                }

        }

}
