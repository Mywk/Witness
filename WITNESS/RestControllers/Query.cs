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
                List<object[]> dic = new List<object[]>();

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

                        dic.Add(new object[] { timer.Id, string.Format("{0:D2}:{1:D2}", timespan_from.Hours, timespan_from.Minutes), string.Format("{0:D2}:{1:D2}", timespan_to.Hours, timespan_to.Minutes) });
                    }
                }
                catch (Exception)
                {
                    queryResult = false;
                }

                return Common.RestGetResponse(queryResult, dic);
            }
        }

        [UriFormat("/query/relay")]
        public IGetResponse GetPower()
        {
            List<object[]> dic = new List<object[]>();

            bool queryResult = true;
            try
            {
                var query = Database.Active.GetConnection().Table<DatabaseModel.Relay>();
                foreach (var relay in query)
                {
                    dic.Add(new object[] { relay.Id, relay.Name, relay.LastState, relay.TimerActive });
                }
            }
            catch (Exception)
            {
                queryResult = false;
            }

            return Common.RestGetResponse(queryResult, dic);
        }

        [UriFormat("/query/relay/timer/{powerId}")]
        public IGetResponse GetTimer(int powerId)
        {
            return Helper.GetTimers(powerId, Enums.TimerType.Relay);
        }
    }
}
