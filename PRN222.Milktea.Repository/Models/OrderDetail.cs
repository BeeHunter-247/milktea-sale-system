﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PRN222.Milktea.Repository.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Order Order { get; set; }

    public virtual Product Product { get; set; }
}