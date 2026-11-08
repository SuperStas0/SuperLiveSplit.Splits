using System;
using System.Xml;

namespace LiveSplit.UI;

public class ColumnData
{
    public const string AllRuns = "All runs";
    public const string Last50Runs = "Last 50 runs";
    public const string Last10PercentRuns = "Last 10% of runs (min 10)";

    public string Name { get; set; }
    public ColumnType Type { get; set; }
    public string Comparison { get; set; }
    public string TimingMethod { get; set; }
    public bool ColorByRate { get; set; }
    public string RateRuns { get; set; }

    public RateWindow Window => RateRuns switch
    {
        Last50Runs => RateWindow.Last50,
        Last10PercentRuns => RateWindow.Last10Percent,
        _ => RateWindow.All,
    };

    public bool IsStatistic => Type is ColumnType.Completed or ColumnType.Reached or ColumnType.Passrate or ColumnType.Reachrate or ColumnType.EstimatedReachingTime;

    public ColumnData(string name, ColumnType type, string comparison, string method, bool colorByRate = false, string rateRuns = AllRuns)
    {
        Name = name;
        Type = type;
        Comparison = comparison;
        TimingMethod = method;
        ColorByRate = colorByRate;
        RateRuns = rateRuns;
    }

    public static ColumnData FromXml(XmlNode node)
    {
        var element = (XmlElement)node;
        return new ColumnData(element["Name"].InnerText,
            (ColumnType)Enum.Parse(typeof(ColumnType), element["Type"].InnerText),
            element["Comparison"].InnerText,
            element["TimingMethod"].InnerText,
            SettingsHelper.ParseBool(element["ColorByRate"], false),
            element["RateRuns"]?.InnerText ?? AllRuns);
    }

    public int CreateElement(XmlDocument document, XmlElement element)
    {
        return SettingsHelper.CreateSetting(document, element, "Version", "1.5") ^
        SettingsHelper.CreateSetting(document, element, "Name", Name) ^
        SettingsHelper.CreateSetting(document, element, "Type", Type) ^
        SettingsHelper.CreateSetting(document, element, "Comparison", Comparison) ^
        SettingsHelper.CreateSetting(document, element, "TimingMethod", TimingMethod) ^
        SettingsHelper.CreateSetting(document, element, "ColorByRate", ColorByRate) ^
        SettingsHelper.CreateSetting(document, element, "RateRuns", RateRuns);
    }
}
