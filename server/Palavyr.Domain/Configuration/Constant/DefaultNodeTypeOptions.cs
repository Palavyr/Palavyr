using System.Collections.Generic;

namespace Server.Domain.Configuration.Constant
{
    public static class DefaultNodeTypeOptions
    {
        public static List<NodeTypeOption> DefaultNodeTypeOptionsList => new List<NodeTypeOption>()
        {
            new YesNo(),
            new YesNoNotSure(),
            new YesNotSureCombined(),
            new NoNotSureCombined(),
            new TakeText(),
            new ProvideInfo(),
            new MultipleChoiceContinue(),
            new MultipleChoiceAsPath(),
            new TooComplicated(),
            new EndingSequence(),
            new Email(),
            new SendEmail(),
            new Phone(),
            new Name(),
            new Restart()
        };

        public static YesNo CreateYesNo() => new YesNo();
        public static YesNoNotSure CreateYesNoNotSure() => new YesNoNotSure();
        public static YesNotSureCombined CreateYesNotSureCombined() => new YesNotSureCombined();
        public static NoNotSureCombined CreateNoNotSureCombined() => new NoNotSureCombined();
        public static TakeText CreateTakeText() => new TakeText();
        public static ProvideInfo CreateProvideInfo() => new ProvideInfo();
        public static MultipleChoiceContinue CreateMultipleChoiceContinue() => new MultipleChoiceContinue();
        public static MultipleChoiceAsPath CreateMultipleChoiceAsPath() => new MultipleChoiceAsPath();
        public static TooComplicated CreateTooComplicated() => new TooComplicated();
        public static EndingSequence CreateEndingSequence() => new EndingSequence();
        public static Email CreateEmail() => new Email();
        public static SendEmail CreateSendEmail() => new SendEmail();
        public static Phone CreatePhone() => new Phone();
        public static Name CreateName() => new Name();
        public static Restart CreateRestart() => new Restart();
        
        public class YesNo : NodeTypeOption
        {
            public static string StringName => nameof(YesNo);

            public YesNo()
            {
                Value = nameof(YesNo);
                Text = "Yes or No";
                PathOptions = new List<string>() {"No", "Yes"};
                ValueOptions = new List<string>() {"No", "Yes"};
                IsMultiOptionType = true;
                IsTerminalType = false;
            }
        }
        
        
        public class YesNoNotSure : NodeTypeOption
        {
            public static string StringName => nameof(YesNoNotSure);

            public YesNoNotSure()
            {
                Value = nameof(YesNoNotSure);
                Text = "Yes, No, Not Sure";
                PathOptions = new List<string>() {"Yes", "No", "Not Sure"};
                ValueOptions = new List<string>() {"Yes", "No", "Not Sure"};
                IsMultiOptionType = true;
                IsTerminalType = false;
            }
        }

        public class YesNotSureCombined : NodeTypeOption
        {
            public static string StringName => nameof(YesNotSureCombined);

            public YesNotSureCombined()
            {
                Value = nameof(YesNotSureCombined);
                Text = "Yes / Not Sure, No";
                PathOptions = new List<string>() {"Yes / Not Sure", "No"};
                ValueOptions = new List<string>() {"Yes / Not Sure", "No"};
                IsMultiOptionType = true;
                IsTerminalType = false;
            }
        }

        public class NoNotSureCombined : NodeTypeOption
        {
            public static string StringName => nameof(NoNotSureCombined);

            public NoNotSureCombined()
            {
                Text = "Yes, No / Not Sure";
                Value = nameof(NoNotSureCombined);
                PathOptions = new List<string>() {"Yes", "No / Not Sure"};
                ValueOptions = new List<string>() {"Yes", "No / Not Sure"};
                IsMultiOptionType = true;
                IsTerminalType = false;
            }
        }

        public class TakeText : NodeTypeOption
        {
            public static string StringName => nameof(TakeText);

            public TakeText()
            {
                Text = "Take Text";
                Value = nameof(TakeText);
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = false;
            }
        }

        public class ProvideInfo : NodeTypeOption
        {
            public static string StringName => nameof(ProvideInfo);

            public ProvideInfo()
            {
                Text = "Provide Info";
                Value = nameof(ProvideInfo);
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = false;
            }
        }

        public class MultipleChoiceAsPath : NodeTypeOption
        {
            public static string StringName => nameof(MultipleChoiceAsPath);

            public MultipleChoiceAsPath()
            {
                Text = "Multiple Choice (as Paths)";
                Value = nameof(MultipleChoiceAsPath);
                PathOptions = new List<string>() { };
                ValueOptions = new List<string>() { };
                IsMultiOptionType = true;
                IsTerminalType = false;
            }
        }

        public class MultipleChoiceContinue : NodeTypeOption
        {
            public static string StringName => nameof(MultipleChoiceContinue);

            public MultipleChoiceContinue()
            {
                Text = "Multiple Choice (Continue)";
                Value = nameof(MultipleChoiceContinue);
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = false;
            }
        }

        //Ending Sequences
        public class TooComplicated : NodeTypeOption
        {
            public static string StringName => nameof(TooComplicated);

            public TooComplicated()
            {
                Text = "Too Complicated";
                Value = nameof(TooComplicated);
                PathOptions = new List<string>() { };
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = false;
            }
        }

        public class EndingSequence : NodeTypeOption
        {
            public static string StringName => nameof(EndingSequence);

            public EndingSequence()
            {
                Text = "Ending Sequence";
                Value = nameof(EndingSequence);
                PathOptions = new List<string>() { };
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = true;
            }
        }

        public class Name : NodeTypeOption
        {
            public static string StringName => nameof(Name);

            public Name()
            {
                Text = "Collect Name";
                Value = nameof(Name);
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = false;
            }
        }

        public class Email : NodeTypeOption
        {
            public static string StringName => nameof(Email);

            public Email()
            {
                Text = "Collect Email Address";
                Value = nameof(Email);
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = false;
            }
        }

        public class Phone : NodeTypeOption
        {
            public static string StringName => nameof(Phone);

            public Phone()
            {
                Text = "Phone Number";
                Value = nameof(Phone);
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = false;
            }
        }

        public class SendEmail : NodeTypeOption
        {
            public static string StringName => nameof(SendEmail);

            public SendEmail()
            {
                Text = "Send Email";
                Value = nameof(SendEmail);
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = true;
            }
        }

        public class Restart : NodeTypeOption
        {
            public static string StringName => nameof(Restart);

            public Restart()
            {
                Text = "Restart the Chat";
                Value = nameof(Restart);
                PathOptions = new List<string>() { };
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
            }
        }
    }
}