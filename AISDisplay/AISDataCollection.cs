using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using SharpAIS;

public class AISDataCollection : ObservableCollection<AISData>
{
    public List<AISData> AISDataList = new List<AISData>();
    private List<string> MMSICheckList = new List<string>();
    public AISData AISDataItem = new AISData();
    public AISData PrevAISDataItem = new AISData();
    public int DataListLimit = 40;
    /// <summary>
    /// Parses data from a file
    /// </summary>
    /// <param name="fileLocation"></param>
    /// <returns></returns>
    public List<AISData> ParseToText(string fileLocation)
    {
        int MaxLines = 20000;
        DateTime currentTime = DateTime.Now;
        CultureInfo culture = new CultureInfo("en-US");

        Parser parser = new Parser();
        if (File.Exists(fileLocation))
        {
            int currentpart = 1;
            int currentline = 0;
            FileInfo inComing = new FileInfo(fileLocation);
            string filename = string.Format("{0}-{1:00}{2}", inComing.Name.Replace(inComing.Extension, string.Empty), currentpart, inComing.Extension);
            Console.WriteLine(string.Format("Writing: {0}", filename));
            TextWriter outComing = new StreamWriter(filename);


            using (StreamReader sr = new StreamReader(fileLocation))
            {
                string textline;
                List<string> MMSICheckList = new List<string>();

                while ((textline = sr.ReadLine()) != null)
                {
                    if (currentline > MaxLines)
                    {
                        outComing.Flush();
                        outComing.Close();

                        currentpart++;
                        currentline = 0;
                        filename = string.Format("{0}-{1:00}{2}", inComing.Name.Replace(inComing.Extension, string.Empty), currentpart, inComing.Extension);
                        Console.WriteLine(string.Format("Writing: {0}", filename));
                        outComing = new StreamWriter(filename);
                    }


                    Hashtable rs = parser.Parse(textline);

                    if (rs != null)
                    {
                        AISData AisData = new AISData();
                        AisData.CommandLine = textline.Substring(0, textline.IndexOf(','));
                        AisData.Heading = (rs["TrueHeading"] == null) ? "0" : rs["TrueHeading"].ToString();
                        AisData.Lon = string.Format(culture.NumberFormat, "{0:####.00000}", (rs["Longitude"] == null) ?
                            (rs["LongitudeDecimalDegrees"] == null) ? "" : rs["LongitudeDecimalDegrees"] : rs["Longitude"]);
                        AisData.Lat = string.Format(culture.NumberFormat, "{0:####.00000}", (rs["Latitude"] == null) ?
                            (rs["LatitudeDecimalDegrees"] == null) ? "" : rs["LatitudeDecimalDegrees"] : rs["Latitude"]);
                        AisData.COG = string.Format(culture.NumberFormat, "{0:####.0}", rs["CourseOverGround"]);
                        AisData.SOG = string.Format(culture.NumberFormat, "{0:####.0}", rs["SpeedOverGround"]);
                        if (rs["UTCYear"] != null)
                        {
                            string date = rs["UTCMonth"].ToString() + "/" + rs["UTCDay"].ToString() + "/" + rs["UTCYear"].ToString();
                            string time = rs["UTCHour"].ToString() + ":" + rs["UTCMinute"].ToString() + ":" + rs["UTCSecond"].ToString();
                            AisData.UTCDateTime = DateTime.Parse(date + " " + time);
                        }
                        else if (rs["Year"] != null)
                        {
                            string date = rs["Month"].ToString() + "/" + rs["Day"].ToString() + "/" + rs["Year"].ToString();
                            string time = rs["Hour"].ToString() + ":" + rs["Minute"].ToString() + ":" + rs["Second"].ToString();
                            AisData.UTCDateTime = DateTime.Parse(date + " " + time);
                        }
                        else
                            AisData.UTCDateTime = new DateTime();

                        AisData.BRG = (rs["Bearing"] == null) ? "-" : rs["Bearing"].ToString();
                        AisData.MMSI = (rs["MMSI"] == null) ? "-" : rs["MMSI"].ToString();

                        if (rs["VesselName"] != null)
                            AisData.Name = rs["VesselName"].ToString();
                        else if (rs["Name"] != null)
                            AisData.Name = rs["Name"].ToString();
                        else if (rs["UserID"] != null)
                            AisData.Name = rs["UserID"].ToString();
                        else
                            AisData.Name = "-";

                        if (AisData.MMSI != "-")
                            PrevAISDataItem = AisData;
                        ExistingUpdatedData(AisData);
                    }
                }
            }

            outComing.Flush();
            outComing.Close();
        }

        return CleanAndSortAISDataList();
    }

