using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WITNESS.RestControllers
{
    [RestController(InstanceCreationType.Singleton)]
    public sealed class Query
    {
        static class Helper
        {
            public static IGetResponse GetTimers(int id, Enums.TimerType type)
            {
                List<object[]> list = new List<object[]>();

                bool queryResult = true;
                try
                {
                    var timers = Database.Active.GetConnection().Table<DatabaseModel.Timer>().Where(t => (t.TargetId == id && t.Type == (int)type)).ToList();
                    foreach (var timer in timers)
                    {
                        TimeSpan timespan_from = TimeSpan.FromSeconds(timer.From);
                        TimeSpan timespan_to = TimeSpan.FromSeconds(timer.To);

                        if(timespan_from > timespan_to)
                        {
                            TimeSpan temp = timespan_from;
                            timespan_from = timespan_to;
                            timespan_to = temp;
                        }

                        list.Add(new object[] { timer.Id, string.Format("{0:D2}:{1:D2}", timespan_from.Hours, timespan_from.Minutes), string.Format("{0:D2}:{1:D2}", timespan_to.Hours, timespan_to.Minutes) });
                    }
                }
                catch (Exception)
                {
                    queryResult = false;
                }

                return Common.RestGetResponse(queryResult, list);
            }
        }

        [UriFormat("/query/relay")]
        public IGetResponse GetPower()
        {
            List<object[]> list = new List<object[]>();

            bool queryResult = true;
            try
            {
                var query = Database.Active.GetConnection().Table<DatabaseModel.Relay>();
                foreach (var relay in query)
                {
                    list.Add(new object[] { relay.Id, relay.Name, relay.LastState, relay.TimerActive });
                }
            }
            catch (Exception)
            {
                queryResult = false;
            }

            return Common.RestGetResponse(queryResult, list);
        }

        [UriFormat("/query/relay/timer/{powerId}")]
        public IGetResponse GetTimer(int powerId)
        {
            return Helper.GetTimers(powerId, Enums.TimerType.Relay);
        }

        [UriFormat("/query/lights")]
        public IGetResponse GetLights()
        {
            List<object[]> list = new List<object[]>();
            Dictionary<int, int> foundCombos = new Dictionary<int,int>();

            bool queryResult = true;
            try
            {
                var bridges = Database.Active.GetConnection().Table<DatabaseModel.MilightBridge>();
                foreach (var bridge in bridges)
                {
                    var groups = Database.Active.GetConnection().Table<DatabaseModel.MilightGroup>();
                    foreach (var group in groups)
                    {
                        var combos = Database.Active.GetConnection().Table<DatabaseModel.MilightCombo>().Where(c => (c.FirstGroupId == group.Id || c.SecondGroupId == group.Id)).ToList();
                        if (combos.Count > 0)
                        {
                            if (foundCombos.ContainsKey(group.Id) || foundCombos.ContainsValue(group.Id))
                                list.Add(new object[] { combos[0].Id,  group.BridgeId, combos[0].Name, combos[0].Brightness * 10, combos[0].Color, combos[0].LastState, true });
                            else
                                foundCombos.Add(combos[0].FirstGroupId, combos[0].SecondGroupId);
                            continue;
                        }
                        else
                        {
                            list.Add(new object[] { group.Id, group.BridgeId, group.Name, group.Brightness * 10, group.Color, group.LastState, false });
                        }
                    }
                }
            }
            catch (Exception)
            {
                queryResult = false;
            }

            return Common.RestGetResponse(queryResult, list);
        }
    }
}
