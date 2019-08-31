using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Study
{
    public class FlashCard
    {
        public FlashCard()
        {
            //default card
            topic = "Topic";
            points = new List<string>();
            points.Add("Point1");
            points.Add("Point2");
        }
        public FlashCard(String theTopic, List<String> thePoints)
        {
            //object input
            topic = theTopic;
            points = thePoints;
        }
        public FlashCard(FlashCard Object)
        {
            topic = Object.topic;
            points = Object.points;
        }

        private String topic;
        private List<String> points;

        public void Shuffle()
        {
            Random random = new Random();
            int num = points.Count;

            for (int i = num - 1; i > 1; i--)
            {
                int rnd = random.Next(i + 1);

                String value = points[rnd];
                points[rnd] = points[i];
                points[i] = value;
            }
        }
        public void rotate()
        {
            String temp = points.First();
            points.Remove(temp);
            points.Add(temp);
        }

        public void setTopic(String theTopic)
        {
            topic = theTopic;
        }
        public void setPoints(List<String> thePoints)
        {
            points = thePoints;
        }
        public String getTopic()
        {
            return topic;
        }
        public List<String> getPoints()
        {
            return points;
        }

        public String toString()
        {
            String output = topic;
            int num = 0;
            foreach (String bullet in points)
            {
                num++;
                output += "\n" + num + ") " + bullet;
            }
            return output;
        }
        public bool equals(FlashCard Object)
        {
            return (Object.topic.Equals(topic) && (Object.points.Equals(points)));
        }
    }
}
