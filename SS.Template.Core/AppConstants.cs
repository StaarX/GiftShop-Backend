namespace SS.Template.Core
{
    public static class AppConstants
    {
        public const int EmailLength = 100;
        public const int StandardValueLength = 100;
        public const int PostalCodeLength = 10;
        public const int CountryCodeLength = 3;
        public const int MaxLength = 4000;
        public const int StandardDecimalValueLength = 20;

        public const int QrCodeLength = 1024;
    }

    public static class Regex
    {

//Special thanks to https://ihateregex.io/


        public const string PhoneNumber = "^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$";
        public const string UserName = "^[a-z0-9_-]{3,15}$";
        public const string Date = "(?:(?:31(\\/|-|\\.)(?:0?[13578]|1[02]))\\1|(?:(?:29|30)(\\/|-|\\.)(?:0?[13-9]|1[0-2])\\2))(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$|^(?:29(\\/|-|\\.)0?2\\3(?:(?:(?:1[6-9]|[2-9]\\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\\d|2[0-8])(\\/|-|\\.)(?:(?:0?[1-9])|(?:1[0-2]))\\4(?:(?:1[6-9]|[2-9]\\d)?\\d{2})";
        public const string Letters = "^[a-zA-Z]+$";
        public const string Desc= "^[a-zA-Z ]+$";
        public const string Numbers = "^[0-9]+$";
        public const string Password = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$";
        public const string SimpleEmail = "[^@ \\t\\r\\n]+@[^@ \\t\\r\\n]+\\.[^@ \\t\\r\\n]+";
        public const string LettersAndNumbers = "[a-zA-Z0-9]+";




    }
}
