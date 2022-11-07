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
            var text = HeaderEvaluate(Markdown);
            text = UnorderedListEvaluate(text);
            text = OrderedListEvaluate(text);

            Console.WriteLine(text);
        }

        private string HtmlEvaluate(string text)
        {
            for (int i = 0; i < text.Length; ++i)
            {

            }

            return text;
        }

        /// <summary>
        /// Replaces markdown headers with html headers
        /// </summary>
        /// <param name="text">Text to be evaluated</param>
        /// <returns>Result text after evaluation</returns>
        private string HeaderEvaluate(string text)
        {
            Regex regex = new(@"^(\#{1,6})[ ]+(.+?)[ ]*\#*\n+",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            Match match;

            while ((match = regex.Match(text)).Success)
            {
                string replacement = string.Format("<h{0}>{1}</h{0}>\n", match.Groups[1].Value.Length, SpanEvaluate(match.Groups[2].Value));
                text = text.Replace(match.Value, replacement);
            }

            return text;
        }

        /// <summary>
        /// Replaces markdown bold text with html bold text
        /// </summary>
        /// <param name="text">Text to be evaluated</param>
        /// <returns>Result text after evaluation</returns>
        private string BoldEvaluate(string text)
        {
            Regex regex = new(@"(?:\*\*|__)((.|\s)*\S(.|\s)*)(?:\*\*|__)",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            Match match;
            
            while ((match = regex.Match(text)).Success)
            {
                string replacement = string.Format("<strong>{0}</strong>", match.Groups[1].Value);
                text = text.Replace(match.Value, replacement);
            }

            return text;
        }

        /// <summary>
        /// Replaces markdown italic text with html italic text
        /// </summary>
        /// <param name="text">Text to be evaluated</param>
        /// <returns>Result text after evaluation</returns>
        private string ItalicEvaluate(string text)
        {
            Regex regex = new(@"(?:\*|_)((.|\s)*\S(.|\s)*)(?:\*|_)",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            Match match;

            while ((match = regex.Match(text)).Success)
            {
                string replacement = string.Format("<em>{0}</em>", match.Groups[1].Value);
                text = text.Replace(match.Value, replacement);
            }

            return text;
        }

        private string HorizontalLineEvaluate(string text)
        {
            Regex regex = new(@"^[ ]{0,3}([-*_])(?>[ ]{0,2}\1){2,}[ ]*$",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            Match match;

            while((match = regex.Match(text)).Success)
            {
                text = text.Replace(match.Value, "<hr />");
            }
            
            return text;
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

                replacement += string.Format("\t<li>{0}</li>\n", BlockProcess(match.Groups[2].Value));

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

        private string BlockProcess(string text)
        {
            text = HeaderEvaluate(text);
            text = UnorderedListEvaluate(text);
            text = OrderedListEvaluate(text);
            text = BoldEvaluate(text);
            text = ItalicEvaluate(text);
            return text;
        }

        private string SpanEvaluate(string text)
        {
            text = BoldEvaluate(text);
            text = ItalicEvaluate(text);
            return text;
        }
    }
}
