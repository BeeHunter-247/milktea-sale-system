﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PRN222.Milktea.Repository.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int CategoryId { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; }

    public string Image { get; set; }

    public int ProductType { get; set; }

    public bool? IsActive { get; set; }

    public virtual Category Category { get; set; }

    public virtual ICollection<ComboProduct> ComboProductComboProductNavigations { get; set; } = new List<ComboProduct>();

    public virtual ICollection<ComboProduct> ComboProductIncludedProducts { get; set; } = new List<ComboProduct>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}