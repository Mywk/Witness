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

        public bool SetPower(int id, bool state)
        {
            return SetPower(id, state, true);
        }

        public bool SetPower(int id, bool state, bool writeToDatabase)
        {
            var relay = Database.Active.GetConnection().Table<DatabaseModel.Relay>().Where(t => t.Id == id).ToList();
            if (relay.Count > 0)
            {
                if(GpioControlCenter.Active.SetGpio(relay[0].Gpio, state))
                {
                    if (writeToDatabase)
                    {
                        relay[0].LastState = state;
                        Database.Active.GetConnection().Update(relay[0]);
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