    /// <summary>
    /// Parses list of data imported from a COM Serial Port
    /// </summary>
    /// <param name="COMPortLine"></param>
    /// <returns></returns>
    public List<AISData> ParseToTextFromCOM(string COMPortLine)
    {
        DateTime currentTime = DateTime.Now;
        CultureInfo culture = new CultureInfo("en-US");

        Parser parser = new Parser();

        List<string> MMSICheckList = new List<string>();

        Hashtable rs = parser.Parse(COMPortLine);

        if (rs != null)
        {
            AISData AisData = new AISData();
            AisData.CommandLine = COMPortLine.Substring(0, COMPortLine.IndexOf(','));
            AisData.Heading = (rs["TrueHeading"] == null) ? "0" : rs["TrueHeading"].ToString();
            AisData.Lon = string.Format(culture.NumberFormat, "{0:####.00000}", (rs["Longitude"] == null) ?
                (rs["LongitudeDecimalDegrees"] == null) ? "" : rs["LongitudeDecimalDegrees"] : rs["Longitude"]);
            AisData.Lat = string.Format(culture.NumberFormat, "{0:####.00000}", (rs["Latitude"] == null) ?
                (rs["LatitudeDecimalDegrees"] == null) ? "" : rs["LatitudeDecimalDegrees"] : rs["Latitude"]);
            AisData.COG = string.Format(culture.NumberFormat, "{0:####.0}", rs["CourseOverGround"]);
            AisData.SOG = string.Format(culture.NumberFormat, "{0:####.0}", rs["SpeedOverGround"]);
            if (rs["UTCYear"] != null)
            {
                string date = rs["UTCMonth"].ToString() + "/" + rs["UTCDay"].ToString() + "/" + rs["UTCYear"].ToString();
                string time = rs["UTCHour"].ToString() + ":" + rs["UTCMinute"].ToString() + ":" + rs["UTCSecond"].ToString();
                AisData.UTCDateTime = DateTime.Parse(date + " " + time);
            }
            else if (rs["Year"] != null)
            {
                string date = rs["Month"].ToString() + "/" + rs["Day"].ToString() + "/" + rs["Year"].ToString();
                string time = rs["Hour"].ToString() + ":" + rs["Minute"].ToString() + ":" + rs["Second"].ToString();
                AisData.UTCDateTime = DateTime.Parse(date + " " + time);
            }
            else
                AisData.UTCDateTime = new DateTime();

            AisData.BRG = (rs["Bearing"] == null) ? (rs["True Bearing"] == null) ? "-" : rs["Bearing"].ToString() : rs["Bearing"].ToString();
            AisData.MMSI = (rs["MMSI"] == null) ? "-" : rs["MMSI"].ToString();

            if (rs["VesselName"] != null)
                AisData.Name = rs["VesselName"].ToString();
            else if (rs["Name"] != null)
                AisData.Name = rs["Name"].ToString();
            else if (rs["UserID"] != null)
                AisData.Name = rs["UserID"].ToString();
            else if (rs["CallSign"] != null)
                AisData.Name = rs["CallSign"].ToString();
            else
                AisData.Name = "-";

            if (AisData.MMSI != "-" && AisData.CommandLine != "$GPRMC")
                PrevAISDataItem = AisData;
            ExistingUpdatedData(AisData);
        }

        //List<AISData> SortedList = AISDataList.OrderBy(o => o.Range).ToList();
        //int updatedInt = SortedList.FindIndex(a => a.CommandLine == "!AIVDO");

        //if (updatedInt >= 0)
        //{
        //    AISData tmpData = SortedList[updatedInt];
        //    SortedList.RemoveAt(updatedInt);
        //    SortedList.Insert(0, tmpData);
        //}
        //return SortedList;
        return CleanAndSortAISDataList();
    }

