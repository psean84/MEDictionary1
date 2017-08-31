using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MEDictionary
{
    public class WordDescription
    {
        public String Defination { get; private set; }
        public String Sentence { get; private set; }
        
        public WordDescription(string def, String sent)
        {
            Defination = def;
            Sentence = sent;
        }
    }
    public class DictionaryClass
    {
        public enum Language { English, Marathi };
        public String ExampleImageURL { get; private set; }
        public Language LanguageUsed { get; private set; }
        public List<WordDescription> Descriptions { get; private set; }

        public DictionaryClass()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    ExampleImageURL = "notfound.png";
                    break;
                default:
                    ExampleImageURL = "Images\\notfound.png";
                    break;
            }

            Descriptions = new List<WordDescription>();
        }

        public void setLanguage(Language l)
        {
            LanguageUsed = l;
        }

        public void setImage(string imageURL)
        {
            ExampleImageURL = imageURL;
        }

        public void addDefination(String def, String sent)
        {
            Descriptions.Add(new WordDescription(def, sent));
        }

        public void addDefinations(List<WordDescription> wdd)
        {
            if (wdd != null)
            {
                Descriptions.AddRange(wdd);
            }
        }

    }
}
