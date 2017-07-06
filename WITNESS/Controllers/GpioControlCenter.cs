using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace WITNESS
{
    public sealed class GpioControlCenter
    {
        internal static GpioControlCenter Active;

        private GpioController gpio = null;
        private Dictionary<int, GpioPin> gpioPins = new Dictionary<int, GpioPin>();

        public bool SetGpio(int pin, bool state)
        {
            try
            {
                GpioPin gpioPin = OpenPin(pin);
                if (gpioPin != null)
                    gpioPin.Write(!state ? GpioPinValue.High : GpioPinValue.Low);
            }
            catch (Exception)
            {
                // Nope
                return false;
            }

            return true;
        }

        private GpioPin OpenPin(int pin)
        {
            if (gpio == null)
                gpio = GpioController.GetDefault();

            GpioPin gpioPin = null;
            if (!gpioPins.ContainsKey(pin))
            {
                // Try at least 3 times before failing completely
                for (int i = 0; i < 3; i++)
                {
                    GpioOpenStatus status;
                    if (gpio.TryOpenPin(pin, GpioSharingMode.Exclusive, out gpioPin, out status)){

                        if (status == GpioOpenStatus.PinOpened)
                        {
                            gpioPin.SetDriveMode(GpioPinDriveMode.Output);
                            gpioPins.Add(pin, gpioPin);
                            break;
                        }
                        else
                            System.Threading.Tasks.Task.Delay(250);
                    }
                }
            }
            else
                gpioPin = gpioPins[pin];

            return gpioPin;
        }
    }
}
