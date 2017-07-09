using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WITNESS.RestControllers;
using WITNESS.Controllers;

namespace WITNESS.RestControllers
{
    [RestController(InstanceCreationType.Singleton)]
    public sealed class Relays
    {
        [UriFormat("/relay/power/{id}/{state}")]
        public IGetResponse SetPower(int id, bool state)
        {
            return Common.RestGetResponse(RelayControlCenter.Active.SetPower(id, state), new List<object> { id.ToString(), state.ToString() }); 
        }

        [UriFormat("/relay/timer/{id}/{state}")]
        public IGetResponse SetTimer(int id, bool state)
        {
            return Common.RestGetResponse(TimersControlCenter.Active.SetTimer((int)Enums.TimerType.Relay, id, state), new List<object> { id.ToString(), state.ToString() });
        }
    }
}
