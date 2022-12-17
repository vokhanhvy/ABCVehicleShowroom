using ABCVehicleShowroom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABCVehicleShowroom.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        private ABCVehicleShowroomDbContext db = new ABCVehicleShowroomDbContext();
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            ViewBag.NewOrder = db.Orders.Where(m => m.Status == 2).Count();
            ViewBag.CountAdmin = db.Users.Where(m => m.status == 1 && m.access!=0).Count();
            ViewBag.CountUser = db.Users.Where(m => m.status == 1 && m.access == 0).Count();
            ViewBag.CountProduct = db.Products.Where(m => m.Status == 1).Count();
            return View();
        }
    }
}