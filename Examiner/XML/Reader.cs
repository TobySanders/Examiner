using Android.OS;
using System;
using System.Xml.Linq;
using Java.Lang;
using System.Net;
using System.IO;
using System.Text;

namespace XML_testing
{
    class Reader
    {
        AssesmentItem assesmentItem;
        Block block;
        VariableDeclaration declaration;
        dynamic focus;
        dynamic temp;
        public AssesmentItem Main(XDocument doc)
        {

            Read(doc);
#if (DEBUG)
            Console.WriteLine("title: {0}", assesmentItem.title);
            Console.WriteLine("identifier: {0}", assesmentItem.identifier);
            Console.WriteLine("adaptive = {0}", assesmentItem.adaptive);
            Console.WriteLine("time dependent = {0}", assesmentItem.timeDependent);
            Console.WriteLine(assesmentItem.DeclerationsToString());
            Console.WriteLine(assesmentItem.ItemBodyToString());
#endif
            return assesmentItem;
        }
        public void Read(XDocument doc)
        {
            foreach (XElement el in doc.Root.Elements())
            {
                    AssignEl(el);
            }
        }
        public void AssignEl(XElement element)
        {

            switch(element.Name.ToString())
            {
                case "assessmentItem": //question root
                    focus = assesmentItem = new AssesmentItem();
                    AssignAtt(element);
                    foreach(XElement child in element.Elements())
                        AssignEl(child);
                    break;
                case "associateInteraction": //associates root
                    focus = block = new AssociateInteraction();
                    AssignAtt(element);
                    assesmentItem.itemBody.Add(block);
                    foreach (XElement child in element.Elements())
                        AssignEl(child);
                    break;
                case "choiceInteraction": // choices root
                    focus = block = new ChoiceInteraction();
                    AssignAtt(element);
                    assesmentItem.itemBody.Add(block);
                    foreach (XElement child in element.Elements())
                        AssignEl(child);
                    break;
                case "matchInteraction": //matches root
                    focus = block = new MatchInteraction();
                    AssignAtt(element);
                    assesmentItem.itemBody.Add(block);
                    foreach (XElement child in element.Elements())
                        AssignEl(child);
                    break;
                case "orderInteraction": //orders root
                    focus = block = new OrderInteraction();
                    AssignAtt(element);
                    assesmentItem.itemBody.Add(block);
                    foreach (XElement child in element.Elements())
                        AssignEl(child);
                    break;
                case "outcomeDeclaration":
                    focus = declaration = new OutcomeDeclaration();
                    AssignAtt(element);
                    assesmentItem.declerations.Add(declaration);
                    foreach (XElement child in element.Elements())
                        AssignEl(child);
                    break;
                case "prompt": //Question prompt
                    focus = (block as BlockInteraction).prompt = element.Value;
                    AssignAtt(element);
                    foreach (XElement child in element.Elements())
                        AssignEl(child);
                    break;
                case "p": //paragraph
                    if (focus is Block)
                    {
                        temp = focus;
                        P p = new P();
                        focus = p;
                        AssignAtt(element);
                        temp.blockElements.Add(focus);
                    }
                    else
                    {
                        Block b = new Block();
                        block = b;
                        P p = new P();
                        focus = p;
                        AssignAtt(element);
                        if (element.Value != "")
                        {
                            p.inline = new Inline();
                            p.inline.content = element.Value;
                        }
                        b.blockElements.Add(focus);
                        assesmentItem.itemBody.Add(block);
                    }
                    foreach (XElement child in element.Elements())
                        AssignEl(child);
                    break;
                case "img":
                    temp = focus;
                    Image img = new Image();
                    focus = img;
                    AssignAtt(element);
                    if (element.Value != "")
                    {
                            img.content = element.Value;
                    }
                    temp.inline = focus;
                    foreach (XElement child in element.Elements())
                        AssignEl(child);
                    break;
                case "responseDeclaration":
                    focus = declaration = new ResponseDeclaration();
                    AssignAtt(element);
                    assesmentItem.declerations.Add(declaration);
                    foreach (XElement child in element.Elements())
                        AssignEl(child);
                    break;
                case "simpleChoice":
                    focus = new SimpleChoice(element.Attribute("identifier").Value, element.Value);
                    AssignAtt(element);
                    (block as BlockInteraction).choices.Add(focus);
                    foreach (XElement child in element.Elements())
                        AssignEl(child);
                    break;
                case "simpleAssociableChoice":
                    focus = new SimpleAssociableChoice(element.Attribute("identifier").Value, element.Value);
                    AssignAtt(element);
                    (block as BlockInteraction).choices.Add(focus);
                    foreach (XElement child in element.Elements())
                        AssignEl(child);
                    break;
                case "itemBody":
                    focus = assesmentItem.itemBody;
                    AssignAtt(element);
                    foreach (XElement child in element.Elements())
                        AssignEl(child);
                    break;
                case "defaultValue":
                    focus = (declaration as OutcomeDeclaration).lookupTable.defaultValue = new DefaultValue();
                    AssignAtt(element);
                    foreach (XElement child in element.Elements())
                        AssignEl(child);
                    break;
                case "correctResponse":
                    focus = (declaration as ResponseDeclaration).correctResponse = new CorrectResponse();
                    AssignAtt(element);
                    foreach (XElement child in element.Elements())
                        AssignEl(child);
                    break;
                case "value":
                    temp = focus;
                    focus = new Value();
                    focus.content = element.Value;
                    AssignAtt(element);
                    temp.value = focus;
                    foreach (XElement child in element.Elements())
                        AssignEl(child);
                    break;
                default:
                    Console.WriteLine("Invalid XML element {0}\n Attempting to Dive\n", element.Name);
                    foreach (XElement child in element.Elements())
                        AssignEl(child);
                    break;
            }
            

        }
        public void AssignAtt(XElement element)
        {
            BlockInteraction iBlock = block as BlockInteraction;
            foreach (XAttribute attr in element.Attributes())
            {
                switch (attr.Name.ToString())
                {
                    case "alt":
                        focus.alt = attr.Value;
                        break;
                    case "cardinality":
                        focus.cardinality = (Cardinality)System.Enum.Parse(typeof(Cardinality), attr.Value);
                        break;
                    case "interpretation":
                        (declaration as ResponseDeclaration).interpretation = attr.Value;
                        break;
                    case "identifier": //for metadata
                        focus.identifier = attr.Value;
                        break;
                    case "title": //title of question
                        focus.title = attr.Value;
                        break;
                    case "label": //label for marking
                        focus.label = attr.Value;
                        break;
                    case "language": //language assesment is in
                        focus.language = attr.Value;
                        break;
                    case "adaptive":
                        focus.adaptive = bool.Parse(attr.Value);
                        break;
                    case "fieldIndentifier":
                        focus.id = attr.Value;
                        break;
                    case "baseType":
                        focus.baseType = (BaseType)System.Enum.Parse(typeof(BaseType),attr.Value);
                        break;
                    case "maxChoices": //number of choices allowed
                         focus.maxChoices = int.Parse(attr.Value);
                        break;
                    case"responseIdentifier":
                        focus.responseIdentifier = attr.Value;
                        break;
                    case "shuffle": //shuffle question order
                         focus.shuffle = bool.Parse(attr.Value);
                        break;
                    case "src":
                        focus.source = attr.Value;
                        break;
                    case "timeDependent"://make question timed
                        focus.timeDependent = bool.Parse(attr.Value);
                        break;       
                    default:
                        Console.WriteLine("Invalid XML attribute {0}\nIn: {1}\n", attr, element.Name);
                        break;
                }
            }
        }
    }
}
