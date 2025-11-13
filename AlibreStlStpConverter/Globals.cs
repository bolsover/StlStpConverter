using System.Drawing;
using Microsoft.Win32;

namespace Bolsover.AlibreStlStpConverter
{
    public class Globals
    {
        public static readonly string InstallPath = (string) Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Alibre Design Add-Ons\",
            "{37882997-F122-5217-92E0-976ADD4F9AA8}", null);

        public static Icon Icon = new Icon(InstallPath + "\\3DPrint.ico");
        public static readonly string AppName = "Stl->Stp Converter ";

    }
}