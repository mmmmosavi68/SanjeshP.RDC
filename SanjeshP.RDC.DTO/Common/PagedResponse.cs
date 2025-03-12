using System;
using System.Collections.Generic;
using System.Text;

namespace SanjeshP.RDC.DTO.Common
{
    public class PagedResponse<T>
    {
        public List<T> Data { get; set; } // لیست داده‌ها
        public int TotalCount { get; set; } // تعداد کل آیتم‌ها
        public int PageNumber { get; set; } // شماره صفحه فعلی
        public int PageSize { get; set; } // اندازه صفحه
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize); // محاسبه کل صفحات

        public PagedResponse(List<T> data, int totalCount, int pageNumber, int pageSize)
        {
            Data = data;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

}
