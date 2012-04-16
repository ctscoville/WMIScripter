using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WMIScripter
{
    //---------------------------------------------------------------
    // This class is used to find the .NET framework installation
    // folder.
    //---------------------------------------------------------------
    [ComVisible(false)]
    public class NativeMethods
    {
        [DllImport("mscoree.dll")]
        static extern string GetCORSystemDirectory
        ([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder buffer, Int32
        buflen, ref Int32 numbytes);

        public static string SystemDirectory()
        {
            System.Text.StringBuilder buf = new System.Text.StringBuilder(1024);
            Int32 iBytes = 0;
            string ret = GetCORSystemDirectory(buf, buf.Capacity, ref iBytes);
            return buf.ToString().Substring(0, iBytes - 1);
        }

        private NativeMethods()
        {
            // Default Constructor
        }
    }
}
