using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuiTools
{
    // https://stackoverflow.com/questions/17922308/use-latest-version-of-internet-explorer-in-the-webbrowser-control
    // This is a class will get the latest version of IE on windows and make changes as desired
    // Example use of class:
    // WebBrowserHelper.FixBrowserVersion();
    // WebBrowserHelper.FixBrowserVersion("SomeAppName");
    // WebBrowserHelper.FixBrowserVersion("SomeAppName", intIeVer);
    public class WebBrowserHelper
    {
        public static int GetEmbVersion()
        {
            int ieVer = GetBrowserVersion();

            if (ieVer > 9)
                return ieVer * 1000 + 1;

            if (ieVer > 7)
                return ieVer * 1111;

            return 7000;
        }

        public static void FixBrowserVersion()
        {
            string appName = System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location);
            FixBrowserVersion(appName);
        }

        public static void FixBrowserVersion(string appName)
        {
            FixBrowserVersion(appName, GetEmbVersion());
        }

        // FixBrowserVersion("<YourAppName>", 9000);
        public static void FixBrowserVersion(string appName, int ieVer)
        {
            FixBrowserVersion_Internal("HKEY_LOCAL_MACHINE", appName + ".exe", ieVer);
            FixBrowserVersion_Internal("HKEY_CURRENT_USER", appName + ".exe", ieVer);
            FixBrowserVersion_Internal("HKEY_LOCAL_MACHINE", appName + ".vshost.exe", ieVer);
            FixBrowserVersion_Internal("HKEY_CURRENT_USER", appName + ".vshost.exe", ieVer);
        }

        private static void FixBrowserVersion_Internal(string root, string appName, int ieVer)
        {
            try
            {
                //For 64 bit Machine 
                if (Environment.Is64BitOperatingSystem)
                    Microsoft.Win32.Registry.SetValue(root + @"\Software\Wow6432Node\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", appName, ieVer);
                else  //For 32 bit Machine 
                    Microsoft.Win32.Registry.SetValue(root + @"\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", appName, ieVer);
            }
            catch (Exception)
            {
                // some config will hit access rights exceptions
                // this is why we try with both LOCAL_MACHINE and CURRENT_USER
            }
        }

        public static int GetBrowserVersion()
        {
            // string strKeyPath = @"HKLM\SOFTWARE\Microsoft\Internet Explorer";
            string strKeyPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer";
            string[] ls = new string[] { "svcVersion", "svcUpdateVersion", "Version", "W2kVersion" };

            int maxVer = 0;
            for (int i = 0; i < ls.Length; ++i)
            {
                object objVal = Microsoft.Win32.Registry.GetValue(strKeyPath, ls[i], "0");
                string strVal = System.Convert.ToString(objVal);
                if (strVal != null)
                {
                    int iPos = strVal.IndexOf('.');
                    if (iPos > 0)
                        strVal = strVal.Substring(0, iPos);

                    int res = 0;
                    if (int.TryParse(strVal, out res))
                        maxVer = Math.Max(maxVer, res);
                }
            }

            return maxVer;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static int GetInstalledIEVersion(WebBrowser wb)
        {
            int browserVer;

            // get the installed IE version
            //using (WebBrowser Wb = new WebBrowser())
            browserVer = wb.Version.Major;
            return browserVer;
        }

        public static void UpdateWebBrowserRegistryKey(WebBrowser wb)
        {
            // get the installed IE version
            //using (WebBrowser Wb = new WebBrowser())
            int browserVer = GetInstalledIEVersion(wb);

            // set the appropriate IE version
            int regVal;
            if (browserVer >= 11)
                regVal = 11001;
            else if (browserVer == 10)
                regVal = 10001;
            else if (browserVer == 9)
                regVal = 9999;
            else if (browserVer == 8)
                regVal = 8888;
            else
                regVal = 7000;

            // set the actual key
            using (RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", RegistryKeyPermissionCheck.ReadWriteSubTree))
                if (Key.GetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe") == null)
                    Key.SetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", regVal, RegistryValueKind.DWord);
        }

        //--------------------------------------------------------------------------------------------------------------

    } // end of CLASS

} // end of NAMESPACE


/*
Key values:
11001 (0x2AF9) - Internet Explorer 11. Webpages are displayed in IE11 edge mode, regardless of the !DOCTYPE directive.
11000 (0x2AF8) - Internet Explorer 11. Webpages containing standards-based !DOCTYPE directives are displayed in IE11 edge mode.Default value for IE11.
10001 (0x2711)- Internet Explorer 10. Webpages are displayed in IE10 Standards mode, regardless of the !DOCTYPE directive.
10000 (0x2710)- Internet Explorer 10. Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode.Default value for Internet Explorer 10.
9999 (0x270F) - Internet Explorer 9. Webpages are displayed in IE9 Standards mode, regardless of the !DOCTYPE directive.
9000 (0x2328) - Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode.
8888 (0x22B8) - Webpages are displayed in IE8 Standards mode, regardless of the !DOCTYPE directive.
8000 (0x1F40) - Webpages containing standards-based !DOCTYPE directives are displayed in IE8 mode.
7000 (0x1B58) - Webpages containing standards-based !DOCTYPE directives are displayed in IE7 Standards mode.
*/
