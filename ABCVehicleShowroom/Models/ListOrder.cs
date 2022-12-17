﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABCVehicleShowroom.Models
{
    public class ListOrder
    {
        public int ID { get; set; }
        public String CustomerName { get; set; }
        public double SAmount { get; set; }
        public int? Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ExportDate { get; set; }
    }
}