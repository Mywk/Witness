using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

#pragma warning disable 4014

namespace WITNESS
{
    public sealed class TimersControlCenter
    {
        internal static TimersControlCenter Active;

        private class PeriodicTask
        {
            public static async Task Run(Action action, TimeSpan period, CancellationToken cancellationToken)
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(period, cancellationToken);

                    if (!cancellationToken.IsCancellationRequested)
                        action();
                }
            }

            public static Task Run(Action action, TimeSpan period)
            {
                return Run(action, period, CancellationToken.None);
            }
        }

        private PeriodicTask task;
        private CancellationToken cancellationToken;

        public TimersControlCenter()
        {
            cancellationToken = new CancellationToken();

            // Do NOT await!
            PeriodicTask.Run(CheckRelayTimers, new TimeSpan(0, 0, 10), cancellationToken);
        }

        public bool SetTimer(int type, int id, bool state)
        {
            switch ((Enums.TimerType)type)
            {
                case Enums.TimerType.Relay:

                    var relays = Database.Active.GetConnection().Table<DatabaseModel.Relay>().Where(r => (r.Id == id)).ToList();
                    if (relays.Count > 0)
                    {
                        // If we are setting the timer and the relay is active we need to disable it

                        relays[0].TimerActive = state;

                        if (RelayControlCenter.Active.SetPower(id, false, false))
                            relays[0].LastState = false;
                        else return false;

                        Database.Active.GetConnection().Update(relays[0]);
                        return true;
                    }

                    break;
                default:
                    break;
            }

            return false;
        }

        private void CheckRelayTimers()
        {
            if (Database.Active != null && RelayControlCenter.Active != null)
            {
                var relays = Database.Active.GetConnection().Table<DatabaseModel.Relay>().Where(r => (r.TimerActive == true)).ToList();
                foreach (var relay in relays)
                {
                    var timers = Database.Active.GetConnection().Table<DatabaseModel.Timer>().Where(t => (t.Type == (int)Enums.TimerType.Relay) && t.TargetId == relay.Id).ToList();
                    foreach (var timer in timers)
                    {
                        TimeSpan timespan_from = TimeSpan.FromSeconds(timer.From);
                        TimeSpan timespan_to = TimeSpan.FromSeconds(timer.To);

                        TimeSpan now = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                        if ((now > timespan_from) && (now < timespan_to))
                        {
                            if (relay.LastState != true)
                            {
                                if (RelayControlCenter.Active.SetPower(relay.Id, true))
                                {
                                    relay.LastState = true;
                                    Database.Active.GetConnection().Update(relay);
                                }
                            }
                        }
                        else if (relay.LastState != false)
                        {
                            if (RelayControlCenter.Active.SetPower(relay.Id, false))
                            {
                                relay.LastState = false;
                                Database.Active.GetConnection().Update(relay);
                            }
                        }
                    }
                }
            }
        }
    }
}
