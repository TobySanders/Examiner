using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_testing
{
    struct Value
    {
        public string identifier {get; set;}
        public string content { get; set; }
        public BaseType baseType { get; set; }
        public override string ToString()
        {
            return string.Format("\tidentifier : {0}\n\ttype = {1}\n\tcontent: {2}", identifier, baseType,content);
        }
    }
    enum Cardinality
    {
        single,
        multiple,
        ordered,
        record
    }
    enum BaseType
    {
        inherit,
        identifier,
        boolean,
        integer,
        @float,
        @string,
        point,
        pair,
        directedPair,
        duration,
        file,
        url
    }
    class VariableDeclaration
    {
        public string identifier { get; set; }
        public Cardinality cardinality { get; set; }
        public BaseType baseType { get; set; }
        public override string ToString()
        {
            return string.Format("identifier: {0}\ncardinality = {1}\nbaseType = {2}",identifier,cardinality,baseType);
        }
    }
    class ResponseDeclaration : VariableDeclaration
    {
        public string interpretation { get; set; }
        public CorrectResponse correctResponse = new CorrectResponse();
        public override string ToString()
        {
            return string.Format("{0}\ninterpretation = {1}\nvalue:\n {2}\n", base.ToString(),interpretation,correctResponse.ToString());
        }
    }
    class CorrectResponse
    {
        public Value value { get; set; }
        public override string ToString()
        {
            return value.ToString();
        }
    }
    class OutcomeDeclaration : VariableDeclaration
    {
        public string interpretation { get; set; }
        public Value masteryValue { get; set; }
        public LookupTable lookupTable = new LookupTable();
        public override string ToString()
        {
            return string.Format("{0}\ninterpretation = {1}\nmasteryValue:\n {2}\n lookupTable:\n{3} ", base.ToString(), interpretation, masteryValue.ToString(),lookupTable.ToString());
        }
    }
    class LookupTable
    {
        public DefaultValue defaultValue { get; set; }
        public override string ToString()
        {
            return defaultValue.ToString();
        }
    }
    class DefaultValue
    {
        public Value value { get; set; }
        public override string ToString()
        {
            return value.ToString();
        }
    }
    class InterpolationTable : LookupTable
    { }
    class MatchTable : LookupTable
    { }

    class TemplateDeclaration : VariableDeclaration
    {

    }
}
