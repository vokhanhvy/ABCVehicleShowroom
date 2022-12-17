using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ABCVehicleShowroom.Models;

namespace ABCVehicleShowroom.Areas.Admin.Controllers
{
    public class BrandController : BaseController
    {
        private ABCVehicleShowroomDbContext db = new ABCVehicleShowroomDbContext();

        // GET: Admin/Brand
        public ActionResult Index()
        {
            var list = db.Brands.Where(m => m.Status != 0).ToList();

            ViewBag.demrac = db.Categories.Where(m => m.Status == 0).Count();

            foreach (var row in list)
            {
                var temp_link = db.Links
                    .Where(m => m.Type == "brand" && m.TableId == row.Id);
                if (temp_link.Count() > 0)
                {
                    var row_link = temp_link.First();
                    row_link.Name = row.Name;
                    row_link.Slug = row.Slug;
                    db.Entry(row_link).State = EntityState.Modified;
                }
                else
                {
                    var row_link = new modelLink();
                    row_link.Name = row.Name;
                    row_link.Slug = row.Slug;
                    row_link.Type = "brand";
                    row_link.TableId = row.Id;
                    db.Links.Add(row_link);
                }
            }
            db.SaveChanges();
            return View(list);
            //return View(db.Categories.ToList());
        }

        // GET: Admin/Brand/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelBrand modelBrand = db.Brands.Find(id);
            if (modelBrand == null)
            {
                return HttpNotFound();
            }
            return View(modelBrand);
        }

        // GET: Admin/Brand/Create
        public ActionResult Create()
        {
            ViewBag.List = new SelectList(db.Categories.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            //ViewBag.Orders = new SelectList(db.Categories.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);

            return View();
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(modelBrand modelBrand)
        {
            if (ModelState.IsValid)
            {
                //if (modelBrand.ParentId == null)
                //{
                //    modelBrand.ParentId = 0;
                //}

                String slug = XString.ToAscii(modelBrand.Name);
                modelBrand.Slug = slug;
                modelBrand.Created_at = DateTime.Now;
                modelBrand.Updated_at = DateTime.Now;
                modelBrand.Updated_by = int.Parse(Session["Admin_ID"].ToString());
                modelBrand.Created_by = int.Parse(Session["Admin_ID"].ToString());
                db.Brands.Add(modelBrand);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.List = new SelectList(db.Categories.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            //ViewBag.Orders = new SelectList(db.Categories.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);

            return View(modelBrand);
        }

        // GET: Admin/Brand/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.List = new SelectList(db.Brands.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            //ViewBag.Orders = new SelectList(db.Brands.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelBrand modelBrand = db.Brands.Find(id);
            if (modelBrand == null)
            {
                return HttpNotFound();
            }
            return View(modelBrand);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(modelBrand modelBrand)
        {
            if (ModelState.IsValid)
            {
                //if (modelBrand.ParentId == null)
                //{
                //    modelBrand.ParentId = 0;
                //}
                String slug = XString.ToAscii(modelBrand.Name);
                modelBrand.Slug = slug;

                modelBrand.Updated_at = DateTime.Now;
                modelBrand.Updated_by = int.Parse(Session["Admin_ID"].ToString());
                modelBrand.Created_by = int.Parse(Session["Admin_ID"].ToString()); 
                db.Brands.Add(modelBrand);
                // db.SaveChanges();

                db.Entry(modelBrand).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.List = new SelectList(db.Categories.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            //ViewBag.Orders = new SelectList(db.Categories.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            return View(modelBrand);
        }
        public ActionResult delTrash(int? id)
        {
            modelBrand modelBrand = db.Brands.Find(id);
            if (modelBrand == null)
            {
                return RedirectToAction("Index");
            }

            modelBrand.Status = 0;

            modelBrand.Updated_at = DateTime.Now;
            modelBrand.Created_by = int.Parse(Session["Admin_ID"].ToString());
            modelBrand.Updated_at = DateTime.Now;
            modelBrand.Updated_by = int.Parse(Session["Admin_ID"].ToString());
            modelBrand.Updated_by = int.Parse(Session["Admin_ID"].ToString()); 

            db.Entry(modelBrand).State = EntityState.Modified;
            db.SaveChanges();
            Thongbao.set_flash("Ném vào thùng rác thành công !" + " ID = " + id, "success");
            return RedirectToAction("Index");

        }
        public ActionResult Undo(int? id)
        {
            modelBrand modelBrand = db.Brands.Find(id);
            if (modelBrand == null)
            {

                return RedirectToAction("Trash");
            }
            modelBrand.Status = 2;

            modelBrand.Updated_at = DateTime.Now;
            modelBrand.Created_by = int.Parse(Session["Admin_ID"].ToString());
            modelBrand.Updated_at = DateTime.Now;
            modelBrand.Updated_by = int.Parse(Session["Admin_ID"].ToString());

            db.Entry(modelBrand).State = EntityState.Modified;
            db.SaveChanges();
            Thongbao.set_flash("Khôi phục thành công !" + " ID = " + id, "success");
            return RedirectToAction("Trash");

        }
        //trash
        public ActionResult Trash()
        {
            var list = db.Brands.Where(m => m.Status == 0).ToList();

            return View(list);
        }
        //doi trang thai
        public ActionResult Status(int? id)
        {
            modelBrand modelBrand = db.Brands.Find(id);
            if (modelBrand == null)
            {

                return RedirectToAction("Index");
            }
            modelBrand.Status = (modelBrand.Status == 1) ? 2 : 1;

            modelBrand.Updated_at = DateTime.Now;
            modelBrand.Created_by = int.Parse(Session["Admin_ID"].ToString());
            modelBrand.Updated_at = DateTime.Now;
            modelBrand.Updated_by = int.Parse(Session["Admin_ID"].ToString());

            db.Entry(modelBrand).State = EntityState.Modified;
            db.SaveChanges();
            Thongbao.set_flash("Thay đổi trạng thái thành công!" + " ID = " + id, "success");
            return RedirectToAction("Index");

        }
        // GET: Admin/Brand/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelBrand modelBrand = db.Brands.Find(id);
            if (modelBrand == null)
            {
                return HttpNotFound();
            }
            return View(modelBrand);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            modelBrand modelBrand = db.Brands.Find(id);
            db.Brands.Remove(modelBrand);
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
