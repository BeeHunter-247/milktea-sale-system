using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.BusinessModels
{
    public class ComboModel
    {
        public int ComboId { get; set; }

        public string ComboName { get; set; }

        public decimal? ComboPrice { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public int? ProductId1 { get; set; }

        public int? ProductId2 { get; set; }

        public int? ProductId3 { get; set; }
        public bool? IsActive { get; set; }
        public string? ProductId1Name { get; set; }
        public string? ProductId2Name { get; set; }
        public string? ProductId3Name { get; set; }
    }
}
