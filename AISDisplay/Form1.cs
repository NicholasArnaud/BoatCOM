using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SerialPortListener.Serial;

namespace AISDisplay
{
    //TODO:: SAVE TO FILES S
    public partial class Form1 : Form
    {
        SerialPortManager _spManager;
        AISDataCollection AISDataCollectionClass = new AISDataCollection();
        List<AISData> AISDataList = new List<AISData>();
        AISData YourAISShipData = new AISData();
        private string stringTmpData = "";

        public Form1()
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
                MessageBox.Show("No port was found. Please insert a port and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
        private void UserInitialization()
        {
            _spManager = new SerialPortManager();
            XMLManager xmlManager = new XMLManager();
            SerialSettings mySerialSettings = _spManager.CurrentSerialSettings;
            XMLManager.SerializeDataToXML(mySerialSettings);
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

            int maxParseLength = 1000; // maximum text length in text
            if (stringTmpData.Length > maxParseLength)
            {
                stringTmpData = stringTmpData.Split(new[] { '\n' }, 2)[1];

            }

            //This application is connected to a GPS sending ASCCI characters, so data is converted to text
            string str = Encoding.ASCII.GetString(e.Data);
            stringTmpData += str;


        }

        private void UpdateTable()
        {
            if (stringTmpData.Contains("\n"))
            {
                string[] dataToRead = stringTmpData.Split('\n');
                foreach (string i in dataToRead)
                {
                    //CHECK TO ENSURE STRING CAPTURED THE ENTIRE LINE TO BE PARSED
                    // IF CHECK LOOKS FOR THE FOLLOWING NMEA SENTENCES:
                    //!AIVDO, !AIVDM, !BSVDM, !BSVDO, $GPRMC, $AIALR, $PFEC, $AITXT
                    if (i.Contains("\r") &&
                        (i.Contains("!") || i.Contains("$")))
                    {
                        AISDataList = AISDataCollectionClass.ParseToTextFromCOM(i);
                        YourAISShipData = AISDataList[0];
                        AISDataList.RemoveAt(0);
                        if (AISDataList.Count >= 1)
                            LinkTableData(AISDataList);
                        if (YourAISShipData != null)
                            LinkTableData(YourAISShipData);
                    }

                }
                AISDataTable.Update();
            }
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

        private void Form1_Load(object sender, EventArgs e)
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
        }


    }

}
