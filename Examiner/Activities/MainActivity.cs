using Android.App;
using System;
using Android.OS;
using Android.Content;
using System.Collections.Generic;
using Java.Lang;
using Android.Widget;
using System.Net;
using System.Text;
using System.IO;

namespace Examiner.Activities
{
    [Activity(Label = "Examiner", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        List<string> filePaths;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //var webClient = new WebClient();
            //webClient.DownloadStringCompleted += (s, e) => {
            //    var text = e.Result;
            //    Console.WriteLine(text);
            //};

            //var url = new Uri("http://xamarin.com"); // Html home page
            //webClient.Encoding = Encoding.UTF8;
            //webClient.DownloadStringAsync(url);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
             filePaths = new  List<string>();     
            listAssetFiles("qtiFiles");
            filePaths.Reverse();
            Button buttonExample = FindViewById<Button>(Resource.Id.buttonExample);
            buttonExample.Click += delegate
            {
                foreach (string file in filePaths)
                {
                    var intent = new Intent(this, typeof(QuestionActivity));
                    intent.PutExtra("qtiPath", file);
                    StartActivity(intent);
                }
            };
        }
        private bool listAssetFiles(string path)
        {

            List<string> list;
            try
            {
                list = new List<string>(Assets.List(path));
                if (list.Count > 0)
                {
                    // This is a folder
                    foreach (string file in list)
                    {
                        if (!listAssetFiles(path + "/" + file))
                            return false;
                    }
                }
                else {
                    // This is a file
                    filePaths.Add(path);
                }
            }
            catch (System.Exception e)
            {
                return false;
            }

            return true;
        }
    }
}

