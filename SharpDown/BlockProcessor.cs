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
            text = BoldEvaluate(text);

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
            Regex regex = new Regex(@"^(\#{1,6})[ ]*(.+?)[ ]*\#*\n+", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            Match match;

            while ((match = regex.Match(text)).Success)
            {
                string replacement = string.Format("<h{0}>{1}</h{0}>\n", match.Groups[1].Value.Length, match.Groups[2].Value);
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
            Regex regex = new Regex(@"\*\*(.*?)\*\*", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
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
            Regex regex = new Regex(@"\*(.*?)\*", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            Match match;

            while ((match = regex.Match(text)).Success)
            {
                string replacement = string.Format("<i>{0}</i>", match.Groups[1].Value);
                text = text.Replace(match.Value, replacement);
            }

            return text;
        }

    }
}
