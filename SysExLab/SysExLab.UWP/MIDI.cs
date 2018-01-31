﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Midi;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using Xamarin.Forms;
using SysExLab.UWP;

[assembly: Xamarin.Forms.Dependency(typeof(GenericHandlerInterface))]

namespace SysExLab.UWP
{
    public class MIDI : IMidi
    {
        public MidiDeviceWatcher midiOutputDeviceWatcher;
        public MidiDeviceWatcher midiInputDeviceWatcher;
        public MidiOutPort midiOutPort;
        public MidiInPort midiInPort;
        public byte MidiOutPortChannel { get; set; }
        public byte MidiInPortChannel { get; set; }
        public Int32 MidiOutPortSelectedIndex { get; set; }
        public Int32 MidiInPortSelectedIndex { get; set; }
        public SysExLab.MainPage mainPage { get; set; }
        public MainPage mainPage_UWP { get; set; }
        public byte[] rawData;
        public DispatcherTimer timer;
        public Boolean MessageReceived = false;

        public MIDI()
        {
            if (midiOutPort == null)
            {
                mainPage_UWP = DependencyService.Get<SysExLab.UWP.MainPage>();
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            if (MessageReceived)
            {
                if (!(Boolean)mainPage.uIHandler.rcvKeepAlive.Switch.IsToggled && rawData.Length == 1 && rawData[0] == 0xfe)
                {
                    return;
                }
                String line = "";
                Boolean lineWritten = false;
                for (UInt16 i = 0; i < rawData.Length; i++)
                {
                    line += ToHex(rawData[i]);
                    lineWritten = false;
                    if (line.Length >= 16 * 5)
                    {
                        mainPage.uIHandler.receivedLines.Add(line);
                        line = "";
                        lineWritten = true;
                    }
                }
                if (!lineWritten)
                {
                    mainPage.uIHandler.receivedLines.Add(line);
                }
                MessageReceived = false;
            }
        }

        public void MidiInPort_MessageReceived(MidiInPort sender, MidiMessageReceivedEventArgs args)
        {
            IMidiMessage receivedMidiMessage = args.Message;
            rawData = receivedMidiMessage.RawData.ToArray();
            MessageReceived = true;
        }

        // Constructor using a combobox for full device watch:
        public MIDI(SysExLab.MainPage mainPage, MainPage mainPage_UWP, Picker OutputDeviceSelector, Picker InputDeviceSelector, byte MidiOutPortChannel, byte MidiInPortChannel)
        {
            this.mainPage = mainPage;
            this.mainPage_UWP = mainPage_UWP;
            midiOutputDeviceWatcher = new MidiDeviceWatcher(MidiOutPort.GetDeviceSelector(), OutputDeviceSelector, mainPage_UWP.dispatcher);
            midiInputDeviceWatcher = new MidiDeviceWatcher(MidiInPort.GetDeviceSelector(), InputDeviceSelector, mainPage_UWP.dispatcher);
            midiOutputDeviceWatcher.StartWatcher();
            midiInputDeviceWatcher.StartWatcher();
            this.MidiOutPortChannel = MidiOutPortChannel;
            this.MidiInPortChannel = MidiInPortChannel;
        }

        ~MIDI()
        {
            try
            {
                midiOutputDeviceWatcher.StopWatcher();
                midiInputDeviceWatcher.StopWatcher();
                midiOutPort.Dispose();
                midiInPort.MessageReceived -= MidiInPort_MessageReceived;
                midiInPort.Dispose();
                midiOutPort = null;
                midiInPort = null;
            } catch { }
        }

        // Simpleconstructor that takes the name of the device:
        public MIDI(String deviceName)
        {
            Init(deviceName);
        }

        public void Init(String deviceName, SysExLab.MainPage mainPage, Picker OutputDeviceSelector, Picker InputDeviceSelector, byte MidiOutPortChannel, byte MidiInPortChannel)
        {
            Init(deviceName, OutputDeviceSelector, InputDeviceSelector, mainPage_UWP.dispatcher, MidiOutPortChannel, MidiInPortChannel);
        }

        public void Init(String deviceName, Picker OutputDeviceSelector, Picker InputDeviceSelector, CoreDispatcher Dispatcher, byte MidiOutPortChannel, byte MidiInPortChannel)
        {
            //this.mainPage = mainPage;
            midiOutputDeviceWatcher = new MidiDeviceWatcher(MidiOutPort.GetDeviceSelector(), OutputDeviceSelector, Dispatcher);
            midiInputDeviceWatcher = new MidiDeviceWatcher(MidiInPort.GetDeviceSelector(), InputDeviceSelector, Dispatcher);
            midiOutputDeviceWatcher.StartWatcher();
            midiInputDeviceWatcher.StartWatcher();
            this.MidiOutPortChannel = MidiOutPortChannel;
            this.MidiInPortChannel = MidiInPortChannel;
            Init(deviceName);
        }

        public async void Init(String deviceName, SysExLab.MainPage mainPage)
        {
            this.mainPage = mainPage;
            Init(deviceName);
        }

        public async void Init(String deviceName)
        {
            DeviceInformationCollection midiOutputDevices = await DeviceInformation.FindAllAsync(MidiOutPort.GetDeviceSelector());
            DeviceInformationCollection midiInputDevices = await DeviceInformation.FindAllAsync(MidiInPort.GetDeviceSelector());
            DeviceInformation midiOutDevInfo = null;
            DeviceInformation midiInDevInfo = null;

            foreach (DeviceInformation device in midiOutputDevices)
            {
                if (device.Name.Contains(deviceName))
                {
                    midiOutDevInfo = device;
                    break;
                }
            }

            if (midiOutDevInfo != null)
            {
                midiOutPort = (MidiOutPort)await MidiOutPort.FromIdAsync(midiOutDevInfo.Id);
            }

            foreach (DeviceInformation device in midiInputDevices)
            {
                if (device.Name.Contains(deviceName))
                {
                    midiInDevInfo = device;
                    break;
                }
            }

            if (midiInDevInfo != null)
            {
                midiInPort = await MidiInPort.FromIdAsync(midiInDevInfo.Id);
            }

            if (midiOutPort == null)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create MidiOutPort from output device");
            }

            if (midiInPort == null)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create MidiInPort from output device");
            }
            else
            {
                midiInPort.MessageReceived += MidiInPort_MessageReceived;
            }
        }

