namespace Palavyr.API.settings
{
    public class ConfigurationSettings
    {

        public ConfigurationSettings Settings { get; set; }    
        
        public class AWSSettings
        {
            public string AccessKey { get; set; }
            public string SecretKey { get; set; }
            public string Region { get; set; }
            public string ProfileLocation { get; set; }
        }
    }
}