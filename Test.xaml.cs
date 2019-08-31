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
	public partial class Test : ContentPage
	{//needs text to voice
        CardSet set;
        bool audioOn;
        List<MultipleChoice> questions;
        int count;
        double grade;
        double UP = 5;
        double DOWN = 1;
        double MAX = 100;
        List<Student> studentList;

        Label scoreLbl;
        Label questionLbl;
        Button buttonA;
        Button buttonB;
        Button buttonC;
        Button buttonD;
        Button buttonNext;
		public Test ()
		{
			InitializeComponent ();
            set = new CardSet(); //default
            audioOn = false;
            start();
            init();
        }
        public Test(List<Student> users,CardSet list, bool audio)
        {
            InitializeComponent();
            studentList = users;
            set = list;
            audioOn = audio;
            start();
            init();
        }

        private void start()
        {
            CrossTextToSpeech.Dispose();
            int num = 20; //number of questions
            if (set.getCards().Count < num)
                num = set.getCards().Count;
            UP = MAX / num;
            DOWN = UP / 5;
            set.setSet("Test");
            questionLbl = this.FindByName<Label>("question");
            scoreLbl = this.FindByName<Label>("title");
            buttonA = this.FindByName<Button>("a");
            buttonB = this.FindByName<Button>("b");
            buttonC = this.FindByName<Button>("c");
            buttonD = this.FindByName<Button>("d");
            buttonNext = this.FindByName<Button>("next");
            grade = 0;
            count = 0;

            
            questions = new List<MultipleChoice>();

            Random random = new Random();
            int rnd;

            List<String> options = new List<String>();
            foreach(FlashCard card in set.getCards())
            {
                String ques = card.getTopic();
                String point1, point2, point3, correct;
                rnd = random.Next(0, 4);
                //probability
                if ((card.getPoints().Count >= 3) && (rnd == 1)) //at least 3 points
                {
                    Shuffle<String> allTrue = new Shuffle<String>(card.getPoints());

                    point1 = allTrue.getList()[0];
                    point2 = allTrue.getList()[1];
                    point3 = allTrue.getList()[2];
                    correct = "All are true";
                    questions.Add(new MultipleChoice(ques, point1, point2, point3, correct, card));
                }
                else
                {
                    correct = card.getPoints().First();
                    int rnd1 = random.Next(0, 10);
                    if (rnd1 == 1)//chance for 'all are true' incorrect option
                        options.Add("All are true");
                    while (options.Count < 3)
                    {
                        rnd = random.Next(set.getCards().Count - 1);
                        
                        if (set.getCards()[rnd] != card)
                        { //if not looking at same card, choose one point as wrong answer
                            if (!options.Contains(set.getCards()[rnd].getPoints().First()))
                                options.Add(set.getCards()[rnd].getPoints().First());
                        }
                    }
                    //incorrect options
                    questions.Add(new MultipleChoice(ques, options[0], options[1], options[2], correct, card));
                    while (options.Count > 0)
                        options.Remove(options.First());
                }
            }
            
            if (questions.Count > num)
            {
                Shuffle<MultipleChoice> ques = new Shuffle<MultipleChoice>(questions);
                questions = ques.getList();
                while (questions.Count > num)
                { //only num number of questions
                    questions.Remove(questions.First());
                }
            }
        }
        private void init()
        {
            CrossTextToSpeech.Dispose();
            //iterate question count
            count++;
            //set colors
            buttonNext.IsVisible = false;
            scoreLbl.Text = "Test  Score: " + Math.Round(grade,2);
            buttonA.BackgroundColor = Color.Orange;
            buttonB.BackgroundColor = Color.CornflowerBlue;
            buttonC.BackgroundColor = Color.Salmon;
            buttonD.BackgroundColor = Color.MediumPurple;

            MultipleChoice current = questions.First();
            List<String> options = new List<String>();
            options.Add(current.getA());
            options.Add(current.getB());
            options.Add(current.getC());
            options.Add(current.getCorrect());
            Shuffle<String> opt = new Shuffle<String>(options);
            
            questionLbl.Text = count + ") " + current.getQuestion();
            if (audioOn == true)
            {
                var text = questionLbl.Text;
                CrossTextToSpeech.Current.Speak(text);
            }
            buttonA.Text = opt.getList()[0];
            buttonB.Text = opt.getList()[1];
            buttonC.Text = opt.getList()[2];
            buttonD.Text = opt.getList()[3];
            while (options.Count > 0)
                options.Remove(options.First());
        }
        private void check(Button sender)
        {
            CrossTextToSpeech.Dispose();
            buttonA.BackgroundColor = Color.LightGray;
            buttonB.BackgroundColor = Color.LightGray;
            buttonC.BackgroundColor = Color.LightGray;
            buttonD.BackgroundColor = Color.LightGray;
            if (sender.Text == questions.First().getCorrect())
            {
                sender.BackgroundColor = Color.Green;
                grade += UP;
                scoreLbl.Text = "Test  Score: " + Math.Round(grade,2) + " CORRECT!";
                if (audioOn == true)
                {
                    CrossTextToSpeech.Current.Speak("correct");
                }
            }
           else
            {
                sender.BackgroundColor = Color.Red;
                grade -= DOWN;
                scoreLbl.Text = "Test  Score: " + Math.Round(grade,2) + " INCORRECT!";
                if (audioOn == true)
                {
                    CrossTextToSpeech.Current.Speak("incorrect");
                }
                //error catch
                if (questions.Count == 0)
                    return;
                //find correct answer
                if (buttonA.Text == questions.First().getCorrect())
                {
                    buttonA.BackgroundColor = Color.Green;
                }
                else if (buttonB.Text == questions.First().getCorrect())
                {
                    buttonB.BackgroundColor = Color.Green;
                }
                else if (buttonC.Text == questions.First().getCorrect())
                {
                    buttonC.BackgroundColor = Color.Green;
                }
                else if (buttonD.Text == questions.First().getCorrect())
                {
                    buttonD.BackgroundColor = Color.Green;
                }
            }
            questions.Remove(questions.First());
            buttonNext.IsVisible = true;
        }
        private void clickLogic(Button sender)
        {
            CrossTextToSpeech.Dispose();
            if (next.IsVisible == false)
            {
                if ((audioOn == true) && (sender.BackgroundColor != Color.Yellow))
                {
                    var text = sender.Text;
                    CrossTextToSpeech.Current.Speak(text);
                    buttonA.BackgroundColor = Color.LightGray;
                    buttonB.BackgroundColor = Color.LightGray;
                    buttonC.BackgroundColor = Color.LightGray;
                    buttonD.BackgroundColor = Color.LightGray;
                    sender.BackgroundColor = Color.Yellow;
                }
                else
                    check(sender);
            }
            else if (sender.BackgroundColor == Color.Green)
            {
                if (audioOn == true)
                {
                    var text = sender.Text;
                    CrossTextToSpeech.Current.Speak(text);
                }
            }
        }
        private void a_Click(Button sender, EventArgs e)
        {
            clickLogic(sender);
        }
        private void b_Click(Button sender, EventArgs e)
        {
            clickLogic(sender);
        }
        private void c_Click(Button sender, EventArgs e)
        {
            clickLogic(sender);
        }
        private void d_Click(Button sender, EventArgs e)
        {
            clickLogic(sender);
        }
        private void next_Click(Button sender, EventArgs e)
        {
            if (questions.Count == 0)
            {
                //final score
                grade = Math.Round(grade, 2);
                Navigation.PushAsync(new FinalScore(studentList,grade,set,audioOn));
            }
            else
            {
                init();
            }
        }
    }
}