        public void UpdateMidiComboBoxes(Picker midiOutputComboBox, Picker midiInputComboBox)
        {
            midiOutputDeviceWatcher.UpdateComboBox(midiOutputComboBox, MidiOutPortSelectedIndex);
            midiInputDeviceWatcher.UpdateComboBox(midiInputComboBox, MidiInPortSelectedIndex);
        }

        public async void OutputDeviceChanged(Picker DeviceSelector)
        {
            try
            {
                if (!String.IsNullOrEmpty((String)DeviceSelector.SelectedItem))
                {
                    var midiOutDeviceInformationCollection = midiOutputDeviceWatcher.DeviceInformationCollection;

                    if (midiOutDeviceInformationCollection == null)
                    {
                        return;
                    }

                    DeviceInformation midiOutDevInfo = midiOutDeviceInformationCollection[DeviceSelector.SelectedIndex];

                    if (midiOutDevInfo == null)
                    {
                        return;
                    }

                    midiOutPort = (MidiOutPort)await MidiOutPort.FromIdAsync(midiOutDevInfo.Id);

                    if (midiOutPort == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Unable to create MidiOutPort from output device");
                        return;
                    }
                }
            }
            catch { }
        }

        public async void InputDeviceChanged(Picker DeviceSelector)
        {
            try
            {
                if (!String.IsNullOrEmpty((String)DeviceSelector.SelectedItem))
                {
                    var midiInDeviceInformationCollection = midiInputDeviceWatcher.DeviceInformationCollection;

                    if (midiInDeviceInformationCollection == null)
                    {
                        return;
                    }

                    DeviceInformation midiInDevInfo = midiInDeviceInformationCollection[DeviceSelector.SelectedIndex];

                    if (midiInDevInfo == null)
                    {
                        return;
                    }

                    midiInPort = await MidiInPort.FromIdAsync(midiInDevInfo.Id);

                    if (midiInPort == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Unable to create MidiInPort from input device");
                        return;
                    }
                    midiInPort.MessageReceived += MidiInPort_MessageReceived;
                }
            }
            catch { }
        }

        public void NoteOn(byte currentChannel, byte noteNumber, byte velocity)
        {
            if (midiOutPort != null)
            {
                IMidiMessage midiMessageToSend = new MidiNoteOnMessage(currentChannel, noteNumber, velocity);
                midiOutPort.SendMessage(midiMessageToSend);
            }
        }

        public void NoteOff(byte currentChannel, byte noteNumber)
        {
            if (midiOutPort != null)
            {
                IMidiMessage midiMessageToSend = new MidiNoteOnMessage(currentChannel, noteNumber, 0);
                midiOutPort.SendMessage(midiMessageToSend);
            }
        }

        public void SendControlChange(byte channel, byte controller, byte value)
        {
            if (midiOutPort != null)
            {
                IMidiMessage midiMessageToSend = new MidiControlChangeMessage(channel, controller, value);
                midiOutPort.SendMessage(midiMessageToSend);
            }
        }

        public void SetVolume(byte currentChannel, byte volume)
        {
            if (midiOutPort != null)
            {
                IMidiMessage midiMessageToSend = new MidiControlChangeMessage(currentChannel, 0x07, volume);
                midiOutPort.SendMessage(midiMessageToSend);
            }
        }

        public void ProgramChange(byte currentChannel, String smsb, String slsb, String spc)
        {
            try
            {
                MidiControlChangeMessage controlChangeMsb = new MidiControlChangeMessage(currentChannel, 0x00, (byte)(UInt16.Parse(smsb)));
                MidiControlChangeMessage controlChangeLsb = new MidiControlChangeMessage(currentChannel, 0x20, (byte)(UInt16.Parse(slsb)));
                MidiProgramChangeMessage programChange = new MidiProgramChangeMessage(currentChannel, (byte)(UInt16.Parse(spc) - 1));
                midiOutPort.SendMessage(controlChangeMsb);
                midiOutPort.SendMessage(controlChangeLsb);
                midiOutPort.SendMessage(programChange);
            }
            catch { }
        }

        public void ProgramChange(byte currentChannel, byte msb, byte lsb, byte pc)
        {
            try
            {
                MidiControlChangeMessage controlChangeMsb = new MidiControlChangeMessage(currentChannel, 0x00, msb);
                MidiControlChangeMessage controlChangeLsb = new MidiControlChangeMessage(currentChannel, 0x20, lsb);
                MidiProgramChangeMessage programChange = new MidiProgramChangeMessage(currentChannel, (byte)(pc - 1));
                midiOutPort.SendMessage(controlChangeMsb);
                midiOutPort.SendMessage(controlChangeLsb);
                midiOutPort.SendMessage(programChange);
            }
            catch { }
        }

        public void SendSystemExclusive(byte[] bytes)
        {
            IBuffer buffer = bytes.AsBuffer();
            MidiSystemExclusiveMessage midiMessageToSend = new MidiSystemExclusiveMessage(buffer);
            midiOutPort.SendMessage(midiMessageToSend);
        }

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
