using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace Ex4.Models
{
    public class InfoModel
    {

        #region SingleTon
        private static InfoModel m_Instance = null;
        public static InfoModel Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new InfoModel();
                return m_Instance;
            }
        }
        #endregion

        // private members
        private TcpListener listener;
        private NetworkStream stearm;
        private StreamReader reader;
        private TcpClient client;
        private bool connected = false; 
        public InfoModel() { }

        public void Connect(int port, string id)
        {

            IPEndPoint ep = new IPEndPoint(IPAddress.Any, port);
            listener = new TcpListener(ep);
            // start listening for client requests.
            listener.Start();
            try
            {
                // after the connection, get client.
                client = listener.AcceptTcpClient();
                connected = true;
                stearm = client.GetStream();
                reader = new StreamReader(stearm);

            }
            catch (SocketException) { }
        }
        public string[] get()
        {
            string commnadLine = reader.ReadLine();
            string[] tokens = commnadLine.Split(',');
            string lon = tokens[0];
            string lat = tokens[1];
            
            return tokens;
        }
        // stop listenning for another requests
        public void Stop()
        {
            stearm.Close();
            client.Close();
            listener?.Stop();
        }

        public bool IsConnected()
        {
            return connected;
        }
    }
}