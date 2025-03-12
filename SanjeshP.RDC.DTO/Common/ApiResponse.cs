namespace SanjeshP.RDC.DTO.Common
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; } // موفقیت‌آمیز بودن عملیات
        public string Message { get; set; } // پیام وضعیت
        public T Data { get; set; } // داده‌های اصلی

        public ApiResponse(T data, bool isSuccess, string message = null)
        {
            Data = data;
            IsSuccess = isSuccess;
            Message = message;
        }
    }

}
