using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.TextToSpeech;


namespace Study
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Memory : ContentPage
	{
        CardSet set;
        CardSet copy;
        FlashCard current;
        double grade;
        double UP = 5;
        double DOWN = 1;
        double MAX = 100;
        bool audioOn;
        int left;

        Label score;
        Label num;
        Button card;
        Entry entry;
        Button enterBtn;
        Button nextBtn;
        Button fixBtn;
        Button wrongBtn;
        Button recordBtn;
        Label correctLbl;
        List<Student> studentList;
		public Memory ()
		{
			InitializeComponent ();

            set = new CardSet();
            copy = new CardSet();
            audioOn = false;
            start();
		}
        public Memory(List<Student> users,CardSet list, bool audio)
        {
            InitializeComponent();
            studentList = users;
            set = list;
            audioOn = audio;
            String name = set.getSet();
            List<FlashCard> myCards = new List<FlashCard>();
            foreach(FlashCard card in list.getCards())
            {
                myCards.Add(card);
            }
            copy = new CardSet(name, myCards);
            start();
        }

        private void start()
        {
            copy.setSet("Memory");
            UP = MAX / set.getCards().Count();
            DOWN = UP / 5;
            grade = 0;
            score = this.FindByName<Label>("title");
            num = this.FindByName<Label>("page");
            card = this.FindByName<Button>("cardFace");
            entry = this.FindByName<Entry>("answer");
            enterBtn = this.FindByName<Button>("try");
            nextBtn = this.FindByName<Button>("next");
            fixBtn = this.FindByName<Button>("fix");
            wrongBtn = this.FindByName<Button>("wrong");
            correctLbl = this.FindByName<Label>("correct");
            recordBtn = this.FindByName<Button>("record");
            card.BackgroundColor = Color.DarkTurquoise;
            //entry.BackgroundColor = Color.White;
            fixBtn.IsVisible = false; //hide
            wrongBtn.IsVisible = false; //hide
            nextBtn.IsVisible = false; //hide

            left = 1;
            num.Text = left.ToString() + " / " + copy.getCards().Count().ToString();
            current = set.getCards().First();
            string output = "";
            int i = 0;
            foreach (String point in current.getPoints())
            {
                i++;
                output += i.ToString() + ") " + point + "\n";
            }
            card.Text = output;
            if (audioOn == true)
            {
                CrossTextToSpeech.Dispose();
                var text = card.Text;
                CrossTextToSpeech.Current.Speak(text);
            }
        }
        private void update()
        {
            score.Text = "Memory  Score: " + Math.Round(grade,2);
            left++;
            num.Text = left.ToString() + " / " + copy.getCards().Count().ToString();
            correctLbl.Text = "";
            card.BackgroundColor = Color.DarkTurquoise;
            entry.Text = "";
            nextBtn.IsVisible = false;
            fixBtn.IsVisible = false;
            wrongBtn.IsVisible = false;
            enterBtn.IsVisible = true;
            
            if (set.getCards().Count > 0)
            {
                current = set.getCards().First();
                string output = "";
                foreach (String point in current.getPoints())
                {
                    output += point + "\n";
                }
                card.Text = output;
                if (audioOn == true)
                {
                    CrossTextToSpeech.Dispose();
                    var text = card.Text;
                    CrossTextToSpeech.Current.Speak(text);
                }
            }
        }
        
        //objects
        private void cardFace_Click(Button sender, EventArgs e)
        {
            CrossTextToSpeech.Dispose();
            //take text and present audio
            if (audioOn == true)
            {
                if (card.BackgroundColor != Color.DarkTurquoise)
                {
                    var text = correctLbl.Text;
                    CrossTextToSpeech.Current.Speak(text);
                }
                else
                {
                    var text = sender.Text;
                    CrossTextToSpeech.Current.Speak(text);
                }
            }
        }
        private void try_Click(object sender, EventArgs e)
        {
            CrossTextToSpeech.Dispose();
            //print answer
           
            correctLbl.Text = current.getTopic();
            bool pass = false;

            if ((entry.Text == null) || (entry.Text == ""))
            {
                //visibly check
                card.BackgroundColor = Color.Yellow;
                wrongBtn.IsVisible = true;
                fixBtn.IsVisible = true;
                var text = correctLbl.Text;
                CrossTextToSpeech.Current.Speak(text);
            }
            else
            {
                
                //check answer
                if (current.getTopic().ToUpper() == entry.Text.ToUpper())
                {
                    //correct
                    card.BackgroundColor = Color.Green;
                    grade += UP;
                    score.Text = "Memory  Score: " + Math.Round(grade, 2) + " CORRECT!";
                    if (audioOn == true)
                        CrossTextToSpeech.Current.Speak("correct");
                    nextBtn.IsVisible = true;
                }
                else
                {
                    //incorrect
                    card.BackgroundColor = Color.Red;
                    grade -= DOWN;
                    score.Text = "Memory  Score: " + Math.Round(grade, 2) + " INCORRECT!";

                    if (audioOn == true)
                    {
                        CrossTextToSpeech.Current.Speak("incorrect");

                    }
                    nextBtn.IsVisible = true;
                    fixBtn.IsVisible = true;

                    var text = correctLbl.Text;
                    CrossTextToSpeech.Current.Speak(text);
                }

            }


            enterBtn.IsVisible = false;
        }
        private void next_Click(object sender, EventArgs e)
        {
            if (set.getCards().Count > 0)
                set.getCards().Remove(set.getCards().First());
            if (set.getCards().Count == 0)
            {
                grade = Math.Round(grade, 2);
                Navigation.PushAsync(new FinalScore(studentList,grade,copy,audioOn));
            }
            else
                update();
        }
        private void wrong_Click(object sender, EventArgs e)
        {
            CrossTextToSpeech.Dispose();
            if (audioOn == true)
                CrossTextToSpeech.Current.Speak("Incorrect");
            //remove card
            if (set.getCards().Count > 0)
                set.getCards().Remove(set.getCards().First());
            grade -= DOWN;
            //if set is done
            if (set.getCards().Count == 0)
            {
                grade = Math.Round(grade, 2);
                Navigation.PushAsync(new FinalScore(studentList, grade, copy, audioOn));
            }
            else
                update();
        }
        private void fix_Click(object sender, EventArgs e)
        {
            CrossTextToSpeech.Dispose();
            if (wrongBtn.IsVisible == true)
            {
                if (audioOn == true)
                {
                    CrossTextToSpeech.Current.Speak("correct");
                }
                if (set.getCards().Count > 0)
                    set.getCards().Remove(set.getCards().First());
                grade += UP;
            }
            else
            {
                if (audioOn == true)
                {
                    CrossTextToSpeech.Current.Speak("fixed");
                }
                if (set.getCards().Count > 0)
                    set.getCards().Remove(set.getCards().First());

                grade += UP + DOWN;
            }
            if (set.getCards().Count == 0)
            {
                grade = Math.Round(grade, 2);
                Navigation.PushAsync(new FinalScore(studentList,grade, copy,audioOn));
            }
            else
                update();
        }
    }
    
}