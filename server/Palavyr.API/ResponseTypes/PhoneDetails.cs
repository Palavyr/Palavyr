namespace Palavyr.API.ResponseTypes
{
    public class PhoneDetails
    {
        public string PhoneNumber { get; set; }
        public string Locale { get; set; }

        public static PhoneDetails Create(string phoneNumber, string locale)
        {
            return new PhoneDetails()
            {
                PhoneNumber = phoneNumber,
                Locale = locale
            };
        }
    }
}