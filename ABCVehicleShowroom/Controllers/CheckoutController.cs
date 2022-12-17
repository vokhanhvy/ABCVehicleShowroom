
using Newtonsoft.Json.Linq;
using ABCVehicleShowroom.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABCVehicleShowroom.Controllers
{
    public class CheckoutController : BaseController
    {
        // GET: Checkout
        private const string SessionCart = "SessionCart";
        private ABCVehicleShowroomDbContext db = new ABCVehicleShowroomDbContext();
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Index()
        {
            var cart = Session[SessionCart];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            else if (cart == null)
            {
                ViewBag.statusCart = "Giỏ hàng trống";

            }
            return View(list);
        }

        [HttpPost]
        public ActionResult Index(modelOrder order)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            int numIterations = 0;
            numIterations = rand.Next(1, 100000);
            DateTime time = DateTime.Now;
            string orderCode = XString.ToStringNospace(numIterations + "" + time);
            string sumOrder = Request["sumOrder"];
            string payment_method = Request["option_payment"];
            // Neu Ship COde
            if (payment_method.Equals("COD"))
            {
                // cap nhat thong tin sau khi dat hang thanh cong

                saveOrder(order, "COD", 2, orderCode);
                var cart = Session[SessionCart];
                var list = new List<CartItem>();
                ViewBag.cart = (List<CartItem>)cart;
                ViewBag.statusCod = 1;
                ViewBag.Methodpayment = "COD";
                Session["SessionCart"] = null;
                var listProductOrder = db.Orders.Where(m => m.Id == order.Id).FirstOrDefault();
                return View("oderComplete", listProductOrder);
            }
            

            return View("confirm_orderPaymentOnline");
        }
        //function save order when order success!
        public void saveOrder(modelOrder order, string paymentMethod, int StatusPayment, string ordercode)
        {
            var cart = Session[SessionCart];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }

            if (ModelState.IsValid)
            {
                order.Code = ordercode;
                order.CustemerId = 1;
                order.DeliveryPaymentMethod = paymentMethod;
                order.StatusPayment = StatusPayment;
                order.CreateDate = DateTime.Now;
                order.updated_by = 1;
                order.updated_at = DateTime.Now;
                order.updated_by = 1;
                order.Status = 2;
                order.ExportDate = DateTime.Now;
                db.Orders.Add(order);
                db.SaveChanges();
                Session["OrderId"] = ordercode;
                ViewBag.name = order.DeliveryName;
                ViewBag.email = order.DeliveryEmail;
                ViewBag.address = order.DeliveryAddress;
                ViewBag.code = order.Code;
                ViewBag.phone = order.DeliveryPhone;
                modelOrderdetail orderdetail = new modelOrderdetail();

                foreach (var item in list)
                {
                    float price = 0;
                    int sale = 0;
                    if (sale > 0)
                    {
                        price = (float)item.product.Price_sale * item.quantity;
                    }
                    else
                    {
                        price = (float)item.product.Price * (int)item.quantity;
                    }
                    orderdetail.OrderId = order.Id;
                    orderdetail.ProductId = item.product.Id;
                    orderdetail.priceSale = (int)item.product.Price_sale;
                    orderdetail.Price = item.product.Price;
                    orderdetail.Quantity = item.quantity;
                    orderdetail.Amount = price;

                    db.Orderdetails.Add(orderdetail);
                    db.SaveChanges();
                    var updatedProduct = db.Products.Find(item.product.Id);
                    db.Products.Attach(updatedProduct);
                    db.Entry(updatedProduct).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
        }
        //

        public ActionResult productDetailCheckOut(int orderId)
        {
            var list = db.Orderdetails.Where(m => m.OrderId == orderId).ToList();
            return View("_DetailProduct", list);

        }
        public ActionResult orderDetail(int orderId)
        {
            var single = db.Orders.Find(orderId);
            return View("oderComplete", single);
        }

        public ActionResult subnameProduct(int id)
        {
            var list = db.Products.Find(id);
            return View("_SubNameProduct", list);

        }
        public ActionResult formCheckOut()
        {
            //Muser user = (Muser)Session[Common.CommonConstants.CUSTOMER_SESSION];
            return View("_formCheckout"/*, user*/);

        }



    }
}