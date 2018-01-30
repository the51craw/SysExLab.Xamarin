using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Xamarin.Forms;
using CoreMidi;

namespace SysExLab_MacOS
{
    public class MIDI
    {
        //public MidiDeviceWatcher midiOutputDeviceWatcher;
        //public MidiDeviceWatcher midiInputDeviceWatcher;
        //public MidiOutPort midiOutPort;
        //public MidiInPort midiInPort;
        public MidiDevice midiOutPort;
        public MidiDevice midiInPort;
        public byte MidiOutPortChannel { get; set; }
        public byte MidiInPortChannel { get; set; }
        public Int32 MidiOutPortSelectedIndex { get; set; }
        public Int32 MidiInPortSelectedIndex { get; set; }
        Picker OutputDeviceSelector;
        Picker InputDeviceSelector;
        List<String> deviceNames;

        // Constructor using a combobox for full device watch:
        public MIDI(SysExLab.MainPage mainPage, Picker OutputDeviceSelector, Picker InputDeviceSelector, /*CoreDispatcher Dispatcher,*/ byte MidiOutPortChannel, byte MidiInPortChannel)
        {
            //midiOutputDeviceWatcher = new MidiDeviceWatcher(MidiOutPort.GetDeviceSelector(), OutputDeviceSelector, Dispatcher);
            //midiInputDeviceWatcher = new MidiDeviceWatcher(MidiInPort.GetDeviceSelector(), InputDeviceSelector, Dispatcher);
            //midiOutputDeviceWatcher.StartWatcher();
            //midiInputDeviceWatcher.StartWatcher();
            this.OutputDeviceSelector = OutputDeviceSelector;
            this.InputDeviceSelector = InputDeviceSelector;
            this.MidiOutPortChannel = MidiOutPortChannel;
            this.MidiInPortChannel = MidiInPortChannel;
            deviceNames = new List<String>();

            //for (Int32 i = 0; i < Midi.ExternalDeviceCount; i++)
            for (Int32 i = 0; i < Midi.DeviceCount; i++)
            {
                //MidiDevice externalDevice = Midi.GetExternalDevice(i);
                MidiDevice externalDevice = Midi.GetDevice(i);
                deviceNames.Add(externalDevice.Name);
            }
            mainPage.uIHandler.midiOutputDevice.ItemsSource = deviceNames.ToArray();
        }

        // Simpleconstructor that takes the name of the device:
        public MIDI(String deviceName)
        {
            //Init(deviceName);
        }

        ~MIDI()
        {
            try
            {
                //midiOutputDeviceWatcher.StopWatcher();
                //midiInputDeviceWatcher.StopWatcher();
                midiOutPort.Dispose();
                //midiInPort.MessageReceived -= MidiInPort_MessageReceived;
                midiInPort.Dispose();
                midiOutPort = null;
                midiInPort = null;
            } catch { }
        }

        public void Init(String deviceName)
        {
            for (Int32 i = 0; i < Midi.DeviceCount; i++)
            {
                MidiDevice md = Midi.GetDevice(i);
                //OutputDeviceSelector.Items.Add(md.DisplayName);
                //InputDeviceSelector.Items.Add(md.DisplayName);
            }
        //    //DeviceInformationCollection midiOutputDevices = await DeviceInformation.FindAllAsync(MidiOutPort.GetDeviceSelector());
        //    //DeviceInformationCollection midiInputDevices = await DeviceInformation.FindAllAsync(MidiInPort.GetDeviceSelector());
        //    //DeviceInformation midiOutDevInfo = null;
        //    //DeviceInformation midiInDevInfo = null;

        //    foreach (DeviceInformation device in midiOutputDevices)
        //    {
        //        if (device.Name.Contains(deviceName))
        //        {
        //            midiOutDevInfo = device;
        //            break;
        //        }
        //    }

        //    if (midiOutDevInfo != null)
        //    {
        //        midiOutPort = (MidiOutPort)await MidiOutPort.FromIdAsync(midiOutDevInfo.Id);
        //    }

        //    foreach (DeviceInformation device in midiInputDevices)
        //    {
        //        if (device.Name.Contains(deviceName))
        //        {
        //            midiInDevInfo = device;
        //            break;
        //        }
        //    }

        //    if (midiInDevInfo != null)
        //    {
        //        midiInPort = await MidiInPort.FromIdAsync(midiInDevInfo.Id);
        //    }

        //    if (midiOutPort == null)
        //    {
        //        System.Diagnostics.Debug.WriteLine("Unable to create MidiOutPort from output device");
        //    }

        //    if (midiInPort == null)
        //    {
        //        System.Diagnostics.Debug.WriteLine("Unable to create MidiInPort from output device");
        //    }
        }

