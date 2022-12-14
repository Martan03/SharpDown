using System.Diagnostics;

namespace SharpDown
{
    public class SharpDown
    {
        public string MarkdownToHtml(string md)
        {
            if (string.IsNullOrEmpty(md))
                return string.Empty;
            BlockProcessor blockProcessor = new(Prepare(md));
            return blockProcessor.Process();
        }

        private string Prepare(string text)
        {
            text = text.Replace("\r\n", "\n");
            text = text.Replace("\r", "\n");
            text += "\n\n";
            return text;
        }
    }
}