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
        /// <param name="text">Markdown text</param>
        /// <returns>Text with only html headers</returns>
        private string HeaderEvaluate(string text)
        {
            Regex regex = new Regex(@"^(\#{1,6})[ ]*(.+?)[ ]*\#*\n+");
            string result = text;
            Match match;

            while ((match = regex.Match(text)).Success)
            {
                string replacement = string.Format("<h{0}>{1}</h{0}>\n", match.Groups[1].Value.Length, match.Groups[2].Value);
                text = text.Replace(match.Value, "");
                result = result.Replace(match.Value, replacement);
            }

            return result;
        }

        private string BoldEvaluate(string text)
        {
            Regex regex = new Regex(@"(\*{2})[ ]*(.+?)[ ]*\**\n+");
            string result = text;
            Match match;
            
            while ((match = regex.Match(text)).Success)
            {
                string replacement = string.Format("<strong>{0}</strong>", match.Groups[2].Value);
                text = text.Replace(match.Value, "");
                result = result.Replace(match.Value, replacement);
            }

            return result;
        }
    }
}
