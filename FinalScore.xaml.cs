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
	public partial class FinalScore : ContentPage
	{
        double score;
        CardSet set;
        bool audioOn;
        List<Student> studentList;
        public FinalScore()
        {
            InitializeComponent();
            audioOn = false;
            int score = 0;
            set = new CardSet();
            Label finalScore = this.FindByName<Label>("finalScore");
            finalScore.Text = score.ToString();
        }
		public FinalScore (List<Student> users, double grade, CardSet deck, bool audio)
		{
			InitializeComponent ();
            studentList = users;
            score = grade;
            set = deck;
            audioOn = audio;
            Label finalScore = this.FindByName<Label>("finalScore");
            finalScore.Text = score.ToString();
            Label listScores = this.FindByName<Label>("highScore");
            //add user's grade to history
            studentList.First().addScore(new Score(set.getSet(), grade));
            //show user's high score
            Student user = studentList.First();
            String numStr = "";
            int k = 0;
            foreach(Score scr in user.getGrades())
            {
                if (scr.getName() == set.getSet())//compare scores of same game
                {
                    numStr += scr.getNum().ToString() + "\n";
                    k++;
                    if (k >= 5) //list 5 max scores
                        break;
                }
            }
            listScores.Text = numStr;
            if (audioOn == true)
            {
                var text = "final score " + score.ToString();
                CrossTextToSpeech.Current.Speak(text);
            }
		}

       

        private void main_Click(Button sender, EventArgs e)
        {
            if (audioOn == true)
            {
                var text = sender.Text;
                CrossTextToSpeech.Current.Speak(text);
            }
            Navigation.PushAsync(new MainPage(studentList,audioOn));
        }
        private void game_Click(Button sender, EventArgs e)
        {
            Navigation.PushAsync(new Games(studentList,set,audioOn));
        }
        private void study_Click(Button sender, EventArgs e)
        {
            if (audioOn == true)
            {
                var text = sender.Text;
                CrossTextToSpeech.Current.Speak(text);
            }

            Navigation.PushAsync(new StudySets(studentList,audioOn));
        }
    }
}