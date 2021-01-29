using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShadiMubarak.Models;
using System.Web.Mvc;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using System.Web.Configuration;
using System.Net.Mail;
using System.Net;

namespace ShadiMubarak.Controllers
{
    public class VendorController : BaseController
    {

        // GET: Vendor
        public async Task<ActionResult> Index()
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
                    return View(vendorlist);
                }
                catch(Exception e) {
                    Response.Write("<script>alert('Failed to load Data!')</script>");
                    return RedirectToAction("Home","Index");
                }
                
            }
        } 
        [Route("Vendor/Details")]
        public async Task<ActionResult> Details(string id)
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
                    DocumentReference qry = db.Collection("vendors").Document(id);
                    DocumentSnapshot qrysnp = await qry.GetSnapshotAsync();
                    Vendor ven = qrysnp.ConvertTo<Vendor>();
                    ven.resId = id;
                    return View(ven);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        public async Task<ActionResult> Create(Vendor vendor)
        {
            try
            {
                await AddVendorToFirebaseAsync(vendor);
                ModelState.AddModelError(string.Empty,"Added Successfully!");

            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty,"Added Successfully!");
            }
            return RedirectToAction("Index");
        }

         public async Task<ActionResult> Edit(Vendor vendor)
       {
            try { 
            DocumentReference qry = db.Collection("vendors").Document(vendor.resId);
            Dictionary<string, object> updates = new Dictionary<string, object>
            { 
                { "name", vendor.name },
                { "address", vendor.address },
                { "contact", vendor.contact },
                { "title", vendor.title },
                { "city", vendor.city },
                { "postal_code", vendor.postal_code },
                { "category", vendor.category },
                { "country", vendor.country },
                { "isRecommended", vendor.isRecommended },
                { "subEndDate", vendor.subEndDate.ToUniversalTime() },
                { "subStartDate", vendor.subStartDate.ToUniversalTime() }
            };
            await qry.UpdateAsync(updates);

            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
           public async Task<ActionResult> Delete(string id)
        {
            try { 
            DocumentReference qry = db.Collection("vendors").Document(id);
            DocumentSnapshot docsnp = await qry.GetSnapshotAsync();
            await qry.DeleteAsync();
            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        public async Task AddVendorToFirebaseAsync(Vendor vendor)
        {
            try { 
            CollectionReference collection = db.Collection("vendors");
            DocumentReference document = await collection.AddAsync(new 
            {   name = vendor.name, 
                email = vendor.email, 
                address = vendor.address, 
                contact = vendor.contact,
                title =vendor.title,
                city = vendor.city,
                postal_code = vendor.postal_code,
                category =  vendor.category

            });
            }
            catch (Exception ex)
            {
            }
        }
        [HttpPost]
        public async Task<ActionResult> PaidPaymentUserModel(string user_id)
        {
            try { 
            DocumentReference dr = db.Collection("vendors").Document(user_id);
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "hasPaid", true }
            };
            await dr.UpdateAsync(updates);
            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> UnpaidPaymentUserModel(string user_id)
        {
            try { 
            DocumentReference dr = db.Collection("vendors").Document(user_id);
            await dr.UpdateAsync("hasPaid", false);

            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<ActionResult> ActivateUser(string user_id,DateTime subStartDate,DateTime subEndDate)
        {
            try { 
            DocumentReference dr = db.Collection("vendors").Document(user_id);
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "isApproved", true },
                { "subEndDate", subEndDate.ToUniversalTime() },
                { "subStartDate", subStartDate.ToUniversalTime() }
            };
            await dr.UpdateAsync(updates);
            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeactivateUser(string user_id)
        {
            try { 
            DocumentReference dr = db.Collection("vendors").Document(user_id);
            await dr.UpdateAsync("isApproved", false);

            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult SendPaymentReminderEmail(string email)
        {
            try
            {
               // var user = UserManager.FindByEmail(email);
                if (email != null)
                {
                   // string token = UserManager.GenerateEmailConfirmationToken(user.Id);
                   // string codeHtmlVersion = HttpUtility.UrlEncode(token);

                    string message = "Hi, Its a reminder for the payment from ShadiMubarak to continue your subscription! Thanks";
                    SendMailFunc("Payment Reminder", email, message, "");
                }
            }
            catch (Exception ex)
            {
            }
            return RedirectToAction("Index");
        }
        private string SendMailFunc(string subject, string recepient, string message, string filePath)
        {
            try
            {
                string From_Mail = WebConfigurationManager.AppSettings["SMEmail"];
                string From_Password = WebConfigurationManager.AppSettings["SMPassword"];
                MailAddress fromAddress = new MailAddress(From_Mail);
                MailAddress toAddress = new MailAddress(recepient);
                MailMessage mail = new MailMessage(fromAddress.Address, toAddress.Address)
                {
                    Subject = subject,
                    Body = message
                };

                if (filePath != "")
                {
                    Attachment attachment = new Attachment(filePath);
                    mail.Attachments.Add(attachment);
                }

                SmtpClient client = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Timeout = 50000,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(From_Mail, From_Password)
                };

                client.Send(mail);
                return "Mail sent";
            }
            catch (Exception ex)
            {

            }
            return "Error occured";
        }
        //private void collectPhoneNumbers(Map<String, Object> users)
        //{

        //    List<string> phoneNumbers = new List<string>();

        //    //iterate through each user, ignoring their UID
        //    for (Map.Entry<String, Object> entry : users.entrySet())
        //    {

        //        //Get user map
        //        Map singleUser = (Map)entry.getValue();
        //        //Get phone field and append to list
        //        phoneNumbers.add((Long)singleUser.get("phone"));
        //    }

        //    System.out.println(phoneNumbers.toString());
        //}

    }
}