using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ExchangeAbstraction;

namespace ExchangeTasks
{
    /// <summary>
    /// Interaction logic for NewTask.xaml
    /// </summary>
    public partial class NewTask : Window
    {
        public Tasks Task { get; set; }

        public NewTask()
        {
            InitializeComponent();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            TaskData td = new TaskData();

            td.Subject = this.subject.Text;
            td.Category = this.category.SelectedItem.ToString();
            td.Body = this.body.Text;

            Task.AddTask(td);

            this.Close();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            foreach (string s in Task.GetMasterCategories())
            {
                this.category.Items.Add(s);
            }
        }
    }
}
