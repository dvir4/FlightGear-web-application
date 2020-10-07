using Ex4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Ex4.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        static string lonPath = "position/longitude-deg";
        static string latPath = "position/latitude-deg";
        static string throttlePath = "/controls/engines/current-engine/throttle";
        static string rudderPath = "/controls/flight/rudder";

        public ActionResult display(string ip, int port, int time)
        {
            
            if (ClientModel.Instance.IsConnact)
            {
                ClientModel.Instance.Close();
            }

            ClientModel.Instance.Connect(port, ip);
            ViewBag.lon = double.Parse(ClientModel.Instance.Get(lonPath));
            ViewBag.lat = double.Parse(ClientModel.Instance.Get(latPath));
            
            ViewBag.time = time;
            return View();
        }

        [HttpPost]
        public string GetLocation()
        {
            if (ClientModel.Instance.IsConnact)
            {
                var point = new Point();
                point.Lon = double.Parse(ClientModel.Instance.Get(lonPath));
                point.Lat = double.Parse(ClientModel.Instance.Get(latPath));
                return ToXml(point);
            //return FileManager.Instance.GetPoint();
            }
            return null;
        }

        private string ToXml(Point point)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Points");

            point.ToXml(writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

        public ActionResult Save(string ip, int port, int time, int period,string fileName)
        {
            FileManager.Instance.SetFileName(fileName);
            if (ClientModel.Instance.IsConnact)
            {
                ClientModel.Instance.Close();
            }

            ClientModel.Instance.Connect(port, ip);
            Point startPoint = new Point();
            startPoint.Lon = ViewBag.Lon = double.Parse(ClientModel.Instance.Get(lonPath));
            startPoint.Lat =  ViewBag.Lat = double.Parse(ClientModel.Instance.Get(latPath));
            startPoint.Throttle = double.Parse(ClientModel.Instance.Get(throttlePath));
            startPoint.Rudder = double.Parse(ClientModel.Instance.Get(rudderPath));

            FileManager.Instance.AddPoint(startPoint);
            ViewBag.time = time;
            ViewBag.period = period;

            return View();
        }


        [HttpPost]
        public string SaveGetLocation()
        {
            if (ClientModel.Instance.IsConnact)
            {
                // create point, save values.
                var point = new Point();
                point.Lon = double.Parse(ClientModel.Instance.Get(lonPath));
                point.Lat = double.Parse(ClientModel.Instance.Get(latPath));
                point.Throttle = double.Parse(ClientModel.Instance.Get(throttlePath));
                point.Rudder = double.Parse(ClientModel.Instance.Get(rudderPath));
                // add point to list.
                FileManager.Instance.AddPoint(point);
                return ToXml(point);

            }
            return null;
        }
        [HttpPost]
        public void SaveListOfPoints()
        {
            FileManager.Instance.SaveData(); 
        }

        public ActionResult load(string file,int time)
        {
            FileManager.Instance.SetFileName(file);
            FileManager.Instance.LoadData();
            ViewBag.time = time;
            return View();

        }

        [HttpPost]
        public string GetPointAfterSave()
        {
            List<string> listOfXmlPoints = new List<string>();
            if (!FileManager.Instance.IsLoaded)
            {
                listOfXmlPoints = FileManager.Instance.LoadData();
            }
            string xmlPoint = FileManager.Instance.GetPoint();
            return xmlPoint;
        }
    }
}
