using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace WITNESS.Controllers
{
    public sealed class RelayControlCenter
    {
        internal static RelayControlCenter Active;

        public bool SetPower(int id, bool state)
        {
            return SetPower(id, state, true);
        }

        public bool SetPower(int id, bool state, bool writeToDatabase)
        {
            var relay = Database.Active.GetConnection().Table<DatabaseModel.Relay>().Where(t => t.Id == id).FirstOrDefault();

            if (relay != null)
            {
                if (GpioControlCenter.Active.SetGpio(relay.Gpio, state))
                {
                    if (writeToDatabase)
                    {
                        relay.LastState = state;
                        Database.Active.GetConnection().Update(relay);
                    }
                    return true;
                }
            }
            
            return false;
        }

        public bool SetTimer(int id, bool on)
        {
            return true;
        }
    }
}
