using System;

namespace APi.Extensions
{
    public static class DateTiemeExtenstion
    {
        public static int CalaculateAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year -dob.Year;
            if (dob.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
