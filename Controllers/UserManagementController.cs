using FireSharp.Response;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShadiMubarak.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static ShadiMubarak.Models.ViewModel.AccountViewModel;

namespace ShadiMubarak.Controllers
{
    public class UserManagementController : BaseController
    {
        // GET: UserManagement
        // GET: UserManagement
        public ActionResult Index()
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
                    FirebaseResponse response = client.Get("Admin");
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var userlist = new List<Admin>();
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            userlist.Add(JsonConvert.DeserializeObject<Admin>(((JProperty)item).Value.ToString()));
                        }
                    }

                    return View(userlist);
                }
                catch (Exception e)
                {
                    Response.Write("<script>alert('Failed to load Data!')</script>");
                    return RedirectToAction("Home", "Index");
                }
            }
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            try { 
            FirebaseResponse response = client.Get("Admin/" + id);
            Admin user = JsonConvert.DeserializeObject<Admin>(response.Body);
            ManageUserViewModel userViewModel = new ManageUserViewModel();

            if (user != null)
            {
                userViewModel.UserName = user.Email;
                userViewModel.Id = user.Id;
                userViewModel.FirstName = user.FirstName;
                userViewModel.LastName = user.LastName;
                userViewModel.PhoneNumber = user.PhoneNumber;
            }

            return View(userViewModel);
            }
            catch (Exception e)
            {
                Response.Write("<script>alert('Failed to load Data!')</script>");
                return RedirectToAction("Home", "Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ManageUserViewModel user)
        {
            try { 
            FirebaseResponse response = client.Get("Admin/" + user.Id);
            Admin dbUser = JsonConvert.DeserializeObject<Admin>(response.Body);

            dbUser.Email = user.UserName;
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.PhoneNumber = user.PhoneNumber;

            if (user != null && user.ImageFile != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(user.ImageFile.FileName);
                string extension = Path.GetExtension(user.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                dbUser.Image = "/images/admins/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/images/admins"), fileName);
                user.ImageFile.SaveAs(fileName);
            }

            if (!string.IsNullOrEmpty(user.NewPassword) && !string.IsNullOrEmpty(user.ConfirmPassword) && !string.IsNullOrEmpty(user.OldPassword))
            {

                dbUser.Password = user.NewPassword;
                //await client.(user.UserName, user.OldPassword, user.NewPassword);
            }
            
            await client.UpdateAsync("Admin/" + user.Id, dbUser);
            //db.Entry(dbUser).State = EntityState.Modified;
            //db.SaveChanges();

            return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Response.Write("<script>alert('Failed to load Data!')</script>");
                return RedirectToAction("Home", "Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditUserProfile(ManageUserViewModel user)
        {
            try { 
            var response = client.Get("Admin/" + user.Id);
            Admin dbUser = JsonConvert.DeserializeObject<Admin>(response.Body);
            if (dbUser != null)
            {
                dbUser.Email = user.UserName;
                dbUser.FirstName = user.FirstName;
                dbUser.LastName = user.LastName;
                dbUser.PhoneNumber = user.PhoneNumber;

                if (!string.IsNullOrEmpty(user.NewPassword) && !string.IsNullOrEmpty(user.ConfirmPassword) && !string.IsNullOrEmpty(user.OldPassword))
                {
                    var db = client.ChangePassword(user.UserName, user.OldPassword, user.NewPassword);
                }

                if (user != null && user.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(user.ImageFile.FileName);
                    string extension = Path.GetExtension(user.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    dbUser.Image = "/images/admins/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/images/admins"), fileName);
                    user.ImageFile.SaveAs(fileName);
                }

                await client.UpdateAsync("Admin/" + user.Id, dbUser);
            }

            return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception e)
            {
                Response.Write("<script>alert('Failed to load Data!')</script>");
                return RedirectToAction("Home", "Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            try { 
            var response = client.Get("Admin/" + id);
            Admin user = JsonConvert.DeserializeObject<Admin>(response.Body);
            user.IsDeleted = true;
            client.Update("Admin/" + id, user);
            var task = Task.Run(() => Delete(id));
            if (task.Wait(TimeSpan.FromSeconds(1)))
                return task.Result;
            return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Response.Write("<script>alert('Failed to load Data!')</script>");
                return RedirectToAction("Index");
            }
        }
    }
}