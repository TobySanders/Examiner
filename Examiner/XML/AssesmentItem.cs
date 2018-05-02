using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_testing
{
    class AssesmentItem
    {
        public string identifier, title, label, toolName, toolVersion, language;
        public bool adaptive, timeDependent;
        public int bodyCount;
        public List<VariableDeclaration> declerations { get; set; }
        public List<Block> itemBody;
        public AssesmentItem()
        {
            declerations = new List<VariableDeclaration>();
            itemBody = new List<Block>();
        }
        public string DeclerationsToString()
        {
            string res = "";
            foreach(VariableDeclaration d in declerations)
            {
                res += "Decleration:\n" + d.ToString() + "\n\n";
            }
            return res;
        }
        public string ItemBodyToString()
        {
            string res = "";
            foreach(Block b in itemBody)
            {
                res += "ItemBody:\n" + b.ToString() + "\n\n";
            }
            return res;
        }
    }
}
