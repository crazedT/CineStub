using System;

namespace CineStub.Web.Helpers
{
    public static class ScraperUtilities
    {
        public static DateTime? StringToNullableDateTime(string dateTime)
        {
            DateTime testDate;

            if (!DateTime.TryParse(dateTime, out testDate)) return null;

            DateTime? returnDate = DateTime.Parse(dateTime);
            return returnDate;
        }
    }
}
