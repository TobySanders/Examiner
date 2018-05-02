using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_testing
{
    abstract class BlockElement
    {
        public string identifier { get; set; }
        public string language { get; set; }
        public string label { get; set; }
        public override string ToString()
        {
            return string.Format("identifier: {0}\n\tlanguage: {1}\n\tlabel: {2}", identifier, language, label);
        }

    }
    class P : BlockElement
    {
        public Inline inline { get; set; }
        public override string ToString()
        {
            return inline.ToString();
        }
    }
    class Inline
    {
        public string content { get; set; }
        public override string ToString()
        {
            return content;
        }
    }
    class Image : Inline
    {
        public string source { get; set; }
        public string alt { get; set; }
        public Uri longDesc { get; set; }
        public double height { get; set; }
        public double width { get; set; }
        public override string ToString()
        {
            return base.ToString() + string.Format("\n\t\tImage:\n\t\t\tsource: {0}\n\t\t\talt: {1} \n\t\t\tlongDesc: {2}\n\t\t\theight = {3}\n\t\t\twidth = {4}", source,alt,longDesc,height,width);
        }
    }
}
