using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using GHI.Pins;
using GHI.IO;
using GHI.IO.Storage;
using GHI.Processor;
using GHI.Usb;
using GHI.Usb.Host;

using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;

namespace GHIElectronics.Gadgeteer
{
    /// <summary>
    /// Support class for GHIElectronics FEZCerbot for Microsoft .NET Gadgeteer
    /// </summary>
    public class FEZCerbot : GT.Mainboard
    {
        // The mainboard constructor gets called before anything else in Gadgeteer (module constructors, etc), 
        // so it can set up fields in Gadgeteer.dll specifying socket types supported, etc.

        private IRemovable[] storageDevices;

        /// <summary>
        /// Instantiates a new FEZCerbot mainboard
        /// </summary>
        public FEZCerbot()
        {
            this.storageDevices = new IRemovable[2];

            Controller.Start();

            // comment the following if you do not support NativeI2C for faster DaisyLink performance
            // otherwise, the DaisyLink I2C interface will be supported in Gadgeteer.dll in managed code.
            GT.SocketInterfaces.I2CBusIndirector nativeI2C = (s, sdaPin, sclPin, address, clockRateKHz, module) => new InteropI2CBus(s, sdaPin, sclPin, address, clockRateKHz, module);
            GT.Socket socket;

            socket = GT.Socket.SocketInterfaces.CreateNumberedSocket(1);
            socket.SupportedTypes = new char[] { 'F', 'Y' };

            socket.CpuPins[3] = Generic.GetPin('B', 15);
            socket.CpuPins[4] = Generic.GetPin('C', 8);
            socket.CpuPins[5] = Generic.GetPin('C', 9);
            socket.CpuPins[6] = Generic.GetPin('D', 2);
            socket.CpuPins[7] = Generic.GetPin('C', 10);
            socket.CpuPins[8] = Generic.GetPin('C', 11);
            socket.CpuPins[9] = Generic.GetPin('C', 12);


            GT.Socket.SocketInterfaces.RegisterSocket(socket);


            socket = GT.Socket.SocketInterfaces.CreateNumberedSocket(2);
            socket.SupportedTypes = new char[] { 'I', 'X' };
            socket.CpuPins[3] = Generic.GetPin('C', 5);
            socket.CpuPins[4] = Generic.GetPin('A', 10);
            socket.CpuPins[5] = Generic.GetPin('B', 12);
            socket.CpuPins[6] = Generic.GetPin('C', 7);
            socket.CpuPins[8] = Generic.GetPin('B', 7);
            socket.CpuPins[9] = Generic.GetPin('B', 6);

            GT.Socket.SocketInterfaces.RegisterSocket(socket);


            socket = GT.Socket.SocketInterfaces.CreateNumberedSocket(3);
            socket.SupportedTypes = new char[] { 'U', 'S', 'X' };
            socket.CpuPins[3] = Generic.GetPin('B', 8);
            socket.CpuPins[4] = Generic.GetPin('B', 10);
            socket.CpuPins[5] = Generic.GetPin('B', 11);
            socket.CpuPins[6] = Generic.GetPin('A', 0);
            socket.CpuPins[7] = Generic.GetPin('B', 5);
            socket.CpuPins[8] = Generic.GetPin('B', 4);
            socket.CpuPins[9] = Generic.GetPin('B', 3);

            socket.SerialPortName = "COM3";
            socket.SPIModule = SPI.SPI_module.SPI1;
            GT.Socket.SocketInterfaces.RegisterSocket(socket);


            socket = GT.Socket.SocketInterfaces.CreateNumberedSocket(4);
            socket.SupportedTypes = new char[] { 'A', 'O', 'I', 'X' };
            socket.CpuPins[3] = Generic.GetPin('C', 0);
            socket.CpuPins[4] = Generic.GetPin('C', 1);
            socket.CpuPins[5] = Generic.GetPin('A', 4);
            socket.CpuPins[6] = Generic.GetPin('A', 1);
            socket.CpuPins[8] = Generic.GetPin('B', 7);
            socket.CpuPins[9] = Generic.GetPin('B', 6);

            socket.AnalogOutput5 = Cpu.AnalogOutputChannel.ANALOG_OUTPUT_0;
            GT.Socket.SocketInterfaces.SetAnalogInputFactors(socket, 3.3, 0, 12);
            GT.Socket.SocketInterfaces.SetAnalogOutputFactors(socket, 3.3, 0, 12);
            socket.AnalogInput3 = Cpu.AnalogChannel.ANALOG_3;
            socket.AnalogInput4 = Cpu.AnalogChannel.ANALOG_4;
            socket.AnalogInput5 = Cpu.AnalogChannel.ANALOG_5;
            GT.Socket.SocketInterfaces.RegisterSocket(socket);


            socket = GT.Socket.SocketInterfaces.CreateNumberedSocket(5);
            socket.SupportedTypes = new char[] { 'U', 'I' };
            socket.CpuPins[3] = Generic.GetPin('C', 14);
            socket.CpuPins[4] = Generic.GetPin('A', 2);
            socket.CpuPins[5] = Generic.GetPin('A', 3);
            socket.CpuPins[6] = Generic.GetPin('C', 15);
            socket.CpuPins[8] = Generic.GetPin('B', 7);
            socket.CpuPins[9] = Generic.GetPin('B', 6);

            socket.SerialPortName = "COM2";
            GT.Socket.SocketInterfaces.RegisterSocket(socket);


            socket = GT.Socket.SocketInterfaces.CreateNumberedSocket(6);
            socket.SupportedTypes = new char[] { 'P' };
            socket.CpuPins[3] = Generic.GetPin('C', 13);
            socket.CpuPins[6] = Generic.GetPin('C', 3);
            socket.CpuPins[7] = Generic.GetPin('A', 9);
            socket.CpuPins[8] = Generic.GetPin('B', 9);
            socket.CpuPins[9] = Generic.GetPin('A', 8);
            socket.PWM7 = (Cpu.PWMChannel)12;
            socket.PWM8 = (Cpu.PWMChannel)15;
            socket.PWM9 = Cpu.PWMChannel.PWM_3;
            GT.Socket.SocketInterfaces.RegisterSocket(socket);
        }

