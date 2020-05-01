using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeneratorKrzyzowek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        /// list of texbox where crossword password will be written
        public List<List<TextBox>> list = new List<List<TextBox>>();
        public List<String> passwordList = new List<string>();
        public List<Label> labels = new List<Label>();
        public string password = "";
        public Window1 Window1{get;set;}



        //public List<Window> windows = new List<Window>(); 
        public MainWindow()
        {
            InitializeComponent();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            list.Clear();
            passwordList.Clear();
            labels.Clear();
            var t = new TrackClueList();
            int x = t.GetPasswordLenght(CBI.Text);
            password = ChoosePassword(x);

            int k = 1;
            for (int i = 0; i < x; i++)
            {

                Random rand = new Random();
                int y = rand.Next();

                y = (y % 12) + 3;
                passwordList.Add(ChoosePassword1(y, password[i].ToString()));
                var letterind = passwordList[i].IndexOf(password[i]);

                List<TextBox> textBoxes = new List<TextBox>();
                for (int j = 0; j < y; j++)
                {
                    var textbox = new TextBox
                    {
                        IsReadOnly = true,
                        //Text = " " + passwordList[i][j],
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

                k = k + 1;
                labels.Add(label);
                list.Add(textBoxes);

            }
            
            Window1 Window1= new Window1(passwordList, list);
            foreach (List<TextBox> sublist in list)
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


            //windows.Add(window1);
            //windows[windows.Count - 1].Show();
            
            Window1.Show();
        }
        public String ChoosePassword(int x)
        {
            String vs = "";
            if (x == 3)
            {

                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                int toSkip = rand.Next(1, context.Hasla3s.Count());
                KrzyzowkiTabele.Hasla3 a = context.Hasla3s.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                vs = a.haslo;
                return vs;
            }
            if (x == 4)
            {

                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                int toSkip = rand.Next(1, context.Hasla4s.Count());
                KrzyzowkiTabele.Hasla4 a = context.Hasla4s.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                vs = a.haslo;
                return vs;
            }
            if (x == 5)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                int toSkip = rand.Next(1, context.Hasla5s.Count());
                KrzyzowkiTabele.Hasla5 a = context.Hasla5s.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                vs = a.haslo;
                return vs;
            }
            if (x == 6)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                int toSkip = rand.Next(1, context.Hasla6s.Count());
                KrzyzowkiTabele.Hasla6 a = context.Hasla6s.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                vs = a.haslo;
                return vs;
            }
            if (x == 7)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                int toSkip = rand.Next(1, context.Hasla7s.Count());
                KrzyzowkiTabele.Hasla7 a = context.Hasla7s.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                vs = a.haslo;
                return vs;
            }
            if (x == 8)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                int toSkip = rand.Next(1, context.Hasla8s.Count());
                KrzyzowkiTabele.Hasla8 a = context.Hasla8s.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                vs = a.haslo;
                return vs;
            }
            if (x == 9)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                int toSkip = rand.Next(1, context.Hasla9s.Count());
                KrzyzowkiTabele.Hasla9 a = context.Hasla9s.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                vs = a.haslo;
                return vs;
            }
            if (x == 10)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                int toSkip = rand.Next(1, context.Hasla10s.Count());
                KrzyzowkiTabele.Hasla10 a = context.Hasla10s.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                vs = a.haslo;
                return vs;
            }
            if (x == 11)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                int toSkip = rand.Next(1, context.Hasla11s.Count());
                KrzyzowkiTabele.Hasla11 a = context.Hasla11s.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                vs = a.haslo;
                return vs;
            }
            if (x == 12)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                int toSkip = rand.Next(1, context.Hasla12s.Count());
                KrzyzowkiTabele.Hasla12 a = context.Hasla12s.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                vs = a.haslo;
                return vs;
            }
            if (x == 13)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                int toSkip = rand.Next(1, context.Hasla13s.Count());
                KrzyzowkiTabele.Hasla13 a = context.Hasla13s.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                vs = a.haslo;
                return vs;
            }
            if (x == 14)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                int toSkip = rand.Next(1, context.Hasla14s.Count());
                KrzyzowkiTabele.Hasla14 a = context.Hasla14s.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                vs = a.haslo;
                return vs;
            }
            if (x == 15)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();
                int toSkip = rand.Next(1, context.Hasla15s.Count());
                KrzyzowkiTabele.Hasla15 a = context.Hasla15s.OrderBy(obj => obj.ID).Skip(toSkip).Take(1).First();
                vs = a.haslo;
                return vs;
            }
            return vs;
        }




        private String ChoosePassword1(int x, string password)
        {
            string vs = "";
            if (x == 3)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();

                var a = (from st in context.Hasla3s
                         orderby st.ID
                         where st.haslo.Contains(password)
                         select st);
                int toSkip = rand.Next(1, a.Count());
                var b = a.Skip(toSkip).Take(1).First();

                vs = b.haslo;
                return vs;
            }
            if (x == 4)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();

                var a = (from st in context.Hasla4s
                         orderby st.ID
                         where st.haslo.Contains(password)
                         select st);
                int toSkip = rand.Next(1, a.Count());
                var b = a.Skip(toSkip).Take(1).First();

                vs = b.haslo;
                return vs;
            }
            if (x == 5)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();

                var a = (from st in context.Hasla5s
                         orderby st.ID
                         where st.haslo.Contains(password)
                         select st);
                int toSkip = rand.Next(1, a.Count());
                var b = a.Skip(toSkip).Take(1).First();

                vs = b.haslo;
                return vs;
            }
            if (x == 6)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();

                var a = (from st in context.Hasla6s
                         orderby st.ID
                         where st.haslo.Contains(password)
                         select st);
                int toSkip = rand.Next(1, a.Count());
                var b = a.Skip(toSkip).Take(1).First();

                vs = b.haslo;
                return vs;
            }
            if (x == 7)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();

                var a = (from st in context.Hasla7s
                         orderby st.ID
                         where st.haslo.Contains(password)
                         select st);
                int toSkip = rand.Next(1, a.Count());
                var b = a.Skip(toSkip).Take(1).First();

                vs = b.haslo;
                return vs;
            }
            if (x == 8)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();

                var a = (from st in context.Hasla8s
                         orderby st.ID
                         where st.haslo.Contains(password)
                         select st);
                int toSkip = rand.Next(1, a.Count());
                var b = a.Skip(toSkip).Take(1).First();

                vs = b.haslo;
                return vs;
            }
            if (x == 9)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();

                var a = (from st in context.Hasla9s
                         orderby st.ID
                         where st.haslo.Contains(password)
                         select st);
                int toSkip = rand.Next(1, a.Count());
                var b = a.Skip(toSkip).Take(1).First();

                vs = b.haslo;
                return vs;
            }
            if (x == 10)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();

                var a = (from st in context.Hasla10s
                         orderby st.ID
                         where st.haslo.Contains(password)
                         select st);
                int toSkip = rand.Next(1, a.Count());
                var b = a.Skip(toSkip).Take(1).First();

                vs = b.haslo;
                return vs;
            }
            if (x == 11)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();

                var a = (from st in context.Hasla11s
                         orderby st.ID
                         where st.haslo.Contains(password)
                         select st);
                int toSkip = rand.Next(1, a.Count());
                var b = a.Skip(toSkip).Take(1).First();

                vs = b.haslo;
                return vs;
            }
            if (x == 12)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();

                var a = (from st in context.Hasla12s
                         orderby st.ID
                         where st.haslo.Contains(password)
                         select st);
                int toSkip = rand.Next(1, a.Count());
                var b = a.Skip(toSkip).Take(1).First();

                vs = b.haslo;
                return vs;
            }
            if (x == 13)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();

                var a = (from st in context.Hasla13s
                         orderby st.ID
                         where st.haslo.Contains(password)
                         select st);
                int toSkip = rand.Next(1, a.Count());
                var b = a.Skip(toSkip).Take(1).First();

                vs = b.haslo;
                return vs;
            }
            if (x == 14)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();

                var a = (from st in context.Hasla14s
                         orderby st.ID
                         where st.haslo.Contains(password)
                         select st);
                int toSkip = rand.Next(1, a.Count());
                var b = a.Skip(toSkip).Take(1).First();

                vs = b.haslo;
                return vs;
            }
            if (x == 15)
            {
                Random rand = new Random();
                KrzyzowkiTabele.MyContext context = new KrzyzowkiTabele.MyContext();

                var a = (from st in context.Hasla15s
                         orderby st.ID
                         where st.haslo.Contains(password)
                         select st);
                int toSkip = rand.Next(1, a.Count());
                var b = a.Skip(toSkip).Take(1).First();

                vs = b.haslo;
                return vs;
            }
            return vs;

        }
    }
}

