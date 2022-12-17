﻿using ABCVehicleShowroom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ABCVehicleShowroom.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        // Do login for checkout
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            //modelUser sessionUser = (modelUser)Session[Common.CommonConstants.USER_SESSION];
            modelUser sessionCustomer = (modelUser)Session[Common.CommonConstants.CUSTOMER_SESSION];
            if (sessionCustomer == null)
            {
                RouteValueDictionary route = new RouteValueDictionary(new { Controller = "Site", Action = "Index" });
                Message.set_flash("You need to login", "danger");
                filterContext.Result = new RedirectToRouteResult(route);
                return;
            }
        }
    }
}