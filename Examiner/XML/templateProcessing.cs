using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_testing
{
    class TemplateProcessing
    {
        List<TemplateRule> rules;
    }
    abstract class TemplateRule
    {
    }
    class ExitTemplate : TemplateRule
    { }
    class SetCorrectResponse : TemplateRule
    { }
    class SetDefaultValue : TemplateRule
    {
        string identifier;

    }
    class SetTemplateValue : TemplateRule
    { }
    class TemplateCondition : TemplateRule
    { }
    class TemplateConstraint : TemplateRule
    { }
}
