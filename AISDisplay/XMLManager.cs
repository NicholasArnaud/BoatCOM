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
using System.IO.Ports;
using System.Windows.Forms;

namespace AISDisplay
{
    public sealed class XMLManager
    {
        public static string fileName;
        public static string fileDirectory;
        private static SerialSettings serialSettings;
        private static FileSystemWatcher watcher;
        public XMLManager()
        {
            fileName = "COMSettings.xml";
            Task.Factory.StartNew(() => VerifyXMLFile());
            serialSettings = grabDataFromFile();
        }

        ~XMLManager()
        {
        }

        public SerialSettings _serialSettings
        {
            get { return serialSettings; }
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
        private static void OnChanged(object source, FileSystemEventArgs e) => Task.Delay(5000).ContinueWith(t => fileChanged(source, e));

        private static void OnRenamed(object source, RenamedEventArgs e) => fileRenamed(source, e);

        private static void fileChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");

            switch (e.ChangeType)
            {
                case (WatcherChangeTypes)ListChangedType.ItemChanged:
                    {
                        if (xmlValuesNotSet())
                            updateSettingsFromXML();
                        break;
                    }

                case (WatcherChangeTypes)ListChangedType.ItemAdded:
                    break;
                case (WatcherChangeTypes)ListChangedType.ItemDeleted:
                    {
                        serializeDataToXML(serialSettings);
                        break;
                    }

                default:
                    break;
            }
        }

        private static void fileRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            fileName = e.Name;
            watcher.Filter = fileName;
            Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
        }

        public static void serializeDataToXML(SerialSettings _spSettings)
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
            writer.WriteString(serialSettings.PortNameCollection[0]);
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

        /// <summary>
        /// Checks if values are up to date and returns true or false if so.
        /// </summary>
        /// <returns></returns>
        private static bool xmlValuesNotSet()
        {
            try
            {
                SerialSettings tmpFileSettings = new SerialSettings();
                XDocument xmlDoc = XDocument.Load(fileName);
                XElement xRootElement = xmlDoc.Root.Element("COMPort");
                tmpFileSettings.PortName = xRootElement.Element("COMPort_Name").Value;
                tmpFileSettings.BaudRate = int.Parse(xRootElement.Element("Baud_Rate").Value);
                tmpFileSettings.DataBits = int.Parse(xRootElement.Element("Data_Bits").Value);
                tmpFileSettings.Parity = (Parity)Enum.Parse(typeof(Parity), xRootElement.Element("Parity").Value, true);
                xmlDoc = null;
                //Function reads as if asking if files are not the same.
                //If the values are the same then return false because nothing needs to be changed.
                if (tmpFileSettings == serialSettings)
                    return false;
                return true;
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FileStream fs = new FileStream(fileDirectory + "\\" + fileName, FileMode.OpenOrCreate);
                fs.Close();
                fs.Dispose();
                serializeDataToXML(serialSettings);
                return true;
            }

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
            //READ FILE AND SET THE SETTINGS WITHIN THIS CLASS AND RETURN THAT. RETURN AND SET FULL VALUES
            //WE NEED TO CHECK IF COMPORT IS CURRENTLY BEING USED BY ANOTHER PROCESS
            try
            {

                XDocument xmlDoc = XDocument.Load(fileName);
                XElement xRootElement = xmlDoc.Root.Element("COMPort");
                serialSettings.PortName = xRootElement.Element("COMPort_Name").Value;
                serialSettings.BaudRate = int.Parse(xRootElement.Element("Baud_Rate").Value);
                serialSettings.DataBits = int.Parse(xRootElement.Element("Data_Bits").Value);
                serialSettings.Parity = (Parity)Enum.Parse(typeof(Parity), xRootElement.Element("Parity").Value, true);
                xmlDoc = null;


            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FileStream fs = new FileStream(fileDirectory + "\\" + fileName, FileMode.OpenOrCreate);
                fs.Close();
                fs.Dispose();
                serializeDataToXML(serialSettings);
            }

            return serialSettings;
        }

        private static SerialSettings grabDataFromFile()
        {
           
            try
            {
                SerialSettings tmpFileSettings = new SerialSettings();
                XDocument xmlDoc = XDocument.Load(fileName);
                XElement xRootElement = xmlDoc.Root.Element("COMPort");
                tmpFileSettings.PortName = xRootElement.Element("COMPort_Name").Value;
                tmpFileSettings.BaudRate = int.Parse(xRootElement.Element("Baud_Rate").Value);
                tmpFileSettings.DataBits = int.Parse(xRootElement.Element("Data_Bits").Value);
                tmpFileSettings.Parity = (Parity)Enum.Parse(typeof(Parity), xRootElement.Element("Parity").Value, true);
                xmlDoc = null;
                return tmpFileSettings;
            }
            catch
            {
                return null;
            }
           
        }
    }

}
