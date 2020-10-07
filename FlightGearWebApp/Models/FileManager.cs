using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Ex4.Models
{
    public class FileManager
    {
        
        private const string direction = "~/App_Data/{0}.xml";
        private XmlWriterSettings settings;
        private string path;
        private List<Point> listOfPoints;
        private List<string> listOfXmlPoints;
        public bool IsLoaded { set; get; }
        //public string FileName { set; get; }

        #region SingleTon
        private static FileManager m_Instance = null;
        public static FileManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new FileManager();
                return m_Instance;
            }
        }
        #endregion

        public FileManager()
        {
            listOfPoints = new List<Point>();
            listOfXmlPoints = new List<string>();
            //path = HttpContext.Current.Server.MapPath(String.Format(direction));
            settings = new XmlWriterSettings();
            IsLoaded = false;
            
        }

        public void SetFileName(string fileName)
        {
            path = HttpContext.Current.Server.MapPath(String.Format(direction,fileName));
        }

        public void AddPoint(Point p)
        {
            listOfPoints.Add(p);
        }
        public void SaveData()
        {
            XmlWriter writer = XmlWriter.Create(path, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Points");
            foreach (Point point in listOfPoints)
            {
                //point.ToXml(writer);
                point.ToFileXml(writer);
                writer.Flush();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
            listOfPoints.Clear();
        }
        
        public List<string> LoadData()
        {
            
            // load xml file.
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            //List<string> listOfXmlPoints = new List<string>();
            StringBuilder sb = new StringBuilder();
            // for each point in xml file, create a xml format and add to list.
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                XmlWriter writer = XmlWriter.Create(sb, settings);
               
                writer.WriteStartDocument();
                writer.WriteStartElement("Points");
                
                // get lon value.
                writer.WriteRaw(node.ChildNodes[0].OuterXml);
                // get lan value.
                writer.WriteRaw(node.ChildNodes[1].OuterXml);
                // get point node from xml file.
                //writer.WriteRaw(node.OuterXml);

                writer.WriteEndElement();
                writer.WriteEndDocument();

                writer.Flush();
                // add point as xml format to list.
                listOfXmlPoints.Add(sb.ToString());
                sb.Clear();
            }
            IsLoaded = true;
            return listOfXmlPoints;
        }

        public string GetPoint()
        {
            if (listOfXmlPoints.Count() == 0)
                return null;
            // get first Point
            string point = listOfXmlPoints[0];
            listOfXmlPoints.RemoveAt(0);
            return point;
        }
    }
}