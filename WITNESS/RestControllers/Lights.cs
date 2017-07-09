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
    public sealed class Lights
    {
        [UriFormat("/light/{id}/{state}")]
        public IGetResponse SetLight(int id, bool state)
        {
            return Common.RestGetResponse(LightsControlCenter.Active.SetLight(id, state), new List<object> { id.ToString(), state.ToString() }); 
        }

        [UriFormat("/light/combo/{id}/{state}")]
        public IGetResponse SetLightCombo(int id, bool state)
        {
            return Common.RestGetResponse(LightsControlCenter.Active.SetLightCombo(id, state), new List<object> { id.ToString(), state.ToString() });
        }

        [UriFormat("/light/brightness/{id}/{val}")]
        public IGetResponse SetBrightness(int id, int val)
        {
            return Common.RestGetResponse(LightsControlCenter.Active.SetBrightness(id, val), new List<object> { id.ToString(), val.ToString() });
        }
    }
}