        private class InteropI2CBus : GT.SocketInterfaces.I2CBus
        {
            public override ushort Address { get; set; }
            public override int Timeout { get; set; }
            public override int ClockRateKHz { get; set; }

            private Cpu.Pin sdaPin;
            private Cpu.Pin sclPin;

            public InteropI2CBus(GT.Socket socket, GT.Socket.Pin sdaPin, GT.Socket.Pin sclPin, ushort address, int clockRateKHz, GTM.Module module)
            {
                this.sdaPin = socket.CpuPins[(int)sdaPin];
                this.sclPin = socket.CpuPins[(int)sclPin];
                this.Address = address;
                this.ClockRateKHz = clockRateKHz;
            }

            public override void WriteRead(byte[] writeBuffer, int writeOffset, int writeLength, byte[] readBuffer, int readOffset, int readLength, out int numWritten, out int numRead)
            {
                // implement this method if you support NativeI2CWriteRead for faster DaisyLink performance
                // otherwise, the DaisyLink I2C interface will be supported in Gadgeteer.dll in managed code. 
                numRead = 0;
                numWritten = 0;
                return;
            }
        }

        private static string[] sdVolumes = new string[] { "SD" };

        /// <summary>
        /// Allows mainboards to support storage device mounting/umounting.  This provides modules with a list of storage device volume names supported by the mainboard. 
        /// </summary>
        public override string[] GetStorageDeviceVolumeNames()
        {
            return sdVolumes;
        }

        /// <summary>
        /// Functionality provided by mainboard to mount storage devices, given the volume name of the storage device (see <see cref="GetStorageDeviceVolumeNames"/>).
        /// This should result in a <see cref="Microsoft.SPOT.IO.RemovableMedia.Insert"/> event if successful.
        /// </summary>
        public override bool MountStorageDevice(string volumeName)
        {
            try
            {
                if (volumeName == "SD" && this.storageDevices[0] == null)
                {
                    this.storageDevices[0] = new SDCard();
                    this.storageDevices[0].Mount();

                    return true;
                }
                else if (volumeName == "USB" && this.storageDevices[1] == null)
                {
                    foreach (BaseDevice dev in Controller.GetConnectedDevices())
                    {
                        if (dev.GetType() == typeof(MassStorage))
                        {
                            this.storageDevices[1] = (MassStorage)dev;
                            this.storageDevices[1].Mount();

                            return true;
                        }
                    }
                }
            }
            catch
            {

            }

            return false;
        }

