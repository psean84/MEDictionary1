using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace MEDictionary
{   
    public class WordInformation
    {
        public String Word { get; set; }
        public String ImageUrl { get; set; }
        public String Grammer { get; set; }
        public String EPronunciation { get; set; }
        public String MPronunciation { get; set; }
        public bool IsEnglish { get; set; }
        public bool IsBookmarked { get; set; }
    }
    
    class DictionaryData
    {
        private JObject allObjects;
        private String _searchWord;

        public DictionaryData()
        {
        }

        public void initRead()
        {
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("MEDictionary.DictionaryData.medict_comp.json");

            StreamReader sr = new StreamReader(stream);

            allObjects = JObject.Parse(sr.ReadLine());

            sr.Dispose();
        }

        public List<String> getStringList()
        {
            List<String> words = new List<string>();
            IList<JToken> results = allObjects["dictionary"]["words"]["word"].Children().ToList();

            foreach (JToken jt in results)
            {
                WordInformation wi = jt.ToObject<WordInformation>();
                words.Add(wi.Word);
            }

            return words;
        }

        public void setSearchWord(String word)
        {
            _searchWord = word;
        }


        public WordInformation getInformation()
        {
            String searchToken = "$.dictionary['words']['word'][?(@.word == '" + _searchWord + "')]";

            JToken jwi = allObjects.SelectToken(searchToken);

            return jwi.ToObject<WordInformation>();

        }

        public List<WordDescription> getDefinations()
        {
            List<WordDescription> wdd = new List<WordDescription>();
            try
            {
                IList<JToken> lstjt = allObjects["dictionary"]["definations"][_searchWord].Children().ToList();
                if (lstjt != null)
                {
                    foreach (JToken jt in lstjt)
                    {
                        wdd.Add(new WordDescription(jt["defination"].ToString(), jt["sentence"].ToString()));
                    }
                    return wdd;
                }
                else
                {
                    return null;
                }
            }catch(Exception ex)
            {
                return null;
            }
        }


        public List<WordDescription> getTranslations()
        {
            List<WordDescription> wdt = new List<WordDescription>();
            try
            {
                IList<JToken> lstjt = allObjects["dictionary"]["translations"][_searchWord].Children().ToList();

                if (lstjt != null)
                {
                    foreach (JToken jt in lstjt)
                    {
                        wdt.Add(new WordDescription(jt["transword"].ToString(), jt["sentence"].ToString()));
                    }

                    return wdt;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                return null;
            }

        }
    }
}
