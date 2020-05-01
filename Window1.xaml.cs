using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public List<List<TextBox>> TexBoxList { get; set; }
        public List<String> PasswordList { get; set; }
        public List<List<string>> cluelist = new List<List<string>>();
        public ObservableCollection<ComboBoxItem> CbItems { get; set; }
        public ComboBoxItem SelectedcbItem { get; set; }
        public List<string> ListPasswordYouTriedEnter { get; set; }
        public TrackClueList TrackClueList{get;set;}


        //public TrackClueList TrackClueList { get; set; }
        //TrackClueList TrackClueList = new TrackClueList();

        public Window1(List<String> passwordList, List<List<TextBox>> list)
        {
#if DEBUG
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
#endif
            InitializeComponent();
            DataContext = this;
            PasswordList = passwordList;
            TexBoxList = list;
            TrackClueList = new TrackClueList();
            TrackClueList.Index = new List<int>();
            TrackClueList.Check = new List<bool>();
            CbItems = new ObservableCollection<ComboBoxItem>();
            ListPasswordYouTriedEnter = new List<string>();

            foreach (var record in PasswordList)
            {
                TrackClueList.Check.Add(false);
                TrackClueList.Index.Add(0);
                ListPasswordYouTriedEnter.Add("");
                var subcluelist = SubClueList(record.Length, record);
                cluelist.Add(subcluelist);
            }


            for (int i = 0; i < PasswordList.Count; i++)
            {
                var cbItem = new ComboBoxItem
                {
                    Content = "" + (i + 1).ToString(),
                    Name = "CBI" + (i + 1).ToString()
                };
                var spTextBox = new TextBox
                {
                    IsReadOnly = true,
                    Text = (i + 1).ToString() + "." + cluelist[i][0] + "(" + PasswordList[i].Count() + ")",
                    Name = "TB" + i.ToString(),
                    TextWrapping = TextWrapping.Wrap

                };

                SelectedcbItem = cbItem;
                ListBoxWithClues.Items.Add(spTextBox);
                CbItems.Add(cbItem);


            }



        }


        private void Button_Click_Try_Enter_Password(object sender, RoutedEventArgs e)
        {
            int.TryParse(SelectedcbItem.Content.ToString(), out int i);

            if (PasswordList[i - 1] == TBToEnterPassword.Text.ToUpper())
            {
                int j = 0;

                foreach (var obj in TexBoxList[i - 1])
                {
                    obj.Text = PasswordList[i - 1][j].ToString();
                    j = j + 1;
                }

                var textBox = new TextBox
                {
                    IsReadOnly = true,
                    Text = i.ToString() + "." + cluelist[i - 1][TrackClueList.Index[i - 1]] + "(" + PasswordList[i - 1].Count() + ")",
                    Name = "TB" + (i - 1).ToString(),
                    TextWrapping = TextWrapping.Wrap
                };
                TrackClueList.Check[i - 1] = true;
                if (TrackClueList.Check[i - 1])
                {
                    textBox.TextDecorations = TextDecorations.Strikethrough;
                }

                ListBoxWithClues.Items[i - 1] = textBox;


            }

            ListPasswordYouTriedEnter[i - 1] += TBToEnterPassword.Text.ToUpper() + "\n";
            ListBoxOfPasswordYouTriedEnter.Text = ListPasswordYouTriedEnter[CB.SelectedIndex];


        }



        private void Button_Click_Change_Clue(object sender, RoutedEventArgs e)
        {
            int.TryParse(SelectedcbItem.Content.ToString(), out int j);
            TrackClueList.Index[j - 1] = TrackClueList.NextIndex(TrackClueList.Index[j - 1], cluelist[j - 1].Count);

            var textBox = new TextBox
            {
                IsReadOnly = true,
                Text = (j).ToString() + "." + cluelist[j - 1][TrackClueList.Index[j - 1]] + "(" + PasswordList[j - 1].Count() + ")",
                Name = "TB" + (j - 1).ToString(),
                TextWrapping = TextWrapping.Wrap


            };

            if (TrackClueList.Check[j - 1])
            {
                textBox.TextDecorations = TextDecorations.Strikethrough;
            }

            ListBoxWithClues.Items[j - 1] = textBox;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxOfPasswordYouTriedEnter.Text = ListPasswordYouTriedEnter[CB.SelectedIndex];
        }

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
            return subcluelist;
        }

        
    }
}

