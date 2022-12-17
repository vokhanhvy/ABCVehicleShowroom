namespace ABCVehicleShowroom.Models
{
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
    public class ABCVehicleShowroomDbContext : DbContext
    {
        public ABCVehicleShowroomDbContext() : base("name=StrC")
        { }
        public virtual DbSet<modelProduct> Products { get; set; }
        public virtual DbSet<modelCategory> Categories { get; set; }
        public virtual DbSet<modelBrand> Brands { get; set; }
        public virtual DbSet<modelSlider> Sliders { get; set; }
        public virtual DbSet<modelMenu> Menus { get; set; }
        public virtual DbSet<modelOrder> Orders { get; set; }
        public virtual DbSet<modelOrderdetail> Orderdetails { get; set; }
       
       
        
        public virtual DbSet<modelUser> Users { get; set; }
        public virtual DbSet<modelLink> Links { get; set; }
    }
}