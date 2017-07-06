using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace WITNESS
{
    public sealed class RelayControlCenter
    {
        internal static RelayControlCenter Active;

        public bool SetPower(int id, bool on)
        {
            var relay = Database.Active.GetConnection().Table<DatabaseModel.Relay>().Where(t => t.Id == id).ToList();
            if (relay.Count > 0)
                return GpioControlCenter.Active.SetGpio(relay[0].Gpio, on);
            else
                return false;
        }

        public bool SetTimer(int id, bool on)
        {
            return true;
        }
    }
}
