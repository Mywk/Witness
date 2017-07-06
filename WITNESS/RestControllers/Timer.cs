using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WITNESS.RestControllers;

namespace WITNESS.RestControllers
{
    [RestController(InstanceCreationType.Singleton)]
    public sealed class Timer
    {
        [UriFormat("/timer/set/{relayId}/{timers}")]
        public IGetResponse SetTimer(int relayId, string timers)
        {
            // This is so overkill and unecessary :(
            bool validData = true;
            try
            {
                string[] _timers = timers.Split(';');
                foreach (var _timer in _timers)
                {
                    string[] data = _timer.Split(',');
                    if (data.Length == 2)
                    {
                        string[] dataFrom = data[0].Split(':');
                        string[] dataTo = data[1].Split(':');

                        int fromHours = Int32.Parse(dataFrom[0]);
                        int fromMinutes = Int32.Parse(dataFrom[1]);
                        int toHours = Int32.Parse(dataTo[0]);
                        int toMinutes = Int32.Parse(dataTo[1]);
                        TimeSpan from = new TimeSpan(fromHours, fromMinutes, 0);
                        TimeSpan to = new TimeSpan(toHours, toMinutes, 0);

                        // Delete all previous timers for that id, and yes, this is horrible, just need it working fast
                        var old_timers = Database.Active.GetConnection().Table<DatabaseModel.Timer>().Where(t => (t.Type == (int)Enums.TimerType.Relay) && t.TargetId == relayId).ToList();
                        foreach (var timer in old_timers)
                        {
                            Database.Active.GetConnection().Delete(timer);
                        }

                        Database.Active.GetConnection().Insert(new DatabaseModel.Timer()
                        {
                            TargetId = relayId,
                            From = from.TotalSeconds,
                            To = to.TotalSeconds
                        });
                    }
                }
            }
            catch (Exception)
            {
                validData = false;
            }
            return Common.RestGetResponse(validData);
        }
    }
}
