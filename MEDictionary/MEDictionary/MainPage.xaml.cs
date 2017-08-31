using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MEDictionary
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<DictionaryClass> DictionaryResults;
        private String _srWord;
        private String _srwEnglishPronunciation;
        private String _srwMarathiPronunciation;
        private DictionaryData _dd;
        private List<String> _words;

        public MainPage()
        {
            InitializeComponent();

            loadApplicationImages();
            
            setClickEventForWordPronunciation();
            setClickEventForBookmark();
            setClickEventForTabs();

            _dd = new DictionaryData();
            _dd.initRead();
            _words = _dd.getStringList();

            stResult.Scale = 0;
        }
        
        private void setClickEventForWordPronunciation()
        {
            TapGestureRecognizer stackClick = new TapGestureRecognizer();

            stackClick.Tapped += (s, e) =>
            {
                if (lblPronunciation.Text.Equals(_srwEnglishPronunciation))
                    lblPronunciation.Text = _srwMarathiPronunciation;
                else
                    lblPronunciation.Text = _srwEnglishPronunciation;
            };

            stWordPronunciation.GestureRecognizers.Add(stackClick);
        }

        private void setClickEventForBookmark()
        {
            TapGestureRecognizer stackClick = new TapGestureRecognizer();
            stackClick.Tapped += async (s, e) => 
            {
                if (imgBookmark.TranslationY == -40)
                {
                    setBookmark();
                    await imgBookmark.TranslateTo(imgBookmark.X, 0, 200, Easing.SinIn);
                    imgBookmark.TranslationY = 0;
                }
                else
                {
                    removeBookmark();
                    await imgBookmark.TranslateTo(imgBookmark.X, -40, 200, Easing.Linear);
                    imgBookmark.TranslationY = -40;
                }
            };
            abBookmark.GestureRecognizers.Add(stackClick);
        }

        private void setBookmark()
        {

        }

        private void removeBookmark()
        {

        }

        private void loadApplicationImages()
        {
            imgBookmark.Source = ImageSource.FromResource("MEDictionary.Images.bookmark.png");
            imgEnglish.Source = ImageSource.FromResource("MEDictionary.Images.underline.png");
            imgMarathi.Source = ImageSource.FromResource("MEDictionary.Images.underline.png");
        }

        private void setClickEventForTabs()
        {
            TapGestureRecognizer tabClick = new TapGestureRecognizer();

            tabClick.Tapped += (s, e) =>
            {
                if (frEnglish.Equals(s))
                    cvResults.Position = 0;
                else
                    cvResults.Position = 1;
            };

            frEnglish.GestureRecognizers.Add(tabClick);

            frMarathi.GestureRecognizers.Add(tabClick);
        }

        private void populateData(String word)
        {
            _dd.setSearchWord(word);

            WordInformation wi = _dd.getInformation();

            DictionaryClass dce = new DictionaryClass();

            DictionaryClass dcm = new DictionaryClass();

            _srWord = wi.Word;
            _srwEnglishPronunciation = wi.EPronunciation;
            if (wi.MPronunciation == "null")
            {
                _srwMarathiPronunciation = "";
            }
            else
            {
                _srwMarathiPronunciation = wi.MPronunciation;
            }
            lblPronunciation.Text = _srwEnglishPronunciation;
            lblWord.Text = _srWord;

            if (wi.IsEnglish)
            {
                dce.addDefinations(_dd.getDefinations());
                dcm.addDefinations(_dd.getTranslations());

                if (wi.ImageUrl != "null")
                {
                    switch (Device.RuntimePlatform)
                    {
                        case Device.Android:
                            dce.setImage(wi.ImageUrl);
                            break;
                        default:
                            dce.setImage("Images\\" + wi.ImageUrl);
                            break;
                    }
                }

            }
            else
            {
                dce.addDefinations(_dd.getTranslations());
                dcm.addDefinations(_dd.getDefinations());

                if (wi.ImageUrl != "null")
                {
                    switch (Device.RuntimePlatform)
                    {
                        case Device.Android:
                            dcm.setImage(wi.ImageUrl);
                            break;
                        default:
                            dcm.setImage("Images\\" + wi.ImageUrl);
                            break;
                    }
                }
            }

            dcm.setLanguage(DictionaryClass.Language.Marathi);
            dce.setLanguage(DictionaryClass.Language.English);

            DictionaryResults = new ObservableCollection<DictionaryClass>() { dce, dcm };
            cvResults.ItemsSource = DictionaryResults;
            stResult.ScaleTo(1);
        }
        private void cvResults_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (((DictionaryClass)e.SelectedItem).LanguageUsed == DictionaryClass.Language.English)
            {
                imgEnglish.IsVisible = true;
                imgMarathi.IsVisible = false;
            }
            else
            {
                imgEnglish.IsVisible = false;
                imgMarathi.IsVisible = true;
            }

        }

        private void Frame_SizeChanged(object sender, EventArgs e)
        {
            Frame fr = (Frame)sender;

            fr.WidthRequest = this.Width / 2;
        }

        private void sbSearch_SearchButtonPressed(object sender, EventArgs e)
        {
            stResult.Scale = 0;
            String searchText = sbSearch.Text;
            sbSearch.Text = "";
            if (_words.Contains(searchText))
            {
                populateData(searchText);
            }
        }

        private void lvDescriptions_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                String selectedString = ((WordDescription)e.SelectedItem).Defination;

                ((ListView)sender).SelectedItem = null;

                selectedString = selectedString.Replace(';', ' ');

                List<String> wordsInString = selectedString.Split(' ').ToList();

                if (wordsInString.Count() == 1)
                {
                    populateData(wordsInString.First());
                }
                else
                {
                    lstSelectedWords.ItemsSource = wordsInString;
                    frSelectedWordsList.IsVisible = true;
                }
            }
        }

        private void lstSelectedWords_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            frSelectedWordsList.IsVisible = false;
            String selectedWord = e.SelectedItem.ToString();
            if (_words.Contains(selectedWord))
                populateData(selectedWord);
        }
    }
}
