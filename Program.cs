using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace SimpleSniffer
{
    static class Program
    {
        //some consts
        static Dictionary<int, string> p = new Dictionary<int, string>() { { 1, "ICMP" }, { 6, "TCP" }, { 17, "UDP" } };
        const string RowHeader = "Year,Month,Day,Hour,Minute,Second,ms,ReceiverIP,Protocol,SourceIP,Port,DestinationIP,Port,Size";
        const string RowFormat = "{0},{1},{2},{3},{4},{5},{6}";
        const string TimeFormat = "yyyy,MM,HH,mm,ss,fff";

        static void Main()
        {
            
            // Get local IPv4 Addresses
            var IPv4Addresses = Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.Where(al => al.AddressFamily == AddressFamily.InterNetwork)
                .AsEnumerable();
            
            // Header Row
            Console.WriteLine(RowHeader);
            
            // start a sniffer for each IPv4 Interface
            foreach (IPAddress ip in IPv4Addresses)
                Sniff(ip.ToString());

            // wait until a key is pressed
            Console.Read();
            
        }

        static void Sniff(string ip)
        {

            // http://msdn.microsoft.com/en-us/library/system.net.sockets.socket.aspx
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
            s.Bind(new IPEndPoint(IPAddress.Parse(ip), 0));
            s.IOControl(IOControlCode.ReceiveAll, BitConverter.GetBytes(1), null);

            //assuming default (20byte) IP header size + 4 bytes for TCP header to get ports
            byte[] b = new byte[24];

            // Async methods for recieving and processing data
            Action<IAsyncResult> r = null;
            r = (ar) =>
            {
                int pNum = b[9]; //cache this otherwise have to make buffer call 3 times
                //echo the data. details at http://en.wikipedia.org/wiki/IPv4_packet#Packet_structure
                Console.WriteLine( 
                    RowFormat , DateTime.Now.ToString(TimeFormat) , ip
                    , p.ContainsKey(pNum) ? p[pNum] : "#" + pNum
                    , new IPAddress(BitConverter.ToUInt32(b, 12)).ToString()
                    , ((ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(b, 20))).ToString()
                    , new IPAddress(BitConverter.ToUInt32(b, 16)).ToString()
                    , ((ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(b, 22))).ToString()
                    , ((ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(b, 2))).ToString());
                
                //clean out our buffer
                b = new byte[24];
                
                //listen some more
                s.BeginReceive(b, 0, 24, SocketFlags.None, new AsyncCallback(r), null);
            };

            // begin listening to the socket
            s.BeginReceive(b, 0, b.Length, SocketFlags.None,
                    new AsyncCallback(r), null);
        }
    }
}

                            