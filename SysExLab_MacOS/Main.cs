using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
//using UIKit;
using AppKit;

namespace SysExLab_MacOS
{
    static class MainClass
    {
        static void Main(string[] args)
        {
            NSApplication.Init();
            NSApplication.Main(args/*, null, "AppDelegate"*/);
            //UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
