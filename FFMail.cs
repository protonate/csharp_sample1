using System;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Xml;

using System.Configuration;
using System.Web.Configuration;
using System.Net.Configuration;

/// <summary>
/// Summary description for FFMail
/// </summary>
public class FFMail
{
    /*  args[0] == To Address
     *  args[1] == From Address
     *  args[2] == Message Subject
     *  args[3] == Message Body
     */

    public static int TO_ADDRESS = 0;
    public static int FROM_ADDRESS = 1;
    public static int MESSAGE_SUBJECT = 2;
    public static int MESSAGE_BODY = 3;

    public static string [] getParams(int message, string toAddress)
    {
        FileStream content = null;
        try
        {
            content = new FileStream(HttpContext.Current.Server.MapPath("../App_Data/email.xml"), FileMode.Open, FileAccess.Read);
            XmlDocument mail = new XmlDocument();
            mail.Load(content);
            XmlNodeList messages = mail.GetElementsByTagName("message");

            string[] args = new string[4];

            args[TO_ADDRESS] = toAddress;
            args[FROM_ADDRESS] = messages[message].ChildNodes[0].InnerText;
            args[MESSAGE_SUBJECT] = messages[message].ChildNodes[1].InnerText;
            args[MESSAGE_BODY] = messages[message].ChildNodes[2].InnerText;
            args[MESSAGE_BODY] = autoReplace(args[MESSAGE_BODY]);

            content.Close();
            return args;
        }
        catch (Exception e)
        {
            content.Close();
            throw e;
        }
    }

    public static bool sendMail(string [] args)
    {
        if (args.Length < 4) return false;

        MailMessage message = new MailMessage();

        message.To.Add(new MailAddress(args[TO_ADDRESS]));
        message.From = new MailAddress(args[FROM_ADDRESS]);
        message.Subject = args[MESSAGE_SUBJECT];
        message.IsBodyHtml = true;
        message.Body = args[MESSAGE_BODY];

        SmtpClient client = new SmtpClient();
        /*Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
        MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");

        if (settings.Smtp.Network.UserName != "")
        {
            System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(settings.Smtp.Network.UserName, settings.Smtp.Network.Password);
            client.UseDefaultCredentials = false;
            client.Credentials = SMTPUserInfo;
        }
        else
        {
            client.UseDefaultCredentials = true;
        }*/
        try
        {
            client.Send(message);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return true;
    }

    private static string autoReplace(string text) 
    {
        string temp = text;
        string o_br = "BREAK_HERE";
        string n_br = "<br />";
        string o_hr = "HOME_PAGE_LINK";
        string n_hr = "REDACTED";
        string o_pr = "HOME_PAGE_POST";
        string n_pr = "REDACTED";

        temp = specReplace(temp, o_br, n_br);
        temp = specReplace(temp, o_hr, n_hr);
        temp = specReplace(temp, o_pr, n_pr);

        return temp;
    }

    public static string specReplace(string t, string c, string r)
    {
        t = t.Replace(c, r);
        return t;
    }
}
