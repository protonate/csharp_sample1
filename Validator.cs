using System;
using System.Text.RegularExpressions;

public class Validator
{
    private static Regex rxPageId = new Regex(@"^[a-z_]+$", RegexOptions.Compiled);
    private static Regex rxTab = new Regex(@"^[0123456789]{1}$", RegexOptions.Compiled);
    private static Regex rxName = new Regex(@"^[0-9A-Za-zÀ-ÿŠOEŽšoežŸ.\s]+(([\-\' ]|[\.\,][ ]?)[0-9A-Za-zÀ-ÿŠOEŽšoežŸ\s]+)*[\.]?$", RegexOptions.Compiled);                      
    private static Regex rxFullName = new Regex(@"^[A-Za-zÀ-ÿŠOEŽšoežŸ\s]+(([\-\' ]|[\.\,][ ]?)[A-Za-zÀ-ÿŠOEŽšoežŸ\s]+)*[\.]?$", RegexOptions.Compiled);                           
    //private static Regex rxPhone = new Regex(@"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$", RegexOptions.Compiled);
    private static Regex rxPhone = new Regex(@"^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$", RegexOptions.Compiled);
    private static Regex rxGuid = new Regex(@"^[\{]?[0-9A-Fa-f]{8}[\-][0-9A-Fa-f]{4}[\-][0-9A-Fa-f]{4}[\-][0-9A-Fa-f]{4}[\-][0-9A-Fa-f]{12}[\}]?$", RegexOptions.Compiled);
    private static Regex rxEmail = new Regex(@"^[a-zA-Z0-9\-\._\']+@([a-zA-Z0-9\-]+[\.])+[a-zA-Z]{2,3}$", RegexOptions.Compiled);
    //private static Regex rxEmail = new Regex(@"^[a-z][\w.-]+@\w[\w.-]+\.[\w.-]*[a-z][a-z]$", RegexOptions.Compiled);
    private static Regex rxSearch = new Regex(@"^\s*[\s\w\-\+]+\s*$", RegexOptions.Compiled);
    //private static Regex rxPassword = new Regex(@"^(?=\S*?[A-Za-z])(?=\S*?[^A-Za-z])\S{6,20}$", RegexOptions.Compiled);
    private static Regex rxPassword = new Regex(@"^[A-Za-z0-9]{1,20}$", RegexOptions.Compiled);
    private static Regex rxSecurityAnswer = new Regex(@"^[A-Za-zÀ-ÿŠOEŽšoežŸ0-9+-_ ]+$", RegexOptions.Compiled);
    private static Regex rxFund = new Regex(@"^([A-Z]{3}[\-]){14}[A-Z]{3}$", RegexOptions.Compiled);
    private static Regex rxZip = new Regex(@"^[0-9]{5}([\-][0-9]{4})?$", RegexOptions.Compiled);

    public static bool IsValid(string candidate, string type)
    {
        bool isValid = false;

        switch (type)
        {
            case "password":
                if (!String.IsNullOrEmpty(candidate) && rxPassword.IsMatch(candidate))
                {
                    isValid = true;
                }
                break;                    
            case "pageid":
                if (!String.IsNullOrEmpty(candidate) && rxPageId.IsMatch(candidate))
                {
                    isValid = true;
                }
                break;
            case "tab":
                if (!String.IsNullOrEmpty(candidate) && rxTab.IsMatch(candidate))
                {
                    isValid = true;
                }
                break;
            case "fullname":
                if (!String.IsNullOrEmpty(candidate) && rxFullName.IsMatch(candidate))
                {
                    isValid = true;
                }
                break;
            case "name":
                if (!String.IsNullOrEmpty(candidate) && rxName.IsMatch(candidate))
                {
                    isValid = true;
                }
                break;
            case "phone":
                if (!String.IsNullOrEmpty(candidate) && rxPhone.IsMatch(candidate))
                {
                    isValid = true;
                }
                break;
            case "userid":
                if (!String.IsNullOrEmpty(candidate) && rxGuid.IsMatch(candidate))
                {
                    isValid = true;
                }
                break;
            case "email":
                if (!String.IsNullOrEmpty(candidate) && rxEmail.IsMatch(candidate))
                {
                    isValid = true;
                }
                break;
            case "search":
                if (!String.IsNullOrEmpty(candidate) && rxSearch.IsMatch(candidate))
                {
                    isValid = true;
                }
                break;
            case "securityAnswer":
                if (!String.IsNullOrEmpty(candidate) && rxSecurityAnswer.IsMatch(candidate))
                {
                    isValid = true;
                }
                break;
            case "fund":
                if (!String.IsNullOrEmpty(candidate) && rxFund.IsMatch(candidate))
                {
                    isValid = true;
                }
                break;
            case "zip":
                if (!String.IsNullOrEmpty(candidate) && rxZip.IsMatch(candidate))
                {
                    isValid = true;
                }
                break;
            default:
                break;
        }

        return isValid;
    }
}