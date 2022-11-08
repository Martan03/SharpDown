using System.Diagnostics;

namespace SharpDown
{
    public class SharpDown
    {
        public string MarkdownToHtml(string md)
        {
            BlockProcessor blockProcessor = new(md);
            blockProcessor.Process();

            return string.Empty;
        }

        private string Prepare(string text)
        {
            text = ToHtmlRegex.hardBreak.Replace(text, "<br />");
            return text;
        }
    }
}