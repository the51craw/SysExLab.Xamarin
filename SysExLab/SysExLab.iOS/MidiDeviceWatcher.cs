using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SysExLab.iOS
{
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
    [ContractVersion(typeof(FoundationContract), 65536)]
    public sealed class ApiContractAttribute : Attribute
    {
        public ApiContractAttribute();
    }

    [ApiContract]
    [ContractVersion(131072)]
    public struct FoundationContract
    {
    }

    //
    // Summary:
    //     Indicates that multiple instances of a custom attribute can be applied to a target.
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [ContractVersion(typeof(FoundationContract), 65536)]
    public sealed class AllowMultipleAttribute : Attribute
    {
        //
        // Summary:
        //     Creates and initializes a new instance of the attribute.
        public AllowMultipleAttribute();
    }

    [AllowMultiple]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = true)]
    [ContractVersionAttribute(typeof(FoundationContract), 65536)]
    public sealed class ContractVersionAttribute : Attribute
    {
        public ContractVersionAttribute(System.UInt32 version);
        public ContractVersionAttribute(Type contract, System.UInt32 version);
        public ContractVersionAttribute(System.String contract, System.UInt32 version);
    }

    [ContractVersion(typeof(UniversalApiContract), 65536)]
    [ExclusiveTo(typeof(DeviceWatcher))]
    [GuidAttribute(3387603325, 36715, 20374, 169, 244, 171, 200, 20, 226, 34, 113)]
    public enum DeviceWatcherStatus
    {
        //
        // Summary:
        //     This is the initial state of a Watcher object. During this state clients can
        //     register event handlers.
        Created = 0,
        //
        // Summary:
        //     The watcher transitions to the Started state once Start is called. The watcher
        //     is enumerating the initial collection. Note that during this enumeration phase
        //     it is possible to receive Updated and Removed notifications but only to items
        //     that have already been Added.
        Started = 1,
        //
        // Summary:
        //     The watcher has completed enumerating the initial collection. Items can still
        //     be added, updated or removed from the collection.
        EnumerationCompleted = 2,
        //
        // Summary:
        //     The client has called Stop and the watcher is still in the process of stopping.
        //     Events may still be raised.
        Stopping = 3,
        //
        // Summary:
        //     The client has called Stop and the watcher has completed all outstanding events.
        //     No further events will be raised.
        Stopped = 4,
        //
        // Summary:
        //     The watcher has aborted operation. No subsequent events will be raised.
        Aborted = 5
    }

    public delegate void TypedEventHandler<TSender, TResult>(TSender sender, TResult args);

    internal interface IDeviceInformation
    {
        EnclosureLocation EnclosureLocation { get; }
        System.String Id { get; }
        System.Boolean IsDefault { get; }
        System.Boolean IsEnabled { get; }
        System.String Name { get; }
        IReadOnlyDictionary<System.String, System.Object> Properties { get; }

        [RemoteAsync]
        IAsyncOperation<DeviceThumbnail> GetGlyphThumbnailAsync();
        [RemoteAsync]
        IAsyncOperation<DeviceThumbnail> GetThumbnailAsync();
        void Update(DeviceInformationUpdate updateInfo);
    }

    public sealed class DeviceInformation : IDeviceInformation, IDeviceInformation2
    {
        //
        // Summary:
        //     The physical location of the device in its enclosure.
        //
        // Returns:
        //     The object that describes the physical location of the device.
        public EnclosureLocation EnclosureLocation { get; }
        //
        // Summary:
        //     A string representing the identity of the device.
        //
        // Returns:
        //     A string representing the identity of the device.
        public System.String Id { get; }
        //
        // Summary:
        //     Indicates whether this device is the default device for the class.
        //
        // Returns:
        //     Indicates whether this device is the default device for the class.
        public System.Boolean IsDefault { get; }
        //
        // Summary:
        //     Indicates whether this device is enabled.
        //
        // Returns:
        //     Indicates whether this device is enabled.
        public System.Boolean IsEnabled { get; }
        //
        // Summary:
        //     Gets the type of DeviceInformation represented by this object.
        //
        // Returns:
        //     The type of information represented by this object.
        public DeviceInformationKind Kind { get; }
        //
        // Summary:
        //     The name of the device.
        //
        // Returns:
        //     The name of the device. This name is in the best available language for the app.
        public System.String Name { get; }
        //
        // Summary:
        //     Gets the information about the capabilities for this device to pair.
        //
        // Returns:
        //     The pairing information for this device.
        public DeviceInformationPairing Pairing { get; }
        //
        // Summary:
        //     Property store containing well-known values as well as additional properties
        //     that can be specified during device enumeration.
        //
        // Returns:
        //     The property store for the device.
        public IReadOnlyDictionary<System.String, System.Object> Properties { get; }

        //
        // Summary:
        //     Creates a DeviceInformation object from a DeviceInformation ID.
        //
        // Parameters:
        //   deviceId:
        //     The device ID.
        //
        // Returns:
        //     An object for starting and managing the asynchronous creation of the DeviceInformation
        //     object.
        [Overload("CreateFromIdAsync")]
        [RemoteAsync]
        public static IAsyncOperation<DeviceInformation> CreateFromIdAsync(System.String deviceId);
        [Overload("CreateFromIdAsyncAdditionalProperties")]
        public static IAsyncOperation<DeviceInformation> CreateFromIdAsync(System.String deviceId, IEnumerable<System.String> additionalProperties);
        [Overload("CreateFromIdAsyncWithKindAndAdditionalProperties")]
        public static IAsyncOperation<DeviceInformation> CreateFromIdAsync(System.String deviceId, IEnumerable<System.String> additionalProperties, DeviceInformationKind kind);
        //
        // Summary:
        //     Creates a DeviceWatcher for all devices.
        //
        // Returns:
        //     The created DeviceWatcher.
        [Overload("CreateWatcher")]
        public static DeviceWatcher CreateWatcher();
        //
        // Summary:
        //     Creates a DeviceWatcher for devices matching the specified DeviceClass.
        //
        // Parameters:
        //   deviceClass:
        //     The class of device to enumerate using the DeviceWatcher.
        //
        // Returns:
        //     The created DeviceWatcher.
        [DefaultOverload]
        [Overload("CreateWatcherDeviceClass")]
        public static DeviceWatcher CreateWatcher(DeviceClass deviceClass);
        //
        // Summary:
        //     Creates a DeviceWatcher for devices matching the specified Advanced Query Syntax
        //     (AQS) string.
        //
        // Parameters:
        //   aqsFilter:
        //     An AQS string that filters the DeviceInformation objects to enumerate. Typically
        //     this string is retrieved from the GetDeviceSelector method of a class that interacts
        //     with devices. For example, GetDeviceSelector retrieves the string for the StorageDevice
        //     class.
        //
        // Returns:
        //     The created DeviceWatcher.
        [Overload("CreateWatcherAqsFilter")]
        public static DeviceWatcher CreateWatcher(System.String aqsFilter);
        [Overload("CreateWatcherAqsFilterAndAdditionalProperties")]
        public static DeviceWatcher CreateWatcher(System.String aqsFilter, IEnumerable<System.String> additionalProperties);
        [Overload("CreateWatcherWithKindAqsFilterAndAdditionalProperties")]
        public static DeviceWatcher CreateWatcher(System.String aqsFilter, IEnumerable<System.String> additionalProperties, DeviceInformationKind kind);
        //
        // Summary:
        //     Enumerates all DeviceInformation objects.
        //
        // Returns:
        //     The object for managing the asynchronous operation.
        [Overload("FindAllAsync")]
        [RemoteAsync]
        public static IAsyncOperation<DeviceInformationCollection> FindAllAsync();
        //
        // Summary:
        //     Enumerates DeviceInformation objects of the specified class.
        //
        // Parameters:
        //   deviceClass:
        //     The class of devices to enumerate.
        //
        // Returns:
        //     The object for managing the asynchronous operation.
        [DefaultOverload]
        [Overload("FindAllAsyncDeviceClass")]
        [RemoteAsync]
        public static IAsyncOperation<DeviceInformationCollection> FindAllAsync(DeviceClass deviceClass);
        //
        // Summary:
        //     Enumerates DeviceInformation objects matching the specified Advanced Query Syntax
        //     (AQS) string.
        //
        // Parameters:
        //   aqsFilter:
        //     An AQS string that filters the DeviceInformation objects to enumerate. Typically
        //     this string is retrieved from the GetDeviceSelector method of a class that interacts
        //     with devices. For example, GetDeviceSelector retrieves the string for the StorageDevice
        //     class.
        //
        // Returns:
        //     The object for managing the asynchronous operation.
        [Overload("FindAllAsyncAqsFilter")]
        [RemoteAsync]
        public static IAsyncOperation<DeviceInformationCollection> FindAllAsync(System.String aqsFilter);
        [Overload("FindAllAsyncAqsFilterAndAdditionalProperties")]
        public static IAsyncOperation<DeviceInformationCollection> FindAllAsync(System.String aqsFilter, IEnumerable<System.String> additionalProperties);
        [Overload("FindAllAsyncWithKindAqsFilterAndAdditionalProperties")]
        public static IAsyncOperation<DeviceInformationCollection> FindAllAsync(System.String aqsFilter, IEnumerable<System.String> additionalProperties, DeviceInformationKind kind);
        //
        // Summary:
        //     Creates a filter to use to enumerate through a subset of device types.
        //
        // Parameters:
        //   deviceClass:
        //     The type of devices that you want to create a filter for.
        //
        // Returns:
        //     The Advanced Query Syntax (AQS) filter used to specifically enumerate through
        //     the device type specified by deviceClass.
        public static System.String GetAqsFilterFromDeviceClass(DeviceClass deviceClass);
        //
        // Summary:
        //     Gets a glyph for the device.
        //
        // Returns:
        //     The object for managing the asynchronous operation that will return a DeviceThumbnail
        [RemoteAsync]
        public IAsyncOperation<DeviceThumbnail> GetGlyphThumbnailAsync();
        //
        // Summary:
        //     Returns a thumbnail image for the device.
        //
        // Returns:
        //     The object for managing the asynchronous operation that will return a DeviceThumbnail.
        [RemoteAsync]
        public IAsyncOperation<DeviceThumbnail> GetThumbnailAsync();
        //
        // Summary:
        //     Updates the properties of an existing DeviceInformation object.
        //
        // Parameters:
        //   updateInfo:
        //     Indicates the properties to update.
        public void Update(DeviceInformationUpdate updateInfo);
    }

    internal interface IDeviceWatcher
    {
        DeviceWatcherStatus Status { get; }

        event TypedEventHandler<DeviceWatcher, DeviceInformation> Added;
        event TypedEventHandler<DeviceWatcher, System.Object> EnumerationCompleted;
        event TypedEventHandler<DeviceWatcher, DeviceInformationUpdate> Removed;
        event TypedEventHandler<DeviceWatcher, System.Object> Stopped;
        event TypedEventHandler<DeviceWatcher, DeviceInformationUpdate> Updated;

        void Start();
        void Stop();
    }

    internal interface IDeviceWatcher2
    {
        DeviceWatcherTrigger GetBackgroundTrigger(IEnumerable<DeviceWatcherEventKind> requestedEventKinds);
    }

    public sealed class DeviceWatcher : IDeviceWatcher, IDeviceWatcher2
    {
        //
        // Summary:
        //     The status of the DeviceWatcher.
        //
        // Returns:
        //     The status of the DeviceWatcher.
        public DeviceWatcherStatus Status { get; }

        //
        // Summary:
        //     Event that is raised when a device is added to the collection enumerated by the
        //     DeviceWatcher.
        public event TypedEventHandler<DeviceWatcher, DeviceInformation> Added;
        //
        // Summary:
        //     Event that is raised when the enumeration of devices completes.
        public event TypedEventHandler<DeviceWatcher, System.Object> EnumerationCompleted;
        //
        // Summary:
        //     Event that is raised when a device is removed from the collection of enumerated
        //     devices.
        public event TypedEventHandler<DeviceWatcher, DeviceInformationUpdate> Removed;
        //
        // Summary:
        //     Event that is raised when the enumeration operation has been stopped.
        public event TypedEventHandler<DeviceWatcher, System.Object> Stopped;
        //
        // Summary:
        //     Event that is raised when a device is updated in the collection of enumerated
        //     devices.
        public event TypedEventHandler<DeviceWatcher, DeviceInformationUpdate> Updated;

        public DeviceWatcherTrigger GetBackgroundTrigger(IEnumerable<DeviceWatcherEventKind> requestedEventKinds);
        //
        // Summary:
        //     Starts a search for devices, and subscribes to device enumeration events.
        public void Start();
        //
        // Summary:
        //     Stop raising the events that add, update and remove enumeration results.
        public void Stop();
    }

    public class MidiDeviceWatcher
    {
        DeviceWatcher deviceWatcher;
        string deviceSelectorString;
        Picker deviceComboBox = null;
        CoreDispatcher coreDispatcher;
        public DeviceInformationCollection DeviceInformationCollection { get; set; }

        public MidiDeviceWatcher(string midiDeviceSelectorString, Picker midiDeviceComboBox, CoreDispatcher dispatcher)
        {
            deviceComboBox = midiDeviceComboBox;
            coreDispatcher = dispatcher;

            deviceSelectorString = midiDeviceSelectorString;

            deviceWatcher = DeviceInformation.CreateWatcher(deviceSelectorString);
            deviceWatcher.Added += DeviceWatcher_Added;
            deviceWatcher.Removed += DeviceWatcher_Removed;
            deviceWatcher.Updated += DeviceWatcher_Updated;
            deviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
        }

        ~MidiDeviceWatcher()
        {
            deviceWatcher.Added -= DeviceWatcher_Added;
            deviceWatcher.Removed -= DeviceWatcher_Removed;
            deviceWatcher.Updated -= DeviceWatcher_Updated;

            deviceWatcher = null;
        }

        public void SelectDevice(String DeviceName)
        {

        }

        private async void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            await coreDispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                // Update the device list
                UpdateDevices();
            });
        }

        private async void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            await coreDispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                // Update the device list
                UpdateDevices();
            });
        }

        private async void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            await coreDispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                // Update the device list
                UpdateDevices();
            });
        }

        private async void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            await coreDispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                // Update the device list
                UpdateDevices();
            });
        }

        private async void UpdateDevices()
        {
            // Get a list of all MIDI devices
            this.DeviceInformationCollection = await DeviceInformation.FindAllAsync(deviceSelectorString);

            if (deviceComboBox != null)
            {
                deviceComboBox.Items.Clear();

                if (!this.DeviceInformationCollection.Any())
                {
                    deviceComboBox.Items.Add("No MIDI devices found!");
                }

                foreach (var deviceInformation in this.DeviceInformationCollection)
                {
                    deviceComboBox.Items.Add(deviceInformation.Name);
                }

                for (Int32 i = 0; i < deviceComboBox.Items.Count(); i++)
                {
                    if (((String)deviceComboBox.Items[i]).Contains("INTEGRA-7"))
                    {
                        deviceComboBox.SelectedIndex = i;
                    }
                }
                if (deviceComboBox.SelectedIndex < 0 && deviceComboBox.Items.Count() > 0)
                {
                    deviceComboBox.SelectedIndex = 0;
                }
            }
        }

        public void UpdateComboBox(Picker comboBox, Int32 selectedIndex)
        {
            try
            {
                deviceComboBox = comboBox;
                foreach (var deviceInformation in this.DeviceInformationCollection)
                {
                    deviceComboBox.Items.Add(deviceInformation.Name);
                }
                deviceComboBox.SelectedIndex = selectedIndex;
            }
            catch { }
        }

        public void StartWatcher()
        {
            deviceWatcher.Start();
        }

        public void StopWatcher()
        {
            deviceWatcher.Stop();
        }
    }
}
