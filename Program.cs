using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewsConfidenceScore
{

    class Program
    {

        public static Dictionary<String,int> positive = new Dictionary<String,int>();
        public static Dictionary<String,int> negative = new Dictionary<String,int>();

        public static Dictionary<String, int> positiveFound = new Dictionary<String, int>();
        public static Dictionary<String, int> negativeFound = new Dictionary<String, int>();

        static void Main(string[] args)
        {
            loadConfidenceKeyFiles();
            
            Console.WriteLine($"Loaded {positive.Count.ToString()} positive keys and {negative.Count.ToString()} negative keys.");

            string News = "Diffusion Pharmaceuticals Announces Pre-IND Submission To FDA Of Design For TSC Trials To Treat Acute Respiratory Distress Syndrome In COVID-19";

            int confidenceScore = GetConfidenceScoreWeighted(News);

            Console.WriteLine("Test News Title: " + News);

            Console.WriteLine("Positive Keys Found: ");
            foreach (var entry in positiveFound) Console.WriteLine(entry);

            Console.WriteLine("Negative Keys Found: ");
            foreach (var entry in negativeFound) Console.WriteLine(entry);

            Console.WriteLine("Weighted Score Result: " + confidenceScore.ToString());

            Console.ReadLine();
        }

        public static void loadConfidenceKeyFiles()
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader("positive.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string splitMe = sr.ReadLine();
                    string[] sp = splitMe.Split(new char[] { ',' });

                    if (sp.Length < 2) continue;
                    else if (sp.Length == 2 && !positive.ContainsKey(sp[0].Trim().ToLower())) positive.Add(sp[0].Trim().ToLower(), int.Parse(sp[1].Trim()));

                }
            }
            using (System.IO.StreamReader sr = new System.IO.StreamReader("negative.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string splitMe = sr.ReadLine();
                    string[] sp = splitMe.Split(new char[] { ',' });

                    if (sp.Length < 2) continue;
                    else if (sp.Length == 2 && !negative.ContainsKey(sp[0].Trim().ToLower())) negative.Add(sp[0].Trim().ToLower(), int.Parse(sp[1].Trim()));

                }
            }
        }//

        public static int GetConfidenceScoreWeighted(string News)
        {
            string[] n = News.ToLower().Split(new char[] { ' ' });
            int pos = 0;
            int neg = 0;

            foreach (string word in n)
            {
                if (positive.ContainsKey(word)) { 
                    pos += positive[word];
                    positiveFound.Add(word, positive[word]);
                }
                if (negative.ContainsKey(word)) { 
                    neg += negative[word];
                    negativeFound.Add(word, negative[word]);
                }
            }

                return pos-neg;
        }//

    }
}
