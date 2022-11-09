using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharpDown
{
    public class ToHtmlRegex
    {
        public static readonly Regex hardBreak = new(@"\n.",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        public static readonly Regex headerRegex = new(@"^(\#{1,6})[ ]+(.+?)[ ]*\#*\n+",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        public static readonly Regex headerAltRegex = new(@"^(.+?)[ ]*\n(=+|-+)[ ]*\n+",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        public static readonly Regex horizontalLineRegex = new(@"^[ ]{0,3}([-*_])(?>[ ]{0,2}\1){2,}[ ]*$",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        public static readonly Regex listRegex = new(@"^([ ]*)([-\+\*].|\d+\..)[ ]*(.+?)\n+",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        public static readonly Regex orderedListRegex = new(@"\d+\..",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        public static readonly Regex unorderedListRegex = new(@"[-\+\*].",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        public static readonly Regex blockQuoteRegex = new(@"^(>+)([ ]*)(.+?)\n+",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        public static readonly Regex boldalicRegex = new(@"(?:\*\*\*|___)((.|\s)*\S(.|\s)*)(?:\*\*\*|___)",
                RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        public static readonly Regex boldRegex = new(@"(?:\*\*|__)((.|\s)*\S(.|\s)*)(?:\*\*|__)",
                RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        public static readonly Regex italicRegex = new(@"(?:\*|_)((.|\s)*\S(.|\s)*)(?:\*|_)",
                RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        public static readonly Regex imageRegex = new(@"(?:!\[(.+?)\])\((.+?)\)",
                RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        public static readonly Regex linkRegex = new(@"(?:\[(.+?)\])(?:\((\S+)(?:[ ]+(.+?))?\))",
                RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        public static readonly Regex paragraphRegex = new(@"^[ ]*([^<]+)\n{2,}",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
    }
}
