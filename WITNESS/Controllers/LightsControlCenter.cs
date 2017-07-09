using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WITNESS.Milight;

namespace WITNESS.Controllers
{
    public sealed class LightsControlCenter
    {
        internal static LightsControlCenter Active;

        private List<Milight.Bridge> bridges = new List<Milight.Bridge>();

        public LightsControlCenter()
        {
            var dbBridges = Database.Active.GetConnection().Table<DatabaseModel.MilightBridge>().ToList();
            foreach (var bridge in dbBridges)
            {
                bridges.Add(new Milight.Bridge(bridge.Id, bridge.Name, bridge.Ip.ToString(), bridge.Port.ToString()));
            }

        }

        private DatabaseModel.MilightGroup GetGroupById(int id)
        {
            return Database.Active.GetConnection().Table<DatabaseModel.MilightGroup>().Where(g => g.Id == id).FirstOrDefault();
        }

        private Bridge GetBridgeByGroupId(int id)
        {
            var group = GetGroupById(id);
            if (group != null)
            {
                Milight.Bridge bridge = bridges.Find(b => b.GetId() == group.BridgeId);
                return bridge;
            }
            return null;
        }

        public bool SetLight(int id, bool state)
        {
            Bridge bridge = GetBridgeByGroupId(id);
            if (bridge != null)
            {
                var group = GetGroupById(id);
                Milight.Enums.Command command = Milight.Enums.Command.None;

                switch ((Milight.Enums.GroupType)group.GroupType)
                {
                    case Milight.Enums.GroupType.One:
                        command = (state) ? Milight.Enums.Command.RGBWGroup1AllOn : Milight.Enums.Command.RGBWGroup1AllOff;
                        break;
                    case Milight.Enums.GroupType.Two:
                        command = (state) ? Milight.Enums.Command.RGBWGroup2AllOn : Milight.Enums.Command.RGBWGroup2AllOff;
                        break;
                    case Milight.Enums.GroupType.Three:
                        command = (state) ? Milight.Enums.Command.RGBWGroup3AllOn : Milight.Enums.Command.RGBWGroup3AllOff;
                        break;
                    case Milight.Enums.GroupType.Four:
                        command = (state) ? Milight.Enums.Command.RGBWGroup4AllOn : Milight.Enums.Command.RGBWGroup4AllOff;
                        break;
                    default:
                        break;
                }

                if (command != Milight.Enums.Command.None)
                {
                    Task.Run(async () => {

                        await bridge.SendCommandAsync(command);

                    }).Wait();

                    group.LastState = state;
                    Database.Active.GetConnection().Update(group);
                    return true;
                }
                else
                    return false;
            }
            return false;
        }

        // We need to prevent the Brightness to be set over and over again
        private DateTime lastBrightnessRequest;
        private Timer brightnessRequestTimer = null;

        public bool SetBrightness(int id, int val)
        {
            lastBrightnessRequest = DateTime.UtcNow;
            if(brightnessRequestTimer == null)
            {
                brightnessRequestTimer = new Timer(SetBrightnessCallback, new object[] { id, val }, 1500, 1500);
            }

            return true;
        }

        private void SetBrightnessCallback(object state)
        {
            if (DateTime.UtcNow >= (lastBrightnessRequest + TimeSpan.FromMilliseconds(1200))) {
                object[] data = (object[])state;
                SetBrightness((int)data[0], (int)data[1], false);

                brightnessRequestTimer.Dispose();
                brightnessRequestTimer = null;
            }
        }

        public bool SetBrightness(int id, int val, bool ignoreCombo)
        {
            Bridge bridge = GetBridgeByGroupId(id);
            if (bridge != null)
            {
                DatabaseModel.MilightCombo combo = null;
                if(!ignoreCombo)
                    combo = Database.Active.GetConnection().Table<DatabaseModel.MilightCombo>().Where(c => c.FirstGroupId == id || c.SecondGroupId == id).FirstOrDefault();
                if (combo != null)
                {
                    SetBrightness(combo.FirstGroupId, val, true);
                    SetBrightness(combo.SecondGroupId, val, true);

                    combo.LastState = true;
                    Database.Active.GetConnection().Update(combo);
                    return true;
                }
                else
                {
                    var group = GetGroupById(id);
                    if(group != null)
                    {
                        SetLight(group.Id, true);
                        Task.Run(async () => {

                            await bridge.SendCommandAsync(Milight.Enums.Command.RGBWBrightness, (byte)val);

                        }).Wait();

                        group.Brightness = val;
                        Database.Active.GetConnection().Update(group);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool SetLightCombo(int id, bool state)
        {
            Bridge bridge = GetBridgeByGroupId(id);
            if (bridge != null)
            {
                var combo = Database.Active.GetConnection().Table<DatabaseModel.MilightCombo>().Where(c => c.FirstGroupId == id || c.SecondGroupId == id).FirstOrDefault();

                if (combo != null)
                {
                    SetLight(combo.FirstGroupId, state);
                    SetLight(combo.SecondGroupId, state);

                    combo.LastState = state;
                    Database.Active.GetConnection().Update(combo);
                    return true;
                }
            }
            return false;
        }
    }
}
