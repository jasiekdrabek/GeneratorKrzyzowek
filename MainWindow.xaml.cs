using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeneratorKrzyzowek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// list of lists texbox where crossword password will be written
        public List<List<TextBox>> TBlist = new List<List<TextBox>>();
        /// list with all passwords
        public List<String> passwordList = new List<string>();
        /// list with labels each label will be before first letter of password
        public List<Label> labels = new List<Label>();
        /// main password (has gray background)
        public string password = "";
        /// new window where crossword will be created
        public Window1 Window1 { get; set; }
        /// this property is used to store information about crossword with across and down passwords
        public AcroosAndDownCrossword Crossword{get;set;}
 
        ///constructor 
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// this function generate crossword with across passwords and one down password on gray background.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Generate_Simple_Crossword(object sender, RoutedEventArgs e)
        {
            TBlist.Clear();
            passwordList.Clear();
            labels.Clear();
            var t = new TrackClueList();
            int x = t.GetPasswordLenght(CBI.Text);
            password = ChoosePassword(x);
            Window1 = null;
            int k = 1;

            for (int i = 0; i < x; i++)
            {
                var rand = new Random();
                var passwordlenght = rand.Next();
                passwordlenght = 3 + (passwordlenght % 12);
                passwordList.Add(ChoosePasswordContains(passwordlenght, password[i].ToString()));
                var letterind = passwordList[i].IndexOf(password[i]);
                List<TextBox> textBoxes = new List<TextBox>();
                for (int j = 0; j < passwordlenght; j++)
                {
                    var textbox = new TextBox
                    {
                        IsReadOnly = true,
                        Height = 20,
                        Width = 20,
                        Margin = new Thickness(-270 + 40 * (j - letterind), -300 + 40 * i, 0, 0)
                    };
                    if (letterind == j)
                    {
                        textbox.Background = Brushes.Gray;
                    }
                    textBoxes.Add(textbox);
                }
                Label label = new Label
                {
                    Content = k.ToString(),
                    Margin = new Thickness(-300 - 40 * letterind, -295 + 40 * (k - 1), 0, 0),
                    Foreground = Brushes.White,
                    Height = 30,
                    Width = 30
                };
                labels.Add(label);
                k = k + 1;
                TBlist.Add(textBoxes);
            }
            Window1 = new Window1(passwordList, TBlist)
            {
                Password = password
            };
            foreach (List<TextBox> sublist in TBlist)
            {
                foreach (TextBox j in sublist)
                {
                    Window1.grid.Children.Add(j);
                }
            }
            foreach (Label label in labels)
            {
                Window1.grid.Children.Add(label);
            }
            Window1.Show();
        }
        /// <summary>
        /// this function generate crossword with across and down passwords
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Generate_Advanced_Crossword(object sender, RoutedEventArgs e)
        {
            TBlist.Clear();
            passwordList.Clear();
            labels.Clear();
            Window1 = null;
            Crossword = null;
            var t = new TrackClueList();
            int x = t.GetPasswordLenght(CBII.Text);
            Crossword = new AcroosAndDownCrossword
            {
                PasswordList = new List<string>(),
                Board = new string[x, x]
            };
            Crossword.Board =Crossword.GetAcroossAndDownCrossword(x);
            for (int i = 0; i < x; i++)
            {
                List<TextBox> textBoxes = new List<TextBox>();
                for (int j = 0; j < x; j++)
                {
                    var textbox = new TextBox
                    {
                        IsReadOnly = true, 
                        Height = 20,
                        Width = 20,
                        Margin = new Thickness(-400 + 40 * j , -400 + 40 * i, 0, 0)
                    };
                    if (Crossword.Board[i,j] == "_")
                    {
                        textbox.Background = Brushes.Gray;
                    }
                    textBoxes.Add(textbox);
                }   
                TBlist.Add(textBoxes);
            }
            Window1 = new Window1(Crossword.PasswordList, TBlist, Crossword.ListStartXY, Crossword.ListEndXY)
            {
                Board = Crossword.Board
            };
            foreach (List<TextBox> sublist in TBlist)
            {
                foreach (TextBox j in sublist)
                {
                    Window1.grid.Children.Add(j);
                }
            }
            Window1.Show();
        }
        /// <summary>
        /// this function load crossword with across passwords and one down password on gray background.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Load_Simple_Crossword(object sender, RoutedEventArgs e)
        {
            TBlist.Clear();
            passwordList.Clear();
            labels.Clear();
            Window1 = null;
            string[] lines;
            var t = new TrackClueList
            {
                Check = new List<bool>(),
                Index = new List<int>()
            };
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Krzyżówka(*.crossword)|*.crossword"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                lines = File.ReadAllLines(openFileDialog.FileName);
                password = lines[0];
                for (int i = 1; i < lines.Length; i+=2)
                {
                    passwordList.Add(lines[i]);
                    if (lines[i + 1] == "True")
                    {
                        t.Check.Add(true);
                    }
                    else
                    {
                        t.Check.Add(false);
                    }
                    t.Index.Add(0);
                }
                int k = 1;
                for (int i = 0; i < passwordList.Count(); i++)
                {
                    int letterind = passwordList[i].IndexOf(password[i]);
                    List<TextBox> textBoxes = new List<TextBox>();
                    for (int j = 0; j < passwordList[i].Length; j++)
                    {
                        var textbox = new TextBox
                        {
                            IsReadOnly = true,
                            Height = 20,
                            Width = 20,
                            Margin = new Thickness(-270 + 40 * (j - letterind), -300 + 40 * i, 0, 0)
                        };
                        if (letterind == j)
                        {
                            textbox.Background = Brushes.Gray;
                        }
                        if (t.Check[i] == true)
                        {
                            textbox.Text = passwordList[i][j].ToString();
                        }
                        textBoxes.Add(textbox);
                    }
                    Label label = new Label
                    {
                        Content = k.ToString(),
                        Margin = new Thickness(-300 - 40 * letterind, -295 + 40 * (k - 1), 0, 0),
                        Foreground = Brushes.White,
                        Height = 30,
                        Width = 30
                    };
                    labels.Add(label);
                    k = k + 1;
                    TBlist.Add(textBoxes);
                }
                Window1 = new Window1(passwordList, TBlist,null,null,t)
                {
                    Password = password
                };
                foreach (List<TextBox> sublist in TBlist)
                {
                    foreach (TextBox j in sublist)
                    {
                        Window1.grid.Children.Add(j);
                    }
                }
                foreach (Label label in labels)
                {
                    Window1.grid.Children.Add(label);
                }
                Window1.Show();
            }
        }

        /// <summary>
        /// this function load crossword with across and down passwords
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Load_Advanced_Crossword(object sender, RoutedEventArgs e)
        {
            TBlist.Clear();
            passwordList.Clear();
            labels.Clear();
            Window1 = null;
            Crossword = null;
            var t = new TrackClueList
            {
                Check = new List<bool>(),
                Index = new List<int>()
            };
            string[] lines;
            Crossword = new AcroosAndDownCrossword
            {
                PasswordList = new List<string>()
            };
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Krzyżówka(*.crosswordadv)|*.crosswordadv"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                lines = File.ReadAllLines(openFileDialog.FileName);
                int x = int.Parse(lines[0]);
                Crossword.Board = new string[x,x];
                for (int k = 1; k < lines.Length; k++)
                {
                    if (k == 1)
                    {
                        for (int i = 0; i < Crossword.Board.GetLength(0); i++)
                        {
                            for (int j = 0; j < Crossword.Board.GetLength(1); j++)
                            {
                                Crossword.Board[i, j] = lines[k];
                                k = k + 1;
                            }
                        }
                    }
                    if(lines[k][0]=='a')
                    {
                        if (lines[k][1] != 'a')
                        {
                            var endxy = new List<int>();
                            int l = lines[k][1] - 48;
                            int m = lines[k][2] - 48;
                            endxy.Add(l);
                            endxy.Add(m);
                            Crossword.ListEndXY.Add(endxy);
                        }
                        if(lines[k][1] == 'a')
                        {
                            var startxy = new List<int>();
                            int l = lines[k][2] - 48;
                            int m = lines[k][3] - 48;
                            startxy.Add(l);
                            startxy.Add(m);
                            Crossword.ListStartXY.Add(startxy);
                        }
                    }
                    else
                    {
                        Crossword.PasswordList.Add(lines[k]);
                        if (lines[k + 1] == "True")
                        {
                            t.Check.Add(true);
                        }
                        else
                        {
                            t.Check.Add(false);
                        }
                        t.Index.Add(0);
                        k += 1;
                    }
                }
                for (int i = 0; i < x; i++)
                {
                    List<TextBox> textBoxes = new List<TextBox>();
                    for (int j = 0; j < x; j++)
                    {
                        var textbox = new TextBox
                        {
                            IsReadOnly = true, 
                            Height = 20,
                            Width = 20,
                            Margin = new Thickness(-270 + 40 * j, -300 + 40 * i, 0, 0)
                        };
                        if (Crossword.Board[i, j] == "_")
                        {
                            textbox.Background = Brushes.Gray;
                        }
                        textBoxes.Add(textbox);
                    }
                    TBlist.Add(textBoxes);
                }
                Window1 = new Window1(Crossword.PasswordList, TBlist, Crossword.ListStartXY, Crossword.ListEndXY,t)
                {
                    Board = Crossword.Board
                };
                foreach (List<TextBox> sublist in TBlist)
                {
                    foreach (TextBox j in sublist)
                    {
                        Window1.grid.Children.Add(j);
                    }
                }
                Window1.Show();
            }
        }
        /// <summary>
        /// this function try to add new password,clue to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Add_Password(object sender, RoutedEventArgs e)
        {
            string str=TBPassword.Text;
            str = Regex.Replace(str, "[^a-zA-Z]", "");
            str = str.ToUpper();
            int len = str.Length;
            if (len >= 3 && len <= 15)
            {
                if (!If_TB_Is_Empty(str))
                {
                    AddPasswordToDatabase(str, len);
                }
            }
            if(len < 3) 
            {
                MessageBox.Show("Password is too short");
            }
            if (len > 15)
            {
                MessageBox.Show("Password is too long");
            }
        }
        /// <summary>
        /// this function try to delete password,clue from database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Delete_Password(object sender, RoutedEventArgs e)
        {
            string str = TBPassword.Text;
            str = Regex.Replace(str, "[^a-zA-Z]", "");
            str = str.ToUpper();
            int len = str.Length;
            if (len >= 3 && len <= 15)
            {
                if (!If_TB_Is_Empty(str))
                {
                    DeletePasswordFromdatabase(str, len);
                }
            }
            if(len < 3)
            {
                MessageBox.Show("Password is too short");
            }
            if(len > 15)
            {
                MessageBox.Show("Pasword is too long");
            }
        }
        /// <summary>
        /// this function check if something is written in textbox
        /// </summary>
        /// <param name="TBPassword">text in textbox whit only letters</param>
        /// <returns> true if text empty else returns false</returns>
        bool If_TB_Is_Empty(string TBPassword) {
            if (TBPassword == "")
            {
                MessageBox.Show("Password can't be empty");
                return true;
            }
            if (TBClue.Text == "")
            {
                MessageBox.Show("Clue can't be empty");
                return true;
            }
            return false;
        }

        /// <summary>
        /// this function search for password with clue that someone write and resoults are display into textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Search_For_Password(object sender, RoutedEventArgs e)
        {
            TBFound.Text = "";
            string clue = TBSearch.Text;
            using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
            {
                var a3 = (from st in context.Hasla3s
                          where DbFunctions.Like(st.podpowiedz,clue)
                          select st.haslo);
                foreach (var haslo in a3)
                {
                    TBFound.Text = TBFound.Text + haslo + Environment.NewLine;
                }
                var a4 = (from st in context.Hasla4s
                          where DbFunctions.Like(st.podpowiedz, clue)
                          select st.haslo);
                foreach (var haslo in a4)
                {
                    TBFound.Text = TBFound.Text + haslo + Environment.NewLine;
                }
                var a5 = (from st in context.Hasla5s
                          where DbFunctions.Like(st.podpowiedz, clue)
                          select st.haslo);
                foreach (var haslo in a5)
                {
                    TBFound.Text = TBFound.Text + haslo + Environment.NewLine;
                }
                var a6 = (from st in context.Hasla6s
                          where DbFunctions.Like(st.podpowiedz, clue)
                          select st.haslo);
                foreach (var haslo in a6)
                {
                    TBFound.Text = TBFound.Text + haslo + Environment.NewLine;
                }
                var a7 = (from st in context.Hasla7s
                          where DbFunctions.Like(st.podpowiedz, clue)
                          select st.haslo);
                foreach (var haslo in a7)
                {
                    TBFound.Text = TBFound.Text + haslo + Environment.NewLine;
                }
                var a8 = (from st in context.Hasla8s
                          where DbFunctions.Like(st.podpowiedz, clue)
                          select st.haslo);
                foreach (var haslo in a8)
                {
                    TBFound.Text = TBFound.Text + haslo + Environment.NewLine;
                }
                var a9 = (from st in context.Hasla9s
                          where DbFunctions.Like(st.podpowiedz, clue)
                          select st.haslo);
                foreach (var haslo in a9)
                {
                    TBFound.Text = TBFound.Text + haslo + Environment.NewLine;
                }
                var a10 = (from st in context.Hasla10s
                           where DbFunctions.Like(st.podpowiedz, clue)
                           select st.haslo);
                foreach (var haslo in a10)
                {
                    TBFound.Text = TBFound.Text + haslo + Environment.NewLine;
                }
                var a11 = (from st in context.Hasla11s
                           where DbFunctions.Like(st.podpowiedz, clue)
                           select st.haslo);
                foreach (var haslo in a11)
                {
                    TBFound.Text = TBFound.Text + haslo + Environment.NewLine;
                }
                var a12 = (from st in context.Hasla12s
                           where DbFunctions.Like(st.podpowiedz, clue)
                           select st.haslo);
                foreach (var haslo in a12)
                {
                    TBFound.Text = TBFound.Text + haslo + Environment.NewLine;
                }
                var a13 = (from st in context.Hasla13s
                           where DbFunctions.Like(st.podpowiedz, clue)
                           select st.haslo);
                foreach (var haslo in a13)
                {
                    TBFound.Text = TBFound.Text + haslo + Environment.NewLine;
                }
                var a14 = (from st in context.Hasla14s
                           where DbFunctions.Like(st.podpowiedz, clue)
                           select st.haslo);
                foreach (var haslo in a14)
                {
                    TBFound.Text = TBFound.Text + haslo + Environment.NewLine;
                }
                var a15 = (from st in context.Hasla15s
                           where DbFunctions.Like(st.podpowiedz, clue)
                           select st.haslo);
                foreach (var haslo in a15)
                {
                    TBFound.Text = TBFound.Text + haslo + Environment.NewLine;
                }
            }
        }

        /// <summary>
        /// choose password from database
        /// </summary>
        /// <param name="x"> lenght of password</param>
        /// <returns> one password with lenght x</returns>
        private string ChoosePassword(int x)
        {
            String vs = "";
            if (x == 3)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    int toSkip = rand.Next(1, context.Hasla3Distincts.Count());
                    KrzyzowkiTabele.Hasla3Distinct a = context.Hasla3Distincts.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                    vs = a.haslo;
                    return vs;
                }
            }
            if (x == 4)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    int toSkip = rand.Next(1, context.Hasla4Distincts.Count());
                    KrzyzowkiTabele.Hasla4Distinct a = context.Hasla4Distincts.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                    vs = a.haslo;
                    return vs;
                }
            }
            if (x == 5)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    int toSkip = rand.Next(1, context.Hasla5Distincts.Count());
                    KrzyzowkiTabele.Hasla5Distinct a = context.Hasla5Distincts.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                    vs = a.haslo;
                    return vs;
                }
            }
            if (x == 6)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    int toSkip = rand.Next(1, context.Hasla6Distincts.Count());
                    KrzyzowkiTabele.Hasla6Distinct a = context.Hasla6Distincts.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                    vs = a.haslo;
                    return vs;
                }
            }
            if (x == 7)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    int toSkip = rand.Next(1, context.Hasla7Distincts.Count());
                    KrzyzowkiTabele.Hasla7Distinct a = context.Hasla7Distincts.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                    vs = a.haslo;
                    return vs;
                }
            }
            if (x == 8)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    int toSkip = rand.Next(1, context.Hasla8Distincts.Count());
                    KrzyzowkiTabele.Hasla8Distinct a = context.Hasla8Distincts.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                    vs = a.haslo;
                    return vs;
                }
            }
            if (x == 9)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    int toSkip = rand.Next(1, context.Hasla9Distincts.Count());
                    KrzyzowkiTabele.Hasla9Distinct a = context.Hasla9Distincts.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                    vs = a.haslo;
                    return vs;
                }
            }
            if (x == 10)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    int toSkip = rand.Next(1, context.Hasla10Distincts.Count());
                    KrzyzowkiTabele.Hasla10Distinct a = context.Hasla10Distincts.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                    vs = a.haslo;
                    return vs;
                }
            }
            if (x == 11)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    int toSkip = rand.Next(1, context.Hasla11Distincts.Count());
                    KrzyzowkiTabele.Hasla11Distinct a = context.Hasla11Distincts.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                    vs = a.haslo;
                    return vs;
                }
            }
            if (x == 12)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    int toSkip = rand.Next(1, context.Hasla12Distincts.Count());
                    KrzyzowkiTabele.Hasla12Distinct a = context.Hasla12Distincts.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                    vs = a.haslo;
                    return vs;
                }
            }
            if (x == 13)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    int toSkip = rand.Next(1, context.Hasla13Distincts.Count());
                    KrzyzowkiTabele.Hasla13Distinct a = context.Hasla13Distincts.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                    vs = a.haslo;
                    return vs;
                }
            }
            if (x == 14)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    int toSkip = rand.Next(1, context.Hasla14Distincts.Count());
                    KrzyzowkiTabele.Hasla14Distinct a = context.Hasla14Distincts.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                    vs = a.haslo;
                    return vs;
                }
            }
            if (x == 15)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    int toSkip = rand.Next(1, context.Hasla15Distincts.Count());
                    KrzyzowkiTabele.Hasla15Distinct a = context.Hasla15Distincts.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                    vs = a.haslo;
                    return vs;
                }
            }
            return vs;
        }

        /// <summary>
        /// choose password from database
        /// </summary>
        /// <param name="x">lenght of password</param>
        /// <param name="password">one letter which choosen password must contain</param>
        /// <returns>one password with lenght x and contains letter password</returns>
        private string ChoosePasswordContains(int x, string password)
        {
            string vs = "";
            if (x == 3)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla3Distincts
                             orderby st.ID
                             where st.haslo.Contains(password)
                             select st);
                    int toSkip = rand.Next(1, a.Count());
                    var b = a.Skip(toSkip).Take(1).First();
                    vs = b.haslo;
                    return vs;
                }
            }
            if (x == 4)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla4Distincts
                             orderby st.ID
                             where st.haslo.Contains(password)
                             select st);
                    int toSkip = rand.Next(1, a.Count());
                    var b = a.Skip(toSkip).Take(1).First();
                    vs = b.haslo;
                    return vs;
                }
            }
            if (x == 5)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla5Distincts
                             orderby st.ID
                             where st.haslo.Contains(password)
                             select st);
                    int toSkip = rand.Next(1, a.Count());
                    var b = a.Skip(toSkip).Take(1).First();
                    vs = b.haslo;
                    return vs;
                }
            }
            if (x == 6)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla6Distincts
                             orderby st.ID
                             where st.haslo.Contains(password)
                             select st);
                    int toSkip = rand.Next(1, a.Count());
                    var b = a.Skip(toSkip).Take(1).First();
                    vs = b.haslo;
                    return vs;
                }
            }
            if (x == 7)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla7Distincts
                             orderby st.ID
                             where st.haslo.Contains(password)
                             select st);
                    int toSkip = rand.Next(1, a.Count());
                    var b = a.Skip(toSkip).Take(1).First();
                    vs = b.haslo;
                    return vs;
                }
            }
            if (x == 8)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla8Distincts
                             orderby st.ID
                             where st.haslo.Contains(password)
                             select st);
                    int toSkip = rand.Next(1, a.Count());
                    var b = a.Skip(toSkip).Take(1).First();
                    vs = b.haslo;
                    return vs;
                }
            }
            if (x == 9)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla9Distincts
                             orderby st.ID
                             where st.haslo.Contains(password)
                             select st);
                    int toSkip = rand.Next(1, a.Count());
                    var b = a.Skip(toSkip).Take(1).First();
                    vs = b.haslo;
                    return vs;
                }
            }
            if (x == 10)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla10Distincts
                             orderby st.ID
                             where st.haslo.Contains(password)
                             select st);
                    int toSkip = rand.Next(1, a.Count());
                    var b = a.Skip(toSkip).Take(1).First();
                    vs = b.haslo;
                    return vs;
                }
            }
            if (x == 11)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla11Distincts
                             orderby st.ID
                             where st.haslo.Contains(password)
                             select st);
                    int toSkip = rand.Next(1, a.Count());
                    var b = a.Skip(toSkip).Take(1).First();
                    vs = b.haslo;
                    return vs;
                }
            }
            if (x == 12)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla12Distincts
                             orderby st.ID
                             where st.haslo.Contains(password)
                             select st);
                    int toSkip = rand.Next(1, a.Count());
                    var b = a.Skip(toSkip).Take(1).First();
                    vs = b.haslo;
                    return vs;
                }
            }
            if (x == 13)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla13Distincts
                             orderby st.ID
                             where st.haslo.Contains(password)
                             select st);
                    int toSkip = rand.Next(1, a.Count());
                    var b = a.Skip(toSkip).Take(1).First();
                    vs = b.haslo;
                    return vs;
                }
            }
            if (x == 14)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla14Distincts
                             orderby st.ID
                             where st.haslo.Contains(password)
                             select st);
                    int toSkip = rand.Next(1, a.Count());
                    var b = a.Skip(toSkip).Take(1).First();
                    vs = b.haslo;
                    return vs;
                }
            }
            if (x == 15)
            {
                Random rand = new Random();
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla15Distincts
                             orderby st.ID
                             where st.haslo.Contains(password)
                             select st);
                    int toSkip = rand.Next(1, a.Count());
                    var b = a.Skip(toSkip).Take(1).First();
                    vs = b.haslo;
                    return vs;
                }
            }
            return vs;
        }

        /// <summary>
        /// this function add password to database
        /// </summary>
        /// <param name="str">password </param>
        /// <param name="len">lenght of password</param>
        void AddPasswordToDatabase(string str, int len)
        {
            if (len == 3)
            {
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla3s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() == 0)
                    {
                        var obj = new KrzyzowkiTabele.Hasla3
                        {
                            haslo = str,
                            podpowiedz = TBClue.Text
                        };
                        var b = (from st in context.Hasla3Distincts
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 0)
                        {
                            var obj1 = new KrzyzowkiTabele.Hasla3Distinct
                            {
                                haslo = str
                            };
                            context.Hasla3Distincts.Add(obj1);
                        }
                        context.Hasla3s.Add(obj);
                        context.SaveChanges();
                        MessageBox.Show("Password added successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are already in database");
                    }
                }
            }
            if (len == 4)
            {
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla4s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() == 0)
                    {
                        var obj = new KrzyzowkiTabele.Hasla4
                        {
                            haslo = str,
                            podpowiedz = TBClue.Text
                        };
                        var b = (from st in context.Hasla4Distincts
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 0)
                        {
                            var obj1 = new KrzyzowkiTabele.Hasla4Distinct
                            {
                                haslo = str
                            };
                            context.Hasla4Distincts.Add(obj1);
                        }
                        context.Hasla4s.Add(obj);
                        context.SaveChanges();
                        MessageBox.Show("Password added successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are already in database");
                    }
                }
            }
            if (len == 5)
            {
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla5s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() == 0)
                    {
                        var obj = new KrzyzowkiTabele.Hasla5
                        {
                            haslo = str,
                            podpowiedz = TBClue.Text
                        };
                        var b = (from st in context.Hasla5Distincts
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 0)
                        {
                            var obj1 = new KrzyzowkiTabele.Hasla5Distinct
                            {
                                haslo = str
                            };
                            context.Hasla5Distincts.Add(obj1);
                        }
                        context.Hasla5s.Add(obj);
                        context.SaveChanges();
                        MessageBox.Show("Password added successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are already in database");
                    }
                }
            }
            if (len == 6)
            {
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla6s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() == 0)
                    {
                        var obj = new KrzyzowkiTabele.Hasla6
                        {
                            haslo = str,
                            podpowiedz = TBClue.Text
                        };
                        var b = (from st in context.Hasla6Distincts
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 0)
                        {
                            var obj1 = new KrzyzowkiTabele.Hasla6Distinct
                            {
                                haslo = str
                            };
                            context.Hasla6Distincts.Add(obj1);
                        }
                        context.Hasla6s.Add(obj);
                        context.SaveChanges();
                        MessageBox.Show("Password added successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are already in database");
                    }
                }
            }
            if (len == 7)
            {
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla7s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() == 0)
                    {
                        var obj = new KrzyzowkiTabele.Hasla7
                        {
                            haslo = str,
                            podpowiedz = TBClue.Text
                        };
                        var b = (from st in context.Hasla7Distincts
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 0)
                        {
                            var obj1 = new KrzyzowkiTabele.Hasla7Distinct
                            {
                                haslo = str
                            };
                            context.Hasla7Distincts.Add(obj1);
                        }
                        context.Hasla7s.Add(obj);
                        context.SaveChanges();
                        MessageBox.Show("Password added successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are already in database");
                    }
                }
            }
            if (len == 8)
            {
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla8s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() == 0)
                    {
                        var obj = new KrzyzowkiTabele.Hasla8
                        {
                            haslo = str,
                            podpowiedz = TBClue.Text
                        };
                        var b = (from st in context.Hasla8Distincts
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 0)
                        {
                            var obj1 = new KrzyzowkiTabele.Hasla8Distinct
                            {
                                haslo = str
                            };
                            context.Hasla8Distincts.Add(obj1);
                        }
                        context.Hasla8s.Add(obj);
                        context.SaveChanges();
                        MessageBox.Show("Password added successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are already in database");
                    }
                }
            }
            if (len == 9)
            {
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla9s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() == 0)
                    {
                        var obj = new KrzyzowkiTabele.Hasla9
                        {
                            haslo = str,
                            podpowiedz = TBClue.Text
                        };
                        var b = (from st in context.Hasla9Distincts
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 0)
                        {
                            var obj1 = new KrzyzowkiTabele.Hasla9Distinct
                            {
                                haslo = str
                            };
                            context.Hasla9Distincts.Add(obj1);
                        }
                        context.Hasla9s.Add(obj);
                        context.SaveChanges();
                        MessageBox.Show("Password added successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are already in database");
                    }
                }
            }
            if (len == 10)
            {
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla10s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() == 0)
                    {
                        var obj = new KrzyzowkiTabele.Hasla10
                        {
                            haslo = str,
                            podpowiedz = TBClue.Text
                        };
                        var b = (from st in context.Hasla10Distincts
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 0)
                        {
                            var obj1 = new KrzyzowkiTabele.Hasla10Distinct
                            {
                                haslo = str
                            };
                            context.Hasla10Distincts.Add(obj1);
                        }
                        context.Hasla10s.Add(obj);
                        context.SaveChanges();
                        MessageBox.Show("Password added successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are already in database");
                    }
                }
            }
            if (len == 11)
            {
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla11s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() == 0)
                    {
                        var obj = new KrzyzowkiTabele.Hasla11
                        {
                            haslo = str,
                            podpowiedz = TBClue.Text
                        };
                        var b = (from st in context.Hasla11Distincts
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 0)
                        {
                            var obj1 = new KrzyzowkiTabele.Hasla11Distinct
                            {
                                haslo = str
                            };
                            context.Hasla11Distincts.Add(obj1);
                        }
                        context.Hasla11s.Add(obj);
                        context.SaveChanges();
                        MessageBox.Show("Password added successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are already in database");
                    }
                }
            }
            if (len == 12)
            {
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla12s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() == 0)
                    {
                        var obj = new KrzyzowkiTabele.Hasla12
                        {
                            haslo = str,
                            podpowiedz = TBClue.Text
                        };
                        var b = (from st in context.Hasla12Distincts
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 0)
                        {
                            var obj1 = new KrzyzowkiTabele.Hasla12Distinct
                            {
                                haslo = str
                            };
                            context.Hasla12Distincts.Add(obj1);
                        }
                        context.Hasla12s.Add(obj);
                        context.SaveChanges();
                        MessageBox.Show("Password added successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are already in database");
                    }
                }
            }
            if (len == 13)
            {
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla13s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() == 0)
                    {
                        var obj = new KrzyzowkiTabele.Hasla13
                        {
                            haslo = str,
                            podpowiedz = TBClue.Text
                        };
                        var b = (from st in context.Hasla13Distincts
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 0)
                        {
                            var obj1 = new KrzyzowkiTabele.Hasla13Distinct
                            {
                                haslo = str
                            };
                            context.Hasla13Distincts.Add(obj1);
                        }
                        context.Hasla13s.Add(obj);
                        context.SaveChanges();
                        MessageBox.Show("Password added successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are already in database");
                    }
                }
            }
            if (len == 14)
            {
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla14s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() == 0)
                    {
                        var obj = new KrzyzowkiTabele.Hasla14
                        {
                            haslo = str,
                            podpowiedz = TBClue.Text
                        };
                        var b = (from st in context.Hasla14Distincts
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 0)
                        {
                            var obj1 = new KrzyzowkiTabele.Hasla14Distinct
                            {
                                haslo = str
                            };
                            context.Hasla14Distincts.Add(obj1);
                        }
                        context.Hasla14s.Add(obj);
                        context.SaveChanges();
                        MessageBox.Show("Password added successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are already in database");
                    }
                }
            }
            if (len == 15)
            {
                using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
                {
                    var a = (from st in context.Hasla15s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() == 0)
                    {
                        var obj = new KrzyzowkiTabele.Hasla15
                        {
                            haslo = str,
                            podpowiedz = TBClue.Text
                        };
                        var b = (from st in context.Hasla15Distincts
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 0)
                        {
                            var obj1 = new KrzyzowkiTabele.Hasla15Distinct
                            {
                                haslo = str
                            };
                            context.Hasla15Distincts.Add(obj1);
                        }
                        context.Hasla15s.Add(obj);
                        context.SaveChanges();
                        MessageBox.Show("Password added successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are already in database");
                    }
                }
            }
        }

        /// <summary>
        /// this function delete passwrod from database
        /// </summary>
        /// <param name="str">password </param>
        /// <param name="len">lenght of password</param>
        void DeletePasswordFromdatabase(string str, int len)
        {
            using (KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext())
            {
                if (len == 3)
                {
                    var a = (from st in context.Hasla3s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() != 0)
                    {
                        var b = (from st in context.Hasla3s
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 1)
                        {
                            var c = (from st in context.Hasla3Distincts
                                     where st.haslo == str
                                     select st);
                            foreach (var record in c)
                            {
                                context.Hasla3Distincts.Remove(record);
                            }
                        }
                        foreach (var record in a)
                        {
                            context.Hasla3s.Remove(record);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Password deleted successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are not in database");
                    }
                }

                if (len == 4)
                {
                    var a = (from st in context.Hasla4s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() != 0)
                    {
                        var b = (from st in context.Hasla4s
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 1)
                        {
                            var c = (from st in context.Hasla4Distincts
                                     where st.haslo == str
                                     select st);
                            foreach (var record in c)
                            {
                                context.Hasla4Distincts.Remove(record);
                            }
                        }
                        foreach (var record in a)
                        {
                            context.Hasla4s.Remove(record);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Password deleted successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are not in database");
                    }
                }
                if (len == 5)
                {
                    var a = (from st in context.Hasla5s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() != 0)
                    {
                        var b = (from st in context.Hasla5s
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 1)
                        {
                            var c = (from st in context.Hasla5Distincts
                                     where st.haslo == str
                                     select st);
                            foreach (var record in c)
                            {
                                context.Hasla5Distincts.Remove(record);
                            }
                        }
                        foreach (var record in a)
                        {
                            context.Hasla5s.Remove(record);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Password deleted successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are not in database");
                    }
                }
                if (len == 6)
                {
                    var a = (from st in context.Hasla6s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() != 0)
                    {
                        var b = (from st in context.Hasla6s
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 1)
                        {
                            var c = (from st in context.Hasla6Distincts
                                     where st.haslo == str
                                     select st);
                            foreach (var record in c)
                            {
                                context.Hasla6Distincts.Remove(record);
                            }
                        }
                        foreach (var record in a)
                        {
                            context.Hasla6s.Remove(record);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Password deleted successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are not in database");
                    }
                }
                if (len == 7)
                {
                    var a = (from st in context.Hasla7s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() != 0)
                    {
                        var b = (from st in context.Hasla7s
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 1)
                        {
                            var c = (from st in context.Hasla7Distincts
                                     where st.haslo == str
                                     select st);
                            foreach (var record in c)
                            {
                                context.Hasla7Distincts.Remove(record);
                            }
                        }
                        foreach (var record in a)
                        {
                            context.Hasla7s.Remove(record);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Password deleted successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are not in database");
                    }
                }
                if (len == 8)
                {
                    var a = (from st in context.Hasla8s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() != 0)
                    {
                        var b = (from st in context.Hasla8s
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 1)
                        {
                            var c = (from st in context.Hasla8Distincts
                                     where st.haslo == str
                                     select st);
                            foreach (var record in c)
                            {
                                context.Hasla8Distincts.Remove(record);
                            }
                        }
                        foreach (var record in a)
                        {
                            context.Hasla8s.Remove(record);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Password deleted successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are not in database");
                    }
                }
                if (len == 9)
                {
                    var a = (from st in context.Hasla9s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() != 0)
                    {
                        var b = (from st in context.Hasla9s
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 1)
                        {
                            var c = (from st in context.Hasla9Distincts
                                     where st.haslo == str
                                     select st);
                            foreach (var record in c)
                            {
                                context.Hasla9Distincts.Remove(record);
                            }
                        }
                        foreach (var record in a)
                        {
                            context.Hasla9s.Remove(record);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Password deleted successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are not in database");
                    }
                }
                if (len == 10)
                {
                    var a = (from st in context.Hasla10s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() != 0)
                    {
                        var b = (from st in context.Hasla10s
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 1)
                        {
                            var c = (from st in context.Hasla10Distincts
                                     where st.haslo == str
                                     select st);
                            foreach (var record in c)
                            {
                                context.Hasla10Distincts.Remove(record);
                            }
                        }
                        foreach (var record in a)
                        {
                            context.Hasla10s.Remove(record);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Password deleted successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are not in database");
                    }
                }
                if (len == 11)
                {
                    var a = (from st in context.Hasla11s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() != 0)
                    {
                        var b = (from st in context.Hasla11s
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 1)
                        {
                            var c = (from st in context.Hasla11Distincts
                                     where st.haslo == str
                                     select st);
                            foreach (var record in c)
                            {
                                context.Hasla11Distincts.Remove(record);
                            }
                        }
                        foreach (var record in a)
                        {
                            context.Hasla11s.Remove(record);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Password deleted successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are not in database");
                    }
                }
                if (len == 12)
                {
                    var a = (from st in context.Hasla12s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() != 0)
                    {
                        var b = (from st in context.Hasla12s
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 1)
                        {
                            var c = (from st in context.Hasla12Distincts
                                     where st.haslo == str
                                     select st);
                            foreach (var record in c)
                            {
                                context.Hasla12Distincts.Remove(record);
                            }
                        }
                        foreach (var record in a)
                        {
                            context.Hasla12s.Remove(record);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Password deleted successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are not in database");
                    }
                }
                if (len == 13)
                {
                    var a = (from st in context.Hasla13s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() != 0)
                    {
                        var b = (from st in context.Hasla13s
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 1)
                        {
                            var c = (from st in context.Hasla13Distincts
                                     where st.haslo == str
                                     select st);
                            foreach (var record in c)
                            {
                                context.Hasla13Distincts.Remove(record);
                            }
                        }
                        foreach (var record in a)
                        {
                            context.Hasla13s.Remove(record);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Password deleted successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are not in database");
                    }
                }
                if (len == 14)
                {
                    var a = (from st in context.Hasla14s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() != 0)
                    {
                        var b = (from st in context.Hasla14s
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 1)
                        {
                            var c = (from st in context.Hasla14Distincts
                                     where st.haslo == str
                                     select st);
                            foreach (var record in c)
                            {
                                context.Hasla14Distincts.Remove(record);
                            }
                        }
                        foreach (var record in a)
                        {
                            context.Hasla14s.Remove(record);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Password deleted successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are not in database");
                    }
                }
                if (len == 15)
                {
                    var a = (from st in context.Hasla15s
                             where st.podpowiedz == TBClue.Text && st.haslo == str
                             select st);
                    if (a.Count() != 0)
                    {
                        var b = (from st in context.Hasla15s
                                 where st.haslo == str
                                 select st);
                        if (b.Count() == 1)
                        {
                            var c = (from st in context.Hasla15Distincts
                                     where st.haslo == str
                                     select st);
                            foreach (var record in c)
                            {
                                context.Hasla15Distincts.Remove(record);
                            }
                        }
                        foreach (var record in a)
                        {
                            context.Hasla15s.Remove(record);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Password deleted successfully");
                    }
                    else
                    {
                        MessageBox.Show("This password and clue are not in database");
                    }
                }
            }
        }
    }
}