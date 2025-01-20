﻿using System.Runtime.InteropServices;


namespace WinDirector.Processes
{
    public static class Service
    {
        
    }
}
namespace WinDirector
{
    public partial class WinApi
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, out Rectangle pvParam, uint fWinIni);
    }

    public enum SPI
    {
        GETBEEP = 0x0001,
        SETBEEP = 0x0002,
        GETMOUSE = 0x0003,
        SETMOUSE = 0x0004,
        GETBORDER = 0x0005,
        SETBORDER = 0x0006,
        GETKEYBOARDSPEED = 0x000A,
        SETKEYBOARDSPEED = 0x000B,
        LANGDRIVER = 0x000C,
        ICONHORIZONTALSPACING = 0x000D,
        GETSCREENSAVETIMEOUT = 0x000E,
        SETSCREENSAVETIMEOUT = 0x000F,
        GETSCREENSAVEACTIVE = 0x0010,
        SETSCREENSAVEACTIVE = 0x0011,
        GETGRIDGRANULARITY = 0x0012,
        SETGRIDGRANULARITY = 0x0013,
        SETDESKWALLPAPER = 0x0014,
        SETDESKPATTERN = 0x0015,
        GETKEYBOARDDELAY = 0x0016,
        SETKEYBOARDDELAY = 0x0017,
        ICONVERTICALSPACING = 0x0018,
        GETICONTITLEWRAP = 0x0019,
        SETICONTITLEWRAP = 0x001A,
        GETMENUDROPALIGNMENT = 0x001B,
        SETMENUDROPALIGNMENT = 0x001C,
        SETDOUBLECLKWIDTH = 0x001D,
        SETDOUBLECLKHEIGHT = 0x001E,
        GETICONTITLELOGFONT = 0x001F,
        SETDOUBLECLICKTIME = 0x0020,
        SETMOUSEBUTTONSWAP = 0x0021,
        SETICONTITLELOGFONT = 0x0022,
        GETFASTTASKSWITCH = 0x0023,
        SETFASTTASKSWITCH = 0x0024,
        SETDRAGFULLWINDOWS = 0x0025,
        GETDRAGFULLWINDOWS = 0x0026,
        GETNONCLIENTMETRICS = 0x0029,
        SETNONCLIENTMETRICS = 0x002A,
        GETMINIMIZEDMETRICS = 0x002B,
        SETMINIMIZEDMETRICS = 0x002C,
        GETICONMETRICS = 0x002D,
        SETICONMETRICS = 0x002E,
        SETWORKAREA = 0x002F,
        GETWORKAREA = 0x0030,
        SETPENWINDOWS = 0x0031,
        GETHIGHCONTRAST = 0x0042,
        SETHIGHCONTRAST = 0x0043,
        GETKEYBOARDPREF = 0x0044,
        SETKEYBOARDPREF = 0x0045,
        GETSCREENREADER = 0x0046,
        SETSCREENREADER = 0x0047,
        GETANIMATION = 0x0048,
        SETANIMATION = 0x0049,
        GETFONTSMOOTHING = 0x004A,
        SETFONTSMOOTHING = 0x004B,
        SETDRAGWIDTH = 0x004C,
        SETDRAGHEIGHT = 0x004D,
        SETHANDHELD = 0x004E,
        GETLOWPOWERTIMEOUT = 0x004F,
        GETPOWEROFFTIMEOUT = 0x0050,
        SETLOWPOWERTIMEOUT = 0x0051,
        SETPOWEROFFTIMEOUT = 0x0052,
        GETLOWPOWERACTIVE = 0x0053,
        GETPOWEROFFACTIVE = 0x0054,
        SETLOWPOWERACTIVE = 0x0055,
        SETPOWEROFFACTIVE = 0x0056,
        SETCURSORS = 0x0057,
        SETICONS = 0x0058,
        GETDEFAULTINPUTLANG = 0x0059,
        SETDEFAULTINPUTLANG = 0x005A,
        SETLANGTOGGLE = 0x005B,
        GETWINDOWSEXTENSION = 0x005C,
        SETMOUSETRAILS = 0x005D,
        GETMOUSETRAILS = 0x005E,
        SETSCREENSAVERRUNNING = 0x0061,
        GETFILTERKEYS = 0x0032,
        SETFILTERKEYS = 0x0033,
        GETTOGGLEKEYS = 0x0034,
        SETTOGGLEKEYS = 0x0035,
        GETMOUSEKEYS = 0x0036,
        SETMOUSEKEYS = 0x0037,
        GETSHOWSOUNDS = 0x0038,
        SETSHOWSOUNDS = 0x0039,
        GETSTICKYKEYS = 0x003A,
        SETSTICKYKEYS = 0x003B,
        GETACCESSTIMEOUT = 0x003C,
        SETACCESSTIMEOUT = 0x003D,
        GETSERIALKEYS = 0x003E,
        SETSERIALKEYS = 0x003F,
        GETSOUNDSENTRY = 0x0040,
        SETSOUNDSENTRY = 0x0041,
        GETSNAPTODEFBUTTON = 0x005F,
        SETSNAPTODEFBUTTON = 0x0060,
        GETMOUSEHOVERWIDTH = 0x0062,
        SETMOUSEHOVERWIDTH = 0x0063,
        GETMOUSEHOVERHEIGHT = 0x0064,
        SETMOUSEHOVERHEIGHT = 0x0065,
        GETMOUSEHOVERTIME = 0x0066,
        SETMOUSEHOVERTIME = 0x0067,
        GETWHEELSCROLLLINES = 0x0068,
        SETWHEELSCROLLLINES = 0x0069,
        GETMENUSHOWDELAY = 0x006A,
        SETMENUSHOWDELAY = 0x006B,
        GETSHOWIMEUI = 0x006E,
        SETSHOWIMEUI = 0x006F,
        GETMOUSESPEED = 0x0070,
        SETMOUSESPEED = 0x0071,
        GETSCREENSAVERRUNNING = 0x0072,
        GETDESKWALLPAPER = 0x0073,
        GETACTIVEWINDOWTRACKING = 0x1000,
        SETACTIVEWINDOWTRACKING = 0x1001,
        GETMENUANIMATION = 0x1002,
        SETMENUANIMATION = 0x1003,
        GETCOMBOBOXANIMATION = 0x1004,
        SETCOMBOBOXANIMATION = 0x1005,
        GETLISTBOXSMOOTHSCROLLING = 0x1006,
        SETLISTBOXSMOOTHSCROLLING = 0x1007,
        GETGRADIENTCAPTIONS = 0x1008,
        SETGRADIENTCAPTIONS = 0x1009,
        GETKEYBOARDCUES = 0x100A,
        SETKEYBOARDCUES = 0x100B,
        GETACTIVEWNDTRKZORDER = 0x100C,
        SETACTIVEWNDTRKZORDER = 0x100D,
        GETHOTTRACKING = 0x100E,
        SETHOTTRACKING = 0x100F,
        GETMENUFADE = 0x1012,
        SETMENUFADE = 0x1013,
        GETSELECTIONFADE = 0x1014,
        SETSELECTIONFADE = 0x1015,
        GETTOOLTIPANIMATION = 0x1016,
        SETTOOLTIPANIMATION = 0x1017,
        GETTOOLTIPFADE = 0x1018,
        SETTOOLTIPFADE = 0x1019,
        GETCURSORSHADOW = 0x101A,
        SETCURSORSHADOW = 0x101B,
        GETMOUSESONAR = 0x101C,
        SETMOUSESONAR = 0x101D,
        GETMOUSECLICKLOCK = 0x101E,
        SETMOUSECLICKLOCK = 0x101F,
        GETMOUSEVANISH = 0x1020,
        SETMOUSEVANISH = 0x1021,
        GETFLATMENU = 0x1022,
        SETFLATMENU = 0x1023,
        GETDROPSHADOW = 0x1024,
        SETDROPSHADOW = 0x1025,
        GETBLOCKSENDINPUTRESETS = 0x1026,
        SETBLOCKSENDINPUTRESETS = 0x1027,
        GETUIEFFECTS = 0x103E,
        SETUIEFFECTS = 0x103F,
        GETFOREGROUNDLOCKTIMEOUT = 0x2000,
        SETFOREGROUNDLOCKTIMEOUT = 0x2001,
        GETACTIVEWNDTRKTIMEOUT = 0x2002,
        SETACTIVEWNDTRKTIMEOUT = 0x2003,
        GETFOREGROUNDFLASHCOUNT = 0x2004,
        SETFOREGROUNDFLASHCOUNT = 0x2005,
        GETCARETWIDTH = 0x2006,
        SETCARETWIDTH = 0x2007,
        GETMOUSECLICKLOCKTIME = 0x2008,
        SETMOUSECLICKLOCKTIME = 0x2009,
        GETFONTSMOOTHINGTYPE = 0x200A,
        SETFONTSMOOTHINGTYPE = 0x200B,
        GETFONTSMOOTHINGCONTRAST = 0x200C,
        SETFONTSMOOTHINGCONTRAST = 0x200D,
        GETFOCUSBORDERWIDTH = 0x200E,
        SETFOCUSBORDERWIDTH = 0x200F,
        GETFOCUSBORDERHEIGHT = 0x2010,
        SETFOCUSBORDERHEIGHT = 0x2011,
        GETFONTSMOOTHINGORIENTATION = 0x2012,
        SETFONTSMOOTHINGORIENTATION = 0x2013,
    }
}