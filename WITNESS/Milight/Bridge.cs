using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WITNESS.Milight
{
    class Bridge
    {
        private static readonly int delay = 100;

        private static readonly object commandLock = new object();
        private string name, host, port;
        private int id;

        public int GetId()
        {
            return this.id;
        }

        private Client client = null;

        public Bridge(int id, string name, string host, string port)
        {
            this.id = id;
            this.name = name;
            this.host = host;
            this.port = port;
        }

        private async Task DelayAsync()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(delay));
        }

        public async Task SendCommandAsync(Enums.Command command, byte param)
        {
            await SendCommandAsync(Commands.GetCommandSequenceWithParam((int)command, param));
        }

        public async Task SendCommandAsync(Enums.Command command)
        {
            await SendCommandAsync(Commands.GetCommandSequence((int)command));
        }

        public async Task SendCommandAsync(byte[] buffer)
        {
            if (client == null)
            {
                lock (commandLock)
                {
                    if (client == null)
                        client = new Client(host, port);
                }
            }
            await client.SendDataAsync(buffer);
        }

        // Adapted from a MiLight plugin used in openHAB
        private static Regex ipIdRegex = new Regex(@"^(?<ip>\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b),.*(?<id>[0-9a-zA-Z]{12}).*$");

        // Currently unused, implemented for testing purposes but not sure if ever going to use it
        public static async Task<Dictionary<string, string>> GetBridgesAsync(string host)
        {
            var bridges = new Dictionary<string, string>();
            var client = new AdminClient(host);
            try
            {
                for (var i = 0; i < 10; i++)
                {
                    await client.SendDataAsync("Link_Wi-Fi");
                }
                var data = string.Empty;
                while (!string.IsNullOrEmpty(data = await client.ReceiveDataAsync()))
                {
                    var ma = ipIdRegex.Match(data);
                    if (ma.Success)
                    {
                        var id = ma.Groups["id"].Value;
                        if (!bridges.ContainsKey(id))
                            bridges.Add(id, ma.Groups["ip"].Value);
                    }
                }
            }
            finally
            {
                client.Close();
            }
            return bridges;
        }
    }
}
