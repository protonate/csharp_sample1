using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Xml;

public class FFPage
{
    private static int pageSize = 10;
    private static int maxPages = 10;
    private static string prefix = "../App_Data/Standings_";
    private static string ext = ".xml";

    private static int getCount()
    {
        string connString = (string)ConfigurationSettings.AppSettings["ConnectionString"];
        string num = "Select count(*) from Standings";

        SqlConnection conn = null;

        int count = 0;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand(num, conn);
            count = (int)cmd.ExecuteScalar();
        }
        catch (SqlException)
        {
            return -1;
        }
        finally
        {
            conn.Close();
        }

        //return Math.Min(maxPages * pageSize - 1, count);
        return count;
    }

    public static string getPage(int page)
    {
        string result = "";
        result = readPage(page);
        if (result.Equals(""))
        {
            if (generateAllPages())
            {
                result = readPage(page);
            }
            else
            {
                result = "<error code='006'>false</error>";
            }
        }
        return result;
    }

    /*public static Boolean generatePage(int page)
    {

        int numPages = 0;
        int count = FFPage.getCount();

        numPages = (int)Math.Ceiling((double)count / pageSize);

        if (page <= maxPages)
        {
            return writePage(page, numPages, count);
        }
        else return false;

    }*/
    
    public static Boolean generateAllPages()
    {
        string connString = (string)ConfigurationSettings.AppSettings["ConnectionString"];

        SqlConnection conn = null;
        SqlDataReader reader = null;
        
        try
        {
            string nick = "";
            int score = 0;
            decimal gains = 0;

            conn = new SqlConnection(connString);
            conn.Open();
        
            int numPages = 0;
            int count = FFPage.getCount();

            numPages = (int)Math.Ceiling((double)count / pageSize);

            string sql1 = "Select * from Standings S order by Score desc, Gains desc, UserId asc";

            SqlCommand cmd = new SqlCommand(sql1, conn);
            reader = cmd.ExecuteReader();

            DateTimeOffset localDateTime = DateTimeOffset.Now;
            DateTimeOffset pacificDateTime = new DateTimeOffset();
            try
            {
                if (TimeZoneInfo.Local.IsDaylightSavingTime(localDateTime))
                {
                    pacificDateTime = TimeZoneInfo.ConvertTime(localDateTime,
                        TimeZoneInfo.FindSystemTimeZoneById("Pacific Daylight Time"));
                }
                else
                {
                    pacificDateTime = TimeZoneInfo.ConvertTime(localDateTime,
                        TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
                }
            }
            catch (Exception)
            {
                pacificDateTime = TimeZoneInfo.ConvertTime(localDateTime,
                    TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
            }

            String time = pacificDateTime.ToString("MM/dd/yyyy");

            for(int i = 1; i <= numPages; ++i) {
                
                string result = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                result += "<standings ";
                result += "asOf=\"";
                result += time;
                result += "\" page=\"";
                result += i;
                result += "\" total=\"";
                result += Math.Min(numPages, maxPages);
                result += "\">";

                for (int j = 0; j < pageSize && reader.Read(); ++j)
                {
                    for (int k = 0;k < reader.FieldCount; ++k)
                    {
                        switch (reader.GetName(k))
                        {
                            case "Nick":
                                nick = (string)reader.GetValue(k);
                                break;
                            case "Score":
                                score = (int)reader.GetValue(k);
                                break;
                            case "Gains":
                                gains = (decimal)reader.GetValue(k);
                                break;
                            default:
                                break;
                        }
                    }
                    result += "<player>";
                    result += "<nick>";
                    result += nick;
                    result += "</nick>";
                    result += "<score>";
                    result += score.ToString();
                    result += "</score>";
                    result += "<gain>";
                    result += gains.ToString();
                    result += "</gain>";
                    result += "</player>";
                }
                result += "</standings>";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                doc.Save(HttpContext.Current.Server.MapPath(prefix + i + ext));
            }
            reader.Close();
        }
        catch (SqlException)
        {
            return false;
        }
        finally
        {
            conn.Close();
        }
        return true;
    }

    /*private static Boolean writePage(int page, int numPages, int count)
    {
        string result = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
        if (page > 0 && page < maxPages + 1)
        {
            string connString = (string)ConfigurationSettings.AppSettings["ConnectionString"];

            SqlConnection conn = null;
            SqlDataReader reader = null;

            try
            {
                string nick = "";
                int score = 0;
                decimal gains = 0;

                conn = new SqlConnection(connString);
                conn.Open();

                int remainder = count % pageSize;
                int actualPage = numPages - (page - 1);
                int test = pageSize;

                if (actualPage < 1)
                {
                    return false;
                }

                string sql1 = "Select top " + pageSize + " * from (Select top " + actualPage * pageSize + " * from Standings order by Score asc, Gains asc, UserID desc) S order by Score desc, Gains desc, UserId asc";

                SqlCommand cmd = new SqlCommand(sql1, conn);
                reader = cmd.ExecuteReader();

                result += "<standings ";
                result += "page=\"";
                result += page;
                result += "\" total=\"";
                result += Math.Min(numPages,maxPages);
                result += "\">";

                while (reader.Read())
                {
                    if (page == numPages && count > pageSize && remainder > 0 && test > remainder)
                    {
                        --test;
                        continue;
                    }
                    for (int i = 0; i < reader.FieldCount; ++i)
                    {
                        switch (reader.GetName(i))
                        {
                            case "Nick":
                                nick = (string)reader.GetValue(i);
                                break;
                            case "Score":
                                score = (int)reader.GetValue(i);
                                break;
                            case "Gains":
                                gains = (decimal)reader.GetValue(i);
                                break;
                            default:
                                break;
                        }
                    }
                    result += "<player>";
                    result += "<nick>";
                    result += nick;
                    result += "</nick>";
                    result += "<score>";
                    result += score.ToString();
                    result += "</score>";
                    result += "<gain>";
                    result += gains.ToString();
                    result += "</gain>";
                    result += "</player>";
                }

                result += "</standings>";

                reader.Close();

            }
            catch (SqlException s)
            {
                throw s;
                //result += "<error code='007'>false</error>";
                //return result;
            }
            finally
            {
                conn.Close();
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            doc.Save(HttpContext.Current.Server.MapPath(prefix + page + ext));
            return true;
        }
        else
        {
            //result += "<error code='006'>false</error>";
            return false;
        }
    }*/

    public static string readPage(int page)
    {
        StreamReader reader = null;
        string result = "";
        if (page > 0 && page < maxPages + 1)
        {
            try
            {
                FileStream fileStream = new FileStream(HttpContext.Current.Server.MapPath(prefix + page + ext), FileMode.Open, FileAccess.Read);
                reader = new StreamReader(fileStream);
                result = reader.ReadToEnd();
            }
            catch (FileNotFoundException)
            {
                return result;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
        return result;
    }
}
