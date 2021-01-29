using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShadiMubarak.Models
{
    [FirestoreData]
    public class Booking
    {
        [FirestoreProperty]
        public string id { get; set; }
        [FirestoreProperty]
        public string customerEmail { get; set; }
        [FirestoreProperty]
        public string customerId { get; set; }
        [FirestoreProperty]
        public DateTime bookingDate { get; set; }
        [FirestoreProperty]
        public string customerName { get; set; }
        [FirestoreProperty]
        public DateTime eventDate { get; set; }
        [FirestoreProperty]
        public string vendorId { get; set; }
        [FirestoreProperty]
        public string vendorName { get; set; }
        [FirestoreProperty]
        public bool isApproved { get; set; }
    }
}