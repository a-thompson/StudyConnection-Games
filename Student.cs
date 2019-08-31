using System;
using System.Collections.Generic;
using System.Text;

namespace Study
{
    public class Student
    {
        public Student()
        {
            grades = new List<Score>();
            grades.Add(new Score());
        }
        public Student(String theUser, List<Score> theGrades)
        {
            user = theUser;
            grades = theGrades;
        }
        public Student(Student Object)
        {
            user = Object.user;
            grades = Object.grades;
        }

        String user;
        List<Score> grades;
        public void addScore(Score score)
        {
            //grades.Add(score);
            Score temp;
            for (int i=0; i<grades.Count; i++)
            {
                if (score.getName() == grades[i].getName())
                {
                    if (score.getNum() > grades[i].getNum())
                    {
                        temp = new Score(grades[i]);
                        grades[i] = score;
                        score = temp;
                    }
                }
            }
            grades.Add(score);
            //grades.Sort(); //alphabatize list
        }
        public void setUser(String theUser)
        {
            user = theUser;
        }
        public void setGrades(List<Score> theGrades)
        {
            grades = theGrades;
        }
        public String getUser()
        {
            return user;
        }
        public List<Score> getGrades()
        {
            return grades;
        }

        public String toString()
        {
            String output = "";
            foreach (Score scr in grades)
            {
                output += "\n" + scr.toString();
            }
            return (user + output);
        }
        public bool equals(Student Object)
        {
            return (user.Equals(Object.user) && (grades.Equals(Object.grades)));
        }
    }
}
