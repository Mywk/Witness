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
        [UriFormat("/query/power")]
        public IGetResponse GetPower()
        {

            try
            {
                List<object[]> dic = new List<object[]>();

                var query = Database.Active.GetConnection().Table<DatabaseModel.Relays>();
                foreach (var relay in query)
                {
                    dic.Add(new object[] { relay.Id, relay.Name, relay.DefaultState });
                }

                return new GetResponse(GetResponse.ResponseStatus.OK, dic);
            }
            catch (Exception)
            {
                return new GetResponse(GetResponse.ResponseStatus.NotFound, null);
            }
        }
    }
}
