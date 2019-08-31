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
	public partial class Games : ContentPage
	{
        bool audioOn;
        CardSet set;
        List<Student> studentList;
		public Games ()
		{
			InitializeComponent ();
            set = new CardSet();
            audioOn = false;

        }
        public Games(List<Student> users, CardSet list, bool audio)
        {
            InitializeComponent();
            studentList = users;
            set = list;
            audioOn = audio;
            if (audioOn == true)
                CrossTextToSpeech.Current.Speak("Games");

            findScores();
           
            Shuffle<FlashCard> st = new Shuffle<FlashCard>(set.getCards());
            set.setCards(st.getList());
            int num = set.getCards().Count;
            foreach (FlashCard card in set.getCards())
            { //shuffle each card point
                Shuffle<String> cd = new Shuffle<String>(card.getPoints());
                card.setPoints(cd.getList());
            }
        }
        private void findScores()
        {
            Label matchScr = this.FindByName<Label>("matchScore");
            Label memoryScr = this.FindByName<Label>("memoryScore");
            Label testScr = this.FindByName<Label>("testScore");
            String matchOut = "";
            String memoryOut = "";
            String testOut = "";
            double matchTk = 0;
            double memoryTk = 0;
            double testTk = 0;
            foreach(Student st in studentList)
            {
                foreach(Score scr in st.getGrades())
                {
                    if (scr.getName() == "Match")
                    {
                        if (scr.getNum() > matchTk)
                        {
                            matchTk = scr.getNum();
                            matchOut = st.getUser() + " " + scr.getNum();
                        }
                    }
                    else if (scr.getName() == "Memory")
                    {
                        if (scr.getNum() > memoryTk)
                        {
                            memoryTk = scr.getNum();
                            memoryOut = st.getUser() + " " + scr.getNum();
                        }
                    }
                    else if (scr.getName() == "Test")
                    {
                        if (scr.getNum() > testTk)
                        {
                            testTk = scr.getNum();
                            testOut = st.getUser() + " " + scr.getNum();
                        }
                    }
                }
            }
            matchScr.Text = matchOut;
            memoryScr.Text = memoryOut;
            testScr.Text = testOut;
        }
        private void match_Click(Button sender, EventArgs e)
        {

            if (audioOn == true)
                CrossTextToSpeech.Current.Speak("match");
            Navigation.PushAsync(new Match(studentList,set,audioOn));
        }
        private void memory_Click(Button sender, EventArgs e)
        {

            if (audioOn == true)
                CrossTextToSpeech.Current.Speak("memory");
            Navigation.PushAsync(new Memory(studentList,set,audioOn));
        }
        private void test_Click(Button sender, EventArgs e)
        {

            if (audioOn == true)
                CrossTextToSpeech.Current.Speak("test");
            Navigation.PushAsync(new Test(studentList,set,audioOn));
        }
    }
}