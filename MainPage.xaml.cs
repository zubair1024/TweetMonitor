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
using System.Xml.Linq;
using Microsoft.Phone.Tasks;
using System.Text;
using System.Diagnostics;
using Microsoft.Phone.Scheduler;
using WPET;
using Microsoft.Phone.Shell;

namespace TweetMonitor
{
    public partial class MainPage : PhoneApplicationPage
    {
        Settings<string> userHandle = new Settings<string>("TwitterHandle", "");
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (!Microsoft.Phone.Net.NetworkInformation.DeviceNetworkInformation.IsNetworkAvailable)
            {
                MessageBox.Show("No network connection is available");
                return;
            }

            userHandle.Value = textBox1.Text;
            textBlock1.Text = "downloading...";

            WebClient connectTwitter = new WebClient();
            connectTwitter.DownloadStringCompleted += new DownloadStringCompletedEventHandler(connectTwitter_DownloadStringCompleted);
            //connectTwitter.DownloadStringAsync(new Uri("http://api.twitter.com/1.1/statuses/user_timeline.json?screen_name=" + textBox1.Text));
            connectTwitter.DownloadStringAsync(new Uri("http://rss.cnn.com/rss/edition.rss"));

        }

        void connectTwitter_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                textBlock1.Text = "download failed!";
                return;
            }

            textBlock1.Text = "download complete";

            ShellTile appTile = ShellTile.ActiveTiles.First();

            if (appTile != null)
            {
                StandardTileData tileData = new StandardTileData
                {
                    BackTitle = userHandle.Value
                };

                appTile.Update(tileData);
            }

            XElement Tweets = XElement.Parse(e.Result);
            listBox1.ItemsSource = from tweet in Tweets.Descendants("status")
                                   select new Tweet
                                   {
                                       Statuses = tweet.Element("user").Element("statuses_count").Value,
                                       Contents = tweet.Element("text").Value,
                                       Name = tweet.Element("user").Element("screen_name").Value
                                   };
        }

        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (listBox1.SelectedIndex == -1)
                return;

            Tweet selectedItem = listBox1.SelectedItem as Tweet;

            if (selectedItem != null)
            {
                string textValue = selectedItem.Contents;
                int linkLocation = textValue.ToLower().IndexOf("http");
                if (linkLocation != -1)
                {
                    StringBuilder b = new StringBuilder();
                    for (int i = linkLocation; i < textValue.Length; i++)
                    {
                        char nextChar = textValue[i];
                        if (nextChar == ' ')
                            break;
                        b.Append(nextChar);
                    }

                    string result = b.ToString();
                    if (result.Length > 0)
                    {
                        WebBrowserTask task = new WebBrowserTask();
                        task.Uri = new Uri(result);
                        task.Show();
                    }
                }
                else
                    MessageBox.Show("No link found in that tweet");
            }
            listBox1.SelectedIndex = -1;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            textBlock1.Text = "";

            Debug.WriteLine("OnNavigatedTo");

            textBox1.Text = userHandle.Value;

            if (userHandle.Value != "" && App.AppWasTombstoned)
            {
                Debug.WriteLine("Reloading list");
                button1_Click(null, null);
            }

        }



        private void About_Click(object sender, EventArgs e)
        {
            // Navigate to the new page
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

    }
}