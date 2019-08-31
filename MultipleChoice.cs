using System;
using System.Collections.Generic;
using System.Text;

namespace Study
{
    public class MultipleChoice
    {
        public MultipleChoice()
        {
            //default
            question = "1 + 1 =";
            correct = "2";
            ansA = "3";
            ansB = "10";
            ansC = "1";
            List<String> points = new List<string>();
            points.Add(correct);
            card = new FlashCard(question, points);
        }
        public MultipleChoice(String theQuestion, String theA, String theB, String theC, String theCorrect, FlashCard theCard)
        {
            question = theQuestion;
            ansA = theA;
            ansB = theB;
            ansC = theC;
            correct = theCorrect;
            card = theCard;
        }
        public MultipleChoice(MultipleChoice Object)
        {
            question = Object.question;
            ansA = Object.ansA;
            ansB = Object.ansB;
            ansC = Object.ansC;
            correct = Object.correct;
            card = Object.card;
        }

        String question;
        String correct;
        String ansA;
        String ansB;
        String ansC;
        FlashCard card;
        public void getQuestion(String theQuestion)
        {
            question = theQuestion;
        }
        public void setCorrect(String theCorrect)
        {
            correct = theCorrect;
        }
        public void setA(String theA)
        {
            ansA = theA;
        }
        public void getB(String theB)
        {
            ansB = theB;
        }
        public void setC(String theC)
        {
            ansC = theC;
        }
        public void setCard(FlashCard theCard)
        {
            card = theCard;
        }
        
        public String getQuestion()
        {
            return question;
        }
        public String getCorrect()
        {
            return correct;
        }
        public String getA()
        {
            return ansA;
        }
        public String getB()
        {
            return ansB;
        }
        public String getC()
        {
            return ansC;
        }
        public FlashCard getCard()
        {
            return card;
        }
        

        public String toString()
        {
            return (question + "\nA) " + ansA + "\nB) " + ansB + "\nC) " + ansC + "\nD) " + correct + "\nCorrect: " + correct);
        }
        public bool equals(MultipleChoice Object)
        {
            return ((Object.question.Equals(question)) && (Object.correct.Equals(correct)) && (Object.ansA.Equals(ansA)) && (Object.ansB.Equals(ansB)) && (Object.ansC.Equals(ansC)));
        }
    }
}
