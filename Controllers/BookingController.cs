using Google.Cloud.Firestore;
using ShadiMubarak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShadiMubarak.Controllers
{
    public class BookingController : BaseController
    {
        // GET: Booking
        public async Task<ActionResult> Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                try { 
                SetUserDataAsync();
                Query qry = db.Collection("bookings");
                QuerySnapshot qrysnp = await qry.GetSnapshotAsync();
                var bookingslist = new List<Booking>();
                if (qrysnp != null)
                {
                    foreach (DocumentSnapshot item in qrysnp)
                    {
                        Booking bok = item.ConvertTo<Booking>();
                        bok.id = item.Id;
                        bookingslist.Add(bok);
                    }
                }
                return View(bookingslist);
                }
                catch (Exception e)
                {
                    Response.Write("<script>alert('Failed to load Data!')</script>");
                    return RedirectToAction("Index", "Home");
                }
            }
        }
    }
}