using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Summary description for FFDomain
/// </summary>
public class FFDomain
{
    public static string[] getBlocked()
    {
        string temp = "";
        string [] domains = null;
        using(StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("../App_Data/domains.csv"))) 
        {
            temp = sr.ReadToEnd();
            domains = Regex.Split(temp, "\r\n");
        }
        return domains;
    }
}
