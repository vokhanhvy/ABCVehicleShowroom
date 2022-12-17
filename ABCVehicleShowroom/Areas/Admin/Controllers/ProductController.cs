using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ABCVehicleShowroom.Models;

namespace ABCVehicleShowroom.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private ABCVehicleShowroomDbContext db = new ABCVehicleShowroomDbContext();

        // GET: Admin/Product
        public ActionResult Index()
        {
            ViewBag.demrac = db.Products.Where(m => m.Status == 0).Count();
            var list = from p in db.Products
                       join c in db.Categories
                       on p.CatId equals c.Id
                       join b in db.Brands
                       on p.BrandId equals b.Id
                       where p.Status != 0
                       orderby p.Created_at descending
                       select new ProductCategory()
                       {
                           ProductId = p.Id,
                           ProductImg = p.Img,
                           ProductName = p.Name,
                           ProductStatus = p.Status,
                           ProductDiscount = p.Discount,
                           CategoryName = c.Name,
                           BrandName =b.Name,
                           ProductQuantity = p.Number
                       };

            return View(list.ToList());
        }
        public ActionResult Trash()
        {

            var list = from p in db.Products
                       join c in db.Categories
                       on p.CatId equals c.Id
                       join b in db.Brands
                      on p.BrandId equals b.Id
                       where p.Status == 0
                       orderby p.Created_at descending
                       select new ProductCategory()
                       {
                           ProductId = p.Id,
                           ProductImg = p.Img,
                           ProductName = p.Name,
                           ProductStatus = p.Status,
                           CategoryName = c.Name,
                           BrandName = b.Name,
                           ProductQuantity = p.Number
                       };
            //  ViewBag.demrac = db.Categories.Where(m => m.Status == 0).Count();
            return View(list.ToList());
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelProduct modelProduct = db.Products.Find(id);
            if (modelProduct == null)
            {
                return HttpNotFound();
            }
            return View(modelProduct);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(db.Categories.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            ViewBag.ListBrand = new SelectList(db.Brands.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            return View();
        }


        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(modelProduct modelProduct)
        {
            if (ModelState.IsValid)
            {
                String strSlug = XString.ToAscii(modelProduct.Name);
                String slug = strSlug;
                modelProduct.Slug = slug;
                modelProduct.Created_at = DateTime.Now;
                modelProduct.Updated_at = DateTime.Now;
                modelProduct.Updated_by = int.Parse(Session["Admin_ID"].ToString());
                modelProduct.Created_by = int.Parse(Session["Admin_ID"].ToString());

                //Upload file
                var f = Request.Files["Img"];

                if (f != null & f.ContentLength > 0)
                {
                    String fileName = strSlug + f.FileName.Substring(f.FileName.LastIndexOf("."));
                    modelProduct.Img = fileName;
                    String Strpath = Path.Combine(Server.MapPath("~/Public/Library/images/Product"), fileName);
                    f.SaveAs(Strpath);
                }
                db.Products.Add(modelProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // list of category
            ViewBag.ListCat = new SelectList(db.Categories.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);

            // list of brand
            ViewBag.ListBrand = new SelectList(db.Brands.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);

            return View(modelProduct);
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelProduct modelProduct = db.Products.Find(id);
            if (modelProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListCat = new SelectList(db.Categories.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            // list of brand
            ViewBag.ListBrand = new SelectList(db.Brands.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            return View(modelProduct);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(modelProduct modelProduct)
        {
            ViewBag.ListCat = new SelectList(db.Categories.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);

            // list of brand
            ViewBag.ListBrand = new SelectList(db.Brands.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);

            if (ModelState.IsValid)
            {
                String strSlug = XString.ToAscii(modelProduct.Name);
                String slug = strSlug;
                modelProduct.Slug = slug;
                modelProduct.Updated_at = DateTime.Now;

                modelProduct.Updated_by = int.Parse(Session["Admin_ID"].ToString());
                modelProduct.Created_by = int.Parse(Session["Admin_ID"].ToString());

                //Upload file
                var f = Request.Files["Img"];

                if (f != null & f.ContentLength > 0)
                {
                    String fileName = strSlug + f.FileName.Substring(f.FileName.LastIndexOf("."));
                    modelProduct.Img = fileName;
                    String Strpath = Path.Combine(Server.MapPath("~/Public/Library/images/Product"), fileName);
                    f.SaveAs(Strpath);
                }
                db.Entry(modelProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(modelProduct);
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            modelProduct modelProduct = db.Products.Find(id);
            if (modelProduct == null)
            {
                return HttpNotFound();
            }

            return View(modelProduct);
        }
        public ActionResult delTrash(int? id)
        {
            modelProduct modelProduct = db.Products.Find(id);
            if (modelProduct == null)
            {
                return RedirectToAction("Index");
            }
            modelProduct.Status = 0;

            modelProduct.Updated_at = DateTime.Now;

            modelProduct.Updated_at = DateTime.Now;
            modelProduct.Updated_by = int.Parse(Session["Admin_ID"].ToString());
            db.Entry(modelProduct).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult Undo(int? id)
        {
            modelProduct modelProduct = db.Products.Find(id);
            if (modelProduct == null)
            {
                return RedirectToAction("Trash");
            }
            modelProduct.Status = 2;

            modelProduct.Updated_at = DateTime.Now;
            modelProduct.Updated_at = DateTime.Now;
            modelProduct.Updated_by = int.Parse(Session["Admin_ID"].ToString());
            db.Entry(modelProduct).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash");

        }

        //doi trang thai
        public ActionResult Status(int? id)
        {
            modelProduct modelProduct = db.Products.Find(id);
            if (modelProduct == null)
            {

                return RedirectToAction("Index");
            }
            modelProduct.Status = (modelProduct.Status == 1) ? 2 : 1;

            modelProduct.Updated_at = DateTime.Now;
            modelProduct.Updated_at = DateTime.Now;
            modelProduct.Updated_by = int.Parse(Session["Admin_ID"].ToString());
            db.Entry(modelProduct).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");

        }
        //doi trạng thái giảm giá sp
        public ActionResult Discount(int? id)
        {
            modelProduct modelProduct = db.Products.Find(id);
            if (modelProduct == null)
            {
                return RedirectToAction("Index");
            }
            modelProduct.Discount = (modelProduct.Discount == 1) ? 2 : 1;

            modelProduct.Updated_at = DateTime.Now;
            modelProduct.Updated_at = DateTime.Now;
            modelProduct.Updated_by = int.Parse(Session["Admin_ID"].ToString());
            db.Entry(modelProduct).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");

        }
        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            modelProduct modelProduct = db.Products.Find(id);
            db.Products.Remove(modelProduct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
