using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShadiMubarak.Models
{
    [FirestoreData]
    public class category
    {
        [FirestoreProperty]
        public string label { get; set; }
        [FirestoreProperty] 
        public int value { get; set; }
    }
}