using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Remainders;

namespace Remainders
{
    public class Remainder
    {
        public int RemainderId;
        public string Description;
        public DateTime DateAndTime;
        public string dateandtime;
        public bool isCompleted;
        public ObservableCollection<string> zuids;
        public string channelId;

        public MainPage m = new MainPage();
        Dictionary<string, string> MapOfIdAndName = new Dictionary<string, string>();
        public Remainder(string description, DateTime dateandtime, bool k,ObservableCollection<string>z,string channelId,int dateSet,int remainderId)
        {
            this.RemainderId = remainderId;
            this.Description = description;
            this.DateAndTime = dateandtime;
            this.isCompleted = k;
            this.zuids = new ObservableCollection<string>(z);
            this.channelId = channelId;
            if (dateSet == 1)
                this.dateandtime = dateandtime.ToString();
            else
                this.dateandtime = "";
        }
        public string subtract1(int z)
        {
            return (z - 1).ToString();
        }
        public string getPersonsList(ObservableCollection<string> personZuids)
        {
            if (MapOfIdAndName.Count == 0)
            {
                foreach (var i in m.Persons)
                {
                    MapOfIdAndName.Add(i.zuid, i.name);
                }
            }
            string res = "";
            foreach(var i in personZuids.ToHashSet())
            {
                if (i == "1")
                    continue;
                if (res == "")
                    res += MapOfIdAndName[i];
                else
                    res += " and " + MapOfIdAndName[i];
            }
            return res;
        }

        public Windows.UI.Xaml.Visibility CheckWhetherAssignedOrNot(ObservableCollection<string>zuids)
        {
            if (zuids.Count >1)
                return Windows.UI.Xaml.Visibility.Visible;
            else
                return Windows.UI.Xaml.Visibility.Collapsed;
        }
        public Windows.UI.Xaml.Visibility isDateAndTimeSet(string DateAndTime)
        {
            if(DateAndTime.Length!=0)
                return Windows.UI.Xaml.Visibility.Visible;
            else
                return Windows.UI.Xaml.Visibility.Collapsed;
        }

    }
}
