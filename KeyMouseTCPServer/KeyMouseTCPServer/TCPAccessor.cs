using System;
using System.Net;
using System.Net.Sockets;

namespace KeyMouseTCPServer
{
    class TCPAccessor
    {
        private delegate void Delegate_write(string str);
        public String ipAddress;
        public int portNumber;
        public String data;
        public TCPAccessor(String ipAddress, int portNumber)
        {
            this.ipAddress = ipAddress;
            this.portNumber = portNumber;
        }
        public void connect()
        {

            // C# TCPソケット通信・サーバ
            // http://www.macs123.dtdns.net/algo/cs/cs008.html
            try
            {
                //サーバーを開始 
                //Console.WriteLine("サーバーを開始");
                Int32 port = portNumber;// 8087;
                IPAddress localAddr = IPAddress.Parse(ipAddress);
                TcpListener server = new TcpListener(localAddr, port);
                server.Start();

                while (true)
                {
                    //接続待機 
                    Console.WriteLine("接続待機中");
                    TcpClient client = server.AcceptTcpClient();

                    //接続 
                    Console.WriteLine("接続されました");
                    NetworkStream stream = client.GetStream();

                    Byte[] bytes = new Byte[256];
                    int siz;

                    //メッセージを受信 
                    while ((siz = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        String str = System.Text.Encoding.GetEncoding(65001).GetString(bytes, 0, siz);
                        data += str;
                    }

                    stream.Close();
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
