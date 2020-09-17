namespace Server.Domain
{
    public static class NodeTypes
    {
        public const string YesNo = "YesNo";
        public const string YesNoNotSure = "YesNoNotSure";
        public const string YesNotSureCombined = "YesNotSureCombined";
        public const string NotNotSureCombined = "NoNotSureCombined";
        public const string TakeText = "TakeText";
        public const string Info = "Info";
        public const string MultipleChoiceByPath = "MultipleChoiceByPath"; // generic of yes / no
        public const string MultipleChoiceContinue = "MultipleChoiceContinue"; // continues to next node
        public const string HowMuch = "HowMuch";
        public const string HowMany = "HowMany";
        
        public const string TooComplicated = "TooComplicated";
        public const string EndingSequence = "EndingSequence";
        public const string Name = "Name";
        public const string Email = "Email";
        public const string Phone = "Phone";

        public const string SendEmail = "SendEmail";
        public const string Restart = "Restart";
    }
}