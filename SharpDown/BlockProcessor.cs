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
            Regex headerRegex = new Regex(@"^(\#{1,6})[ ]*(.+?)[ ]*\#*\n+");
            Match match = headerRegex.Match(text);
            StringBuilder result = new();

            while (match.Success)
            {
                string replacement = string.Format("<h{0}>{1}</h{0}>\n", match.Groups[1].Value.Length, match.Groups[2].Value);
                text = text.Replace(match.Value, "");
                result.Append(replacement);

                match = headerRegex.Match(text);
            }

            return result.ToString();
        }
    }
}
