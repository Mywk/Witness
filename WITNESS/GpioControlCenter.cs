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

        private static class GpioPins
        {
            public static class Power
            {
                public enum Defaults
                {
                    TV = 22,
                    Monitors = 27
                }

                public static GpioPin TV, Monitors = null;
            }
        }

        private GpioController gpio = null;
        
        public bool SetPower(int type, bool on)
        {
            return SetGpio(type, on);
        }

        private bool SetGpio(int id, bool status)
        {
            if(gpio == null)
                gpio = GpioController.GetDefault();

            GpioPin pin = null;
            var pinValue = !status ? GpioPinValue.High : GpioPinValue.Low;

            try
            {
                switch (id)
                {
                    case 1:
                        pin = OpenPin(ref GpioPins.Power.TV, (int)GpioPins.Power.Defaults.TV);
                        break;
                    case 2:
                        pin = OpenPin(ref GpioPins.Power.Monitors, (int)GpioPins.Power.Defaults.Monitors);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                // Nope
                return false;
            }

            if (pin != null)
            {
                pin.SetDriveMode(GpioPinDriveMode.Output);
                pin.Write(pinValue);
            }

            return true;
        }

        private GpioPin OpenPin(ref GpioPin pin, int id)
        {
            if (pin == null){
                pin = gpio.OpenPin(id);
                pin.SetDriveMode(GpioPinDriveMode.Output);
            }
            return pin;
        }
    }
}
