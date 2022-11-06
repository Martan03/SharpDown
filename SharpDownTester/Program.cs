using SharpDown;
using System.Text.RegularExpressions;

SharpDown.SharpDown convert = new SharpDown.SharpDown();
string test = "# **This is a test**\ntest\n# Testing it\n**test**\n";
convert.MarkdownToHtml(test);