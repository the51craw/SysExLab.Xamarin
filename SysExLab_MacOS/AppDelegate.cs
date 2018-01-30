using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;
using AppKit;
using Foundation;

namespace SysExLab_MacOS
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        NSWindow mainPage;

        public AppDelegate()
        {
            var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;
            var rect = new CoreGraphics.CGRect(200, 1000, 1024, 768);
            mainPage = new NSWindow(rect, style, NSBackingStore.Buffered, false);
            mainPage.Title = "the title";
            mainPage.TitleVisibility = NSWindowTitleVisibility.Hidden;
        }

        public override NSWindow MainWindow
        {
            get { return mainPage; }
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
            Forms.Init();
            LoadApplication(new SysExLab.App());
            SysExLab.MainPage.GetMainPage().uIHandler.DrawMain();
            base.DidFinishLaunching(notification);
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
