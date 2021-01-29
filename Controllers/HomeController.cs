using Google.Cloud.Firestore;
using ShadiMubarak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Windows;

namespace ShadiMubarak.Controllers
{
    public class HomeController : BaseController
    {
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                try
                {
                    SetUserDataAsync();
                    Query qry = db.Collection("vendors");

                    QuerySnapshot qrysnp = await qry.GetSnapshotAsync();
                    Query qry1 = db.Collection("vendors").WhereEqualTo("isApproved", true);
                    QuerySnapshot qrysnp1 = await qry1.GetSnapshotAsync();
                    Query qry2 = db.Collection("vendors").WhereEqualTo("category", "Photographer");
                    QuerySnapshot qrysnp2 = await qry2.GetSnapshotAsync();
                    Query qry3 = db.Collection("users").WhereEqualTo("isVendor", false);
                    QuerySnapshot qrysnp3 = await qry3.GetSnapshotAsync();
                    Query qry4 = db.Collection("vendors").WhereEqualTo("category", "Venues");
                    QuerySnapshot qrysnp4 = await qry4.GetSnapshotAsync();
                    Query qry5 = db.Collection("vendors").WhereEqualTo("category", "Caterers");
                    QuerySnapshot qrysnp5 = await qry5.GetSnapshotAsync();
                    Query qry6 = db.Collection("vendors").WhereEqualTo("category", "Decorator");
                    QuerySnapshot qrysnp6 = await qry6.GetSnapshotAsync();
                    Query qry7 = db.Collection("vendors").WhereEqualTo("category", "Makeup");
                    QuerySnapshot qrysnp7 = await qry7.GetSnapshotAsync();
                    Query qry8 = db.Collection("vendors").WhereEqualTo("category", "Mehndi");
                    QuerySnapshot qrysnp8 = await qry8.GetSnapshotAsync();
                    Query qry9 = db.Collection("vendors").WhereEqualTo("category", "Event Organizer");
                    QuerySnapshot qrysnp9 = await qry9.GetSnapshotAsync();
                    var vendorlist = new List<Vendor>();
                    if (qrysnp != null)
                    {
                        foreach (DocumentSnapshot item in qrysnp)
                        {
                            Vendor ven = item.ConvertTo<Vendor>();
                            ven.resId = item.Id;
                            vendorlist.Add(ven);
                        }
                    }

                    ViewBag.TotalVendors = qrysnp.Count();
                    ViewBag.ApprovedVendors = qrysnp1.Count();
                    ViewBag.PhotographerVendors = qrysnp2.Count();
                    ViewBag.Customers = qrysnp3.Count();
                    ViewBag.VenuesVendors = qrysnp4.Count();
                    ViewBag.DecoratorVendors = qrysnp6.Count();
                    ViewBag.CaterorsVendors = qrysnp5.Count();
                    ViewBag.MehndiVendors = qrysnp8.Count();
                    ViewBag.MakeupVendors = qrysnp7.Count();
                    ViewBag.EventOrganizers = qrysnp9.Count();

                    return View();
                }
                catch (Exception e)
                {
                    Response.Write("<script>alert('Hello');</script>");
                }

                return RedirectToAction("SignIn", "Account");
            }
        }

        //public ActionResult GetUserNotification()
        //{
        //    var userId = Session["UserID"];
        //    var user = db.Set<ApplicationUser>().Where(u => u.Id == userId).FirstOrDefault();
        //    var model1 = db.UserNotifications.Where(x => x.Title == "Meeting").ToList();
        //    foreach (var a in model1)
        //    {
        //        if (a.Date < DateTime.Now)
        //        {
        //            a.IsDeleted = false;
        //        }
        //        db.SaveChanges();
        //    }
        //    db.SaveChanges();
        //    var empId = user.employee_id.ToString();
        //    var model = db.UserNotifications.Where(x => x.IsDeleted == false && x.SentTo == empId).OrderByDescending(d => d.Date).ToList();
        //    ViewBag.UserNotification = model;
        //    return Json(new { Model = model }, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult ChangeNotificationStatus(int NotificationId)
        //{
        //    var Notification = db.UserNotifications.Where(x => x.Id == NotificationId).FirstOrDefault();
        //    Notification.IsRead = true;
        //    db.SaveChanges();
        //    return Json(new { }, JsonRequestBehavior.AllowGet);
        //}
    }
}