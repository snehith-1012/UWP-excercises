using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.UI.Popups;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Remainders;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Remainders
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public List<Channel> channelsList = new List<Channel>();
        public List<Person> Persons = new List<Person>();
        public List<Remainder> Remainders = new List<Remainder>();
        public HashSet<string> Peoplenames = new HashSet<string>();
        public List<string> TemperoryList = new List<string>();

        public ObservableCollection<Remainder> RemaindersForMe = new ObservableCollection<Remainder>();

        public int remainderId = 0;
        public string peopleAdded = "";
        public string peopleIncludedforOneRemainder
        {
            get
            {
                return peopleAdded;
            }
            set
            {
                if (peopleAdded!= value)
                {
                    peopleAdded = value;
                    RaisePropertyChanged(nameof(peopleIncludedforOneRemainder));
                }
            }
        }

        public int channelId=-1;
        public string UIPersonId = "1";
        public int dateSet = 0;
        
        public ObservableCollection<Remainder> CompletedRemainders = new ObservableCollection<Remainder>();
        DateTime RemainderDateAndTime=DateTime.Today;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPage()
        {
            this.InitializeComponent();
            Persons.Add(new Person("1", "snehith", "snehith.oddula@zohocorp.com"));
            Persons.Add(new Person("2", "someone", "someone@zohocorp.com"));
            Persons.Add(new Person("3", "someone2", "someone2@zohocorp.com"));
            Persons.Add(new Person("4", "someone3", "someone3@zohocorp.com"));

            foreach(var i in Persons)
            {
                Peoplenames.Add(i.name);
            }
            
        }


        private void RemaindersButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void SaveDate_Click(object sender, RoutedEventArgs e)
        {

            if (dateSet == 1)
            {
                var date = Calendar.Date.Value.DateTime;
                var formateddate = date.Date;
                RemainderDateAndTime = formateddate + SetTime1.Time;
                DateAndTimeDisplay.Text = RemainderDateAndTime.ToString();
            }
            else
            {
                RemainderDateAndTime = DateTime.Now;
                //DateAndTimeDisplay.Text = DateTime.Now.ToString();
            }
        }

        private void MyAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var Auto = (AutoSuggestBox)sender;
            List<string> Suggestion;
            if (string.IsNullOrEmpty(Auto.Text))
                Suggestion = Peoplenames.Where(p=>p!="snehith").ToList();
            else
                Suggestion = Peoplenames.Where(p => p.StartsWith(Auto.Text, StringComparison.OrdinalIgnoreCase) && p!="snehith" ).ToList();
            Auto.ItemsSource = Suggestion.ToArray();
            
        }

        private void MyAutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            MyAutoSuggestBox.Text = args.SelectedItem.ToString();
            

            foreach (var i in Persons)
            {
                if (i.name == MyAutoSuggestBox.Text)
                {
                    int duplicate = 0;
                    foreach (var j in TemperoryList)
                    {
                        if (j == i.zuid)
                        {
                            duplicate = 1;
                            break;
                        }
                    }
                    if (duplicate == 0)
                    {
                        TemperoryList.Add(i.zuid);
                        if (peopleAdded != "")
                            peopleAdded += " and " + args.SelectedItem.ToString();
                        else
                            peopleAdded += args.SelectedItem.ToString();
                        RaisePropertyChanged(nameof(peopleIncludedforOneRemainder));
                        break;
                    }
                }    
            }
            //PeopleRemoveButton.Content = TemperoryList.Count.ToString();
            if(TemperoryList.Count>0)
            {
                PeopleRemoveButton.Visibility = Visibility.Visible;
            }
            else
            {
                PeopleRemoveButton.Visibility = Visibility.Collapsed;
            }
            foreach(var i in channelsList)
            {
               if(i.channelName==MyAutoSuggestBox.Text)
                {
                    channelId = i.channelId;
                    break;
                }
            }
            MyAutoSuggestBox.Text = "";
            //peopleAdded = "";
        }

        private void RemainderSetButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog messageDialog = new MessageDialog("Please add remainder description", "Dialog Title");

            if (RemainderDescription.Text == "")
            {
                messageDialog.ShowAsync();
                return;
            }
            TemperoryList.Add("1");
            ObservableCollection<string> l = new ObservableCollection<string>(TemperoryList);
            Remainders.Add(new Remainder(RemainderDescription.Text, RemainderDateAndTime, false, l, null,dateSet,remainderId+1));
            remainderId += 1;
            RemainderDescription.Text = "";
            DateAndTimeDisplay.Text = "";

            foreach (var i in TemperoryList)
            {
                if (i == UIPersonId)
                    RemaindersForMe.Add(Remainders[Remainders.Count - 1]);
            }

            TemperoryList.Clear();
            peopleAdded = "";
            peopleIncludedforOneRemainder="";
            RemainderDateAndTime = DateTime.Now;
            RaisePropertyChanged(nameof(peopleIncludedforOneRemainder));
            RemainderDateAndTime = DateTime.Today;
            dateSet = 0;
            PeopleRemoveButton.Visibility = Visibility.Collapsed;
            Calendar.Date = null;
            SetTime1.SelectedTime = null;
        }

        private void MyRemainders_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void ListOfRemainders_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int myValue = (int)(((Button)sender).Tag);
            foreach(var i in RemaindersForMe)
            {
                if (i.RemainderId == myValue)
                {
                    RemaindersForMe.Remove(i);
                    break;
                }
            }
        }

        private void MarkAsComplete_Click(object sender, RoutedEventArgs e)
        {
            int myValue = (int)(((Button)sender).Tag);
            foreach (var i in RemaindersForMe)
            {
                if (i.RemainderId == myValue)
                {
                    CompletedRemainders.Add(i);
                    RemaindersForMe.Remove(i);
                    break;
                }
            }
        }

        private void FlyoutCalender_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
              
        }

        private void MyAutoSuggestBox_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var Auto = (AutoSuggestBox)sender;
            List<string> Suggestion;
            Suggestion = Peoplenames.Where(p => p != "snehith").ToList();
            Auto.ItemsSource = Suggestion.ToArray();
        }
        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void Calendar_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            dateSet = 1;
        }

        private void PeopleRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            peopleAdded = "";
            RaisePropertyChanged(nameof(peopleIncludedforOneRemainder));
            TemperoryList.Clear();
            PeopleRemoveButton.Visibility = Visibility.Collapsed;

        }

        private void MyAutoSuggestBox1_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            int value = (int)sender.Tag;
            
            foreach(var i in RemaindersForMe.ToList())
            {
                if(i.RemainderId==value)
                {
                    foreach(var j in Persons)
                    {
                        if(j.name==args.SelectedItem.ToString())
                        {
                            i.zuids.Add(j.zuid);
                            break;
                        }
                    }
                    break;
                }
            }

            foreach(var i in RemaindersForMe.ToList())
            {
                if(i.RemainderId==value)
                { 
                    if(i.dateandtime.ToString().Count()>0)
                    RemaindersForMe.Add(new Remainder(i.Description, i.DateAndTime, i.isCompleted, i.zuids, i.channelId, 1, remainderId+1));
                    else
                        RemaindersForMe.Add(new Remainder(i.Description, i.DateAndTime, i.isCompleted, i.zuids, i.channelId, 0, remainderId + 1));
                    remainderId += 1;
                    RemaindersForMe.Remove(i);
                }
            }
        }
    }
}


































/*foreach (var i in RemaindersForMe)
{
    if (i.RemainderId == value)
    {
        foreach (var j in i.zuids)
        {
            foreach (var k in Persons)
            {
                if (k.zuid == j)
                {
                    Testing.Text += k.name;
                }
            }
        }
        break;
    }
}*/