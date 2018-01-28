using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SysExLab;
using SysExLab.UWP;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(GenericHandlerInterface))]

namespace SysExLab.UWP
{
    public class GenericHandlerInterface: IGenericHandler
    {
        public MainPage mainPage { get; set; }

        public void GenericHandler(object sender, object e)
        {
            if (mainPage.midi.midiOutPort == null)
            {
                mainPage.midi.Init("INTEGRA-7");
            }
            mainPage.midi.ProgramChange(0, 88, 0, 1);
        }
    }

    public sealed partial class MainPage
    {
        // For accessing SysExLab.MainPage from UWP:
        private SysExLab.MainPage mainPage;
        //public Int32 appType = 5;
        // Invisible comboboxes used by MIDI class (will always have INTEGRA-7 selected):
        private Picker OutputSelector;
        private Picker InputSelector;
        public MIDI midi;
        // For accessing the genericHandlerInterface:
        GenericHandlerInterface genericHandlerInterface;

        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new SysExLab.App());
            Init();
        }
        
        private void Init()
        {
            // Get SysExLab.MainPage:
            mainPage = SysExLab.MainPage.GetMainPage();
            UIHandler.appType = UIHandler._appType.UWP;
            // Get the generic handler (same way as done in SysExLab.UIHandler):
            genericHandlerInterface = (SysExLab.UWP.GenericHandlerInterface)DependencyService.Get<IGenericHandler>();
            // Let genericHandlerInterface know this MainPage:
            genericHandlerInterface.mainPage = this;
            // Draw UI (function is in mainPage.uIHandler):
            mainPage.uIHandler.DrawPage();

            // We need invisible ComboBoxes to hold settings from the
            // corresponding Pickers in the Xamarin code.
            OutputSelector = mainPage.uIHandler.midiOutputDevice;
            InputSelector = mainPage.uIHandler.midiInputDevice;
            midi = new MIDI(this, OutputSelector, InputSelector, Dispatcher, 0, 0);
            midi.Init("INTEGRA-7");
        }

        private void Btn0_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
