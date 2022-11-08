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
        private int liIndent { get; set; } = 0;
        private List<string> listTypes { get; set; } = new List<string>();
        private int blockQuoteIndent { get; set; } = 0;

        public BlockProcessor(string md)
        {
            Markdown = md;
        }

        public void Process()
        {
            var html = HeaderEvaluate(Markdown);
            html = HorizontalLineEvaluate(html);
            html = ListEvaluate(html);
            html = BlockQuoteEvaluate(html);

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

        /// <summary>
        /// Replaces markdown list with html list
        /// </summary>
        /// <param name="text">Text to be evaluated</param>
        /// <returns>Result text after evaluation</returns>
        private string ListEvaluate(string text)
        {
            return regex.listRegex.Replace(text, _ListEvaluate);
        }

        private string BlockQuoteEvaluate(string text)
        {
            return regex.blockQuoteRegex.Replace(text, _BlockQuoteEvaluate);
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
            var text = string.Empty;
            if (listTypes.Count == 0)
            {
                listTypes.Add(regex.orderedListRegex.IsMatch(match.Groups[2].Value) ? "ol" : "ul");
                text = string.Format("<{0}>\n", listTypes.Last());
            }

            text += _ListCheckIndentation(match);
            text += _ListItemEvaluate(match.Groups[3].Value);

            if (match.Groups[0].Value.EndsWith("\n\n"))
            {
                for (; listTypes.Count > 0; listTypes.RemoveAt(listTypes.Count - 1))
                    text += string.Format("{0}</{1}>\n", IndentText(listTypes.Count - 1), listTypes.Last());
            }
            return text;
        }

        private string _ListItemEvaluate(string text)
        {
            return string.Format("{0}<li>{1}</li>\n", IndentText(listTypes.Count), text);
        }

        private string _ListCheckIndentation(Match match)
        {
            var text = string.Empty;
            var value = match.Groups[1].Value.Length;
            if (value > liIndent)
            {
                listTypes.Add(regex.orderedListRegex.IsMatch(match.Groups[2].Value) ? "ol" : "ul");
                text = string.Format("{0}<{1}>\n", IndentText(listTypes.Count - 1), listTypes.Last());
                liIndent = value;
            }
            else if (value < liIndent)
            {
                liIndent = value;
                text = string.Format("{0}</{1}>\n", IndentText(listTypes.Count - 1), listTypes.Last());
                listTypes.RemoveAt(listTypes.Count - 1);
            }
            return text;
        }

        private string _BlockQuoteEvaluate(Match match)
        {
            string text = _BlockQuoteCheckIndentation(match);

            text += string.Format("{0}{1}\n", IndentText(blockQuoteIndent), match.Groups[3].Value);

            if (match.Groups[0].Value.EndsWith("\n\n"))
            {
                for (; blockQuoteIndent > 0; --blockQuoteIndent)
                    text += string.Format("{0}</blockquote>\n", IndentText(blockQuoteIndent - 1));
            }
            return text;
        }

        private string _BlockQuoteCheckIndentation(Match match)
        {
            string text = string.Empty;
            if (match.Groups[1].Value.Length > blockQuoteIndent)
            {
                for (int i = match.Groups[1].Value.Length - blockQuoteIndent; i > 0; --i)
                    text += string.Format("{0}<blockquote>\n", IndentText(blockQuoteIndent));
                blockQuoteIndent = match.Groups[1].Value.Length;
            }
            else if (match.Groups[1].Value.Length < blockQuoteIndent)
            {
                for (int i = blockQuoteIndent - match.Groups[1].Value.Length; i > 0; --i)
                    text += string.Format("{0}</blockquote>\n", IndentText(blockQuoteIndent - 1));
                blockQuoteIndent = match.Groups[1].Value.Length;
            }
            return text;
        }

        private string IndentText(int n)
        {
            string text = string.Empty;
            for (; n > 0; --n)
                text += "    ";
            return text;
        }
    }
}
