using System.Collections.Generic;

namespace Palavyr.Core.Models.Configuration.Constant
{
    public class InternalNodeTypeOptions
    {
        // these are only exposed internal to the API - used in the ending sequence to some extent. These are not availalable right now in the ui until we have very 
        // thorough validation logic when saving trees. Right now we do not.
        public class ShowResponseFileAsset : NodeTypeOptionResource
        {
            public new static string StringName => nameof(ShowResponseFileAsset);

            public ShowResponseFileAsset()
            {
                Text = "Show File Asset";
                Value = StringName;
                PathOptions = new List<string>() { };
                ValueOptions = new List<string>() { };
                IsMultiOptionType = false;
                GroupName = InfoProvide;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsDynamicType = false;
                IsMultiOptionEditable = false;
                IsCurrency = false;
            }
            
        }
        public class Restart : NodeTypeOptionResource
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
                IsDynamicType = false;
                IsMultiOptionEditable = false;
                IsCurrency = false;
            }
        }

        public class SendEmail : NodeTypeOptionResource
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
                IsDynamicType = false;
                IsMultiOptionEditable = false;
                IsCurrency = false;
            }
        }

        public class SendTooComplicatedEmail : NodeTypeOptionResource
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
                IsDynamicType = false;
                IsMultiOptionEditable = false;
                IsCurrency = false;
            }
        }

        public class EvaluateThreshold : NodeTypeOptionResource
        {
            // hidden node
            public new static string StringName => nameof(EvaluateThreshold);

            public EvaluateThreshold()
            {
                Text = "Evaluate Threshold";
                Value = StringName;
                PathOptions = new List<string>() {"Continue"};
                ValueOptions = new List<string>() {"Continue"};
                IsMultiOptionType = true;
                IsTerminalType = false;
                GroupName = InfoCollection;
                IsSplitMergeType = false;
                ShouldRenderChildren = true;
                ShouldShowMultiOption = false;
                IsAnabranchType = false;
                IsAnabranchMergePoint = false;
                IsDynamicType = false;
                NodeComponentType = DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceAsPath;
                IsCurrency = false;
                IsMultiOptionEditable = true;
            }
        }
    }
}