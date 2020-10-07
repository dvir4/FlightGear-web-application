using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace Ex4.Models
{
    public class ClientModel
    {
        private TcpClient clientTcp;
        private NetworkStream stream;
        private StreamReader reader;

        public bool IsConnact { set; get; } = false;

        #region SingleTon
        private static ClientModel m_Instance = null;
        public static ClientModel Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new ClientModel();
                return m_Instance;
            }
        }
        #endregion

        public void Connect(int port, string serverIp)
        {
            clientTcp = new TcpClient();
            while (!clientTcp.Connected)
            {
                try { clientTcp.Connect(IPAddress.Parse(serverIp), port); }
                catch (Exception) { }
            }
            stream = clientTcp.GetStream();
            IsConnact = true;
            reader = new StreamReader(stream);
            Console.WriteLine("u are connacted");
        }

        public string Get(string path)
        {
            string msg = "get" + " " + path + "\r\n";
            byte[] massegeToSend = ASCIIEncoding.ASCII.GetBytes(msg);
            stream.Write(massegeToSend, 0, massegeToSend.Length);

            string commnadLine = reader.ReadLine().Split('\'')[1];
            return commnadLine;
        }
    

        public void Close()
        {
            clientTcp.Close();
            stream.Close();
            IsConnact = false;
        }
    }
}
