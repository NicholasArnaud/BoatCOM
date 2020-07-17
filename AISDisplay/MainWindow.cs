using System;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using SerialPortListener.Serial;

namespace AISDisplay
{
    //TODO:: SAVE INCOMING DATA TO TMP FILES
    //TODO:: FEFACTOR HOW INFO IS PULLED INTO THE DISPLAY TABLE
    public partial class MainWindow : Form
    {
        private readonly XMLManager xmlManager = new XMLManager();
        SerialPortManager _spManager;
        SerialSettings mySerialSettings;
        AISDataCollection AISDataCollectionClass = new AISDataCollection();
        List<AISData> AISDataList = new List<AISData>();
        AISData YourAISShipData = new AISData();
        private string strReadFromCOM = "";
        List<string> strListReadFromCOM = new List<string>();

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                UserInitialization();
                _spManager.StartListening();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("IOException source: {0}", e.Source);
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                int tmpCount = 0;
                foreach (string PortName in _spManager.CurrentSerialSettings.PortNameCollection)
                {
                    if (PortName != _spManager.CurrentSerialSettings.PortName)
                        _spManager.CurrentSerialSettings.PortName = _spManager.CurrentSerialSettings.PortNameCollection[tmpCount];
                    tmpCount++;
                }

            }
        }
        private void UserInitialization()
        {
            _spManager = new SerialPortManager();
            if (xmlManager._serialSettings != null) {
                mySerialSettings = xmlManager._serialSettings;
                _spManager.CurrentSerialSettings = mySerialSettings;
            }
            else {
                mySerialSettings = _spManager.CurrentSerialSettings;
                XMLManager.serializeDataToXML(mySerialSettings);
            }
            _spManager.NewSerialDataRecieved += new EventHandler<SerialDataEventArgs>(_spManager_NewSerialDataRecieved);
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            _spManager.StopListening();
            _spManager.Dispose();
        }

        void _spManager_NewSerialDataRecieved(object sender, SerialDataEventArgs e)
        {
            if (InvokeRequired)
            {
                // Using this.Invoke causes deadlock when closing serial port, and BeginInvoke is good practice anyway.
                this.BeginInvoke(new EventHandler<SerialDataEventArgs>(_spManager_NewSerialDataRecieved), new object[] { sender, e });
                return;
            }

            //This application is connected to a GPS sending ASCCI characters, so data is converted to text
            string str = Encoding.ASCII.GetString(e.Data);

            //int maxParseLength = 1000; // maximum text length in text
            //if (stringTmpData.Length > maxParseLength)

            //if(str == "\n" && strReadFromCOM.Contains("\n"))
            //{
            //    strListReadFromCOM.Add(strReadFromCOM.Substring(strReadFromCOM.LastIndexOf("\n") + 1));
            //}
            if (str.Contains("\n")) //&& !strReadFromCOM.Contains("\n"))
            {
                //Shows the lines generated and should display the same as the NemaFileReader Programm
                //Debug.Print(strReadFromCOM);

                strListReadFromCOM.Add(strReadFromCOM);
                strReadFromCOM = "";
            }
            else
            {
                strReadFromCOM += str;
            }
        }

        private void UpdateTable()
        {
            if (strListReadFromCOM.Count > 0)
            {

                foreach (string LineInfo in strListReadFromCOM)
                {
                    if (!LineInfo.Contains("\r"))
                        continue;
                    AISDataList = (AISDataCollectionClass.ParseToTextFromCOM(LineInfo).Count > 0) ? AISDataCollectionClass.ParseToTextFromCOM(LineInfo) : new List<AISData>();
                    if (AISDataList.Count == 0)
                        continue;
                    YourAISShipData = AISDataList[0];
                    AISDataList.RemoveAt(0);
                    if (AISDataList.Count >= 1)
                        LinkTableData(AISDataList);
                    if (YourAISShipData != null)
                        LinkTableData(YourAISShipData);

                }
                if (strListReadFromCOM.Count >= 35)
                    strListReadFromCOM.RemoveRange(0, 20);

                AISDataTable.Update();
            }

            /*
            if (strReadFromCOM.Contains("\n"))
            {
                string[] dataToRead = strReadFromCOM.Split('\n');
                foreach (string i in dataToRead)
                {
                    //CHECK TO ENSURE STRING CAPTURED THE ENTIRE LINE TO BE PARSED
                    // IF CHECK LOOKS FOR THE FOLLOWING NMEA SENTENCES:
                    //!AIVDO, !AIVDM, !BSVDM, !BSVDO, $GPRMC, $AIALR, $PFEC, $AITXT
                    if (i.Contains("\r") &&
                        (i.Contains("!") || i.Contains("$")))
                    {
                        AISDataList = (AISDataCollectionClass.ParseToTextFromCOM(i).Count > 0) ? AISDataCollectionClass.ParseToTextFromCOM(i) : new List<AISData>();
                        if (AISDataList.Count == 0)
                            return;
                        YourAISShipData = AISDataList[0];
                        AISDataList.RemoveAt(0);
                        if (AISDataList.Count >= 1)
                            LinkTableData(AISDataList);
                        if (YourAISShipData != null)
                            LinkTableData(YourAISShipData);
                    }

                }
                //strReadFromCOM = "";
                AISDataTable.Update();
            }
            */
        }

        private void LinkTableData(List<AISData> AISDataList)
        {
            if (AISDataTable.Rows.Count >= 1)
            {
                AISDataTable.Rows.Clear();
            };
            foreach (AISData data in AISDataList)
            {
                AISDataTable.Rows.Add(new object[] { data.Name, data.MMSI, data.Heading, data.BRG, data.RangeString,
                    data.COG, data.SOG, data.Lat, data.Lon, data.UTCDateTime });

            }
            AISDataTable.Rows[0].Cells[0].Selected = false;
        }
        private void LinkTableData(AISData data)
        {
            if (yourDataTable.Rows.Count >= 1)
            {
                yourDataTable.Rows.Clear();
            };
            yourDataTable.Rows.Add(new object[] { data.Name, data.MMSI, data.Heading, data.COG, data.SOG, data.Lat, data.Lon });
            yourDataTable.Rows[0].Cells[0].Selected = false;
        }

        #region FormFunctions
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateTable();
            if (xmlManager._serialSettings != mySerialSettings)
                mySerialSettings = xmlManager._serialSettings;
        }
    }
}
