using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Restup.Webserver.Rest;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Http;
using Restup.Webserver.File;
using Windows.Devices.Gpio;
using SQLite;
using System.IO;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace WITNESS
{
    public sealed class StartupTask : IBackgroundTask
    {
        private static BackgroundTaskDeferral _Deferral = null;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _Deferral = taskInstance.GetDeferral();

            var restRouteHandler = new RestRouteHandler();
            restRouteHandler.RegisterController<RestControllers.Query>();
            restRouteHandler.RegisterController<RestControllers.Relay>();
            restRouteHandler.RegisterController<RestControllers.Timer>();

            var configuration = new HttpServerConfiguration()
              .ListenOnPort(81)
              .RegisterRoute(new StaticFileRouteHandler(@"web"))
              .RegisterRoute("api", restRouteHandler)
              .EnableCors();

            var httpServer = new HttpServer(configuration);
            await httpServer.StartServerAsync();

            // Initialize the database
            Database.Active = new Database();

            GpioControlCenter.Active = new GpioControlCenter();
            RelayControlCenter.Active = new RelayControlCenter();

            LoadLastStates();


            // Start the timer controler
            TimersControlCenter.Active = new TimersControlCenter();
        }

        private void LoadLastStates()
        {
            var relays = Database.Active.GetConnection().Table<DatabaseModel.Relay>().ToList();
            foreach (var relay in relays)
            {
                // Only set if LastState is true, everything is disabled by default
                if(relay.LastState)
                    RelayControlCenter.Active.SetPower(relay.Id, relay.LastState);
            }
        }

    }

}