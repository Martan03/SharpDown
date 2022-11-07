using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using regex = SharpDown.ToHtmlRegex;

namespace SharpDown
{
    internal class BlockProcessor
    {
        private string Markdown { get; set; }
        private int liCount { get; set; } = 0;
        private int liIndent { get; set; } = 0;
        private int listsNested { get; set; } = 0;

        public BlockProcessor(string md)
        {
            Markdown = md;
        }

        public void Process()
        {
            var html = HeaderEvaluate(Markdown);
            html = HorizontalLineEvaluate(html);
            html = ListEvaluate(html);
            /*
            text = UnorderedListEvaluate(text);
            text = OrderedListEvaluate(text);
            var text = DoHeaders(Markdown);*/

            Console.WriteLine(html);
        }

        /// <summary>
        /// Replaces markdown headers with html headers
        /// </summary>
        /// <param name="text">Text to be evaluated</param>
        /// <returns>Result text after evaluation</returns>
        private string HeaderEvaluate(string text)
        {
            text = regex.headerRegex.Replace(text, _HeaderEvaluate);
            return regex.headerAltRegex.Replace(text, _HeaderAltEvaluate);
        }

        /// <summary>
        /// Replaces markdown horizontal line with html hr
        /// </summary>
        /// <param name="text">Text to be evaluated</param>
        /// <returns>Result text after evaluation</returns>
        private string HorizontalLineEvaluate(string text)
        {
            return regex.horizontalLineRegex.Replace(text, _HorizontalLineEvaluate);
        }

        private string ListEvaluate(string text)
        {
            return regex.listRegex.Replace(text, _ListEvaluate);
        }

        /// <summary>
        /// Replaces markdown unordered list with html ul
        /// </summary>
        /// <param name="text">Text to be evaluated</param>
        /// <returns>Result text after evaluation</returns>
        private string UnorderedListEvaluate(string text)
        {
            Regex regex = new(@"^[ ]*([-*].)[ ]*(.+?)\n+",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            Match match;

            bool nested = false;

            while ((match = regex.Match(text)).Success)
            {
                string replacement = string.Empty;
                if (!nested)
                    replacement += "<ul>\n";

                replacement += string.Format("\t<li>{0}</li>\n", match.Groups[2].Value);

                nested = !match.Groups[0].Value.EndsWith("\n\n");
                if (!nested)
                    replacement += "</ul>\n";

                text = text.Replace(match.Value, replacement);
            }

            return text;
        }

        /// <summary>
        /// Replaces markdown ordered list with html ol
        /// </summary>
        /// <param name="text">Text to be evaluated</param>
        /// <returns>Result text after evaluation</returns>
        private string OrderedListEvaluate(string text)
        {
            Regex regex = new(@"^[ ]*(\d+)\..[ ]*(.+?)\n+",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            Match match;

            bool nested = false;

            while ((match = regex.Match(text)).Success)
            {
                string replacement = string.Empty;
                if (!nested)
                    replacement += "<ol>\n";

                replacement += string.Format("\t<li>{0}</li>\n", match.Groups[2].Value);

                nested = !match.Groups[0].Value.EndsWith("\n\n");
                if (!nested)
                    replacement += "</ol>\n";

                text = text.Replace(match.Value, replacement);
            }

            return text;
        }

        private string _HeaderEvaluate(Match match)
        {
            return string.Format("<h{0}>{1}</h{0}>\n\n",
                match.Groups[1].Value.Length, match.Groups[2].Value);
        }

        private string _HeaderAltEvaluate(Match match)
        {
            return string.Format("<h{0}>{1}</h{0}>\n\n",
                match.Groups[2].Value.StartsWith("=") ? 1 : 2, match.Groups[1].Value);
        }

        private string _HorizontalLineEvaluate(Match match)
        {
            return "<hr />";
        }

        private string _ListEvaluate(Match match)
        {
            var text = (liCount++ == 0) ? "<ul>\n" : "";

            text += _ListCheckIndentation(match.Groups[1].Value.Length);
            text += _ListItemEvaluate(match.Groups[3].Value);

            if (match.Groups[0].Value.EndsWith("\n\n"))
            {
                for (; listsNested >= 0; --listsNested)
                    text += "</ul>\n";
                liCount = 0;
            }
            return text;
        }

        private string _ListItemEvaluate(string text)
        {
            return string.Format("{0}<li>{1}</li>\n", IndentText(listsNested), text);
        }

        private string _ListCheckIndentation(int value)
        {
            var text = string.Empty;
            if (value > liIndent)
            {
                ++listsNested;
                text = string.Format("{0}<ul>\n", IndentText(liIndent));
                liIndent = value;
            }
            else if (value < liIndent)
            {
                --listsNested;
                liIndent = value;
                text = string.Format("{0}</ul>\n", IndentText(liIndent));
            }
            return text;
        }

        private string IndentText(int n)
        {
            string text = string.Empty;
            for (; n >= 0; --n)
                text += "    ";
            return text;
        }
    }
}
