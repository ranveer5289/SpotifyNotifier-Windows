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

namespace ProcessInfo
{
        internal class DLL_Methods
        {
                [DllImport("user32.dll", SetLastError=true)]
                        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

                [DllImport("user32")]
                        internal static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId);
        }
        class ProcessInformation
        {
                public IntPtr getSpotify()
                {
                        return DLL_Methods.FindWindow("SpotifyMainWindow", null);
                }

                public int getProcessId(IntPtr hwnd)
                {
                        int processId = 0;
                        int windowThreadProcessId = DLL_Methods.GetWindowThreadProcessId(hwnd, out processId);
                        return processId;
                }

        }
}
