using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_testing
{
    abstract class Choice
    {
        public string identifier, body;
        public Choice(string inId, string inBody)
        {
            identifier = inId;
            body = inBody;
        }
        public override string ToString()
        {
            return string.Format("\n\t\tidentifier: {0}\n\t\tbody: {1}", identifier, body);
        }
    }
    class SimpleChoice : Choice
    {
        public SimpleChoice(string inId, string inBody) : base(inId,inBody)
        { }
    }
    class SimpleAssociableChoice : Choice
    {
        public SimpleAssociableChoice(string inId, string inBody) : base(inId, inBody) { }
        public int matchMax, matchMin = 0; //A value of 0 de-limits
    }
}
