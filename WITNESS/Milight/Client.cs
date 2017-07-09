using System;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace WITNESS.Milight
{
    class Client
    {
        private static readonly int delay = 150;
        internal string outHost = "", outPort = "8899";
        internal string inHost, inPort;

        // Remove me
        // UDP datagram socket @ https://docs.microsoft.com/en-us/uwp/api/windows.networking.sockets.datagramsocket
        private DatagramSocket socket;
        private DataWriter dataWriter;

        // Holds messages received
        protected StringBuilder dataBuffer = new StringBuilder();

        // Just in case
        private static readonly object commandLock = new object();

        internal bool isAdmin = false;

        public Client(string host, string port)
        {
            this.outHost = host;
            this.outPort = port;
        }

        public async Task SendDataAsync(string data)
        {
            await SendDataAsync(Encoding.UTF8.GetBytes(data));
        }

        public async Task SendDataAsync(byte[] data)
        {
            if (socket == null)
            {
                lock (commandLock)
                {
                    if (socket == null)
                        socket = new DatagramSocket();
                }

                await socket.ConnectAsync(new HostName(outHost), outPort);
                if (isAdmin)
                    socket.MessageReceived += socket_MessageReceived;
                dataWriter = new DataWriter(socket.OutputStream);
            }
            dataWriter.WriteBytes(data);

            await dataWriter.StoreAsync();
            await DelayAsync();
        }

        public async Task DelayAsync()
        {
            await Task.Delay(delay);
        }

        // When we receive a message we need to append the data to the buffer
        private void socket_MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            DataReader reader = args.GetDataReader();
            dataBuffer.AppendLine(reader.ReadString(reader.UnconsumedBufferLength));
        }

        public void Close()
        {
            socket.Dispose();
        }
    }
}
