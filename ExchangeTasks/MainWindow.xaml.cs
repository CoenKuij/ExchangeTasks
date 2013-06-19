using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExchangeAbstraction;

namespace ExchangeTasks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public TaskDataList MyTaskList { get; set; }

        public MainWindow()
        {

            InitializeComponent();
            Credentials logInScreen = new Credentials();
            logInScreen.ShowDialog();

            Tasks t = new Tasks(logInScreen.textBox1.Text, logInScreen.textBox2.Text);
            TaskData td;
            MyTaskList = new TaskDataList();

            foreach (string s in t.RetrieveDistinctTaskCategories())
            {
                td = new TaskData();
                td.Category = s;
                MyTaskList.Add(td);
            }

            this.DataContext = this;
        }

        private void Expander_Initialized(object sender, EventArgs e)
        {
            Expander ex = (Expander)sender;
            Tasks t = new Tasks();
            StackPanel sp = new StackPanel();

            foreach (TaskData td in t.GetTasksByCategory(ex.Header.ToString()))
            {
                if (td.IsComplete) continue;

                Label l = new Label();

                l.Content = td.Subject;
                l.MouseLeftButtonDown += new MouseButtonEventHandler(l_MouseLeftButtonDown);
                sp.Children.Add(l);
            }

            sp.Margin = new Thickness(25, 0, 0, 0);
            ex.Content = sp;
        }

        void l_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Label l = (Label)sender;
            string s = l.Content.ToString();
            Tasks t = new Tasks();

            bodyBrowser.NavigateToString(t.FindTasksBySubject(s).Body);

            NewTask newTask = new NewTask();


            newTask.setCategories(t.RetrieveDistinctTaskCategories());
            newTask.Show();
        }
    }
}
