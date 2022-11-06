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
    }
}