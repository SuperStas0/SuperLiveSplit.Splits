using System.IO;
using System.Linq;
using System.Xml;

namespace SplitsFixer;

public static class LayoutFixer
{
    private static readonly string[] CustomTypes = ["Completed", "Reached", "Passrate", "Reachrate", "EstimatedReachingTime"];

    private static readonly string ColumnQuery = "//Columns/Settings[" + string.Join(" or ", CustomTypes.Select(x => "normalize-space(Type)='" + x + "'")) + "]";

    public static int Fix(string path)
    {
        var document = new XmlDocument { PreserveWhitespace = true };
        var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null };
        using (var reader = XmlReader.Create(path, settings))
        {
            document.Load(reader);
        }

        XmlNode[] columns = document.SelectNodes(ColumnQuery).Cast<XmlNode>().ToArray();
        if (columns.Length == 0)
        {
            return 0;
        }

        foreach (XmlNode column in columns)
        {
            XmlNode parent = column.ParentNode;
            if (column.PreviousSibling is XmlWhitespace whitespace)
            {
                parent.RemoveChild(whitespace);
            }

            parent.RemoveChild(column);
        }

        File.Copy(path, path + ".bak", true);
        document.Save(path);
        return columns.Length;
    }
}
