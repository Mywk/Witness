using Restup.Webserver.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WITNESS.RestControllers
{
    public sealed class Common
    {
        internal static GetResponse RestGetResponse(bool success)
        {
            if (success)
                return new GetResponse(GetResponse.ResponseStatus.OK, null);
            else
                return new GetResponse(GetResponse.ResponseStatus.NotFound, null);
        }
        internal static GetResponse RestGetResponse(bool success, List<object> parameters)
        {
            if (success)
                return new GetResponse(GetResponse.ResponseStatus.OK, parameters);
            else
                return new GetResponse(GetResponse.ResponseStatus.NotFound, null);
        }
        internal static GetResponse RestGetResponse(bool success, List<object[]> parameters)
        {
            if (success)
                return new GetResponse(GetResponse.ResponseStatus.OK, parameters);
            else
                return new GetResponse(GetResponse.ResponseStatus.NotFound, null);
        }
    }
}
