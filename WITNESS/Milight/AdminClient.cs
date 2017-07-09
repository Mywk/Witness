using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WITNESS.Milight
{
    class AdminClient : Client
    {
        public const string defaultInHost = "0.0.0.0", defaultInPort = "8899";

        public AdminClient(string host, string port = defaultInPort,
            string innerHost = defaultInHost, string innerPort = defaultInPort) : base(host, port)
        {
            this.isAdmin = true;
            this.inHost = host;
            this.inPort = port;
        }

        // TODO
        public async Task<string> ReceiveDataAsync()
        {
            string temp = dataBuffer.ToString();
            dataBuffer.Clear();
            // TODO
            await Task.Run(() => { });
            return temp;
        }
    }
}
