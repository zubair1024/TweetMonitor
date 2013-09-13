using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
namespace TakeANote
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
            this.textBlock1.Text = "Created By \n Zubair Ahmed \n Chirag Vadhava.";
        }

        private void Suggestion_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Tasks.EmailComposeTask emailTask = new Microsoft.Phone.Tasks.EmailComposeTask();
            emailTask.Subject = "Suggestion for Tweet Monitor v1.0";
            emailTask.To = "zubair1024@gmail.com";
            emailTask.Show();
        }

        private void More_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new Uri("http://www.christuniversity.in", UriKind.Absolute);
            task.Show();
        }

        private void Learn_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new Uri("http://www.github.com/zubair1024", UriKind.Absolute);
            task.Show();
        }
    }
}