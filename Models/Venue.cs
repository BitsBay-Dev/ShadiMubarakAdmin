using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShadiMubarak.Models
{
    public class Venue
    {

        public Venue()
        {
            this.Vendor = new HashSet<Vendor>();
        }
        public string Id { get; set; }
        public string name { get; set; }

        public virtual ICollection<Vendor> Vendor { get; set; }
    }
}