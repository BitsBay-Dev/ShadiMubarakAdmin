using FireSharp.Config;
using FireSharp.Interfaces;
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

namespace ShadiMubarak.Controllers
{
    public class VendorCategoryController : BaseController
    {

        // GET: VendorCategory
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
                Query qry = db.Collection("vendorscategory");
                QuerySnapshot qrysnp = await qry.GetSnapshotAsync();
                var vendorcategorylist = new List<VendorCategory>();
                if (qrysnp  != null)
                {
                    foreach (DocumentSnapshot item in qrysnp)  
                    {
                        VendorCategory ven = item.ConvertTo<VendorCategory>();
                        vendorcategorylist.Add(ven);
                    }
                }
                return View(vendorcategorylist);
                }
                catch (Exception e)
                {
                    Response.Write("<script>alert('Failed to load Data!')</script>");
                    return RedirectToAction("Home", "Index");
                }
            }
        }
        public async Task<ActionResult> Create(VendorCategory vendorcategory, HttpPostedFileBase file)
        {
            try
            {
                await AddVendorCategoryToFirebaseAsync(vendorcategory,file);
                ModelState.AddModelError(string.Empty, "Added Successfully!");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Added Successfully!");
            }
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Edit(string Id)
        {
            try { 
            Query qry = db.Collection("vendorscategory").WhereEqualTo("name", Id);
            QuerySnapshot qrysnp = await qry.GetSnapshotAsync();
            ViewBag.vcId = qrysnp.Documents.FirstOrDefault().Reference.Id;
            VendorCategory data = qrysnp.Documents.FirstOrDefault().ConvertTo<VendorCategory>();
            return View(data);
            }
            catch (Exception e)
            {
                Response.Write("<script>alert('Failed to load Data!')</script>");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(VendorCategory vendorcategory)
        {
            try { 
            DocumentReference dr = db.Collection("vendorscategory").Document(vendorcategory.Id);
            await dr.UpdateAsync("name", vendorcategory.name);
            return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Response.Write("<script>alert('Failed to load Data!')</script>");
                return RedirectToAction("Index");
            }
        }
        public async Task<ActionResult> Delete(string Id)
        {
            try { 
            Query qry = db.Collection("vendorscategory").WhereEqualTo("name", Id);
            QuerySnapshot qrysnp = await qry.GetSnapshotAsync();
            VendorCategory data = qrysnp.Documents.FirstOrDefault().ConvertTo<VendorCategory>();

            return View(data);
            }
            catch (Exception e)
            {
                Response.Write("<script>alert('Failed to load Data!')</script>");
                return RedirectToAction("Index");
            }
        }

        [ActionName("Delete")]
        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(string name)
        {
            try { 
            Query qry = db.Collection("vendorscategory").WhereEqualTo("name", name);
            QuerySnapshot qrysnp = await qry.GetSnapshotAsync();
            DocumentSnapshot docsnp = qrysnp.Documents.FirstOrDefault();
            await docsnp.Reference.DeleteAsync();
           // FirebaseResponse Response = client.Delete("VendorCategory/" + id);
            return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Response.Write("<script>alert('Failed to load Data!')</script>");
                return RedirectToAction("Index");
            }
        }

        public async Task AddVendorCategoryToFirebaseAsync(VendorCategory vendorcategory, HttpPostedFileBase file)
        {
            try
            {
                FileStream stream;
                string link;
                if (file != null)
                {
                    string path = Path.Combine(Server.MapPath("~/images/vendorcategory"), file.FileName);
                    file.SaveAs(path);
                    stream = new FileStream(Path.Combine(path), FileMode.Open);
                    link = await Task.Run(() => UploadFile(stream, file.FileName));
                }
                else
                {
                    link = null;
                }
                CollectionReference collection = db.Collection("vendorscategory");
                DocumentReference document = await collection.AddAsync(new { name = vendorcategory.name, Image = link });
            }
            catch (Exception e)
            {
                Response.Write("<script>alert('Failed to load Data!')</script>");
            }
        }
    }
}