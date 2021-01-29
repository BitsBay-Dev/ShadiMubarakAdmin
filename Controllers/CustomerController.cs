using FireSharp.Response;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShadiMubarak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShadiMubarak.Controllers
{
    public class CustomerController : BaseController
    {
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
                Query qry = db.Collection("users").WhereEqualTo("isVendor", false);
                QuerySnapshot qrysnp = await qry.GetSnapshotAsync();
                var customerslist = new List<Customer>();
                if (qrysnp != null)
                {
                    foreach (DocumentSnapshot item in qrysnp)
                    {
                        Customer cus = item.ConvertTo<Customer>();
                        cus.id = item.Id;
                        customerslist.Add(cus);
                    }
                }
                return View(customerslist);
                }
                catch (Exception e)
                {
                    Response.Write("<script>alert('Failed to load Data!')</script>");
                    return RedirectToAction("Home", "Index");
                }
            }
        }
        [Route("Customer/Details")]
        public async Task<ActionResult> Details(string id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                try { 
                SetUserDataAsync();
                DocumentReference qry = db.Collection("users").Document(id);
                DocumentSnapshot qrysnp = await qry.GetSnapshotAsync();
                Customer cus;
                cus = qrysnp.ConvertTo<Customer>();

                return View(cus);
            }
                catch (Exception e)
            {
                Response.Write("<script>alert('Failed to load Data!')</script>");
                return RedirectToAction("Customer", "Index");
            }
        }
        }


    }
}