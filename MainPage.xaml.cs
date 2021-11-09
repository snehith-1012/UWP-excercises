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
using System.Runtime.CompilerServices;

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
        public ObservableCollection<Remainder> Remainders = new ObservableCollection<Remainder>();
        

        public List<Person> TemperorySelectedPersonList = new List<Person>();
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
                peopleAdded = value;
                RaisePropertyChanged();
            }
        }

        public int channelId = -1;
        public string UIPersonId = "1";
        public int dateSet = 0;

        public ObservableCollection<Remainder> CompletedRemainders = new ObservableCollection<Remainder>();
        DateTime RemainderDateAndTime = DateTime.Today;



        public MainPage()
        {
            this.InitializeComponent();
            Persons.Add(new Person("1", "snehith", "snehith.oddula@zohocorp.com"));
            Persons.Add(new Person("2", "someone", "someone@zohocorp.com"));
            Persons.Add(new Person("3", "someone2", "someone2@zohocorp.com"));
            Persons.Add(new Person("4", "someone3", "someone3@zohocorp.com"));
            Persons.Add(new Person("'5", "someone4", "someone4@zohocorp.com"));

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
            List<Person> Suggestion = new List<Person>()
            if (string.IsNullOrEmpty(Auto.Text))
                Suggestion = Persons.Where(p => p.Name!="snehith").ToList();
            else
                Suggestion = Persons.Where(p => p.Name.StartsWith(Auto.Text, StringComparison.OrdinalIgnoreCase) && p.Name != "snehith").ToList();
            Auto.ItemsSource = Suggestion;
        }

        private void MyAutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Person selectedPerson = (Person)args.SelectedItem;
            MyAutoSuggestBox.Text = selectedPerson.Name;


            foreach (var i in Persons)
            {
                if (i.Name == selectedPerson.Name)
                {
                    int duplicate = 0;
                    foreach (var j in TemperorySelectedPersonList)
                    {
                        if (j.ZuId == i.ZuId)
                        {
                            duplicate = 1;
                            break;
                        }
                    }
                    if (duplicate == 0)
                    {
                        TemperorySelectedPersonList.Add(i);
                        if (peopleAdded != "")
                            peopleAdded += " and " + selectedPerson.Name;
                        else
                            peopleAdded += selectedPerson.Name;
                        RaisePropertyChanged(nameof(peopleIncludedforOneRemainder));
                        break;
                    }
                }
            }

            if (TemperorySelectedPersonList.Count > 0)
            {
                PeopleRemoveButton.Visibility = Visibility.Visible;
            }
            else
            {
                PeopleRemoveButton.Visibility = Visibility.Collapsed;
            }

            foreach (var i in channelsList)
            {
                if (i.ChannelName == selectedPerson.Name)
                {
                    channelId = i.ChannelId;
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


            var newRemainder = new Remainder(RemainderDescription.Text, RemainderDateAndTime, false, null, dateSet, remainderId + 1);

            foreach (var item in TemperorySelectedPersonList)
            {
                newRemainder.People.Add(item);
            }

            Remainders.Add(newRemainder);
            RemaindersForMe.Add(newRemainder);
            remainderId += 1;
            RemainderDescription.Text = "";
            DateAndTimeDisplay.Text = "";

            

            TemperorySelectedPersonList.Clear();
            peopleAdded = "";
            peopleIncludedforOneRemainder = "";
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
            foreach (var i in Remainders)
            {
                if (i.RemainderId == myValue)
                {
                    RemaindersForMe.Remove(i);
                    Remainders.Remove(i);
                    break;
                }
            }
        }

        private void MarkAsComplete_Click(object sender, RoutedEventArgs e)
        {
            int myValue = (int)(((Button)sender).Tag);

            foreach (var i in Remainders)
            {
                if (i.RemainderId == myValue)
                {
                    CompletedRemainders.Add(i);
                    RemaindersForMe.Remove(i);
                    Remainders.Remove(i);
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
            List<Person> Suggestion = new List<Person>();
            Suggestion = Persons.Where(p => p.Name != "snehith").ToList();
            Auto.ItemsSource = Suggestion.ToArray();
        }



        private void Calendar_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            dateSet = 1;
        }

        private void PeopleRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            peopleAdded = "";
            RaisePropertyChanged(nameof(peopleIncludedforOneRemainder));
            TemperorySelectedPersonList.Clear();
            PeopleRemoveButton.Visibility = Visibility.Collapsed;

        }

        private void MyAutoSuggestBox1_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            int remainderId = (int)sender.Tag;
            Person selectedPerson = (Person)args.SelectedItem;
            
            foreach (var i in RemaindersForMe)
            {
                if(i.RemainderId==remainderId)
                {
                    var already_present = 0;
                    foreach(var j in i.People)
                    {
                        if(j.ZuId==selectedPerson.ZuId)
                        {
                            already_present = 1;
                            break ;
                        }
                    }
                    if (already_present == 0)
                        i.People.Add(selectedPerson);
                    break;
                }
            }
            sender.Text ="" ;
        }



        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}


































/*foreach (var i in RemaindersForMe)
{
    if (i.RemainderId == value)
    {
        foreach (var j in i.zuids)
        {//foreach(var i in RemaindersForMe.ToList())
            //{
            //    if(i.RemainderId==value)
            //    { 
            //        if(i.dateandtime.ToString().Count()>0)
            //        RemaindersForMe.Add(new Remainder(i.Description, i.DateAndTime, i.isCompleted, i.zuids, i.channelId, 1, remainderId+1));
            //        else
            //            RemaindersForMe.Add(new Remainder(i.Description, i.DateAndTime, i.isCompleted, i.zuids, i.channelId, 0, remainderId + 1));
            //        remainderId += 1;
            //        RemaindersForMe.Remove(i);
            //    }
            //}
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




//foreach(var i in RemaindersForMe.ToList())
//{
//    if(i.RemainderId==value)
//    {
//        foreach(var j in Persons)
//        {
//            if(j.name==args.SelectedItem.ToString())
//            {
//                i.zuids.Add(j.zuid);
//                break;
//            }
//        }
//        break;
//    }
//}


/*Remainder selectedRemainder = Remainders.FirstOrDefault(a => a.RemainderId == remainderId);
            if (selectedRemainder.People.Where(a => a.ZuId == selectedPerson.ZuId).ToList().Count == 0)
            {
                selectedRemainder.People.Add(selectedPerson);
                RaisePropertyChanged();
            }*/

//foreach (var i in TemperorySelectedPersonList)
//{
//    if (i == UIPersonId)
//        RemaindersForMe.Add(Remainders[Remainders.Count - 1]);
//}



//public HashSet<string> Peoplenames = new HashSet<string>();
//public List<string> TemperoryList = new List<string>();