using System;
using System.Windows.Forms;

namespace SplitsFixer;

public static class Program
{
    private const string Title = "Splits Fixer";

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        using var dialog = new OpenFileDialog
        {
            Title = "Select a LiveSplit layout",
            Filter = "LiveSplit layouts (*.lsl)|*.lsl|All files (*.*)|*.*",
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            Fix(dialog.FileName);
        }
    }

    private static void Fix(string path)
    {
        try
        {
            int removed = LayoutFixer.Fix(path);
            if (removed == 0)
            {
                MessageBox.Show("No custom columns found. The file was not changed.", Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MessageBox.Show(
                $"Removed {removed} custom column(s).\n\nThe original is saved as:\n{path}.bak\n\nSplits files are not touched, so your stats are kept.",
                Title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
