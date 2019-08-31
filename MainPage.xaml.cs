using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.TextToSpeech;

namespace Study
{
    public partial class MainPage : ContentPage
    {
        bool audioOn;
        List<Student> studentList;
        public MainPage()
        {
            InitializeComponent();
            audioOn = false;
            createStudents();
        }
        public MainPage(List<Student> users, bool audio)
        {
            InitializeComponent();
            CrossTextToSpeech.Dispose();
            studentList = users;
            audioOn = audio;
            ImageButton buttonAudio = this.FindByName<ImageButton>("sound");
            if (audioOn == true)
                buttonAudio.Source = "audioIsOn.png";
        }

        private void createStudents()
        {
            String game1 = "Match";
            String game2 = "Memory";
            String game3 = "Test";
            studentList = new List<Student>();

            //this is our selected user
            List<Score> scoreListDemo = new List<Score>();
            studentList.Add(new Student("Demo User", scoreListDemo));

            List<Score> scoreList1 = new List<Score>();
            scoreList1.Add(new Score(game1, 70));
            scoreList1.Add(new Score(game2, 82));
            scoreList1.Add(new Score(game3, 90));
            studentList.Add(new Student("Abby Reiter", scoreList1));

            List<Score> scoreList2 = new List<Score>();
            scoreList2.Add(new Score(game1, 89));
            scoreList2.Add(new Score(game2, 62));
            scoreList2.Add(new Score(game3, 77));
            studentList.Add(new Student("Brandon Smith", scoreList2));
        }

        private void games_Click(Button sender, EventArgs e)
        {
            if (audioOn == true)
            {
                var text = sender.Text;
                CrossTextToSpeech.Current.Speak(text);
            }
            Navigation.PushAsync(new StudySets(studentList,audioOn));
        }
        private void audio_Click(ImageButton sender, EventArgs e)
        {
            CrossTextToSpeech.Dispose();
            if (audioOn == false)
            {
                CrossTextToSpeech.Current.Speak("Audio On");
                sender.Source = "audioIsOn.png";
                audioOn = true;
            }
            else
            {
                CrossTextToSpeech.Current.Speak("Audio Off");
                sender.Source = "audio.png";
                audioOn = false;
            }
        }

    }
}
