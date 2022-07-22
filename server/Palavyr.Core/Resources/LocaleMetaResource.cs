namespace Palavyr.Core.Resources
{
    public class LocaleMetaResource
    {
        public LocaleResource CurrentLocale { get; set; }
        public LocaleResource[] LocaleMap { get; set; }

        public void AddLocaleMap(LocaleResource[] localeMap)
        {
            LocaleMap = localeMap;
        }
    }
}