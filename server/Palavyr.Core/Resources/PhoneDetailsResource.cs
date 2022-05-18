namespace Palavyr.Core.Resources
{
    public class PhoneDetailsResource
    {
        public string PhoneNumber { get; set; }
        public string Locale { get; set; }

        public static PhoneDetailsResource Create(string phoneNumber, string locale)
        {
            return new PhoneDetailsResource()
            {
                PhoneNumber = phoneNumber,
                Locale = locale
            };
        }
    }
}