        //public void UpdateMidiComboBoxes(Picker midiOutputComboBox, Picker midiInputComboBox)
        //{
        //    midiOutputDeviceWatcher.UpdateComboBox(midiOutputComboBox, MidiOutPortSelectedIndex);
        //    midiInputDeviceWatcher.UpdateComboBox(midiInputComboBox, MidiInPortSelectedIndex);
        //}

        //public async void OutputDeviceChanged(ComboBox DeviceSelector)
        //{
        //    try
        //    {
        //        if (!String.IsNullOrEmpty((String)DeviceSelector.SelectedValue))
        //        {
        //            var midiOutDeviceInformationCollection = midiOutputDeviceWatcher.DeviceInformationCollection;

        //            if (midiOutDeviceInformationCollection == null)
        //            {
        //                return;
        //            }

        //            DeviceInformation midiOutDevInfo = midiOutDeviceInformationCollection[DeviceSelector.SelectedIndex];

        //            if (midiOutDevInfo == null)
        //            {
        //                return;
        //            }

        //            midiOutPort = (MidiOutPort)await MidiOutPort.FromIdAsync(midiOutDevInfo.Id);

        //            if (midiOutPort == null)
        //            {
        //                System.Diagnostics.Debug.WriteLine("Unable to create MidiOutPort from output device");
        //                return;
        //            }
        //        }
        //    }
        //    catch { }
        //}

        //public async void InputDeviceChanged(ComboBox DeviceSelector)
        //{
        //    try
        //    {
        //        if (!String.IsNullOrEmpty((String)DeviceSelector.SelectedValue))
        //        {
        //            var midiInDeviceInformationCollection = midiInputDeviceWatcher.DeviceInformationCollection;

        //            if (midiInDeviceInformationCollection == null)
        //            {
        //                return;
        //            }

        //            DeviceInformation midiInDevInfo = midiInDeviceInformationCollection[DeviceSelector.SelectedIndex];

        //            if (midiInDevInfo == null)
        //            {
        //                return;
        //            }

        //            midiInPort = await MidiInPort.FromIdAsync(midiInDevInfo.Id);

        //            if (midiInPort == null)
        //            {
        //                System.Diagnostics.Debug.WriteLine("Unable to create MidiInPort from input device");
        //                return;
        //            }
        //            //midiInPort.MessageReceived += MidiInPort_MessageReceived;
        //        }
        //    }
        //    catch { }
        //}

        //private async void UpdateDevices()
        //{
        //    // Get a list of all MIDI devices
        //    this.DeviceInformationCollection = await DeviceInformation.FindAllAsync(deviceSelectorString);

        //    if (deviceComboBox != null)
        //    {
        //        deviceComboBox.Items.Clear();

        //        if (!this.DeviceInformationCollection.Any())
        //        {
        //            deviceComboBox.Items.Add("No MIDI devices found!");
        //        }

        //        foreach (var deviceInformation in this.DeviceInformationCollection)
        //        {
        //            deviceComboBox.Items.Add(deviceInformation.Name);
        //        }

        //        for (Int32 i = 0; i < deviceComboBox.Items.Count(); i++)
        //        {
        //            if (((String)deviceComboBox.Items[i]).Contains("INTEGRA-7"))
        //            {
        //                deviceComboBox.SelectedIndex = i;
        //            }
        //        }
        //        if (deviceComboBox.SelectedIndex < 0 && deviceComboBox.Items.Count() > 0)
        //        {
        //            deviceComboBox.SelectedIndex = 0;
        //        }
        //    }
        //}

        //public void UpdateComboBox(Picker comboBox, Int32 selectedIndex)
        //{
        //    try
        //    {
        //        deviceComboBox = comboBox;
        //        foreach (var deviceInformation in this.DeviceInformationCollection)
        //        {
        //            deviceComboBox.Items.Add(deviceInformation.Name);
        //        }
        //        deviceComboBox.SelectedIndex = selectedIndex;
        //    }
        //    catch { }
        //}

        //public void NoteOn(byte currentChannel, byte noteNumber, byte velocity)
        //{
        //    if (midiOutPort != null)
        //    {
        //        IMidiMessage midiMessageToSend = new MidiNoteOnMessage(currentChannel, noteNumber, velocity);
        //        midiOutPort.SendMessage(midiMessageToSend);
        //    }
        //}

        //public void NoteOff(byte currentChannel, byte noteNumber)
        //{
        //    if (midiOutPort != null)
        //    {
        //        IMidiMessage midiMessageToSend = new MidiNoteOnMessage(currentChannel, noteNumber, 0);
        //        midiOutPort.SendMessage(midiMessageToSend);
        //    }
        //}

        //public void SendControlChange(byte channel, byte controller, byte value)
        //{
        //    if (midiOutPort != null)
        //    {
        //        IMidiMessage midiMessageToSend = new MidiControlChangeMessage(channel, controller, value);
        //        midiOutPort.SendMessage(midiMessageToSend);
        //    }
        //}

