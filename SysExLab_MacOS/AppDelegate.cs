using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
//using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;
using AppKit;

namespace SysExLab_MacOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : NSApplicationDelegate
    {
        // For accessing SysExLab.MainPage from IOS:
        private static SysExLab.MainPage mainPage;
        private Picker OutputSelector;
        private Picker InputSelector;
        //public MIDI midi;

        public AppDelegate()
        {
        }

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
            global::Xamarin.Forms.Forms.Init();
            Xamarin.Forms.Platform.MacOS.PageRenderer.
            SysExLab.MainPage. LoadApplication(new SysExLab.App());
            //mainPage = new SysExLab.MainPage();

            // Get SysExLab.MainPage:
            mainPage = SysExLab.MainPage.GetMainPage();
            SysExLab.UIHandler.appType = SysExLab.UIHandler._appType.MacOS;
            mainPage.uIHandler.DrawPage();

            // We need invisible ComboBoxes to hold settings from the
            // corresponding Pickers in the Xamarin code.
            //OutputSelector = mainPage.uIHandler.midiOutputDevice;
            //InputSelector = mainPage.uIHandler.midiInputDevice;
            //midi = new MIDI(mainPage, OutputSelector, InputSelector, /*Dispatcher,*/ 0, 0);
            //midi.Init("INTEGRA-7");

            //return base.FinishedLaunching(app, options);
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
