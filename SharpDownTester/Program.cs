using SharpDown;
using System.Text.RegularExpressions;

SharpDown.SharpDown convert = new SharpDown.SharpDown();
string test = "# This is a test\n## Testing it";
convert.MarkdownToHtml(test);