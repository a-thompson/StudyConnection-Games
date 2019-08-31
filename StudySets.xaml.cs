using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.TextToSpeech;

namespace Study
{
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StudySets : ContentPage
	{
        bool audioOn;
        CardSet set;
        List<CardSet> decks;
        List<FlashCard> cardList;
        List<Button> buttonList;

        Label errorLabel;
        List<Student> studentList;
        public StudySets()
		{
			InitializeComponent ();
            audioOn = false;
            init();
		}
        public StudySets(List<Student> users, bool audio)
        {
            InitializeComponent();
            studentList = users;
            audioOn = audio;
            if (audioOn == true)
                CrossTextToSpeech.Current.Speak("Select a study set");
            init();
        }
        private void init()
        {
            decks = new List<CardSet>();
            decks.Add(new CardSet());
            CardSet set2 = createSet2();
            decks.Add(set2);
            cardList = new List<FlashCard>();
            buttonList = new List<Button>();
            errorLabel = this.FindByName<Label>("error");
            errorLabel.IsVisible = false;

            //dynamically create buttons depending on number of study sets
            StackLayout entireLayout = this.FindByName<StackLayout>("entire");
            //put scroll view around entireLayout
            StackLayout copyLayout = this.FindByName<StackLayout>("select");
            Label copyLabel = this.FindByName<Label>("selectText");
            foreach (CardSet cardSet in decks)
            {//create buttons dynamically
                StackLayout layout = new StackLayout();
                layout.Orientation = copyLayout.Orientation;
                Button button = new Button();
                button.WidthRequest = 40;
                button.HeightRequest = 40;
                Label label = new Label();
                label.Text = cardSet.getSet();
                button.Clicked += delegate (object sender, EventArgs e)
                {
                    add_Click(sender, e);
                    if (audioOn == true)
                    {
                        var text = label.Text;
                        if (button.Text == ".")
                            text = text + " select";
                        else
                            text = text + " deselect";
                        CrossTextToSpeech.Dispose();
                        CrossTextToSpeech.Current.Speak(text);
                    }
                };
                label.FontSize = copyLabel.FontSize;
                label.FontAttributes = copyLabel.FontAttributes;
                label.VerticalOptions = copyLabel.VerticalOptions;

                layout.Children.Add(button);
                layout.Children.Add(label);
                entireLayout.Children.Add(layout);
                buttonList.Add(button);
            }
        }
        private CardSet createSet2()//demo 2, as CardSet contains a default demo 1
        {
            List<FlashCard> demo2 = new List<FlashCard>();
            List<String> points1 = new List<String>();
            points1.Add("user's understanding of system");
            demo2.Add(new FlashCard("mental model", points1));
            List<String> points2 = new List<String>();
            points2.Add("non numerical data");
            points2.Add("subjective responses");
            demo2.Add(new FlashCard("qualitative questions involve", points2));
            List<String> points3 = new List<String>();
            points3.Add("instructing");
            points3.Add("conversing");
            points3.Add("manipulating");
            points3.Add("exploring");
            demo2.Add(new FlashCard("4 interaction types", points3));
            List<String> points4 = new List<String>();
            points4.Add("numerical data");
            points4.Add("objective input");
            demo2.Add(new FlashCard("quantitative questions involve", points4));
            List<String> points5 = new List<String>();
            points5.Add("written reminder");
            points5.Add("ie: diaries, reminders, notes, etc");
            demo2.Add(new FlashCard("external cognition", points5));
            CardSet newSet = new CardSet("Demo 2", demo2);
            return newSet;
        }
        
        //buttons
        protected void add_Click(object sender, EventArgs e)
        {//add deck from dynamic button
            errorLabel.IsVisible = false;
            int i = 0;
            foreach (Button button in buttonList)
            {
                if (button == sender)
                {
                    if (button.Text == ".")
                    {
                        button.Text = "";
                        foreach (FlashCard card in decks[i].getCards())
                        {
                            if (cardList.Contains(card))
                                cardList.Remove(card);
                        }
                    }
                    else
                    {
                        button.Text = ".";
                        foreach (FlashCard card in decks[i].getCards())
                        {
                            if (!cardList.Contains(card))
                                cardList.Add(card);
                        }
                    }
                }
                i++;
            }
        }
        private void all_Click(Button sender, EventArgs e)
        {
            
            errorLabel.IsVisible = false;
            if (sender.Text == ".")
            {
                if (audioOn == true)
                {
                    var text = "Select None";
                    CrossTextToSpeech.Dispose();
                    CrossTextToSpeech.Current.Speak(text);
                }
                int i = 0;
                sender.Text = "";
                foreach(Button button in buttonList)
                {
                    //add_Click(button, e);
                    button.Text = "";
                    foreach (FlashCard card in decks[i].getCards())
                    {
                        if (cardList.Contains(card))
                            cardList.Remove(card);
                    }
                    i++;
                }
            }
            else
            {
                if (audioOn == true)
                {
                    var text = "Select All";
                    CrossTextToSpeech.Dispose();
                    CrossTextToSpeech.Current.Speak(text);
                }
                sender.Text = ".";
                int i = 0;
                foreach (Button button in buttonList)
                {
                    button.Text = ".";
                    foreach (FlashCard card in decks[i].getCards())
                    {
                        if (!cardList.Contains(card))
                            cardList.Add(card);
                    }
                    i++;
                }
            }
        }
        private void next_Click(Button sender, EventArgs e)
        {//Text = "Choose"
            var text = sender.Text;
            if (cardList.Count > 0)
            {
                if (audioOn == true)
                {
                    text = sender.Text;
                    CrossTextToSpeech.Dispose();
                    CrossTextToSpeech.Current.Speak(text);
                }
                set = new CardSet("Deck", cardList);
                Navigation.PushAsync(new Games(studentList,set,audioOn));
            }
            else
            {
                errorLabel.IsVisible = true;
                if (audioOn == true)
                {
                    text = errorLabel.Text;
                    CrossTextToSpeech.Dispose();
                    CrossTextToSpeech.Current.Speak(text);
                }
            }
        }

    }
}