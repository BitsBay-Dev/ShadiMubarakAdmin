using FireSharp;
using FireSharp.Response;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShadiMubarak.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using static ShadiMubarak.Models.ViewModel.AccountViewModel;

namespace ShadiMubarak.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Admin model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    FirebaseResponse response = client.Get("Admin");
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body); bool auth = false;
                    foreach (var item in data)
                    {
                        var a = JsonConvert.DeserializeObject<Admin>(((JProperty)item).Value.ToString());
                        if (a.Email == model.Email && a.Password == model.Password)
                        {
                            auth = true;
                            Session["UserID"] = a.Id;
                            Session["UserType"] = "Admin";
                        }
                    }
                    if (auth)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.error = "Unable to Connect to the Server! Sorry for Inconvinience, Please Come Back Later!";
                    return View("SignIn");
                }
            }

            ViewBag.error = "Incorrect username or passoword";

            // If we got this far, something failed, redisplay form
            return View("SignIn");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            try
            {
                //QueryBuilder queryBuilder;
                FirebaseResponse response = client.Get("Admin");
                Admin admin = null;
                string docId = null;
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                foreach (var item in data)
                {
                    var a = JsonConvert.DeserializeObject<Admin>(((JProperty)item).Value.ToString());
                    if (a.Email == model.UserName)
                    {
                        admin = a;
                        docId = a.Id;
                    }
                }

                //Admin data = JsonConvert.DeserializeObject<Admin>(dbUser.Body);
                if (admin == null)
                {
                    var user = new Admin()
                    {
                        Email = model.UserName,
                        FirstName = model.firstname,
                        LastName = model.lastname,
                        PhoneNumber = model.PhoneNumber,
                        Password = model.Password

                    };

                    if (model != null && model.ImageFile != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                        string extension = Path.GetExtension(model.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        user.Image = "/images/admins/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/images/admins"), fileName);
                        model.ImageFile.SaveAs(fileName);
                    }


                    //set the IsDeleted property to false
                    user.IsDeleted = false;

                    PushResponse Response = client.Push("Admin/", user);
                    user.Id = Response.Result.name;
                    SetResponse setResponse = client.Set("Admin/" + user.Id, user);
                }
                else
                {
                    admin.FirstName = model.firstname;
                    admin.LastName = model.lastname;
                    admin.PhoneNumber = model.PhoneNumber;
                    admin.IsDeleted = false;
                    admin.Password = model.Password;


                    if (model != null && model.ImageFile != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                        string extension = Path.GetExtension(model.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        admin.Image = "/images/admins/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/images/admins"), fileName);
                        model.ImageFile.SaveAs(fileName);
                    }

                    await client.UpdateAsync("Admin/" + docId, admin);

                }
                return RedirectToAction("Index", "UserManagement");
            }
            catch (Exception e)
            {
                Response.Write("<script>alert('Failed to load Data!')</script>");
                return RedirectToAction("Index", "UserManagement");
            }
        }
    
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [Route("Logout")]
        public ActionResult Logout()
        {
            Session.Remove("UserID");

            return RedirectToAction("SignIn", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(string email, string newPassword)
        {
            try
            {
                FirebaseResponse response = client.Get("Admin");
                Admin admin = null;
                string docId = null;
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                foreach (var item in data)
                {
                    var a = JsonConvert.DeserializeObject<Admin>(((JProperty)item).Value.ToString());
                    if (a.Email == email)
                    {
                        admin = a;
                        docId = a.Id;
                    }
                }
                if (admin != null)
                {
                    admin.Password = newPassword;
                    await client.UpdateAsync("Admin/" + docId, admin);
                    ViewBag.error = $"Password updated successfully!! for {email}";

                    return View("SignIn");
                }
                else
                {
                    ViewBag.error = $"User not found";
                    return View("ForgotPassword");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        

    }
}