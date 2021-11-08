using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Remainders;

namespace Remainders
{
    public class Person 
    {
        public string zuid;
        public string name,email;
        public Remainder r { get; set; }
        public Person(string id,string name,string email)
        {
            this.zuid = id;
            this.name = name;
            this.email = email;
        }


        /*public string PeopleIncluded()
        {
            string ans = "";
            foreach(var i in r.m.peopleAdded)
            {
                ans += i.name;
            }
            //foreach (var i in m.peopleAdded)
            //    ans += i;
            return ans;
        }*/
    }
}
