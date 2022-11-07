using SharpDown;
using System.Text.RegularExpressions;

SharpDown.SharpDown convert = new SharpDown.SharpDown();
string test = "# SharpDown test file\n\n## Test of list\n- First list item\n- Second list item\n\n1. Ordered list\n2. Test of it\n\n**test**\n";
convert.MarkdownToHtml(test);