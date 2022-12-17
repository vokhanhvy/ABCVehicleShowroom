﻿using ABCVehicleShowroom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABCVehicleShowroom.Areas.Admin.Controllers
{
    public class AuthController : System.Web.Mvc.Controller
    {

        public ActionResult Login()
        {
            if (Session["Admin_Name"] != null)
            {
                Response.Redirect("~/Admin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection field)
        {
            ABCVehicleShowroomDbContext db = new ABCVehicleShowroomDbContext();
            String username = field["user"];
            String password = MyString.ToMD5(field["password"]);
            //String password = field["password"];
            
            int count_username = db.Users.Where(m => m.status == 1 && (m.username == username || m.email == username) && m.access != 0).Count();
            int count_user = db.Users.Where(m => m.status == 1 && (m.username == username || m.email == username) && m.access == 0).Count();
            if (count_username == 0)
            {
                ViewBag.Error = "<span class ='text-white' style='color:red'>Invalid account</span>";
            }
            else
            {
                var user_acount = db.Users
                .Where(m => m.status == 1 && (m.username == username || m.email == username) && m.access != 0 && m.password == password);
                if (user_acount.Count() == 0)
                {
                    ViewBag.Error = "<span class ='text-white' style='color:red'>Password is incorrect!!!</span>";
                }
                else
                {
                    var user = user_acount.First();
                    Session["Admin_Name"] = user.username;
                    Session["Admin_ID"] = user.ID;
                    Response.Redirect("~/Admin");
                }
            }
            if (count_user == 1)
            {

                ViewBag.Error = "<span class ='text-white' style='color:red' >You don't have administration access</span>";
            }
            else
            {

            }
            
            return View("Login");
            


        }

        public ActionResult Logout()
        {
            Session["Admin_ID"] = null;
            Session["Admin_Name"] = null;
            Response.Redirect("~/Admin/login");
            return null;
        }
    }
}