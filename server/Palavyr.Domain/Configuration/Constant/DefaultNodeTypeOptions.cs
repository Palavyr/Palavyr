using System.Collections.Generic;

namespace Palavyr.Domain.Configuration.Constant
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
                new TakeCurrency(),
                new ProvideInfo(),
                new MultipleChoiceContinue(),
                new MultipleChoiceAsPath(),
                new TakeNumber(),
                new TakeNumberIndividuals(),
                new SendResponse(),
                new TooComplicated(),
                new SplitMerge(),
                new Anabranch()
            };

        public static YesNo CreateYesNo() => new YesNo();
        public static YesNoNotSure CreateYesNoNotSure() => new YesNoNotSure();
        public static YesNotSureCombined CreateYesNotSureCombined() => new YesNotSureCombined();
        public static NoNotSureCombined CreateNoNotSureCombined() => new NoNotSureCombined();
        public static TakeText CreateTakeText() => new TakeText();
        public static ProvideInfo CreateProvideInfo() => new ProvideInfo();
        public static MultipleChoiceContinue CreateMultipleChoiceContinue() => new MultipleChoiceContinue();
        public static MultipleChoiceAsPath CreateMultipleChoiceAsPath() => new MultipleChoiceAsPath();
        public static SplitMerge CreateSplitMerge() => new SplitMerge();
        public static Anabranch CreateAnabranch() => new Anabranch();
        
        public static TakeCurrency CreateTakeCurrency() => new TakeCurrency();
        public static TakeNumber CreateTakeNumber() => new TakeNumber();
        public static TakeNumberIndividuals CreateTakeNumberIndividuals() => new TakeNumberIndividuals();
        public static TooComplicated CreateTooComplicated() => new TooComplicated();
        public static SendResponse CreateSendResponse() => new SendResponse();
        public static Restart CreateRestart() => new Restart();


        public class Anabranch : NodeTypeOption
        {
            public new static string StringName => nameof(Anabranch);

            public Anabranch()
            {
                Text = "Anabranch";
                Value = StringName;
                PathOptions = new List<string>();
                ValueOptions = new List<string>();
                IsMultiOptionType = true;
                IsTerminalType = false;
                GroupName = SplitAndMerge;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = true;
                IsAnabranchType = true;
                IsAnabranchMergePoint = false;
            }
        }
        
        public class SplitMerge : NodeTypeOption
        {
            public new static string StringName => nameof(SplitMerge);

            public SplitMerge()
            {
                Text = "Split Merge";
                Value = StringName;
                PathOptions = new List<string>();
                ValueOptions = new List<string>();
                IsMultiOptionType = true;
                IsTerminalType = false;
                GroupName = SplitAndMerge;
                IsSplitMergeType = true;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = true;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
            }
        }
        

        public class TakeCurrency : NodeTypeOption
        {
            public new static string StringName => nameof(TakeCurrency);

            public TakeCurrency()
            {
                Text = "Take Currency";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() {"Continue"};
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = InfoCollection;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;

            }
        }
        
        public class TakeNumberIndividuals : NodeTypeOption
        {
            public new static string StringName => nameof(TakeNumberIndividuals);

            public TakeNumberIndividuals()
            {
                Text = "Take Number Individuals";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() {"Continue"};
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = InfoCollection;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;

            }
        }
        
        public class TakeNumber : NodeTypeOption
        {
            public new static string StringName => nameof(TakeNumber);

            public TakeNumber()
            {
                Text = "Take Number";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() { "Continue" };
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = InfoCollection;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;

            }
        }


        public class YesNo : NodeTypeOption
        {
            public new static string StringName => nameof(YesNo);
            public const string Yes = "Yes";
            public const string No = "No";

            public YesNo()
            {
                Value = StringName;
                Text = "Yes or No";
                PathOptions = new List<string>() {"No", "Yes"};
                ValueOptions = new List<string>() {"No", "Yes"};
                IsMultiOptionType = true; // set to no if we don't want to allow the node value options presented to the user to change. 
                IsTerminalType = false;
                GroupName = MultipleChoice;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;

            }
        }


        public class YesNoNotSure : NodeTypeOption
        {
            public new static string StringName => nameof(YesNoNotSure);

            public YesNoNotSure()
            {
                Value = StringName;
                Text = "Yes, No, Not Sure";
                PathOptions = new List<string>() {"Yes", "No", "Not Sure"};
                ValueOptions = new List<string>() {"Yes", "No", "Not Sure"};
                IsMultiOptionType = true;
                IsTerminalType = false;
                GroupName = MultipleChoice;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;

            }
        }

        public class YesNotSureCombined : NodeTypeOption
        {
            public new static string StringName => nameof(YesNotSureCombined);

            public YesNotSureCombined()
            {
                Value = StringName;
                Text = "Yes / Not Sure, No";
                PathOptions = new List<string>() {"Yes / Not Sure", "No"};
                ValueOptions = new List<string>() {"Yes / Not Sure", "No"};
                IsMultiOptionType = true;
                IsTerminalType = false;
                GroupName = MultipleChoice;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
            }
        }

        public class NoNotSureCombined : NodeTypeOption
        {
            public new static string StringName => nameof(NoNotSureCombined);

            public NoNotSureCombined()
            {
                Text = "Yes, No / Not Sure";
                Value = StringName;
                PathOptions = new List<string>() {"Yes", "No / Not Sure"};
                ValueOptions = new List<string>() {"Yes", "No / Not Sure"};
                IsMultiOptionType = true;
                IsTerminalType = false;
                GroupName = MultipleChoice;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
            }
        }

        public class TakeText : NodeTypeOption
        {
            public new static string StringName => nameof(TakeText);

            public TakeText()
            {
                Text = "Take Text";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() { "Continue" };
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = InfoCollection;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
            }
        }

        public class ProvideInfo : NodeTypeOption
        {
            public new static string StringName => nameof(ProvideInfo);

            public ProvideInfo()
            {
                Text = "Provide Info";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() { "Continue" };
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = InfoProvide;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
            }
        }

        public class MultipleChoiceAsPath : NodeTypeOption
        {
            public new static string StringName => nameof(MultipleChoiceAsPath);

            public MultipleChoiceAsPath()
            {
                Text = "Multiple Choice (as Paths)";
                Value = StringName;
                PathOptions = new List<string>() { };
                ValueOptions = new List<string>() { };
                IsMultiOptionType = true;
                IsTerminalType = false;
                GroupName = MultipleChoice;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = true;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
            }
        }

        public class MultipleChoiceContinue : NodeTypeOption
        {
            public new static string StringName => nameof(MultipleChoiceContinue);

            public MultipleChoiceContinue()
            {
                Text = "Multiple Choice (Continue)";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() { };
                IsMultiOptionType = true;
                IsTerminalType = false;
                GroupName = MultipleChoice;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = true;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
            }
        }

        //Ending Sequences
        public class TooComplicated : NodeTypeOption
        {
            public new static string StringName => nameof(TooComplicated);

            public TooComplicated()
            {
                Text = "Too Complicated";
                Value = StringName;
                PathOptions = new List<string>() { };
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = true;
                GroupName = Terminal;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
            }
        }

        public class SendResponse : NodeTypeOption
        {
            public new static string StringName => nameof(SendResponse);

            public SendResponse()
            {
                Text = "Send Response";
                Value = StringName;
                PathOptions = new List<string>() { };
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                IsTerminalType = true;
                GroupName = Terminal;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
            }
        }

        public class SendEmail : NodeTypeOption
        {
            public new static string StringName => nameof(SendEmail);

            public SendEmail()
            {
                Text = "Send Email";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() {"Continue"};
                IsMultiOptionType = false;
                IsTerminalType = false;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
            }
        }

        public class SendTooComplicatedEmail : NodeTypeOption
        {
            public new static string StringName => nameof(SendTooComplicatedEmail);

            public SendTooComplicatedEmail()
            {
                Text = "Send Too Complicated Email";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() {"Continue"};
                IsMultiOptionType = false;
                IsTerminalType = false;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
            }
        }

        public class Restart : NodeTypeOption
        {
            public new static string StringName => nameof(Restart);

            public Restart()
            {
                Text = "Restart the Chat";
                Value = StringName;
                PathOptions = new List<string>() { };
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                GroupName = Terminal;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
            }
        }
    }
}