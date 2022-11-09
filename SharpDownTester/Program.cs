using SharpDown;
using System.Text.RegularExpressions;

SharpDown.SharpDown convert = new SharpDown.SharpDown();
string test = File.ReadAllText("text.md").Replace("\r", "");
test += "\n\n";
convert.MarkdownToHtml(test);