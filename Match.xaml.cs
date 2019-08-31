using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.TextToSpeech;

namespace Study
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Match : ContentPage
	{
        List<Button> buttonList;
        Button currentBtn;
        CardSet set;
        List<FlashCard> setCopy;
        double grade;
        double UP = 5;
        double DOWN = 1;
        double MAX = 100;
        Label label;
        bool audioOn;
        FlashCard cardFirst;
        List<Student> studentList;
        public Match ()
		{
			InitializeComponent ();
            audioOn = false;
            set = new CardSet();//default
            start();
        }
        public Match(List<Student> users, CardSet list, bool audio)
        {
            InitializeComponent();
            studentList = users;
            audioOn = audio;
            set = list;
            
            start();
        }
        private void start()
        {
            set.setSet("Match");
            //cardFirst = set.getCards().First();//get first card
            label = this.FindByName<Label>("title");
            grade = 0;
            UP = MAX / set.getCards().Count();
            DOWN = UP / 5;
            label.Text = "Match  Score: " + grade;

            buttonList = new List<Button>();
            buttonList.Add(this.FindByName<Button>("topic1"));
            buttonList.Add(this.FindByName<Button>("topic2"));
            buttonList.Add(this.FindByName<Button>("topic3"));
            buttonList.Add(this.FindByName<Button>("topic4"));
            buttonList.Add(this.FindByName<Button>("point1"));
            buttonList.Add(this.FindByName<Button>("point2"));
            buttonList.Add(this.FindByName<Button>("point3"));
            buttonList.Add(this.FindByName<Button>("point4"));
            Shuffle<FlashCard> card = new Shuffle<FlashCard>(set.getCards());
            setCopy = card.getList();
            reload();
        }

        private void reload()
        {
            List<String> options = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                if (setCopy.Count() == 0)
                    break;
                options.Add(setCopy.First().getTopic());
                options.Add(setCopy.First().getPoints().First());
                setCopy.Remove(setCopy.First());
                
            }
            if (options.Count() == 0)
            {
                showScore();
                return;
            }
            Shuffle<String> opt = new Shuffle<String>(options);
            options = opt.getList();


            foreach (Button btn in buttonList) //assign each button
            {
                String temp = options.First();
                btn.Text = temp;
                options.Remove(temp); //rotate options list
                //options.Add(temp);
                btn.BackgroundColor = Color.LightGray;
                btn.IsVisible = true;
                if (options.Count == 0)
                    break;
            }
            
        }
        //compare buttons to flashcards
        private void compare()
        {
            Button side1 = new Button();
            Button side2 = new Button();
            bool select = false;
            bool run = false;
            foreach (Button btn in buttonList) //assign each button
            {
                if(btn.BackgroundColor == Color.Yellow)
                {
                    if (select == false)
                    {
                        select = true;
                        side1 = btn;
                    }
                    else
                    {
                        run = true;
                        side2 = btn;
                        break;
                    }
                }
            }
            bool correct = false;
            if (run == true)
            {
                //compare text strings to flashcards
                foreach(FlashCard card in set.getCards())
                {
                    if (side1.Text == card.getTopic())
                    {
                        //got topic card
                        //rotate through points
                        foreach(String point in card.getPoints())
                        {
                            if (side2.Text == point)
                            {
                                correct = true;
                                break;
                            }

                        }
                    }
                    else if (side2.Text == card.getTopic())
                    {
                        //got topic card
                        //rotate through points
                        foreach (String point in card.getPoints())
                        {
                            if (side1.Text == point)
                            {
                                correct = true;
                                break;
                            }

                        }
                    }
                }
                if (correct == true) 
                {
                    side1.BackgroundColor = Color.Green;
                    side2.BackgroundColor = Color.Green;
                    grade += UP;
                    
                    label.Text = "Match  Score: " + Math.Round(grade,2) + " CORRECT!";
                    if (audioOn == true)
                        CrossTextToSpeech.Current.Speak("correct");
                }
                else
                {
                    side1.BackgroundColor = Color.Red;
                    side2.BackgroundColor = Color.Red;
                    grade -= DOWN;
                    
                    label.Text = "Match  Score: " + Math.Round(grade,2) + " INCORRECT!";
                    if (audioOn == true)
                        CrossTextToSpeech.Current.Speak("incorrect");
                }
            }
            else
            {
                if (audioOn == true)
                {
                    var text = side1.Text;
                    CrossTextToSpeech.Current.Speak(text);
                }
            }
        }
        //button press
        private void showScore()
        {
            bool end = true;
            foreach (Button btn in buttonList)
            {
                if (btn.IsVisible == true)
                { //hide green buttons
                    end = false;
                }
            }
            if (end == true)
            {
                //show final score
                //call page
                grade = Math.Round(grade, 2);
                Navigation.PushAsync(new FinalScore(studentList,grade,set,audioOn));
            }
            
        }
        private void click(Button sender)
        {
            CrossTextToSpeech.Dispose();
            currentBtn = sender;
            bool view = false;
            foreach (Button btn in buttonList) //reset red buttons
            {
                if (btn.BackgroundColor == Color.Red)
                {
                    btn.BackgroundColor = Color.LightGray;
                }
                else if (btn.BackgroundColor == Color.Green)
                { //hide green buttons
                    btn.IsVisible = false;
                }
                if (btn.IsVisible == true)
                    view = true;
            }
            //check for visibility
            if (view == false)
            {
                //check if final score is ready final score
                reload();
                //exit if invisible button is clicked
                return;
            }
            else if (sender.IsVisible == false)
                return;
            //change button color
            if (sender.BackgroundColor == Color.Yellow)
            {
                sender.BackgroundColor = Color.LightGray;
                
            }
            else
            {
                sender.BackgroundColor = Color.Yellow;
                compare();
            }
        }
        //buttons
        private void topic1_Click(Button sender, EventArgs e)
        {
            click(sender);
        }
        private void topic2_Click(Button sender, EventArgs e)
        {
            click(sender);
        }
        private void topic3_Click(Button sender, EventArgs e)
        {
            click(sender);
        }
        private void topic4_Click(Button sender, EventArgs e)
        {
            click(sender);
        }
        private void point1_Click(Button sender, EventArgs e)
        {
            click(sender);
        }
        private void point2_Click(Button sender, EventArgs e)
        {
            click(sender);
        }
        private void point3_Click(Button sender, EventArgs e)
        {
            click(sender);
        }
        private void point4_Click(Button sender, EventArgs e)
        {
            click(sender);
        }
    }
}