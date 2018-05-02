using Android.App;
using Android.Widget;
using Android.OS;
using XML_testing;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using Android.Views;
using Android.Content;

namespace Examiner.Activities
{
    [Activity(Label = "Examiner")]
    public class QuestionActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            int checkedBoxes = 0;
            int maxChecked = 0;
            int minChecked = 0;
            int click = 0;
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.QuestionBase);
            var baseLayout = FindViewById<LinearLayout>(Resource.Id.outer);
            // Get our button from the layout resource,
            // and attach an event to it
            Button buttonBack = FindViewById<Button>(Resource.Id.buttonNext);
            buttonBack.Click += delegate
            {
                Finish();
            };
            TextView textTitle = FindViewById<TextView>(Resource.Id.textTitle);
            Reader reader = new Reader();
            string qtiPath = (string)Intent.Extras.Get("qtiPath");
            Stream strm = Assets.Open(qtiPath);
            XDocument qFile = XDocument.Load(strm);
            AssesmentItem assessmentItem = reader.Main(qFile);

            List<VariableDeclaration> declarations = assessmentItem.declerations;
            List<Block> itemBody = assessmentItem.itemBody;

            textTitle.Text = assessmentItem.title;
            foreach (Block b in itemBody)
            {
                switch (b.GetType().Name)
                {
                    #region ChoiceInteraction
                    case "ChoiceInteraction":
                        ChoiceInteraction block = (b as ChoiceInteraction);
                        LinearLayout answerLayout = new LinearLayout(this)
                        {
                            LayoutParameters =
                                 new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                               ViewGroup.LayoutParams.WrapContent, 2f / itemBody.Count),
                            Orientation = Android.Widget.Orientation.Vertical
                        };
                        baseLayout.AddView(answerLayout);
                        TextView textPrompt = new TextView(this);
                        ViewGroup.LayoutParams textLayout = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                        textPrompt.LayoutParameters = textLayout;
                        textPrompt.TextSize = 20;
                        textPrompt.Text = block.prompt;
                        answerLayout.AddView(textPrompt);

                        maxChecked = block.maxChoices;
                        ViewGroup.LayoutParams checkLayout = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                        if (maxChecked == 1)//use radio buttons
                        {
                            RadioGroup rGroup = new RadioGroup(this);
                            LinearLayout.LayoutParams rGroupParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                            rGroup.LayoutParameters = rGroupParams;
                            answerLayout.AddView(rGroup);
                            foreach (Choice c in block.choices)
                            {
                                RadioButton choiceButton = new RadioButton(this);
                                choiceButton.LayoutParameters = checkLayout;
                                choiceButton.Text = c.body;
                                rGroup.AddView(choiceButton);
                            }
                        }
                        else {  //use checkboxes
                            foreach (Choice c in block.choices)
                            {
                                CheckBox choiceBox = new CheckBox(this);
                                choiceBox.LayoutParameters = checkLayout;
                                choiceBox.Text = c.body;
                                choiceBox.Click += (o, e) =>
                                {
                                    if (!choiceBox.Checked)
                                        checkedBoxes--;
                                    else if (checkedBoxes < maxChecked)
                                        checkedBoxes++;
                                    else if(maxChecked != 0)
                                    {
                                        AlertDialog.Builder alertTooMany = new AlertDialog.Builder(this);
                                        alertTooMany.SetTitle("Too Many Selections");
                                        alertTooMany.SetMessage("The maximum amount of selections for this question is: " + maxChecked);
                                        alertTooMany.SetPositiveButton("Okay", (senderAlert, args) =>
                                        {
                                            alertTooMany.Dispose();
                                            choiceBox.Toggle();
                                        });
                                        alertTooMany.SetCancelable(false);
                                        alertTooMany.Show();
                                    }
                                };
                                answerLayout.AddView(choiceBox);
                            }
                        }
                        break;
                    #endregion
                    case "Block":
                        LinearLayout blockLayout = new LinearLayout(this)
                        {
                            LayoutParameters =
                                 new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                               0, 1f / itemBody.Count),
                            Orientation = Android.Widget.Orientation.Horizontal
                        };
                        baseLayout.AddView(blockLayout);
                        foreach (BlockElement e in b.blockElements)
                        {
                            if ((e as P).inline.GetType().Name == "Image") // add an image block
                            {
                                ImageView image = new ImageView(this);
                                Inline target = (e as P).inline;
                                string imgSrc = (target as Image).source;
                                var resourceId = (int)typeof(Resource.Drawable).GetField(imgSrc).GetValue(null);
                                image.SetImageResource(resourceId);
                                LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                                layoutParams.Gravity = GravityFlags.Center;
                                image.LayoutParameters = layoutParams;
                                image.Click += (o, x) =>
                                {
                                    ImageView bigImage = new ImageView(this);
                                    bigImage.SetImageResource(resourceId);
                                    LinearLayout.LayoutParams layoutParamsBig = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                                    layoutParams.Gravity = GravityFlags.CenterHorizontal;
                                    bigImage.LayoutParameters = layoutParamsBig;
                                    bigImage.Click += (a, c) =>
                                    {
                                        SetContentView(baseLayout);
                                    };
                                    SetContentView(bigImage);
                                };
                                blockLayout.AddView(image);
                            }
                            else if ((e as P).inline.GetType().Name == "Inline") // add a text block
                            {
                                TextView inlineText = new TextView(this);
                                LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                                layoutParams.Gravity = GravityFlags.Center;
                                inlineText.LayoutParameters = layoutParams;
                                inlineText.Text = (e as P).inline.content;
                                inlineText.TextSize = 15;
                                blockLayout.AddView(inlineText);

                            }
                        }
                        break;
                    default:
                        //textFlavour.Text += b.GetType().Name + "\n";
                        break;
                }
            }
            //textPrompt.Text = (itemBody[2] as ChoiceInteraction).prompt;
            //string res= "";
            //foreach (Choice c in (itemBody[2] as ChoiceInteraction).choices)
            //{
            //    res += c.ToString() + "\n";
            //}
            //textChoices.Text = res;

            //};
        }
    }
}