using System;
using System.Collections.Generic;
using System.Linq;

//using UIKit;
using Xamarin.Forms;
using AppKit;
using Foundation;

namespace SysExLab_MacBook
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        private static SysExLab.MainPage mainPage;

        public AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            global::Xamarin.Forms.Forms.Init();
            //notification.
            //LoadApplication(new SysExLab.App());
            mainPage = new SysExLab.MainPage();

            // Get SysExLab.MainPage:
            //mainPage = SysExLab.MainPage.GetMainPage();

            //mainPage.uIHandler.
            //mainPage.uIHandler.appType = mainPage.uIHandler._appType.IOS;
            mainPage.uIHandler.DrawPage();
            
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
