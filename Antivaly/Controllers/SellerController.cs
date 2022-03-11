using Antivaly.BusinessModel;
using Antivaly.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Antivaly.Controllers
{
    public class SellerController : Controller
    {
        // GET: Seller
        public ActionResult Dashboard()
        {
            if (Session["UID"] != null)
            {
                ViewBag.Title = "Dashboard";
                var db = new AntivalyEntities();
                ViewBag.Products = db.Products.ToList();
                ViewBag.UID = Session["UID"].ToString();
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Dashboard(DeleteProduct product)
        {

            if (ModelState.IsValid)
            {
                var db = new AntivalyEntities();
                var Category = db.Products.Where(x => x.PID == product.PID).FirstOrDefault();
                var category = db.Categories.Where(
                    x => x.CatName == Category.CatName
                    ).FirstOrDefault();
                if (category != null)
                {
                    category.Items = category.Items - 1;
                    db.Entry(category).State = EntityState.Modified;
                }
                db.Products.Remove(db.Products.Where(x => x.PID == product.PID).FirstOrDefault());
                db.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View();

        }

        public ActionResult EditProduct(int ID)
        {
            if (Session["UID"] != null)
            {
                if (ID > 0)
                {
                    ViewBag.Title = "Dashboard";
                    var db = new AntivalyEntities();
                    var id = db.Products.Where(u => u.PID == ID).FirstOrDefault();
                    ViewBag.Product = id.PID;
                    ViewBag.Products = db.Products.ToList();
                    ViewBag.UID = Session["UID"].ToString();
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult EditProduct(EditProduct product)
        {
            if (ModelState.IsValid)
            {
                var db = new AntivalyEntities();
                var Product = db.Products.Where(x => x.PID == product.PID).FirstOrDefault();
                if (Product != null)
                {
                    Product.PName = product.PName;
                    Product.Qty = product.Qty;
                    Product.Price = product.Price;
                    Product.Descr = product.Descr;
                    db.Entry(Product).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Dashboard");
        }
        public ActionResult UProfile()
        {
            if (Session["UID"] != null)
            {
                var UID = Session["UID"].ToString();
                AntivalyEntities db = new AntivalyEntities();
                var data = db.Users.FirstOrDefault(u => u.UID == UID);
                ViewBag.UserDetails = data;
                return View();
            }
            return RedirectToAction("Index", "Home");

        }

        public ActionResult EditProfile()
        {
            if (Session["UID"] != null)
            {
                var UID = Session["UID"].ToString();
                AntivalyEntities db = new AntivalyEntities();
                var data = db.Users.FirstOrDefault(u => u.UID == UID);
                ViewBag.UserDetails = data;
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult EditProfile(EditProfile user)
        {
            if (ModelState.IsValid)
            {
                var db = new AntivalyEntities();
                var User = db.Users.Where(x => x.UID == user.UID).FirstOrDefault();
                if (User != null)
                {
                    User.Name = user.Name;
                    User.Email = user.Email;
                    User.Address = user.Address;
                    User.Contact = user.Contact;
                    db.Entry(User).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("UProfile");
        }


        public ActionResult PostProduct()
        {

            if (Session["UID"] != null)
            {
                ViewBag.Title = "Post Product";
                var db = new AntivalyEntities();
                ViewBag.Category = db.Categories.ToList();
                return View();
            }
            return RedirectToAction("Index", "Home");

        }
        [HttpPost]
        public ActionResult PostProduct(PostProduct product)
        {

            if (ModelState.IsValid)
            {
                var db = new AntivalyEntities();

                var productNew = new Product();
                productNew.CatName = product.CatName;
                productNew.UID = product.UID;
                productNew.PName = product.PName;
                productNew.Qty = product.Qty;
                productNew.Price = product.Price;
                productNew.Descr = product.Descr;
                db.Products.Add(productNew);
                var category = db.Categories.Where(x => x.CatName == productNew.CatName).FirstOrDefault();
                if (category != null)
                {
                    category.Items = category.Items + 1;
                    db.Entry(category).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View();

        }
    }
}