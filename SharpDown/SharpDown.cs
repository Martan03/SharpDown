using System.Diagnostics;

namespace SharpDown
{
    public class SharpDown
    {
        public string MarkdownToHtml(string md)
        {
            BlockProcessor blockProcessor = new(Prepare(md));
            return blockProcessor.Process();
        }

        private string Prepare(string text)
        {
            text = text.Replace("\r", "");
            text += "\n\n";
            return text;
        }
    }
}