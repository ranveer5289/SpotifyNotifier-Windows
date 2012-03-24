using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ProcessInfo
{
        //Dll Imports.
        internal class DLL_Methods
        {
                [DllImport("user32.dll", SetLastError=true)]
                        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

                [DllImport("user32")]
                        internal static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId);
        }

        class ProcessInformation
        {
                //Returns handle to the spotify window.
                public IntPtr getSpotify()
                {
                        return DLL_Methods.FindWindow("SpotifyMainWindow", null);
                }

                //Returns the PID for spotify application.
                public int getProcessId(IntPtr hwnd)
                {
                        int processId = 0;
                        int windowThreadProcessId = DLL_Methods.GetWindowThreadProcessId(hwnd, out processId);
                        return processId;
                }

                //Check spotify running or not.
                public  bool isAvailable()
                {
                        return (getSpotify() != IntPtr.Zero);
                }

        }
}
