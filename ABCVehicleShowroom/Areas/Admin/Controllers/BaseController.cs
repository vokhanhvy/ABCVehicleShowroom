﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABCVehicleShowroom.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Admin/Base
        public BaseController()
        {
            if (System.Web.HttpContext.Current.Session["Admin_Name"] == null)
            {
                System.Web.HttpContext.Current.Response.Redirect("~/Admin/Login");
            }
        }
    }
}