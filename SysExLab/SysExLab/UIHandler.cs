using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SysExLab
{
    public class UIHandler
    {
        public enum _appType
        {
            UWP,
            IOS,
            MacOS,
            ANDROID,
        }

        enum _page
        {
            MAIN,
            SEARCH_RESULTS,
            FAVORITES,
            EDIT,
        }

        StackLayout mainStackLayout { get; set; }
        public static Button btn0, btn1;
        public static _appType appType;
        _page page;
        public Picker midiOutputDevice { get; set; }
        public Picker midiInputDevice { get; set; }
        public LabeledPicker midiOutputDevicePicker { get; set; }
        public LabeledPicker midiInputDevicePicker { get; set; }
        public LabeledPicker midiOutputChannel { get; set; }
        public LabeledPicker midiInputChannel { get; set; }

        LabeledTextInput tbSearch;
        LabeledText ltToneName;
        LabeledText ltType;
        LabeledText ltToneNumber;
        LabeledText ltBankNumber;
        LabeledText ltBankMSB;
        LabeledText ltBankLSB;
        LabeledText ltProgramNumber;

        public UIHandler(StackLayout mainStackLayout)
        {
            this.mainStackLayout = mainStackLayout;
            page = _page.MAIN;
            //genericHandler = DependencyService.Get<IGenericHandler>();
        }

        public void Clear()
        {
            while (mainStackLayout.Children.Count() > 0)
            {
                mainStackLayout.Children.RemoveAt(0);
            }
        }

        public void DrawPage()
        {
            Clear();
            switch (page)
            {
                case _page.MAIN:
                    DrawMain();
                    break;
            }
        }

        public void DrawMain()
        {
            /*                ___________                    _______
             * MIDI out port:|_________|v| MIDI out channel:|_____|v|
             *  MIDI in port:|_________|v|  MIDI in channel:|_____|v|
             *  o DT1 o RQ1   ___________
             *    Company Id:|_________|v|__________________________
             *       Headers:|______________________________________|
             *       Address:|______________________________________|
             *          Data:|______________________________________|(only for DT1)
             *        Length:|______________________________________|(auto-fill, only for RQ1)
             *      Checksum:|______________________________________|(auto-fill)               __________
             * SysEx message:|______________________________________|(auto-fill)              |___Send___|
             */

            // Make pickers for MIDI:
            midiOutputDevice = new Picker();
            midiInputDevice = new Picker();
            midiOutputDevicePicker = new LabeledPicker("Midi out device:", midiOutputDevice, new byte[] { 1, 1 });
            midiInputDevicePicker = new LabeledPicker("Midi in device:", midiInputDevice, new byte[] { 1, 1 });
            midiOutputChannel = new LabeledPicker("Midi out channel:");
            midiOutputDevicePicker.Picker.SelectedIndexChanged += Picker_SelectedIndexChanged;
            for (Int32 i = 0; i < 16; i++)
            {
                String temp = "Part " + (i + 1).ToString();
                midiOutputChannel.Picker.Items.Add(temp);
            }
            midiInputChannel = new LabeledPicker("Midi in channel:");
            for (Int32 i = 0; i < 16; i++)
            {
                String temp = "Part " + (i + 1).ToString();
                midiInputChannel.Picker.Items.Add(temp);
            }

            //// Make a listview lvGroups for column 0:
            //ListView lvGroups = new ListView();

            //// Make a listview lvCategories for column 1:
            //ListView lvCategories = new ListView();

            //// Make a Grid for column 2:
            //Grid gridGroups = new Grid();

            //// Make a filter button for column 2:
            //Button filterPresetAndUser = new Button();
            //filterPresetAndUser.Text = "Preset and User";

            //// Make a listview lvToneNames for column 2:
            //ListView lvToneNames = new ListView();

            //// Assemble column 2:
            //gridGroups.Children.Add((new GridRow(0, new View[] { filterPresetAndUser })).Row);
            //gridGroups.Children.Add((new GridRow(1, new View[] { lvToneNames })).Row);

            //// Make a Grid for column 3:
            //Grid gridToneData = new Grid();

            //// Make labeled editor fields:
            //tbSearch = new LabeledTextInput("Search:", new byte[] { 1, 2 });
            //ltToneName = new LabeledText("Tone Name:", "Full Grand 1", new byte[] { 1, 2 });
            //ltType = new LabeledText("Type:", "(Preset)", new byte[] { 1, 2 });
            //ltToneNumber = new LabeledText("Tone #:", "1", new byte[] { 1, 2 });
            //ltBankNumber = new LabeledText("Bank #:", "11456", new byte[] { 1, 2 });
            //ltBankMSB = new LabeledText("Bank MSB:", "89", new byte[] { 1, 2 });
            //ltBankLSB = new LabeledText("Bank LSB:", "64", new byte[] { 1, 2 });
            //ltProgramNumber = new LabeledText("Program #:", "1", new byte[] { 1, 2 });

            //// Assemble column 3:
            //gridToneData.Children.Add((new GridRow(0, new View[] { midiOutputDevice, midiInputDevice, midiOutputChannel, midiInputChannel }, new byte[] { 255, 1, 255, 1 })).Row);
            //gridToneData.Children.Add((new GridRow(1, new View[] { tbSearch })).Row);
            //gridToneData.Children.Add((new GridRow(2, new View[] { ltToneName })).Row);
            //gridToneData.Children.Add((new GridRow(3, new View[] { ltType })).Row);
            //gridToneData.Children.Add((new GridRow(4, new View[] { ltToneNumber })).Row);
            //gridToneData.Children.Add((new GridRow(5, new View[] { ltBankNumber })).Row);
            //gridToneData.Children.Add((new GridRow(6, new View[] { ltBankMSB })).Row);
            //gridToneData.Children.Add((new GridRow(7, new View[] { ltBankLSB })).Row);
            //gridToneData.Children.Add((new GridRow(8, new View[] { ltProgramNumber })).Row);

            // Assemble mainStackLayout:
            //mainStackLayout.Children.Add((new GridRow(0, new View[] { lvGroups, lvCategories, gridGroups, gridToneData }, new byte[] { 1, 1, 1, 1 })).Row);
            mainStackLayout.Children.Add((new GridRow(0, new View[] { midiOutputDevicePicker, midiOutputChannel }, new byte[] { 1, 1 })).Row);
            mainStackLayout.Children.Add((new GridRow(1, new View[] { midiInputDevicePicker, midiInputChannel }, new byte[] { 1, 1 })).Row);
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ltToneName.Text.Text = (String)((Picker)(sender)).SelectedItem;
        }

        public void Btn0_Clicked(object sender, EventArgs e)
        {
            //genericHandler.GenericHandler(sender, e);
        }
    }
}
