using System.Collections.Generic;

namespace Palavyr.Domain.Resources.Responses
{
    public static class ResponseVariableDefinition
    {
        public const string CompanyVariablePattern = @"{%Company%}";
        public const string Company = "Company";

        public const string NameVariablePattern = @"{%Name%}";
        public const string Name = "Name";

        public const string LogoUriVariablePattern = @"{%Logo%}";
        public const string Logo = "Logo";

        public static List<ResponseVariable> GetAvailableVariables()
        {
            return new List<ResponseVariable>()
            {
                new ResponseVariable(Company, CompanyVariablePattern, "Your company name provided in Settings."),
                new ResponseVariable(Name, NameVariablePattern, "The name provided by the client in the chat dialog."),
                new ResponseVariable(Logo, LogoUriVariablePattern, "The logo you provided in Settings"),
            };
        }
    }
}