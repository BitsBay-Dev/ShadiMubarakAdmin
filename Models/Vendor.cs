using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShadiMubarak.Models
{
    [FirestoreData]
    public class Vendor
    {
        [FirestoreProperty]
        public string resId { get; set; }
        [FirestoreProperty] 
        public string name { get; set; }
        [FirestoreProperty] 
        public string email { get; set; }
        [FirestoreProperty] 
        public string address { get; set; }
        [FirestoreProperty] 
        public string contact { get; set; }
        [FirestoreProperty] 
        public string title { get; set; }
        [FirestoreProperty] 
        public string description { get; set; }
        [FirestoreProperty] 
        public string city { get; set; }
        [FirestoreProperty] 
        public string price { get; set; }
        [FirestoreProperty] 
        public string postal_code { get; set; }
        [FirestoreProperty] 
        public string images { get; set; }
        [FirestoreProperty] 
        public bool isApproved { get; set; }
        [FirestoreProperty] 
        public string country { get; set; }
        [FirestoreProperty] 
        public DateTime subStartDate { get; set; }
        [FirestoreProperty] 
        public DateTime subEndDate { get; set; }
        [FirestoreProperty] 
        public string category { get; set; }
        [FirestoreProperty] 
        public DateTime time { get; set; } 
        [FirestoreProperty] 
        public bool hasPaid { get; set; }
        [FirestoreProperty] 
        public string isRecommended { get; set; }
        [FirestoreProperty] 
        public string avg { get; set; }

    }
}