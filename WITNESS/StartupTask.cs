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
            restRouteHandler.RegisterController<RestControllers.Power>();

            var configuration = new HttpServerConfiguration()
              .ListenOnPort(81)
              .RegisterRoute(new StaticFileRouteHandler(@"web"))
              .RegisterRoute("api", restRouteHandler)
              .EnableCors();

            var httpServer = new HttpServer(configuration);
            await httpServer.StartServerAsync();
            GpioControlCenter.Active = new GpioControlCenter();

            Database.Active = new Database();
        }

    }

}
