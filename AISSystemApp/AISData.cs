using System;
using System.ComponentModel;
using System.Globalization;

public class AISData : INotifyPropertyChanged
{
    private CultureInfo culture = new CultureInfo("en-US");
    private string _name;
    private DateTime _utcdate;
    private string _heading;
    private string _cog;
    private string _sog;
    private string _lat;
    private string _lon;
    private string _mmsi;
    private string _brg;
    private float _range;
    private string _rangestring;
    private string _commandline;
    public AISData(string name, DateTime utcdate, string heading, string cog, string sog,
        string lat, string lon, string mmsi, string brg, float range, string commandline)
    {
        Name = name;
        UTCDateTime = utcdate;
        Heading = heading;
        COG = cog;
        SOG = sog;
        Lat = lat;
        Lon = lon;
        MMSI = mmsi;
        BRG = brg;
        Range = range;
        CommandLine = commandline;
    }
    public AISData()
    {

    }

    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            OnPropertyChanged("Name");
        }
    }
    public DateTime UTCDateTime
    {
        get { return _utcdate; }
        set
        {
            _utcdate = value;
            OnPropertyChanged("UTCDateTime");
        }
    }
    public string Heading
    {
        get { return _heading; }
        set
        {
            _heading = value;
            OnPropertyChanged("Heading");
        }
    }
    public string COG
    {
        get { return _cog; }
        set
        {
            _cog = value;
            OnPropertyChanged("COG");
        }
    }
    public string SOG
    {
        get { return _sog; }
        set
        {
            _sog = value;
            OnPropertyChanged("SOG");
        }
    }
    public string Lat
    {
        get { return _lat; }
        set
        {
            _lat = value;
            OnPropertyChanged("Lat");
        }
    }
    public string Lon
    {
        get { return _lon; }
        set
        {
            _lon = value;
            OnPropertyChanged("Lon");
        }
    }
    public string MMSI
    {
        get { return _mmsi; }
        set
        {
            _mmsi = value;
            OnPropertyChanged("MMSI");
        }
    }
    public string BRG
    {
        get { return _brg; }
        set
        {
            _brg = value;
            OnPropertyChanged("BRG");
        }
    }
    public float Range
    {
        get { return _range; }
        set
        {
            _range = value;
            _rangestring = string.Format(culture.NumberFormat, "{0:##0.## nm}", _range);
            OnPropertyChanged("Range");
        }
    }
    public string RangeString
    {
        get { return _rangestring; }
        set { _rangestring = value; }
    }
    public string CommandLine
    {
        get { return _commandline; }
        set { _commandline = value; }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string info)
    {
        var handler = PropertyChanged;
        handler?.Invoke(this, new PropertyChangedEventArgs(info));
    }

}