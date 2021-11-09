using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Remainders;

namespace Remainders
{
    public class Remainder : INotifyPropertyChanged
    {
        public int RemainderId;
        private string _description;
        private DateTime _dateAndTime;
        private string _dateAndTimeStr;
        private bool _isCompleted;
        //private ObservableCollection<string> _zuids;
        private string _channelId;
        private ObservableCollection<Person> people;


        public string Description
        {
            get { return _description; }
            set { _description = value; RaisePropertyChanged(); }
        }

        public DateTime DateAndTime
        {
            get { return _dateAndTime; }
            set { _dateAndTime = value; RaisePropertyChanged(); }
        }


        public string DateAndTimeStr
        {
            get { return _dateAndTimeStr; }
            set { _dateAndTimeStr = value; RaisePropertyChanged(); }
        }

        public bool IsCompleted
        {
            get { return _isCompleted; }
            set { _isCompleted = value; RaisePropertyChanged(); }
        }

        //public ObservableCollection<string> ZuIds
        //{
        //    get { return _zuids; }
        //    set { _zuids = value; RaisePropertyChanged(); }
        //}

        public string ChannelId
        {
            get { return _channelId; }
            set { _channelId = value; RaisePropertyChanged(); }
        }

        Dictionary<string, string> MapOfIdAndName = new Dictionary<string, string>();

        public ObservableCollection<Person> People
        {
            get { return people; }
            set { people = value; RaisePropertyChanged(); }
        }


        public Remainder(string description, DateTime dateandtime, bool k, string channelId, int dateSet, int remainderId)
        {
            People = new ObservableCollection<Person>();
            this.RemainderId = remainderId;
            this.Description = description;
            this.DateAndTime = dateandtime;
            this.IsCompleted = k;
            // this.ZuIds = z;
            this.ChannelId = channelId;
            if (dateSet == 1)
                this.DateAndTimeStr = dateandtime.ToString();
            else
                this.DateAndTimeStr = "";
        }
        public string subtract1(int z)
        {
            return (z - 1).ToString();
        }

        //public string getPersonsList(ObservableCollection<string> personZuids)
        //{
        //    if (MapOfIdAndName.Count == 0)
        //    {
        //        foreach (var i in m.Persons)
        //        {
        //            MapOfIdAndName.Add(i.zuid, i.name);
        //        }
        //    }
        //    string res = "";
        //    foreach(var i in personZuids.ToHashSet())
        //    {
        //        if (i == "1")
        //            continue;
        //        if (res == "")
        //            res += MapOfIdAndName[i];
        //        else
        //            res += " and " + MapOfIdAndName[i];
        //    }
        //    return res;
        //}





        public Windows.UI.Xaml.Visibility CheckWhetherAssignedOrNot(ObservableCollection<Person> zuids)
        {
            if (zuids.Count > 0)
                return Windows.UI.Xaml.Visibility.Visible;
            else
                return Windows.UI.Xaml.Visibility.Collapsed;
        }
        public Windows.UI.Xaml.Visibility isDateAndTimeSet(string dt)
        {
            if (dt.Length != 0)
                return Windows.UI.Xaml.Visibility.Visible;
            else
                return Windows.UI.Xaml.Visibility.Collapsed;
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
