using System;
using System.Collections.Generic;
using System.Text;

namespace Study
{
    public class Score
    {
        public Score()
        {
            name = "Match";
            num = 0;
        }
        public Score(String theName, double theNum)
        {
            name = theName;
            num = theNum;
        }
        public Score(Score Object)
        {
            name = Object.name;
            num = Object.num;
        }
        String name;
        double num;

        public void setName(String theName)
        {
            name = theName;
        }
        public void setNum(int theNum)
        {
            num = theNum;
        }
        public String getName()
        {
            return name;
        }
        public double getNum()
        {
            return num;
        }

        public String toString()
        {
            return (name + " " + num);
        }
        public bool equals(Score Object)
        {
            return (name.Equals(Object.name) && (num == Object.num));
        }
    }
}