    private double DegeressToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
    private float HaversineDistance(double lat1, double lon1, double lat2, double lon2)
    {
        float earthRad = 6372.8f;
        double dLat = DegeressToRadians(lat2 - lat1);
        double dLon = DegeressToRadians(lon2 - lon1);
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(DegeressToRadians(lat1)) * Math.Cos(DegeressToRadians(lat2)) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        float c = (float)(2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)));
        return earthRad * c;
    }


    private void ExistingUpdatedData(AISData AisData)
    {   
        if(AisData.CommandLine == "$GPRMC")
        {
            AisData.MMSI = PrevAISDataItem.MMSI;

        }
        if (!MMSICheckList.Contains(AisData.MMSI))
        {
            AISDataList.Add(AisData);
            MMSICheckList.Add(AisData.MMSI);
        }
        else
        {
            int tmpInt = AISDataList.FindIndex(a => a.MMSI == AisData.MMSI);
            if (AISDataList[tmpInt].Name != "-")
                AisData.Name = AISDataList[tmpInt].Name;
            if (AISDataList[tmpInt].Lon != "")
                AisData.Lon = AISDataList[tmpInt].Lon;
            if (AISDataList[tmpInt].Lat != "")
                AisData.Lat = AISDataList[tmpInt].Lat;
            if (AISDataList[tmpInt].Heading != "0")
                AisData.Heading = AISDataList[tmpInt].Heading;
            if (AISDataList[tmpInt].COG != "")
                AisData.COG = AISDataList[tmpInt].COG;
            if (AISDataList[tmpInt].SOG != "")
                AisData.SOG = AISDataList[tmpInt].SOG;
            if (AISDataList[tmpInt].BRG != "-")
                AisData.BRG = AISDataList[tmpInt].BRG;
            if (AISDataList[tmpInt].UTCDateTime != new DateTime())
                AisData.UTCDateTime = AISDataList[tmpInt].UTCDateTime;

            if (AisData.Lat != "")
            {
                int tmpInt2 = AISDataList.FindIndex(a => a.CommandLine == "!AIVDO");
                if (AisData.CommandLine != "!AIVDO" && tmpInt2 != -1)
                    AisData.Range = HaversineDistance(Convert.ToDouble(AisData.Lat), Convert.ToDouble(AisData.Lon),
                       Convert.ToDouble(AISDataList[tmpInt2].Lat), Convert.ToDouble(AISDataList[tmpInt2].Lon));
                else
                    AisData.Range = 0;
            }
            else
                AisData.Range = -1;

            AISDataList[tmpInt] = AisData;

        }
    }

    private List<AISData> CleanAndSortAISDataList()
    {
        //ORDER LIST TO GET THE CLOSEST VESSELS FIRST FROM TOP TO BOTTOM
        List<AISData> SortedList = AISDataList.OrderBy(o => o.Range).ToList();


        //GETS OWN VESSEL AND MOVES IT TO THE FIRST INDEX
        int updatedInt = SortedList.FindIndex(a => a.CommandLine == "!AIVDO");
        if (updatedInt >= 0)
        {
            AISData tmpData = SortedList[updatedInt];
            SortedList.RemoveAt(updatedInt);
            SortedList.Insert(0, tmpData);
        }

        //DELETES INCOMPLETE AISDATA FROM LIST  
        List<AISData> ToDeleteList = new List<AISData>();
        foreach (AISData data in SortedList)
        {
            if ((data.Range == -1 || data.RangeString == "") && data.Name == "-")
            {
                ToDeleteList.Add(data);
            }
            if((data.Name == "-" || data.Range == -1) && (data.Lat == "" && data.Lon == "") || (data.SOG == "" && data.COG == ""))
            {
                ToDeleteList.Add(data);
            }
        }
        foreach (AISData data in ToDeleteList)
        {
            SortedList.Remove(data);
        }
        return SortedList;
    }

    public AISDataCollection()
    {

    }
}