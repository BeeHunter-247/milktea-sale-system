using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.BusinessModels
{
	public class PaginationModel
	{
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageTotals { get; set; }
    }

	public class Pagination<T> : List<T>
	{
		public int TotalCount { get; set; }

		public Pagination(List<T> list, int count)
		{
			TotalCount = count;
			AddRange(list);
		}
	}
}
