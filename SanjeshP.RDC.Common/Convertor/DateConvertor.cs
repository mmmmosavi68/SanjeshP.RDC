using System;
using System.Globalization;

namespace SanjeshP.RDC.Convertor
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value) + "/" +
                    pc.GetMonth(value).ToString("00") + "/" +
                    pc.GetDayOfMonth(value).ToString("00");


        }
        public static string ToShamsiDateTime(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value) + "/" +
                   pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00") + " " +
                   value.Hour.ToString("00") + ":" +
                   value.Minute.ToString("00") + ":" +
                   value.Second.ToString("00");
        }

    }
}
