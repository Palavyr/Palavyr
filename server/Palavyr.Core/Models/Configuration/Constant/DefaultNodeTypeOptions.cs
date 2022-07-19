using System.Collections.Generic;

namespace Palavyr.Core.Models.Configuration.Constant
{
    public class DefaultNodeTypeOptions
    {
        public static class NodeComponentTypes
        {
            public static string YesNo => DefaultNodeTypeOptions.YesNo.StringName;
            public static string YesNoNotSure => DefaultNodeTypeOptions.YesNoNotSure.StringName;
            public static string YesNotSureCombined => DefaultNodeTypeOptions.YesNotSureCombined.StringName;
            public static string NoNotSureCombined => DefaultNodeTypeOptions.NoNotSureCombined.StringName;
            public static string TakeText => DefaultNodeTypeOptions.TakeText.StringName;
            public static string TakeNumber => DefaultNodeTypeOptions.TakeNumber.StringName;
            public static string TakeCurrency => DefaultNodeTypeOptions.TakeCurrency.StringName;
            public static string TakeNumberIndividuals => DefaultNodeTypeOptions.TakeNumberIndividuals.StringName;
            public static string ProvideInfo => DefaultNodeTypeOptions.ProvideInfo.StringName;
            public static string MultipleChoiceAsPath => DefaultNodeTypeOptions.MultipleChoiceAsPath.StringName;
            public static string MultipleChoiceContinue => DefaultNodeTypeOptions.MultipleChoiceContinue.StringName;

            public static string ShowFileAsset => DefaultNodeTypeOptions.ShowFileAsset.StringName;
            public static string ShowImage => DefaultNodeTypeOptions.ShowImage.StringName;
            public static string TooComplicated => DefaultNodeTypeOptions.TooComplicated.StringName;
            public static string SendResponse => DefaultNodeTypeOptions.SendResponse.StringName;
            public static string EndWithoutEmail => DefaultNodeTypeOptions.EndWithoutEmail.StringName;
            public static string LoopbackAnchor => DefaultNodeTypeOptions.LoopbackAnchor.StringName;

            public static string Selection => DefaultNodeTypeOptions.Selection.StringName;
            public static string CollectDetails => DefaultNodeTypeOptions.CollectDetails.StringName;

        }

        public static List<NodeTypeOptionResource> IntroNodeOptionList =>
            new List<NodeTypeOptionResource>()
            {
                new ProvideInfo(),
                new CollectDetails(),
                new Selection()
            };

        public static List<NodeTypeOptionResource> DefaultNodeTypeOptionsList => // These get sent to the UI for user selection
            new List<NodeTypeOptionResource>()
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
                new Anabranch(),
                new ShowFileAsset(),
                new EndWithoutEmail(),
                new LoopbackAnchor(),
                new Loopback()
            };

        public static YesNo CreateYesNo() => new YesNo();
        public static YesNoNotSure CreateYesNoNotSure() => new YesNoNotSure();
        public static YesNotSureCombined CreateYesNotSureCombined() => new YesNotSureCombined();
        public static NoNotSureCombined CreateNoNotSureCombined() => new NoNotSureCombined();
        public static TakeText CreateTakeText() => new TakeText();
        public static ProvideInfo CreateProvideInfo() => new ProvideInfo();
        public static MultipleChoiceContinue CreateMultipleChoiceContinue() => new MultipleChoiceContinue();

        public static MultipleChoiceAsPath CreateMultipleChoiceAsPath() => new MultipleChoiceAsPath();

        public static Anabranch CreateAnabranch() => new Anabranch();
        public static ShowImage CreateShowImage() => new ShowImage();
        public static ShowFileAsset CreateShowFileAsset() => new ShowFileAsset();

        public static TakeCurrency CreateTakeCurrency() => new TakeCurrency();
        public static TakeNumber CreateTakeNumber() => new TakeNumber();
        public static TakeNumberIndividuals CreateTakeNumberIndividuals() => new TakeNumberIndividuals();
        public static TooComplicated CreateTooComplicated() => new TooComplicated();

        public static SendResponse CreateSendResponse() => new SendResponse();

        public static LoopbackAnchor CreateLoopbackAnchor() => new LoopbackAnchor();
        public static Loopback CreateLoopback() => new Loopback();

        // public static Restart CreateRestart() => new Restart();
        public static EndWithoutEmail CreateEndWithoutEmail() => new EndWithoutEmail();

        public static Selection CreateSelection() => new Selection();
        public static CollectDetails CreateCollectDetails() => new CollectDetails();


