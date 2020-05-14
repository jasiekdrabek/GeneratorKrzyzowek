using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorKrzyzowek
{
    /// <summary>
    /// class to track clue list 
    /// </summary>
    public class TrackClueList
    {
        /// <summary>
        /// list of index which clue should be display
        /// </summary>
        public List<int> Index;
        /// <summary>
        /// list of bool if password is written true else false
        /// </summary>
        public List<bool> Check;

        /// <summary>
        /// convert string to int if string is not number return random from 3 to 15
        /// </summary>
        /// <param name="str">string number</param>
        /// <returns> password lenght</returns>
        public int GetPasswordLenght(string str)
        {
            var resoult = int.TryParse(str, out int x);
            if (resoult)
            {
                return x;
            }
            else
            {
                Random rand = new Random();
                x = rand.Next(3, 16);
                return x;
            }

        }

        /// <summary>
        /// return next index
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="listsize">list size</param>
        /// <returns>index + 1 if index is last in list return 0</returns>
        public int NextIndex(int index, int listsize)
        {
            index = (index + 1) % listsize;
            return index;
        }
    }
}