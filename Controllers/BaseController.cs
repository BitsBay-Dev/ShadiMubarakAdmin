using FireSharp.Config;
using Google.Cloud.Firestore;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShadiMubarak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Google.Cloud.Firestore.V1;
using Grpc.Core;
using Google.Apis.Auth.OAuth2;
using System.IO;
using Firebase.Auth;
using System.Threading.Tasks;
using System.Threading;
using Firebase.Storage;

namespace ShadiMubarak.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            try
            {
                client = new FireSharp.FirebaseClient(Config);
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
                db = FirestoreDb.Create("shadimubarak-61495");
                //    client1 = new Firestore.FirestoreClient(Config1);

                //    var builder = new FirestoreClientBuilder { JsonCredentials =  {
                //    apiKey = "AIzaSyDzmC2UmoDWMOwSWgSIJ-Azr-6Mx1ZugHo",
                //    authDomain = "shadimubarak-61495.firebaseapp.com",
                //    databaseURL = "https://shadimubarak-61495.firebaseio.com",
                //    projectId = "shadimubarak-61495",
                //    storageBucket = "shadimubarak-61495.appspot.com",
                //    messagingSenderId = "388208080065",
                //    appId = "1:388208080065:web:f5a29c883bbe9d0e89d040",
                //    measurementId = "G-42QEPH4SL9"
                //}
                ////};
                //    db = FirestoreDb.Create("shadimubarak-61495");
            }
            catch(Exception e) { 
            }

        }

        string path = AppDomain.CurrentDomain.BaseDirectory + @"shadimubarak-61495-firebase-adminsdk-agdx4-2298288e65.json";
        public FirestoreDb db;
        public  IFirebaseClient client;
        //public FirestoreClient client1;
        IFirebaseConfig Config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "WKa1miNgfHOs0GfzClV5ExMIJP1ovuphC30lfQUx",
            BasePath = "https://shadimubarak-61495.firebaseio.com/"
        };




    // GET: Base
    public async Task SetUserDataAsync()
        {
            try
            {
                var baseUrl = WebConfigurationManager.AppSettings["BaseUrl"];

                //string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                string id = (string)Session["UserID"];
                Admin admin = null;
                FirebaseResponse dbUser = client.Get("Admin");
                dynamic mList = JsonConvert.DeserializeObject<dynamic>(dbUser.Body);
                var adminlist = new List<Admin>();
                //foreach (var item in mList)
                //{
                //    adminlist.Add(JsonConvert.DeserializeObject<Admin>(((JProperty)item).Value.ToString()));
                //}
                foreach (var item in mList)
                {
                    var a = JsonConvert.DeserializeObject<Admin>(((JProperty)item).Value.ToString());
                    if (a.Id == id)
                    {
                        admin = a;
                    }
                }
                Query qry1 = db.Collection("vendors").WhereEqualTo("isApproved", false);
                QuerySnapshot qrysnp1 = await qry1.GetSnapshotAsync();
                ViewBag.PendingVendors = qrysnp1.Count();
                Query qry2 = db.Collection("bookings");
                QuerySnapshot qrysnp2 = await qry2.GetSnapshotAsync();
                ViewBag.Bookings = qrysnp2.Count();
                ViewBag.tbl_admin = admin;
                ViewBag.currentUser = admin;
                ViewBag.user_image = admin.Image;
            }
            catch (Exception e)
            {

            }
        }

        public async Task<string> UploadFile(FileStream stream, string FileName) 
        {
            var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(WebConfigurationManager.AppSettings["apiKey"]));
            var a = await auth.SignInWithEmailAndPasswordAsync(WebConfigurationManager.AppSettings["AuthEmail"], WebConfigurationManager.AppSettings["AuthPassword"]);

            var cancellation = new CancellationTokenSource();
            var task = new FirebaseStorage(
               WebConfigurationManager.AppSettings["Bucket"] ,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
            })
                .Child("images")
                .Child(FileName)
                .PutAsync(stream, cancellation.Token);

            // cancel the upload
            // cancellation.Cancel();

            try
            {
                // error during upload will be thrown when you await the task
                string link = await task;
                return link;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception was thrown: {0}",ex);
                return null;
            }
        }
        public string GetUserId()
        {
            string id = (string)Session["UserID"];
            //var user = db.Set<ApplicationUser>().Where(u => u.Id == userId).FirstOrDefault();
            //return user.employee_id ?? 0;
            return id ?? null;
        }
    }
}