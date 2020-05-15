using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace GeneratorKrzyzowek
{
    /// <summary>
    /// class where crossword with across and down passwords is generated
    /// </summary>
    public class AcroosAndDownCrossword
    {
        /// <summary>
        /// list with all passwords
        /// </summary>
        public List<string> PasswordList {
            get { return passwordlist;}
            set{ }
        }

        /// <summary>
        /// list of list with starts co-ordinates of password
        /// </summary>
        public List <List<int>> ListStartXY {
            get{return liststartxy;}
            set { }
        }
        /// <summary>
        /// list of list with ends co-ordinates of password
        /// </summary>
        public List<List<int>> ListEndXY
        {
            get{return listendxy;}
            set { }
        }
        /// <summary>
        /// array where crossword is stored
        /// </summary>
        public string[,] Board { get; set; }

        List<string> passwordlist = new List<String>();
        List<List<int>> liststartxy = new List<List<int>>();
        List<List<int>> listendxy = new List<List<int>>();

        /// <summary>
        /// this function check if this field of board is good to place password across
        /// </summary>
        /// <param name="i">first index</param>
        /// <param name="j">second index</param>
        /// <param name="board">array where crossword is stored</param>
        /// <returns>0-this field is not good; 1-this field is good</returns>
        int IsAcrossOk(int i, int j, string[,] board)
        {
            if (j > 0)
            {
                if (board[i, j - 1] != "_")
                {
                    return 0;
                }
            }
            if (i > 0)
            {
                if (board[i - 1, j] != "_")
                {
                    return 0;
                }
            }
            if (i <= board.GetLength(0) - 2)
            {
                if (board[i + 1, j] != "_")
                {
                    return 0;
                }
            }
            return 1;
        }

        /// <summary>
        /// this function check if this field of board is good to place password down
        /// </summary>
        /// <param name="i">first index</param>
        /// <param name="j">second index</param>
        /// <param name="board">array where crossword is stored</param>
        /// <returns> -1-this field is not good; 0-this field is can't be last; 1-this field is good</returns>
        int IsDownOk(int i, int j, string[,] board)
        {
            if (board[i, j] == "_")
            {
                if (j > 0)
                {
                    if (board[i, j - 1] != "_")
                    {
                        return -1;
                    }
                }
                if (j < board.GetLength(1) - 2)
                {
                    if (board[i, j + 1] != "_")
                    {
                        return -1;
                    }
                }
            }
            if (i < board.GetLength(0) - 1)
            {
                if (board[i + 1, j] != "_")
                {
                    return 0;
                }
            }
            return 1;
        }

        /// <summary>
        /// this function check if this field can be first letter of down password 
        /// </summary>
        /// <param name="i">first index</param>
        /// <param name="j">second index</param>
        /// <param name="board">array where crossword is stored</param>
        /// <returns> true if this field can be first letter else return false</returns>
        bool FirstDown(int i, int j, string[,] board)
        {
            if (i > 0)
            {
                if (board[i - 1, j] == "_")
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// this function generate crossword with across and down passwords
        /// </summary>
        /// <param name="size">size of board</param>
        /// <returns>board</returns>
        public string[,] GetAcroossAndDownCrossword(int size)
        {            
            var lenlist = new List<int>();
            string[,] board = new string[size, size];
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = "_";
                }
            }
            int a1 = 0, a2 = 0, b1 = 0, b2 = 0;
            passwordlist.Add(" ");
            var startxy2 = new List<int>();
            startxy2.Add(0);
            startxy2.Add(0);
            liststartxy.Add(startxy2);
            listendxy.Add(startxy2);
            while (true)
            {
                int len = 0, boardlen = board.GetLength(0);

                for (int i = 0; i < boardlen - a2; i++)
                {
                    len += IsAcrossOk(a1, a2 + i, board);
                    if (IsAcrossOk(a1, a2 + i, board) == 0)
                    {
                        break;
                    }
                }
                if (len < 3)
                {
                    if (a2 > board.GetLength(0) - 1)
                    {
                        a2 = -1;
                        a1 = a1 + 1;
                    }
                    a2 = a2 + 1;
                    if (a1 > board.GetLength(1) - 1)
                    {
                        break;
                    }
                    len = 0;
                    continue;
                }
                string slowo = "";
                Random rand = new Random();
                var s = rand.Next(3, len + 1);
                for (int i = 0; i < s; i++)
                {
                    slowo = slowo + board[a1, a2 + i];
                }
                var haslo = GetPassword(s, slowo);
                if (haslo == "")
                {
                    continue;
                }
                else
                {
                    passwordlist.Add(haslo);
                    var startxy = new List<int>
                    {
                        a1,
                        a2
                    };
                    liststartxy.Add(startxy);
                    var endxy = new List<int>
                    {
                        a1,
                        a2 + s - 1
                    };
                    listendxy.Add(endxy);
                }
                for (int i = 0; i < haslo.Length; i++)
                {
                    board[a1, a2 + i] = haslo[i].ToString();
                }
                a2 += 1 + haslo.Length;
                if (a2 >= board.GetLength(0))
                {
                    a2 = 0;
                    a1 = a1 + 1;
                }
                if (a1 >= board.GetLength(1))
                {
                    break;
                }
            }
            passwordlist.Add("  ");
            liststartxy.Add(startxy2);
            listendxy.Add(startxy2);
            int count = 0;
            while (true)
            {
                int boardlen = board.GetLength(1);
                lenlist.Clear();
                if(!FirstDown(b1, b2, board))
                {
                    if (b1 > board.GetLength(0) - 1)
                    {
                        b1 = -1;
                        b2 = b2 + 1;
                    }
                    b1 += 1;
                    if (b2 > board.GetLength(1) - 1)
                    {
                        break;
                    }
                    continue;
                }
                for (int i = 0; i < boardlen - b1; i++)
                {
                    if (IsDownOk(b1 + i, b2, board) == 1)
                    {
                        if (i >= 2 && i <= 14)
                        {
                            lenlist.Add(i + 1);
                        }
                    }
                    if (IsDownOk(b1 + i, b2, board) == -1)
                    {
                        break;
                    }
                }
                if (lenlist.Count == 0)
                {
                    if (b1 > board.GetLength(0) - 1)
                    {
                        b1 = -1;
                        b2 = b2 + 1;
                    }
                    b1 = b1 + 1;
                    if (b2 > board.GetLength(1) - 1)
                    {
                        break;
                    }
                    continue;
                }
                string slowo = "";
                Random rand = new Random();
                var s = rand.Next(0, lenlist.Count);
                for (int i = 0; i < lenlist[s]; i++)
                {
                    slowo = slowo + board[b1 + i, b2];
                }
                var haslo = GetPassword(lenlist[s], slowo);
                if (haslo == "")
                {
                    s = (s + 1) % lenlist.Count;
                    count += 1;
                    if (count == lenlist.Count)
                    {
                        count = 0;
                        b1 += 1;
                        if (b1 >= board.GetLength(0))
                        {
                            b1 = 0;
                            b2 = b2 + 1;
                        }
                        if (b2 >= board.GetLength(1))
                        {
                            break;
                        }
                    }
                    continue;
                }
                else
                {
                    passwordlist.Add(haslo);
                    var startxy = new List<int>
                    {
                        b1,
                        b2
                    };
                    liststartxy.Add(startxy);
                    var endxy = new List<int>
                    {
                        b1 + lenlist[s] - 1,
                        b2
                    };
                    listendxy.Add(endxy);
                    count = 0;
                }
                for (int i = 0; i < haslo.Length; i++)
                {
                    board[b1 + i, b2] = haslo[i].ToString();
                }
                b1 += haslo.Length + 1;
                if (b1 >= board.GetLength(0))
                {
                    b1 = 0;
                    b2 = b2 + 1;
                }
                if (b2 >= board.GetLength(1))
                {
                    break;
                }
            }
            return board;
        }

        /// <summary>
        /// this function get password that can be placed on board
        /// </summary>
        /// <param name="x">lenght</param>
        /// <param name="slowo">letters which password must contain</param>
        /// <returns>one password which met requirements if no password is found then return empty string</returns>
        string GetPassword(int x, string slowo)
        {
            if (x == 3)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var a = (from st in context.Hasla3Distincts
                         orderby st.ID
                         where DbFunctions.Like(st.haslo, slowo)
                         select st);
                if (a.Count() == 0)
                {
                    return "";
                }
                int toSkip = rand.Next(0, a.Count());
                var b = a.Skip(toSkip).Take(1).First();
                return b.haslo;
            }
            if (x == 4)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var a = (from st in context.Hasla4Distincts
                         orderby st.ID
                         where DbFunctions.Like(st.haslo, slowo)
                         select st);
                if (a.Count() == 0)
                {
                    return "";
                }
                int toSkip = rand.Next(0, a.Count());
                var b = a.Skip(toSkip).Take(1).First();
                return b.haslo;
            }
            if (x == 5)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var a = (from st in context.Hasla5Distincts
                         orderby st.ID
                         where DbFunctions.Like(st.haslo, slowo)
                         select st);
                if (a.Count() == 0)
                {
                    return "";
                }
                int toSkip = rand.Next(0, a.Count());
                var b = a.Skip(toSkip).Take(1).First();
                return b.haslo;
            }
            if (x == 6)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var a = (from st in context.Hasla6Distincts
                         orderby st.ID
                         where DbFunctions.Like(st.haslo, slowo)
                         select st);
                if (a.Count() == 0)
                {
                    return "";
                }
                int toSkip = rand.Next(0, a.Count());
                var b = a.Skip(toSkip).Take(1).First();
                return b.haslo;
            }
            if (x == 7)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var a = (from st in context.Hasla7Distincts
                         orderby st.ID
                         where DbFunctions.Like(st.haslo, slowo)
                         select st);
                if (a.Count() == 0)
                {
                    return "";
                }
                int toSkip = rand.Next(0, a.Count());
                var b = a.Skip(toSkip).Take(1).First();
                return b.haslo;
            }
            if (x == 8)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var a = (from st in context.Hasla8Distincts
                         orderby st.ID
                         where DbFunctions.Like(st.haslo, slowo)
                         select st);
                if (a.Count() == 0)
                {
                    return "";
                }
                int toSkip = rand.Next(0, a.Count());
                var b = a.Skip(toSkip).Take(1).First();
                return b.haslo;
            }
            if (x == 9)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var a = (from st in context.Hasla9Distincts
                         orderby st.ID
                         where DbFunctions.Like(st.haslo, slowo)
                         select st);
                if (a.Count() == 0)
                {
                    return "";
                }
                int toSkip = rand.Next(0, a.Count());
                var b = a.Skip(toSkip).Take(1).First();
                return b.haslo;
            }
            if (x == 10)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var a = (from st in context.Hasla10Distincts
                         orderby st.ID
                         where DbFunctions.Like(st.haslo, slowo)
                         select st);
                if (a.Count() == 0)
                {
                    return "";
                }
                int toSkip = rand.Next(0, a.Count());
                var b = a.Skip(toSkip).Take(1).First();
                return b.haslo;
            }
            if (x == 11)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var a = (from st in context.Hasla11Distincts
                         orderby st.ID
                         where DbFunctions.Like(st.haslo, slowo)
                         select st);
                if (a.Count() == 0)
                {
                    return "";
                }
                int toSkip = rand.Next(0, a.Count());
                var b = a.Skip(toSkip).Take(1).First();
                return b.haslo;
            }
            if (x == 12)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var a = (from st in context.Hasla12Distincts
                         orderby st.ID
                         where DbFunctions.Like(st.haslo, slowo)
                         select st);
                if (a.Count() == 0)
                {
                    return "";
                }
                int toSkip = rand.Next(0, a.Count());
                var b = a.Skip(toSkip).Take(1).First();
                return b.haslo;
            }
            if (x == 13)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var a = (from st in context.Hasla13Distincts
                         orderby st.ID
                         where DbFunctions.Like(st.haslo, slowo)
                         select st);
                if (a.Count() == 0)
                {
                    return "";
                }
                int toSkip = rand.Next(0, a.Count());
                var b = a.Skip(toSkip).Take(1).First();
                return b.haslo;
            }
            if (x == 14)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var a = (from st in context.Hasla14Distincts
                         orderby st.ID
                         where DbFunctions.Like(st.haslo, slowo)
                         select st);
                if (a.Count() == 0)
                {
                    return "";
                }
                int toSkip = rand.Next(0, a.Count());
                var b = a.Skip(toSkip).Take(1).First();
                return b.haslo;
            }
            if (x == 15)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var a = (from st in context.Hasla15Distincts
                         orderby st.ID
                         where DbFunctions.Like(st.haslo, slowo)
                         select st);
                if (a.Count() == 0)
                {
                    return "";
                }
                int toSkip = rand.Next(0, a.Count());
                var b = a.Skip(toSkip).Take(1).First();
                return b.haslo;
            }
            else
            {
                return "";
            }
        }
    }
}