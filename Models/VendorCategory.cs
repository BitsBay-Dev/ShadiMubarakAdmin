using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShadiMubarak.Models
{
    [FirestoreData]
    public class VendorCategory
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string images { get; set; }

        [FirestoreProperty]
        public string name { get; set; }
        [FirestoreProperty]
        public DateTime time { get; set; }

    }
}