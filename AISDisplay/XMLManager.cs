using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using SerialPortListener.Serial;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.ComponentModel;

namespace AISDisplay
{
    class XMLManager
    {
        public static string fileName;
        public static string fileDirectory;
        private static SerialSettings serialSettings;
        private static FileSystemWatcher watcher;
        public XMLManager()
        {
            fileName = "COMSettings.xml";
            Task.Factory.StartNew(() => VerifyXMLFile());
            
        }

        public static void VerifyXMLFile()
        {
            fileDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Create a new FileSystemWatcher and set its properties.
            using (watcher = new FileSystemWatcher())
            {
                watcher.Path = fileDirectory;

                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files or directories.
                watcher.NotifyFilter = NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName;

                // Only watch this.
                watcher.Filter = fileName;

                // Add event handlers.
                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;
                watcher.Renamed += OnRenamed;

                // Begin watching.
                watcher.EnableRaisingEvents = true;


                while (watcher.EnableRaisingEvents) ;
            }
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e) => FileChanged(source, e);

        private static void OnRenamed(object source, RenamedEventArgs e) => FileRenamed(source, e);

        private static void FileChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");

            switch (e.ChangeType)
            {
                case (WatcherChangeTypes)ListChangedType.ItemChanged:
                    {
                        break;
                    }
                    
                case (WatcherChangeTypes)ListChangedType.ItemAdded:
                    break;
                case (WatcherChangeTypes)ListChangedType.ItemDeleted:
                    {
                        SerializeDataToXML(serialSettings);
                        break;
                    }
                    
                default:
                    break;
            }
        }

        private static void FileRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            fileName = e.Name;
            watcher.Filter = fileName;
            Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
        }

        public static void SerializeDataToXML(SerialSettings _spSettings)
        {
            serialSettings = _spSettings;
            XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8);
            writer.WriteStartDocument(true);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            writer.WriteStartElement("COMPort_Values");
            createNode(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        private static void createNode(XmlTextWriter writer)
        {
            string comListString = "", baudRateString = "", bitString = "";
            foreach (string comPort in serialSettings.PortNameCollection)
            {
                if (serialSettings.PortNameCollection.Last() == comPort)
                    comListString += comPort;
                else
                    comListString += comPort + ", ";
            };
            foreach (int baudRate in serialSettings.BaudRateCollection)
            {
                if (serialSettings.BaudRateCollection.Last() == baudRate)
                    baudRateString += baudRate.ToString();
                else
                    baudRateString += baudRate.ToString() + ", ";
            };
            foreach (int dataBit in serialSettings.DataBitsCollection)
            {
                if (serialSettings.DataBitsCollection.Last() == dataBit)
                    bitString += dataBit.ToString();
                else
                    bitString += dataBit.ToString() + ", ";
            };


            writer.WriteStartElement("COMPort");
            //LIST OF ALL COMPORTS
            writer.WriteStartElement("COMPort_List");
            writer.WriteString(comListString);
            writer.WriteEndElement();
            //NAME
            writer.WriteStartElement("COMPort_Name");
            writer.WriteString(serialSettings.PortName);
            writer.WriteEndElement();
            //BAUDRATE COLLECTION
            writer.WriteStartElement("Baud_Collection");
            writer.WriteString(baudRateString);
            writer.WriteEndElement();
            //BAUDRATE
            writer.WriteStartElement("Baud_Rate");
            writer.WriteString(serialSettings.BaudRate.ToString());
            writer.WriteEndElement();
            //BIT COLLECTION
            writer.WriteStartElement("Bit_Collection");
            writer.WriteString(bitString);
            writer.WriteEndElement();
            //DATA BITS
            writer.WriteStartElement("Data_Bits");
            writer.WriteString(serialSettings.DataBits.ToString());
            writer.WriteEndElement();
            //PARITY
            writer.WriteStartElement("Parity");
            writer.WriteString(serialSettings.Parity.ToString());
            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        public static void updateNode(SerialSettings _spSettings, String propertyName)
        {
            serialSettings = _spSettings;
            XDocument xmlDoc = XDocument.Load(fileName);
            XElement xRootElement = xmlDoc.Root.Element("COMPort");

            switch (propertyName)
            {
                case "PortName":
                    {
                        xRootElement.Element("COMPort_Name").Value = serialSettings.PortName;
                        break;
                    }
                case "BaudRate":
                    {
                        xRootElement.Element("Baud_Rate").Value = serialSettings.BaudRate.ToString();
                        break;
                    }
                case "DataBits":
                    {
                        xRootElement.Element("Data_Bits").Value = serialSettings.DataBits.ToString();
                        break;
                    }
                case "Parity":
                    {
                        xRootElement.Element("Parity").Value = serialSettings.Parity.ToString();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        public static SerialSettings updateSettingsFromXML()
        {
            //TODO:: This
            return serialSettings;
        }

    }

}
