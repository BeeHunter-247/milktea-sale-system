﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.BusinessModels
{
    public class AccountModelAdmin
    {
        public int AccountId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int AccountRole { get; set; }
        public bool? IsActive { get; set; }
    }
}
