using Java.Util;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        IMidi midi;

        SysExLab.MainPage mainPage;
        StackLayout mainStackLayout { get; set; }
        public static _appType appType;
        _page page;
        public Picker midiOutputDevice { get; set; }
        public Picker midiInputDevice { get; set; }
        public LabeledPicker midiOutputDevicePicker { get; set; }
        public LabeledPicker midiInputDevicePicker { get; set; }
        public LabeledPicker midiOutputChannel { get; set; }
        public LabeledPicker midiInputChannel { get; set; }
        public LabeledPicker messageType { get; set; }
        public LabeledTextInput tbAddress { get; set; }
        public LabeledTextInput tbData { get; set; }
        public LabeledSwitch rcvKeepAlive { get; set; }
        public Button btnClear { get; set; }
        public Button btnSend { get; set; }
        public ListView lvReceivedList { get; set; }
        public ObservableCollection<String> receivedLines { get; set; }

        public UIHandler(StackLayout mainStackLayout, SysExLab.MainPage mainPage)
        {
            this.mainStackLayout = mainStackLayout;
            this.mainPage = mainPage;
            page = _page.MAIN;
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
             * SysEx message:|______________________________________|(auto-fill)              
             * |___Rcv keep alive____| |____Clear____| |____Send____| 
             */

            // Make pickers for MIDI:
            midiOutputDevice = new Picker();
            midiInputDevice = new Picker();
            midiOutputDevicePicker = new LabeledPicker("Midi out device:", midiOutputDevice, new byte[] { 1, 1 });
            midiInputDevicePicker = new LabeledPicker("Midi in device:", midiInputDevice, new byte[] { 1, 1 });
            midiOutputChannel = new LabeledPicker("Midi out channel:");
            midiInputDevicePicker.Picker.SelectedIndexChanged += InputPicker_SelectedIndexChanged;
            midiOutputDevicePicker.Picker.SelectedIndexChanged += OutputPicker_SelectedIndexChanged;
            for (Int32 i = 0; i < 16; i++)
            {
                String temp = "Part " + (i + 1).ToString();
                midiOutputChannel.Picker.Items.Add(temp);
            }
            midiOutputChannel.Picker.SelectedIndex = 0;
            midiInputChannel = new LabeledPicker("Midi in channel:");
            for (Int32 i = 0; i < 16; i++)
            {
                String temp = "Part " + (i + 1).ToString();
                midiInputChannel.Picker.Items.Add(temp);
            }
            midiInputChannel.Picker.SelectedIndex = 0;

            // Make a picker to select DT1 or RQ1 message type:
            messageType = new LabeledPicker("MessageType:");
            messageType.Picker.Items.Add("DT1");
            messageType.Picker.Items.Add("RQ1");
            messageType.Picker.SelectedIndex = 0;
            Label messageTypeHelp = new Label();
            messageTypeHelp.Text = "DT1 just sends SysEx data. DQ1 sends SysEx request and answer will be displayed in list below.";
            messageTypeHelp.HorizontalOptions = LayoutOptions.Start;
            messageType.Picker.SelectedIndexChanged += MessageTypePicker_SelectedIndexChanged;

            // Make input fields for address and data:
            tbAddress = new LabeledTextInput("Address (4 2-bytes hex space separated):", "00 00 00 00");
            tbData = new LabeledTextInput("Length (2-bytes hex space separated):", "");

            // Make buttons for reveive keep alive, clear and send:
            rcvKeepAlive = new LabeledSwitch("Receive keep alive:");
            btnClear = new Button();
            btnClear.Text = "Clear list";
            btnClear.Clicked += BtnClear_Clicked;
            btnSend = new Button();
            btnSend.Text = "Send";
            btnSend.Clicked += BtnSend_Clicked;

            // Make a listview for displaying received messages:
            lvReceivedList = new ListView();
            receivedLines = new ObservableCollection<String>();
            lvReceivedList.ItemsSource = receivedLines;

            // Assemble mainStackLayout:
            mainStackLayout.Children.Add((new GridRow(0, new View[] { midiOutputDevicePicker, midiOutputChannel }, new byte[] { 1, 1 })).Row);
            mainStackLayout.Children.Add((new GridRow(1, new View[] { midiInputDevicePicker, midiInputChannel }, new byte[] { 1, 1 })).Row);
            mainStackLayout.Children.Add((new GridRow(2, new View[] { messageType, messageTypeHelp }, new byte[] { 1, 1 })).Row);
            mainStackLayout.Children.Add((new GridRow(3, new View[] { rcvKeepAlive, btnClear, btnSend }, new byte[] { 1, 1, 1 })).Row);
            mainStackLayout.Children.Add((new GridRow(4, new View[] { tbAddress })).Row);
            mainStackLayout.Children.Add((new GridRow(5, new View[] { tbData })).Row);
            mainStackLayout.Children.Add((new GridRow(6, new View[] { lvReceivedList })).Row);

            midi = DependencyService.Get<IMidi>();
            midi.Init("INTEGRA-7", mainPage);
            //midi.Init("INTEGRA-7", mainPage, midiOutputDevicePicker.Picker, midiInputDevicePicker.Picker,
            //    (byte)midiOutputChannel.Picker.SelectedIndex, (byte)midiInputChannel.Picker.SelectedIndex);
        }

        private void MessageTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (messageType.Picker.SelectedIndex == 0)
            {
                tbData.Label.Text = "Data (2-bytes hex space separated)";
            }
            else
            {
                tbData.Label.Text = "Length (4 2-bytes hex space separated)";
            }
        }

        private void InputPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            midi.InputDeviceChanged((Picker)sender);
        }

        private void OutputPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            midi.OutputDeviceChanged((Picker)sender);
        }

        private void BtnSend_Clicked(object sender, EventArgs e)
        {
            String[] addressStrings = tbAddress.Editor.Text.Trim().Split(' ');
            Boolean error = false;
            if (addressStrings.Length != 4)
            {
                receivedLines.Add("\r\n\r\n" + "Address error! Format should be: \'nn nn nn nn\'\r\nwhere nn are single space separated hex numbers < 80.");
            }
            byte[] address = new byte[4];
            for (Int32 i = 0; i < 4 && !error; i++)
            {
                byte hex = StringToHex(addressStrings[i]);
                if (hex == 0xff)
                {
                    receivedLines.Add("\r\n\r\n" + "Address error! Format should be: \'nn nn nn nn\'\r\nwhere nn are single space separated hex numbers < 80.");
                    error = true;
                }
                address[i] = hex;
            }
            if (error)
            {
                return;
            }
            String[] dataStrings = tbData.Editor.Text.Trim().Split(' ');
            if (dataStrings.Length < 1)
            {
                receivedLines.Add("\r\n\r\n" + "Data error! Format should be: \'nn ...\'\r\nwhere nn are single space separated hex numbers < 80.");
            }
            byte[] data = new byte[dataStrings.Length];

            for (Int32 i = 0; i < dataStrings.Length && !error; i++)
            {
                byte hex = StringToHex(dataStrings[i]);
                if (hex == 0xff)
                {
                    receivedLines.Add("\r\n\r\n" + "Data error! Format should be: \'nn ...\'\r\nwhere nn are single space separated hex numbers < 80.");
                    error = true;
                }
                data[i] = hex;
            }
            byte[] bytes = null;
            if (messageType.Picker.SelectedIndex == 0)
            {
                bytes = midi.SystemExclusiveDT1Message(address, data);
            }
            else
            {
                if (data.Length != 4 || data[0] != 0 || data[1] != 0 || data[2] > 1 || data[3] > 0x7f)
                {
                    error = true;
                    receivedLines.Add("\r\n\r\n" + "Length error! Format should be: \'00 00 00 nn\' or \'00 00 01 nn\' \r\nwhere nn is a hex value < 80.");
                }
                else
                {
                    bytes = midi.SystemExclusiveRQ1Message(address, data);
                }
            }
            if (!error)
            {
                midi.SendSystemExclusive(bytes);
            }
        }

        private void BtnClear_Clicked(object sender, EventArgs e)
        {
            receivedLines.Clear();
        }

        private String ToHex(byte data)
        {
            String[] chars = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };
            byte msb = (byte)((data & 0xf0) >> 4);
            byte lsb = (byte)(data & 0x0f);
            return "0x" + chars[msb] + chars[lsb] + " ";
        }

        private byte StringToHex(String s)
        {
            String chars = "0123456789abcdef";
            if (s.Length != 2)
            {
                return 0xff;
            }
            else
            {
                s = s.ToLower();
                String s1 = s.Remove(1);
                String s2 = s.Remove(0, 1);
                if (!chars.Contains(s1) || !chars.Contains(s2))
                {
                    return 0xff;
                }
                return (byte)(chars.IndexOf(s1) * 16 + chars.IndexOf(s2));
            }
        }
    }
}