        public class LoopbackAnchor : NodeTypeOptionResource
        {
            public new static string StringName => nameof(LoopbackAnchor);

            public LoopbackAnchor()
            {
                Text = "Loopback Anchor";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() {"Continue"};
                IsMultiOptionType = true;
                IsTerminalType = false;
                GroupName = Teleport;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = true;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsLoopbackAnchor = true;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.MultipleChoiceAsPath;
                IsCurrency = false;
                IsMultiOptionEditable = true;
                NodeTypeCode = NodeTypeCode.VII;
            }
        }

        public class Loopback : NodeTypeOptionResource
        {
            public new static string StringName => nameof(Loopback);

            public Loopback()
            {
                Text = "Loopback";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() {"Continue"};
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = Teleport;
                ShouldRenderChildren = false;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.ProvideInfo;
                IsCurrency = false;
                IsMultiOptionEditable = false;
                NodeTypeCode = NodeTypeCode.VIII;
            }
        }


        public class EndWithoutEmail : NodeTypeOptionResource
        {
            public new static string StringName => nameof(EndWithoutEmail);

            public EndWithoutEmail() // This means no email at all. We have an option in the 
            {
                Text = "End Without Response";
                Value = StringName;
                PathOptions = new List<string>();
                ValueOptions = new List<string>();
                IsMultiOptionType = false;
                IsTerminalType = true;
                GroupName = Terminal;
                ShouldRenderChildren = false;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.EndWithoutEmail;
                IsCurrency = false;
                IsMultiOptionEditable = false;
                NodeTypeCode = NodeTypeCode.I;
            }
        }

        public class ShowFileAsset : NodeTypeOptionResource

        {
            public new static string StringName => nameof(ShowFileAsset);

            public ShowFileAsset()
            {
                Text = "Show File";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() {"Continue"};
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = InfoProvide;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.ShowFileAsset;
                IsCurrency = false;
                IsMultiOptionEditable = false;
                IsImageNode = true;
                NodeTypeCode = NodeTypeCode.IX;
            }
        }

        public class ShowImage : NodeTypeOptionResource
        {
            public new static string StringName => nameof(ShowImage);

            public ShowImage()
            {
                Text = "Show Image";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() {"Continue"};
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = InfoProvide;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.ShowImage;
                IsCurrency = false;
                IsMultiOptionEditable = false;
                IsImageNode = true;
                NodeTypeCode = NodeTypeCode.IX;
            }
        }


