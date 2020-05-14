using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GeneratorKrzyzowek
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        /// <summary>
        ///  list of lists texbox where crossword password will be written
        /// </summary>
        public List<List<TextBox>> TexBoxList { get; set; }
        /// <summary>
        /// list with all passwords
        /// </summary>
        public List<String> PasswordList { get; set; }
        /// <summary>
        /// list with all clues
        /// </summary>
        public List<List<string>> Cluelist { get; set; } = new List<List<string>>();
        /// <summary>
        /// property where combobox items will be add 
        /// </summary>
        public ObservableCollection<ComboBoxItem> CbItems { get; set; }
        /// <summary>
        /// property which has selected item in combobox
        /// </summary>
        public ComboBoxItem SelectedcbItem { get; set; }
        /// <summary>
        /// list of password you tried enter
        /// </summary>
        public List<string> ListPasswordYouTriedEnter { get; set; }
        /// <summary>
        /// property to track ClueList
        /// </summary>
        public TrackClueList TrackClueList { get; set; }
        /// <summary>
        /// list of list with starts co-ordinates of password
        /// </summary>
        public List<List<int>> StartXYList { get; set; }
        /// <summary>
        /// list of list with ends co-ordinates of password
        /// </summary>
        public List<List<int>> EndXYList { get; set; }
        /// <summary>
        /// main password (has gray background)
        /// </summary>
        public string Password { get; set; } = " ";
        /// <summary>
        /// array where crossword is stored
        /// </summary>
        public string[,] Board { get; set; }

        /// <summary>
        /// constructor set CbItems, ListPasswordYouTriedEnter
        /// </summary>
        /// <param name="passwordList">set PasswordList</param>
        /// <param name="tblist">set TBList</param>
        /// <param name="startxylist">set StsrtXYList can be null for crossword in only across passwords</param>
        /// <param name="endxylist">set EndXYList can be null for crossword in only across passwords</param>
        /// <param name="trackcluelist">set TrackClueList can be null for crossword in only across passwords</param>
        public Window1(List<String> passwordList, List<List<TextBox>> tblist, List<List<int>> startxylist = null, List<List<int>> endxylist = null,TrackClueList trackcluelist = null)
        {
#if DEBUG
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
#endif
            InitializeComponent();
            DataContext = this;
            PasswordList = passwordList;
            StartXYList = startxylist;
            EndXYList = endxylist;
            TexBoxList = tblist;
            TrackClueList = trackcluelist;
            if (TrackClueList == null)
            {
                TrackClueList = new TrackClueList
                {
                    Check = new List<bool>(),
                    Index = new List<int>()
                };
                for (int i = 0; i < PasswordList.Count; i++)
                {
                    TrackClueList.Check.Add(false);
                    TrackClueList.Index.Add(0);
                }
            }
            CbItems = new ObservableCollection<ComboBoxItem>();
            ListPasswordYouTriedEnter = new List<string>();
            foreach (var record in PasswordList)
            {
                ListPasswordYouTriedEnter.Add("");
                var subcluelist = SubClueList(record.Length, record);
                Cluelist.Add(subcluelist);
            }
            int j = 0;
            for (int i = 0; i < PasswordList.Count; i++)
            {
                var cbItem = new ComboBoxItem
                {
                    Content = "" + (j).ToString(),
                    Name = "CBI" + (i + 1).ToString()
                };
                var spTextBox = new TextBox
                {
                    IsReadOnly = true,
                    Text = (j).ToString() + "." + Cluelist[i][0] + "(" + PasswordList[i].Count() + ")",
                    Name = "TB" + i.ToString(),
                    TextWrapping = TextWrapping.Wrap
                };
                if(TrackClueList.Check[j] == true)
                {
                    spTextBox.TextDecorations = TextDecorations.Strikethrough;
                }
                if (PasswordList[0] != " ")
                {
                    cbItem.Content = "" + (j + 1).ToString();
                    spTextBox.Text = (j + 1).ToString() + "." + Cluelist[i][0] + "(" + PasswordList[i].Count() + ")";
                }
                if (PasswordList[i] == " ")
                {
                    cbItem.Content = "Across";
                    cbItem.IsEnabled = false;
                    spTextBox.Text = "Across";
                }
                if (PasswordList[i] == "  ")
                {
                    cbItem.Content = "Down";
                    cbItem.IsEnabled = false;
                    spTextBox.Text = "Down";
                    j = 0;
                }
                if (TrackClueList.Check[i] == true)
                {
                    if (StartXYList != null)
                    {
                        int x = StartXYList[i][0];
                        int y = StartXYList[i][1];
                        int x2 = EndXYList[i][0];
                        int y2 = EndXYList[i][1];
                        if (x == x2)
                        {
                            for (int k = 0; k < PasswordList[i].Length; k++)
                            {
                                TexBoxList[x][y + k].Text = PasswordList[i][k].ToString();
                            }
                        }
                        if (y == y2)
                        {
                            for (int k = 0; k < PasswordList[i].Length; k++)
                            {
                                TexBoxList[x + k][y].Text = PasswordList[i][k].ToString();
                            }
                        }
                    }
                }
                j += 1;
                SelectedcbItem = cbItem;
                ListBoxWithClues.Items.Add(spTextBox);
                CbItems.Add(cbItem);
            }
        }

        /// <summary>
        /// this function try to enter password written in textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Try_Enter_Password(object sender, RoutedEventArgs e)
        {
            int i = CB.SelectedIndex;
            int l = int.Parse(CB.Text);
            if (PasswordList[i] == TBToEnterPassword.Text.ToUpper())
            {
                if (StartXYList != null)
                {
                    int x = StartXYList[i][0];
                    int y = StartXYList[i][1];
                    int x2 = EndXYList[i][0];
                    int y2 = EndXYList[i][1];
                    if (x == x2)
                    {
                        for (int k = 0; k < PasswordList[i].Length; k++)
                        {
                            TexBoxList[x][y + k].Text = PasswordList[i][k].ToString();
                        }
                    }
                    if (y == y2)
                    {
                        for (int k = 0; k < PasswordList[i].Length; k++)
                        {
                            TexBoxList[x + k][y].Text = PasswordList[i][k].ToString();
                        }
                    }
                }
                else
                {
                    int j = 0;
                    foreach (var obj in TexBoxList[i])
                    {
                        obj.Text = PasswordList[i][j].ToString();
                        j = j + 1;
                    }
                }
                var textBox = new TextBox
                {
                    IsReadOnly = true,
                    Text = l.ToString() + "." + Cluelist[i][TrackClueList.Index[i]] + "(" + PasswordList[i].Count() + ")",
                    Name = "TB" + (i).ToString(),
                    TextWrapping = TextWrapping.Wrap
                };
                if (PasswordList[0] != " ")
                {
                    textBox.Text = (i + 1).ToString() + "." + Cluelist[i][TrackClueList.Index[i]] + "(" + PasswordList[i].Count() + ")";
                }
                TrackClueList.Check[i] = true;
                if (TrackClueList.Check[i])
                {
                    textBox.TextDecorations = TextDecorations.Strikethrough;
                }
                ListBoxWithClues.Items[i] = textBox;
            }
            else
            {
                ListPasswordYouTriedEnter[i] += TBToEnterPassword.Text.ToUpper() + "\n";
                ListBoxOfPasswordYouTriedEnter.Text = ListPasswordYouTriedEnter[CB.SelectedIndex];
            }
        }

        /// <summary>
        /// this function change clue of current password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Change_Clue(object sender, RoutedEventArgs e)
        {
            int j = CB.SelectedIndex;
            int l = int.Parse(CB.Text);
            TrackClueList.Index[j] = TrackClueList.NextIndex(TrackClueList.Index[j], Cluelist[j].Count);
            var textBox = new TextBox
            {
                IsReadOnly = true,
                Text = (l).ToString() + "." + Cluelist[j][TrackClueList.Index[j]] + "(" + PasswordList[j].Count() + ")",
                Name = "TB" + (j).ToString(),
                TextWrapping = TextWrapping.Wrap
            };
            if (PasswordList[0] != " ")
            {
                textBox.Text = (j + 1).ToString() + "." + Cluelist[j][TrackClueList.Index[j]] + "(" + PasswordList[j].Count() + ")";
            }
            if (TrackClueList.Check[j])
            {
                textBox.TextDecorations = TextDecorations.Strikethrough;
            }
            ListBoxWithClues.Items[j] = textBox;
        }

        /// <summary>
        /// change current password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxOfPasswordYouTriedEnter.Text = ListPasswordYouTriedEnter[CB.SelectedIndex];
        }

        /// <summary>
        /// save crossword to text file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (StartXYList != null)
            {
                saveFileDialog.Filter = "Krzyżówka(*.crosswordadv)|*.crosswordadv";
            }
            else
            {
                saveFileDialog.Filter = "Krzyżówka(*.crossword)|*.crossword";
            }
            if (saveFileDialog.ShowDialog() == true)
            {
                if (Board != null)
                {
                    File.AppendAllText(saveFileDialog.FileName, Board.GetLength(0) + Environment.NewLine);
                    for (int i = 0; i < Board.GetLength(0); i++)
                    {
                        for (int j = 0; j < Board.GetLength(1); j++)
                        {
                            File.AppendAllText(saveFileDialog.FileName, Board[i, j] + Environment.NewLine);
                        }
                    }
                }
                if (Password != " ")
                {
                    File.AppendAllText(saveFileDialog.FileName, Password + Environment.NewLine);
                }
                for (int i = 0; i < PasswordList.Count; i++)
                {
                    File.AppendAllText(saveFileDialog.FileName, PasswordList[i] + Environment.NewLine);
                    File.AppendAllText(saveFileDialog.FileName, TrackClueList.Check[i].ToString() + Environment.NewLine);
                }
                if (StartXYList != null)
                {
                    for (int i = 0; i < StartXYList.Count; i++)
                    {
                        File.AppendAllText(saveFileDialog.FileName, "aa" + StartXYList[i][0] + StartXYList[i][1] + Environment.NewLine);
                    }
                }
                if (EndXYList != null)
                {
                    for (int i = 0; i < StartXYList.Count; i++)
                    {
                        File.AppendAllText(saveFileDialog.FileName, "a" + EndXYList[i][0] + EndXYList[i][1] + Environment.NewLine);
                    }
                }
            }
        }

        /// <summary>
        /// this function get all clue for password 
        /// </summary>
        /// <param name="x">password lenght</param>
        /// <param name="record">password</param>
        /// <returns>list of clue for password</returns>
        private List<string> SubClueList(int x, string record)
        {
            var subcluelist = new List<string>();
            if (x == 3)
            {
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var query = from st in context.Hasla3s
                            where st.haslo == record
                            select st;
                foreach (var record1 in query)
                {
                    subcluelist.Add(record1.podpowiedz);
                }
                return subcluelist;
            }
            if (x == 4)
            {
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var query = from st in context.Hasla4s
                            where st.haslo == record
                            select st;
                foreach (var record1 in query)
                {
                    subcluelist.Add(record1.podpowiedz);
                }
                return subcluelist;
            }
            if (x == 5)
            {
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var query = from st in context.Hasla5s
                            where st.haslo == record
                            select st;
                foreach (var record1 in query)
                {
                    subcluelist.Add(record1.podpowiedz);
                }
                return subcluelist;
            }
            if (x == 6)
            {
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var query = from st in context.Hasla6s
                            where st.haslo == record
                            select st;
                foreach (var record1 in query)
                {
                    subcluelist.Add(record1.podpowiedz);
                }
                return subcluelist;
            }
            if (x == 7)
            {
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var query = from st in context.Hasla7s
                            where st.haslo == record
                            select st;
                foreach (var record1 in query)
                {
                    subcluelist.Add(record1.podpowiedz);
                }
                return subcluelist;
            }
            if (x == 8)
            {
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var query = from st in context.Hasla8s
                            where st.haslo == record
                            select st;
                foreach (var record1 in query)
                {
                    subcluelist.Add(record1.podpowiedz);
                }
                return subcluelist;
            }
            if (x == 9)
            {
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var query = from st in context.Hasla9s
                            where st.haslo == record
                            select st;
                foreach (var record1 in query)
                {
                    subcluelist.Add(record1.podpowiedz);
                }
                return subcluelist;
            }
            if (x == 10)
            {
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var query = from st in context.Hasla10s
                            where st.haslo == record
                            select st;
                foreach (var record1 in query)
                {
                    subcluelist.Add(record1.podpowiedz);
                }
                return subcluelist;
            }
            if (x == 11)
            {
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var query = from st in context.Hasla11s
                            where st.haslo == record
                            select st;
                foreach (var record1 in query)
                {
                    subcluelist.Add(record1.podpowiedz);
                }
                return subcluelist;
            }
            if (x == 12)
            {
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var query = from st in context.Hasla12s
                            where st.haslo == record
                            select st;
                foreach (var record1 in query)
                {
                    subcluelist.Add(record1.podpowiedz);
                }
                return subcluelist;
            }
            if (x == 13)
            {
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var query = from st in context.Hasla13s
                            where st.haslo == record
                            select st;
                foreach (var record1 in query)
                {
                    subcluelist.Add(record1.podpowiedz);
                }
                return subcluelist;
            }
            if (x == 14)
            {
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var query = from st in context.Hasla14s
                            where st.haslo == record
                            select st;
                foreach (var record1 in query)
                {
                    subcluelist.Add(record1.podpowiedz);
                }
                return subcluelist;
            }
            if (x == 15)
            {
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                var query = from st in context.Hasla15s
                            where st.haslo == record
                            select st;
                foreach (var record1 in query)
                {
                    subcluelist.Add(record1.podpowiedz);
                }
                return subcluelist;
            }
            if (x == 1)
            {
                subcluelist.Add("Across");
                return subcluelist;
            }
            if (x == 2)
            {
                subcluelist.Add("Down");
                return subcluelist;
            }
            return subcluelist;
        }
    }
}