        //public void SetVolume(byte currentChannel, byte volume)
        //{
        //    if (midiOutPort != null)
        //    {
        //        IMidiMessage midiMessageToSend = new MidiControlChangeMessage(currentChannel, 0x07, volume);
        //        midiOutPort.SendMessage(midiMessageToSend);
        //    }
        //}

        //public void ProgramChange(byte currentChannel, String smsb, String slsb, String spc)
        //{
        //    try
        //    {
        //        MidiControlChangeMessage controlChangeMsb = new MidiControlChangeMessage(currentChannel, 0x00, (byte)(UInt16.Parse(smsb)));
        //        MidiControlChangeMessage controlChangeLsb = new MidiControlChangeMessage(currentChannel, 0x20, (byte)(UInt16.Parse(slsb)));
        //        MidiProgramChangeMessage programChange = new MidiProgramChangeMessage(currentChannel, (byte)(UInt16.Parse(spc) - 1));
        //        midiOutPort.SendMessage(controlChangeMsb);
        //        midiOutPort.SendMessage(controlChangeLsb);
        //        midiOutPort.SendMessage(programChange);
        //    }
        //    catch { }
        //}

        //public void ProgramChange(byte currentChannel, byte msb, byte lsb, byte pc)
        //{
        //    try
        //    {
        //        MidiControlChangeMessage controlChangeMsb = new MidiControlChangeMessage(currentChannel, 0x00, msb);
        //        MidiControlChangeMessage controlChangeLsb = new MidiControlChangeMessage(currentChannel, 0x20, lsb);
        //        MidiProgramChangeMessage programChange = new MidiProgramChangeMessage(currentChannel, (byte)(pc - 1));
        //        midiOutPort.SendMessage(controlChangeMsb);
        //        midiOutPort.SendMessage(controlChangeLsb);
        //        midiOutPort.SendMessage(programChange);
        //    }
        //    catch { }
        //}

        //public void SendSystemExclusive(byte[] bytes)
        //{
        //    IBuffer buffer = bytes.AsBuffer();
        //    MidiSystemExclusiveMessage midiMessageToSend = new MidiSystemExclusiveMessage(buffer);
        //    midiOutPort.SendMessage(midiMessageToSend);
        //}

        //public void MidiInPort_MessageReceived(MidiInPort sender, MidiMessageReceivedEventArgs args)
        //{
        //}

        public byte[] SystemExclusiveRQ1Message(byte[] Address, byte[] Length)
        {
            byte[] result = new byte[17];
            result[0] = 0xf0; // Start of exclusive message
            result[1] = 0x41; // Roland
            result[2] = 0x10; // Device Id is 17 according to settings in INTEGRA-7 (Menu -> System -> MIDI, 1 = 0x00 ... 17 = 0x10)
            result[3] = 0x00;
            result[4] = 0x00;
            result[5] = 0x64; // INTEGRA-7
            result[6] = 0x11; // Command (DT1)
            result[7] = Address[0];
            result[8] = Address[1];
            result[9] = Address[2];
            result[10] = Address[3];
            result[11] = Length[0];
            result[12] = Length[1];
            result[13] = Length[2];
            result[14] = Length[3];
            result[15] = 0x00; // Filled out by CheckSum but present here to avoid confusion about index 15 missing.
            result[16] = 0xf7; // End of sysex
            CheckSum(ref result);
            return (result);
        }

        public byte[] SystemExclusiveDT1Message(byte[] Address, byte[] DataToTransmit)
        {
            Int32 length = 13 + DataToTransmit.Length;
            byte[] result = new byte[length]; 
            result[0] = 0xf0; // Start of exclusive message
            result[1] = 0x41; // Roland
            result[2] = 0x10; // Device Id is 17 according to settings in INTEGRA-7 (Menu -> System -> MIDI, 1 = 0x00 ... 17 = 0x10)
            result[3] = 0x00;
            result[4] = 0x00;
            result[5] = 0x64; // INTEGRA-7
            result[6] = 0x12; // Command (DT1)
            result[7] = Address[0];
            result[8] = Address[1];
            result[9] = Address[2];
            result[10] = Address[3];
            for (Int32 i = 0; i < DataToTransmit.Length; i++)
            {
                result[i + 11] = DataToTransmit[i];
            }
            result[12 + DataToTransmit.Length] = 0xf7; // End of sysex
            CheckSum(ref result);
            return (result);
        }

        public void CheckSum(ref byte[] bytes)
        {
            byte chksum = 0;
            for (Int32 i = 7; i < bytes.Length - 2; i++)
            {
                chksum += bytes[i];
            }
            bytes[bytes.Length - 2] = (byte)((0x80 - (chksum & 0x7f)) & 0x7f);
        }
    }
}
