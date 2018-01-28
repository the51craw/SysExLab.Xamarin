using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

//[assembly: Xamarin.Forms.Dependency(typeof(SysExLab.iOS.GenericHandlerInterface))]

namespace SysExLab.iOS
{
    //public class GenericHandlerInterface : IGenericHandler
    //{
    //    public MainPage mainPage { get; set; }

    //    public void GenericHandler(object sender, object e)
    //    {
    //        //if (mainPage.midi.midiOutPort == null)
    //        //{
    //        //    mainPage.midi.Init("INTEGRA-7");
    //        //}
    //        //mainPage.midi.ProgramChange(0, 88, 0, 1);
    //    }
    //}

    //public interface IGenericHandler
    //{
    //    void GenericHandler(object sender, object e);
    //}

    public class Application
    {
        //GenericHandlerInterface genericHandlerInterface;

        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }


        //private void Btn0_Click(object sender, RoutedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
