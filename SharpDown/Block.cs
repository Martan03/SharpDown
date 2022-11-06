using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDown
{
    internal enum BlockType
    {
        h1,
        h2,
        h3,
        h4,
        h5,
        h6,
        b,
        i,
        blockquote,
        ol,
        ul,
        code,
        hr,
        href,
        image
    }

    internal class Block
    {
        private BlockType Type { get; set; }
        private string Content { get; set; }

        public Block(BlockType type, string content)
        {
            Type = type;
            Content = content;
        }

        public string ToHtml()
        {
            string output = string.Empty;

            switch (Type)
            {
                case BlockType.h1:
                case BlockType.h2:
                case BlockType.h3:
                case BlockType.h4:
                case BlockType.h5:
                case BlockType.h6:
                    return "<" + Type.ToString() + ">" + Content.Trim().TrimStart('#') + "</" + Type.ToString() + ">";
                case BlockType.b:
                case BlockType.i:
                    return "<" + Type.ToString() + ">" + Content.Trim().TrimStart('*').TrimEnd('*') + "</" + Type.ToString() + ">";
            }

            return string.Empty;
        }
    }
}
