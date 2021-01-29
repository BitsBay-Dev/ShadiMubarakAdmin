using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShadiMubarak.Models
{
    [FirestoreData]
    public class Customer
    {
        [FirestoreProperty]
        public string id { get; set; }
        [FirestoreProperty]
        public string name { get; set; }
        [FirestoreProperty]
        public string email { get; set; }
        [FirestoreProperty]
        public string contact { get; set; }
        [FirestoreProperty]
        public string city { get; set; }
        [FirestoreProperty]
        public string country { get; set; }
        [FirestoreProperty]
        public string image { get; set; }
        [FirestoreProperty]
        public bool isVendor { get; set; }

    }
}