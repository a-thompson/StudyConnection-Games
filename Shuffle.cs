using System;
using System.Collections.Generic;
using System.Text;

namespace Study
{
    
    public class Shuffle<T>
    {
        public Shuffle()
        {
            list = new List<T>();
            init();
        }
        public Shuffle(List<T> theList)
        {
            list = new List<T>();
            foreach (T t in theList)
            {
                list.Add(t);
            }
            //list = theList;
            init();
        }
        public Shuffle(Shuffle<T> Object)
        {
            list = Object.list;
            init();
        }

        List<T> list;

        private void init()
        {
            Random random = new Random();
            int num = list.Count;

            for (int i = num-1; i >= 0; i--)
            {
                int rnd = random.Next(i+1);

                T value = list[rnd];
                list[rnd] = list[i];
                list[i] = value;
            }
        }
        

        public void setList(List<T> theList)
        {
            list = theList;
        }
        public List<T> getList()
        {
            return list;
        }

        public bool equals(Shuffle<T> Object)
        {
            return (list.Equals(Object.list));
        }
    }
}
