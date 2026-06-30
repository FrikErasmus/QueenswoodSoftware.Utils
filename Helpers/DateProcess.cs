using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Helpers;

public class DateProcess

{
    public static DateTime? GetMaxDate(DateTime? date1, DateTime? date2)
    {
        DateTime? returnDate;
        returnDate = Convert.ToDateTime("1900-01-01 00:00:01");
        try
        {
            if (date1 == null)
            {
                date1 = returnDate;
            }
            if (date2 == null)
            {
                date2 = returnDate;
            }

            if (date1 > date2)
            {
                returnDate = date1;
            }
            else
            {
                returnDate = date2;
            }
        }
        catch (Exception ex)
        {
            //
        }
        if (returnDate == Convert.ToDateTime("1900-01-01 00:00:01"))
        {
            returnDate = null;
        }
        return returnDate;
    }
}