using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorKrzyzowek
{
    public class TrackClueList
    {
        public List<int> Index;
        public List<bool> Check;

        
        public int GetPasswordLenght(string str)
        {
            int x = 3;
            if (str == "Random")
            {
                Random rand = new Random();
                x = rand.Next(3, 16);
            }
            else
            {
                int.TryParse(str, out x);
            }
            return x;
        }

        public int NextIndex(int index, int listsize)
        {
            index = (index + 1) % listsize;
            return index;
        }

    }
}


