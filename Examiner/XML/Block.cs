using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_testing
{
    enum Orientation
    {
        vertical,
        horizontal
    }
     class Block
    {
        public List<BlockElement> blockElements;
        public Block()
        {
            blockElements = new List<BlockElement>();
        }
        public override string ToString()
        {
            string res = "";
            foreach(BlockElement b in blockElements)
            {
                res += b.ToString()+ "\n";
            }
            return res;
        }
    }
    abstract class BlockInteraction : Block
    {
        public string prompt, responseIdentifier;
        public bool shuffle = false;
        public int maxAssociations = 1, minAssociations = 0;


        public List<Choice> choices;

        public BlockInteraction()
        {
            blockElements = new List<BlockElement>();
            choices = new List<Choice>();
        }
        public void AddChoice(Choice inChoice)
        {
            choices.Add(inChoice);
        }
        public override string ToString()
        {
            string res = "";
            foreach(Choice c in choices)
            {
                res += c.ToString();
            }
            return string.Format("Block Interaction:\n\tprompt: {0}\n\tshuffle = {1}\n\tChoices:{1}\n\tresponseIdentifier: {2}", prompt, shuffle, res,responseIdentifier);
        }
    }
    class ChoiceInteraction : BlockInteraction
    {
        public int maxChoices = 1, minChoices = 0;
        Orientation orientation;
        public override string ToString()
        {
            return base.ToString() + string.Format("\n\torientation = {0}\n\tmaxChoices = {1}\n\tminChoices = {2}", orientation,maxChoices,minChoices);
        }
    }
    class OrderInteraction : ChoiceInteraction
    {
    }
    class AssociateInteraction : BlockInteraction
    {
    }
    class MatchInteraction : AssociateInteraction
    {       
    }
    class BlockStatic : Block
    { }
    class CustomInteraction : Block
    { }
    class PositionObjectStage : Block
    { }
}
