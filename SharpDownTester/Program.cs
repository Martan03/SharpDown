using SharpDown;
using System.Text.RegularExpressions;

SharpDown.SharpDown convert = new SharpDown.SharpDown();
Console.WriteLine(convert.MarkdownToHtml(File.ReadAllText("text.md")));