        public class Anabranch : NodeTypeOptionResource
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
                ShouldRenderChildren = true;
                ShouldShowMultiOption = true;
                IsAnabranchType = true;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.MultipleChoiceAsPath;
                IsCurrency = false;
                IsMultiOptionEditable = true;
                NodeTypeCode = NodeTypeCode.VI;
            }
        }

        public class TakeCurrency : NodeTypeOptionResource
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
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.TakeCurrency;
                IsCurrency = true;
                IsMultiOptionEditable = false;
                NodeTypeCode = NodeTypeCode.II;
            }
        }

        public class TakeNumberIndividuals : NodeTypeOptionResource
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
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.TakeNumber;
                IsCurrency = false;
                IsMultiOptionEditable = false;
                NodeTypeCode = NodeTypeCode.II;
            }
        }

        public class TakeNumber : NodeTypeOptionResource
        {
            public new static string StringName => nameof(TakeNumber);

            public TakeNumber()
            {
                Text = "Take Number";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() {"Continue"};
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = InfoCollection;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.TakeNumber;
                IsCurrency = false;
                IsMultiOptionEditable = false;
                NodeTypeCode = NodeTypeCode.II;
            }
        }


        public class YesNo : NodeTypeOptionResource
        {
            public new static string StringName => nameof(YesNo);
            public const string Yes = "Yes";
            public const string No = "No";

            public YesNo()
            {
                Value = StringName;
                Text = "Yes or No";
                PathOptions = new List<string>() {"Yes", "No"};
                ValueOptions = new List<string>() {"Yes", "No"};
                IsMultiOptionType = true; // set to no if we don't want to allow the node value options presented to the user to change. 
                IsTerminalType = false;
                GroupName = MultipleChoice;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.YesNo;
                IsCurrency = false;
                IsMultiOptionEditable = false;
                NodeTypeCode = NodeTypeCode.V;
            }
        }


        public class YesNoNotSure : NodeTypeOptionResource
        {
            public new static string StringName => nameof(YesNoNotSure);

            public YesNoNotSure()
            {
                Value = StringName;
                Text = "Yes, No, Not Sure";
                PathOptions = new List<string>() {"Yes", "Not Sure", "No"};
                ValueOptions = new List<string>() {"Yes", "Not Sure", "No"};
                IsMultiOptionType = true;
                IsTerminalType = false;
                GroupName = MultipleChoice;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.YesNoNotSure;
                IsCurrency = false;
                IsMultiOptionEditable = false;
                NodeTypeCode = NodeTypeCode.V;
            }
        }

        public class YesNotSureCombined : NodeTypeOptionResource
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
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.YesNotSureCombined;
                IsCurrency = false;
                IsMultiOptionEditable = false;
                NodeTypeCode = NodeTypeCode.V;
            }
        }

        public class NoNotSureCombined : NodeTypeOptionResource
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
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.NoNotSureCombined;
                IsCurrency = false;
                IsMultiOptionEditable = false;
                NodeTypeCode = NodeTypeCode.V;
            }
        }

        public class TakeText : NodeTypeOptionResource
        {
            public new static string StringName => nameof(TakeText);

            public TakeText()
            {
                Text = "Take Text";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() {"Continue"};
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = InfoCollection;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.TakeText;
                IsCurrency = false;
                IsMultiOptionEditable = false;
                NodeTypeCode = NodeTypeCode.II;
            }
        }


        public class CollectDetails : NodeTypeOptionResource
        {
            public new static string StringName => nameof(CollectDetails);

            public CollectDetails()
            {
                Text = "Collect Details";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() {"Continue"};
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = InfoCollection;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.CollectDetails;
                IsCurrency = false;
                IsMultiOptionEditable = false;
                NodeTypeCode = NodeTypeCode.II;
            }
        }

        public class Selection : NodeTypeOptionResource
        {
            public new static string StringName => nameof(Selection);

            public Selection()
            {
                Text = "Selection";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() {"Continue"};
                IsMultiOptionType = false;
                IsTerminalType = true;
                GroupName = Terminal;
                ShouldRenderChildren = false;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.Selection;
                IsCurrency = false;
                IsMultiOptionEditable = false;
                NodeTypeCode = NodeTypeCode.I;
            }
        }

        public class ProvideInfo : NodeTypeOptionResource
        {
            public new static string StringName => nameof(ProvideInfo);

            public ProvideInfo()
            {
                Text = "Provide Info";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() {"Continue"};
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = InfoProvide;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.ProvideInfo;
                IsCurrency = false;
                IsMultiOptionEditable = false;
                NodeTypeCode = NodeTypeCode.II;
            }
        }

        public class MultipleChoiceAsPath : NodeTypeOptionResource
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
                ShouldRenderChildren = true;
                ShouldShowMultiOption = true;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.MultipleChoiceAsPath;
                IsCurrency = false;
                IsMultiOptionEditable = true;
                NodeTypeCode = NodeTypeCode.IV;
            }
        }

        public class MultipleChoiceContinue : NodeTypeOptionResource
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
                ShouldRenderChildren = true;
                ShouldShowMultiOption = true;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.MultipleChoiceContinue;
                IsCurrency = false;
                IsMultiOptionEditable = true;
                NodeTypeCode = NodeTypeCode.III;
            }
        }

        //Ending Sequences
        public class TooComplicated : NodeTypeOptionResource
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
                ShouldRenderChildren = false;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.TooComplicated;
                IsMultiOptionEditable = false;
                IsCurrency = false;
                NodeTypeCode = NodeTypeCode.I;
            }
        }

        public class SendResponse : NodeTypeOptionResource
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
                ShouldRenderChildren = false;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.SendResponse;
                IsMultiOptionEditable = false;
                IsCurrency = false;
                NodeTypeCode = NodeTypeCode.I;
            }
        }
        
        
        public class ProvideInfoWithPdfLink : NodeTypeOptionResource
        {
            public new static string StringName => nameof(ProvideInfoWithPdfLink);

            public ProvideInfoWithPdfLink()
            {
                Text = "Provide Info With Pdf Link";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() {"Continue"};
                IsMultiOptionType = false;
                IsTerminalType = false;
                GroupName = InfoProvide;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsPricingStrategyType = false;
                NodeComponentType = NodeComponentTypes.ProvideInfo;
                IsCurrency = false;
                IsMultiOptionEditable = false;
                NodeTypeCode = NodeTypeCode.II;
            }
        }

    }
}