        /// <summary>
        /// Functionality provided by mainboard to ummount storage devices, given the volume name of the storage device (see <see cref="GetStorageDeviceVolumeNames"/>).
        /// This should result in a <see cref="Microsoft.SPOT.IO.RemovableMedia.Eject"/> event if successful.
        /// </summary>
        public override bool UnmountStorageDevice(string volumeName)
        {
            if (volumeName == "SD" && this.storageDevices[0] != null)
            {
                this.storageDevices[0].Dispose();
                this.storageDevices[0] = null;
            }
            else if (volumeName == "USB" && this.storageDevices[1] != null)
            {
                this.storageDevices[1].Dispose();
                this.storageDevices[1] = null;
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Changes the programming interafces to the one specified.
        /// </summary>
        /// <param name="programmingInterface">The programming interface to use</param>
        public override void SetProgrammingMode(GT.Mainboard.ProgrammingInterface programmingInterface)
        {
            // Change the reflashing interface to the one specified, if possible.
            // This is an advanced API that we don't expect people to call much.
            throw new NotSupportedException();
        }

        /// <summary>
        /// Configure the onboard display controller to fulfil the requirements of a display using the RGB sockets.
        /// If doing this requires rebooting, then the method must reboot and not return.
        /// If there is no onboard display controller, then NotSupportedException must be thrown.
        /// </summary>
        /// <param name="displayModel">Display model name.</param>
        /// <param name="width">Display physical width in pixels, ignoring the orientation setting.</param>
        /// <param name="height">Display physical height in lines, ignoring the orientation setting.</param>
        /// <param name="orientationDeg">Display orientation in degrees.</param>
        /// <param name="timing">The required timings from an LCD controller.</param>
        protected override void OnOnboardControllerDisplayConnected(string displayModel, int width, int height, int orientationDeg, GT.Modules.Module.DisplayModule.TimingRequirements timing)
        {
            throw new NotSupportedException("This mainboard does not support an onboard display controller.");
        }

        /// <summary>
        /// Called when the onboard display controller's display is disconnected, so any resources used by the onboard display controller could be reclaimed. 
        /// </summary>
        protected override void OnOnboardControllerDisplayDisconnected()
        {
            // it is optional to do anything with this method
        }

        /// <summary>
        /// Ensures that the pins on R, G and B sockets (which also have other socket types) are available for use for non-display purposes.
        /// If doing this requires rebooting, then the method must reboot and not return.
        /// If there is no onboard display controller, or it is not possible to disable the onboard display controller, then NotSupportedException must be thrown.
        /// </summary>
        public override void EnsureRgbSocketPinsAvailable()
        {
            throw new NotSupportedException("This mainboard does not support an onboard display controller.");
        }

        // change the below to the debug led pin on this mainboard

        private Microsoft.SPOT.Hardware.OutputPort debugLed = null;
        /// <summary>
        /// Turns the debug LED on or off.
        /// </summary>
        /// <param name="on">True if the debug LED should be on</param>
        public override void SetDebugLED(bool on)
        {
            if (debugLed == null)
                    this.debugLed = new OutputPort(Generic.GetPin('A', 14), on);

            this.debugLed.Write(on);
        }

        /// <summary>
        /// This performs post-initialization tasks for the mainboard.  It is called by Gadgeteer.Program.Run and does not need to be called manually.
        /// </summary>
        public override void PostInit()
        {
            return;
        }

        /// <summary>
        /// The mainboard name, which is printed at startup in the debug window
        /// </summary>
        public override string MainboardName
        {
            get { return "GHIElectronics FEZCerbot"; }
        }

        /// <summary>
        /// The mainboard version, which is printed at startup in the debug window
        /// </summary>
        public override string MainboardVersion
        {
            get { return "1.0"; }
        }

    }
}
