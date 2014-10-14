using System;
using System.Collections.Generic;
using System.Linq;
using DeviceManagement;
using ReactiveUI;

namespace PoC.UsbActions
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            var handler = new VolumeNotificationHandler();

            handler.VolumeArrival += handler_VolumeArrival;
            handler.VolumeRemoveComplete += handler_VolumeRemoveComplete;
            handler.DeviceArrival += handler_DeviceArrival;
            handler.DeviceRemoveComplete += handler_DeviceRemoveComplete;

            var deviceNotification = new DeviceNotification { NotificationHandler = handler };

            deviceNotification.Register(DeviceType.LogicalVolume);

            Log = "USB events wired up\n\n";

            Eject = ReactiveCommand.Create();
            Eject.Subscribe(_ => EjectImpl(@"E:"));
        }

        void handler_DeviceRemoveComplete(object sender, DeviceNotificationEventArgs e)
        {
            Log += string.Format("Device {0} removed completely\n", e.GetVolume().DriveLetter);
        }

        void handler_DeviceArrival(object sender, DeviceNotificationEventArgs e)
        {
            Log += string.Format("Device {0} arrived\n", e.GetVolume().DriveLetter);
        }

        public void EjectImpl(string driveLetter)
        {
            RemoveDriveTools.RemoveDrive(driveLetter);
        }

        void handler_VolumeRemoveComplete(object sender, DeviceNotificationEventArgs e)
        {
            Log += string.Format("Volume {0} removed completely\n", e.GetVolume().DriveLetter);
        }

        void handler_VolumeArrival(object sender, DeviceNotificationEventArgs e)
        {
            Log += string.Format("Volume {0} arrived\n", e.GetVolume().DriveLetter);
        }

        public ReactiveCommand<object> Eject { get; private set; }
        
        string _log;
        public string Log
        {
            get { return _log; }
            set { this.RaiseAndSetIfChanged(ref _log, value); }
        }
    }
}