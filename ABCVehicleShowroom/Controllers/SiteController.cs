using PagedList;
using ABCVehicleShowroom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABCVehicleShowroom.Controllers
{
    public class SiteController : Controller
    {
        private ABCVehicleShowroomDbContext db = new ABCVehicleShowroomDbContext();
        // GET: Site
        public ActionResult Index(String slug = "")
        {
            int pageNumber = 1;
            Session["keywords"] = null;
            if (!string.IsNullOrEmpty(Request.QueryString["page"]))
            {
                pageNumber = int.Parse(Request.QueryString["page"].ToString());
            }
            if (slug == "")
            {
                return this.Home();
            }
            else
            {
                var link = db.Links.Where(m => m.Slug == slug);
                if (link.Count() > 0)
                {
                    var res = link.First();
                    
                    if (res.Type == "category")
                    {
                        return this.ProductCategory(slug, pageNumber);
                    }
                    else if (res.Type == "brand")
                    {
                        return this.ProductBrand(slug, pageNumber);
                    }
                }
                
                    else
                {
                    if (db.Products.Where(m => m.Slug == slug && m.Status == 1).Count() > 0)
                    {
                        return this.ProductDetail(slug);
                    }
                    
                }
                return this.Error(slug);
            }
        }

        public ActionResult ProductDetail(String slug)
        {
            var model = db.Products
                .Where(m => m.Slug == slug && m.Status == 1)
                .First();
            int catid = model.CatId;

            List<int> listcatid = new List<int>();
            listcatid.Add(catid);

            //var list2 = db.Categories
            //    .Where(m => m.ParentId == catid)
            //    .Select(m => m.Id)
            //    .ToList();
            //foreach (var id2 in list2)
            //{
            //    listcatid.Add(id2);
            //    var list3 = db.Categories
            //        .Where(m => m.ParentId == id2)
            //        .Select(m => m.Id)
            //        .ToList();
            //    foreach (var id3 in list3)
            //    {
            //        listcatid.Add(id3);
            //    }
            //}
            // danh mục cùng sản phẩm
            ViewBag.listother = db.Products
                .Where(m => m.Status == 1 && listcatid
                .Contains(m.CatId) && m.Id != model.Id)
                .OrderByDescending(m => m.Created_at)
                .Take(12)
                .ToList();
            // sản phẩm mới nhập
            ViewBag.news = db.Products
                .Where(m => m.Status == 1 /*&& listcatid.Contains(m.CatId)*/ && m.Id != model.Id)
                .OrderByDescending(m => m.Created_at).Take(4).ToList();
            return View("ProductDetail", model);
        }
       
        public ActionResult Error(String slug)
        {
            return View("Error");
        }

        

        
        public ActionResult ProductCategory(String slug, int pageNumber)
        {
            int pageSize = 8;
            var row_cat = db.Categories
                .Where(m => m.Slug == slug)
                .First();
            List<int> listcatid = new List<int>();
            listcatid.Add(row_cat.Id);

            
            var list = db.Products
              .Where(m => m.Status == 1 && listcatid.Contains(m.CatId) )
                .OrderByDescending(m => m.Created_at);
            

            ViewBag.Slug = slug;
            ViewBag.CatName = row_cat.Name;
            return View("ProductCategory", list
                .ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ProductBrand(String slug, int pageNumber)
        {
            int pageSize = 8;
            var row_cat = db.Brands
                .Where(m => m.Slug == slug)
                .First();
            List<int> listcatid = new List<int>();
            listcatid.Add(row_cat.Id);


            var list = db.Products
              .Where(m => m.Status == 1 && listcatid.Contains(m.BrandId))
                .OrderByDescending(m => m.Created_at);


            ViewBag.Slug = slug;
            ViewBag.BrandName = row_cat.Name;
            return View("ProductBrand", list
                .ToPagedList(pageNumber, pageSize));
        }



        //Home Page
        public ActionResult Home()
        {
            var list = db.Categories
               .Where(m => m.Status == 1 /*&& m.ParentId == 0*/)
               .Take(8)
               .ToList();
            return View("Home", list);
        }

        public ActionResult Other()
        {
            return View("Other");
        }
        //Sản phẩm Home Page
        public ActionResult ProductHome(int catid)
        {
            List<int> listcatid = new List<int>();
            listcatid.Add(catid);

            //var list2 = db.Categories
            //    .Where(m => m.ParentId == catid).Select(m => m.Id)
            //    .ToList();
            //foreach (var id2 in list2)
            //{
            //    listcatid.Add(id2);
            //    var list3 = db.Categories
            //        .Where(m => m.ParentId == id2)
            //        .Select(m => m.Id).ToList();
            //    foreach (var id3 in list3)
            //    {
            //        listcatid.Add(id3);
            //    }
            //}

            var list = db.Products
                .Where(m => m.Status == 1 && listcatid
                .Contains(m.CatId))
                .Take(4)
                .OrderByDescending(m => m.Created_at);

            return View("_ProductHome", list);
        }
        //Tat ca sp
        public ActionResult Product(int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            var list = db.Products.Where(m => m.Status == 1)
                .OrderByDescending(m => m.Created_at)
                .ToPagedList(pageNumber, pageSize);
            return View(list);
        }
        // tìm kiếm sản phẩm
        public ActionResult Search(String key, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var list = db.Products.Where(m => m.Status == 1);
            if (String.IsNullOrEmpty(key.Trim()))
            {
                return RedirectToAction("Index", "Site");

            }
            else
            {
                list = list.Where(m => m.Name.Contains(key)).OrderByDescending(m => m.Created_at);
            }

            Session["keywords"] = key;
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        
        public ActionResult Contact()
        {
            return View("Contact");
        }
        [HttpPost]

       
        public ActionResult ThanksContact()
        {
            return View();
        }
    }
}