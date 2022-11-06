using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharpDown
{
    internal class BlockProcessor
    {
        private string Markdown { get; set; }

        public BlockProcessor(string md)
        {
            Markdown = md;
        }

        public void Process()
        {
            HeaderEvaluate(Markdown);

            //Console.WriteLine(text);
        }

        private string HtmlEvaluate(string text)
        {
            for (int i = 0; i < text.Length; ++i)
            {

            }

            return text;
        }

        private string HeaderEvaluate(string text)
        {
            Regex headerRegex = new Regex(@"^(\#{1,6})[ ]*(.+?)[ ]*\#*\n+");

            Match match = headerRegex.Match(text);

            if (!match.Success)
                return text;

            var level = match.Groups[1].Value.Count();
            return "<h{0}>{1}</h{";
        }

        private static readonly Regex _headerAtx = new Regex(@"
                ^(\#{1,6})  # $1 = string of #'s
                [ ]*
                (.+?)       # $2 = Header text
                [ ]*
                \#*         # optional closing #'s (not counted)
                \n+",
            RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        private string SetextHeaderEvaluator(Match match)
        {
            var header = match.Groups[1].Value;
            var level = match.Groups[2].Value.StartsWith("=") ? 1 : 2;
            return string.Format("<h{1}>{0}</h{1}>\n\n", header, level);
        }

        private string AtxHeaderEvaluator(Match match)
        {
            var header = match.Groups[2].Value;
            var level = match.Groups[1].Value.Length;
            return string.Format("<h{1}>{0}</h{1}>\n\n", header, level);
        }
    }
}
