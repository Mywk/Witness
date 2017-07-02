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
    public sealed class Power
    {
        [UriFormat("/power/{id}/{status}")]
        public IGetResponse GetPower(int id, bool status)
        {
            if (GpioControlCenter.Active.SetPower(id, status))
                return new GetResponse(
      GetResponse.ResponseStatus.OK, new List<string> { id.ToString(), status.ToString() });
            else
                return new GetResponse(GetResponse.ResponseStatus.NotFound, null);
        }
    }
}
