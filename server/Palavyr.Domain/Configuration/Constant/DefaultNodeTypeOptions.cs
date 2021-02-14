using System.Collections.Generic;

namespace Server.Domain.Configuration.Constant
{
    public static class DefaultNodeTypeOptions
    {
        public static List<NodeTypeOption> DefaultNodeTypeOptionsList => // These get sent to the UI for user selection
            new List<NodeTypeOption>()
            {
                new YesNo(),
                new YesNoNotSure(),
                new YesNotSureCombined(),
                new NoNotSureCombined(),
                new TakeText(),
                new ProvideInfo(),
                new MultipleChoiceContinue(),
                new MultipleChoiceAsPath(),
                new TakeNumber(),
                new TakeNumberIndividuals(),
                new SendResponse(),
                new TooComplicated(),
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

        public static TakeNumber CreateTakeNumber() => new TakeNumber();
        public static TakeNumberIndividuals CreateTakeNumberIndividuals() => new TakeNumberIndividuals();
        public static TooComplicated CreateTooComplicated() => new TooComplicated();
        public static SendResponse CreateSendResponse() => new SendResponse();
        public static Restart CreateRestart() => new Restart();

        public class TakeNumberIndividuals : NodeTypeOption
        {
            public static string StringName => nameof(TakeNumberIndividuals);

            public TakeNumberIndividuals()
            {
                Text = "Take Number Individuals";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = InfoCollection;
            }
        }
        
        public class TakeNumber : NodeTypeOption
        {
            public static string StringName => nameof(TakeNumber);

            public TakeNumber()
            {
                Text = "Take Number";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = InfoCollection;
            }
        }


        public class YesNo : NodeTypeOption
        {
            public static string StringName => nameof(YesNo);
            public const string Yes = "Yes";
            public const string No = "No";

            public YesNo()
            {
                Value = StringName;
                Text = "Yes or No";
                PathOptions = new List<string>() {"No", "Yes"};
                ValueOptions = new List<string>() {"No", "Yes"};
                IsMultiOptionType = false; // set to no if we don't want to allow the node value options presented to the user to change. 
                IsTerminalType = false;
                GroupName = MultipleChoice;
            }
        }


        public class YesNoNotSure : NodeTypeOption
        {
            public static string StringName => nameof(YesNoNotSure);

            public YesNoNotSure()
            {
                Value = StringName;
                Text = "Yes, No, Not Sure";
                PathOptions = new List<string>() {"Yes", "No", "Not Sure"};
                ValueOptions = new List<string>() {"Yes", "No", "Not Sure"};
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = MultipleChoice;
            }
        }

        public class YesNotSureCombined : NodeTypeOption
        {
            public static string StringName => nameof(YesNotSureCombined);

            public YesNotSureCombined()
            {
                Value = StringName;
                Text = "Yes / Not Sure, No";
                PathOptions = new List<string>() {"Yes / Not Sure", "No"};
                ValueOptions = new List<string>() {"Yes / Not Sure", "No"};
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = MultipleChoice;
            }
        }

        public class NoNotSureCombined : NodeTypeOption
        {
            public static string StringName => nameof(NoNotSureCombined);

            public NoNotSureCombined()
            {
                Text = "Yes, No / Not Sure";
                Value = StringName;
                PathOptions = new List<string>() {"Yes", "No / Not Sure"};
                ValueOptions = new List<string>() {"Yes", "No / Not Sure"};
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = MultipleChoice;
            }
        }

        public class TakeText : NodeTypeOption
        {
            public static string StringName => nameof(TakeText);

            public TakeText()
            {
                Text = "Take Text";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = InfoCollection;
            }
        }

        public class ProvideInfo : NodeTypeOption
        {
            public static string StringName => nameof(ProvideInfo);

            public ProvideInfo()
            {
                Text = "Provide Info";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = InfoProvide;
            }
        }

        public class MultipleChoiceAsPath : NodeTypeOption
        {
            public static string StringName => nameof(MultipleChoiceAsPath);

            public MultipleChoiceAsPath()
            {
                Text = "Multiple Choice (as Paths)";
                Value = StringName;
                PathOptions = new List<string>() { };
                ValueOptions = new List<string>() { };
                IsMultiOptionType = true;
                IsTerminalType = false;
                GroupName = MultipleChoice;
            }
        }

        public class MultipleChoiceContinue : NodeTypeOption
        {
            public static string StringName => nameof(MultipleChoiceContinue);

            public MultipleChoiceContinue()
            {
                Text = "Multiple Choice (Continue)";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() { };
                IsMultiOptionType = true;
                IsTerminalType = false;
                GroupName = MultipleChoice;
            }
        }

        //Ending Sequences
        public class TooComplicated : NodeTypeOption
        {
            public static string StringName => nameof(TooComplicated);

            public TooComplicated()
            {
                Text = "Too Complicated";
                Value = StringName;
                PathOptions = new List<string>() { };
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = true;
                GroupName = Terminal;
            }
        }

        public class SendResponse : NodeTypeOption
        {
            public static string StringName => nameof(SendResponse);

            public SendResponse()
            {
                Text = "Send Response";
                Value = StringName;
                PathOptions = new List<string>() { };
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = true;
                GroupName = Terminal;
            }
        }

        public class SendEmail : NodeTypeOption
        {
            public static string StringName => nameof(SendEmail);

            public SendEmail()
            {
                Text = "Send Email";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = false;
            }
        }

        public class SendTooComplicatedEmail : NodeTypeOption
        {
            public static string StringName => nameof(SendTooComplicatedEmail);

            public SendTooComplicatedEmail()
            {
                Text = "Send Too Complicated Email";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = false;
            }
        }

        public class Restart : NodeTypeOption
        {
            public static string StringName => nameof(Restart);

            public Restart()
            {
                Text = "Restart the Chat";
                Value = StringName;
                PathOptions = new List<string>() { };
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                GroupName = Terminal;
            }
        }
    }
}